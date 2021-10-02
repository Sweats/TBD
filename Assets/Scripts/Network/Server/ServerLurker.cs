using Mirror;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class ServerLurker : MonoBehaviour
{
    private ServerLurker() { }
    private Coroutine ghostFormRoutine;

    private Coroutine physicalFormRoutine;

    private Coroutine attackCooldownRoutine;

    private bool lurkerRoutineRunning = false;

    private List<uint> armableTrapsList;

    private GameObject[] trapsList;


    private bool energyMaxed = false;

    private Lurker lurker;

    public void RegisterNetworkHandlers()
    {
        EventManager.serverClientGameSurvivorsDeadEvent.AddListener(OnAllSurvivorsDead);
        EventManager.serverClientGameSurvivorsEscapedEvent.AddListener(OnAllSurvivorsEscaped);
        NetworkServer.RegisterHandler<ServerClientGameLurkerSwingAttackMessage>(OnServerClientGameLurkerSwingAttack);
        NetworkServer.RegisterHandler<ServerClientGameLurkerRequestToChangeFormMessage>(OnServerClientGameRequestToChangeForm);
        NetworkServer.RegisterHandler<ServerClientGameLurkerSwingAtNothingMessage>(OnServerClienGameLurkerSwingAtNothing);
        NetworkServer.RegisterHandler<ServerClientGameLurkerRequestToArmTrapMessage>(OnServerClientGameLurkerArmTrapRequest);
        EventManager.serverClientGameLurkerJoinedEvent.AddListener(OnServerClientGameLurkerJoinedEvent);
    }

    public void OnServerSceneChanged()
    {
        armableTrapsList = new List<uint>();
    }

    private void OnServerClientGameLurkerJoinedEvent(uint lurkerNetId)
    {
        if (lurker == null)
        {
            lurker = NetworkIdentity.spawned[lurkerNetId].GetComponent<Lurker>();
        }

        trapsList = GameObject.FindGameObjectsWithTag(Tags.TRAP);
        ghostFormRoutine = StartCoroutine(GhostFormRoutine());
        SelectArmableTraps();
        uint[] selectedTrapsToBeArmable = armableTrapsList.ToArray();
        lurker.netIdentity.connectionToClient.Send(new ClientServerGameLurkerArmableTrapsMessage { armableTraps = selectedTrapsToBeArmable });
    }

    private void OnAllSurvivorsEscaped()
    {
        StopLurkerRoutines();
    }

    private void OnAllSurvivorsDead()
    {
        StopLurkerRoutines();
    }

    private void StopLurkerRoutines()
    {
        if (ghostFormRoutine != null)
        {
            StopCoroutine(ghostFormRoutine);

        }

        if (physicalFormRoutine != null)
        {
            StopCoroutine(physicalFormRoutine);
        }

    }

    private IEnumerator AttackCooldownRoutine()
    {
        int seconds = lurker.AttackCooldownInSeconds();
        yield return new WaitForSeconds(seconds);
        lurker.AllowToAttack(true);

    }

    private IEnumerator GhostFormRoutine()
    {
        float energy = lurker.ServerEnergy();
        float maxEnergy = lurker.ServerMaxEnergy();
        float seconds = lurker.ServerEnergyConsumptionRate();

        WaitForSeconds waitForSeconds = new WaitForSeconds(seconds);

        while (energy < maxEnergy)
        {
            energy++;
            lurker.ServerSetEnergy(energy);
            yield return  waitForSeconds;
        }

        lurker.netIdentity.connectionToClient.Send(new ClientServerGameLurkerReadyToGoIntoPhysicalFormMessage{});
        //lurker.ServerSetEnergy(energy);
    }

    private IEnumerator PhysicalFormRoutine()
    {
        float energy = lurker.ServerEnergy();
        float minEnergy = lurker.ServerMinEnergy();
        float seconds = lurker.ServerEnergyRegenerationRate();
        WaitForSeconds waitForSeconds = new WaitForSeconds(seconds);
        uint[] selectedTrapsToBeArmable = armableTrapsList.ToArray();
        
        lurker.netIdentity.connectionToClient.Send(new ClientServerGameLurkerArmableTrapsMessage { armableTraps = selectedTrapsToBeArmable });

        while (energy > minEnergy)
        {
            energy--;
            lurker.ServerSetEnergy(energy);
            yield return waitForSeconds;
        }

        // NOTE: Updating a syncvar here
        lurker.ServerSetEnergy(energy);
        lurker.SetGhostForm();
        ghostFormRoutine = StartCoroutine(GhostFormRoutine());
    }

    private void OnServerClientGameLurkerSwingAttack(NetworkConnection connection, ServerClientGameLurkerSwingAttackMessage message)
    {
        if (!NetworkIdentity.spawned.ContainsKey(message.requestedTargetId))
        {
            return;
        }

        Lurker lurker;

        bool isLurker = connection.identity.TryGetComponent<Lurker>(out lurker);

        if (!isLurker)
        {
            return;
        }

        if (lurker.IsGhostForm())
        {
            return;
        }

        if (!lurker.CanAttack())
        {
            return;
        }

        lurker.AllowToAttack(false);
        attackCooldownRoutine = StartCoroutine(AttackCooldownRoutine());
        NetworkServer.SendToReady(new ClientServerGameLurkerAttackedMessage{lurkerId = connection.identity.netId});

        Survivor survivor;

        bool isSurvivor = NetworkIdentity.spawned[message.requestedTargetId].TryGetComponent<Survivor>(out survivor);

        if (!isSurvivor)
        {
            return;
        }

        Vector3 lurkerPos = lurker.transform.position;
        Vector3 survivorPos = survivor.transform.position;

        float distance = Vector3.Distance(lurkerPos, survivorPos);

        if (distance > lurker.ServerAttackDistance())
        {
            return;
        }
        
        uint deadSurvivorId = survivor.netIdentity.netId;

        NetworkServer.SendToReady(new ClientServerGameSurvivorKilledMessage{survivorId = deadSurvivorId});
    }

    private void OnServerClientGameLurkerArmTrapRequest(NetworkConnection connection, ServerClientGameLurkerRequestToArmTrapMessage message)
    {
        uint requestedTrapId = message.requestedTrapId;

        if (!NetworkIdentity.spawned.ContainsKey(requestedTrapId))
        {
            return;
        }

        Trap trap = NetworkIdentity.spawned[requestedTrapId].GetComponent<Trap>();

        if (trap == null)
        {
            return;

        }

        if (trap.ServerArmed() || !trap.ServerArmable())
        {
            return;
        }

        Lurker lurker = connection.identity.GetComponent<Lurker>();

        if (lurker == null)
        {
            return;
        }

        if (lurker.IsPhysicalForm())
        {
            return;
        }

        Vector3 lurkerPosition = lurker.transform.position;
        Vector3 trapPosition = trap.transform.position;

        float distance = Vector3.Distance(lurkerPosition, trapPosition);

        if (distance > lurker.ServerTrapArmDistance())
        {
            return;
        }

        trap.ServerArm();
        connection.identity.connectionToClient.Send(new ClientServerGameLurkerTrapArmedMessage { trapId = requestedTrapId });

    }

    private void OnServerClienGameLurkerSwingAtNothing(NetworkConnection connection, ServerClientGameLurkerSwingAtNothingMessage message)
    {
        Lurker lurker;

        bool isLurker = connection.identity.TryGetComponent<Lurker>(out lurker);

        if (!isLurker)
        {
            return;
        }

        if (!lurker.CanAttack())
        {
            return;
        }

        lurker.AllowToAttack(false);
        attackCooldownRoutine = StartCoroutine(AttackCooldownRoutine());
        uint lurkerPlayerId = lurker.netIdentity.netId;
        NetworkServer.SendToReady(new ClientServerGameLurkerAttackedMessage{lurkerId = lurkerPlayerId});
    }

    private void OnServerClientGameRequestToChangeForm(NetworkConnection connection, ServerClientGameLurkerRequestToChangeFormMessage message)
    {
        if (connection.identity.netId != lurker.netId)
        {
            return;
        }

        float lurkerEnergy = lurker.ServerEnergy();

        if (lurker.IsGhostForm())
        {
            if (lurkerEnergy >= lurker.ServerMaxEnergy())
            {
                //NOTE: Updating a syncvar.
                lurker.ServerSetPhysicalForm();
                lurker.AllowToAttack(true);
                physicalFormRoutine = StartCoroutine(PhysicalFormRoutine());
                ResetArmableTraps();
                SelectArmableTraps();
            }

        }

        else
        {
            //NOTE: Updating a syncvar.
            lurker.SetGhostForm();
            DisarmTraps();
            StopCoroutine(physicalFormRoutine);
            ghostFormRoutine = StartCoroutine(GhostFormRoutine());
        }

    }

    private void SelectArmableTraps()
    {
        armableTrapsList.Clear();
        int trapSelectedCount = 0;
        int lurkerTrapPercentage = lurker.TrapPercentage();
        int trapsOnStage = trapsList.Length;
        int trapCount = (int)Mathf.Floor(trapsOnStage * (lurkerTrapPercentage / 100));

        while (trapSelectedCount < trapCount)
        {
            int randomTrap = UnityEngine.Random.Range(0, trapsList.Length);
            Trap trap = trapsList[randomTrap].GetComponent<Trap>();

            if (trap == null)
            {
                continue;
            }

            if (trap.ServerArmable())
            {
                continue;
            }

            trap.ServerSetArmable(true);
            trapSelectedCount++;
            uint netId = trap.netIdentity.netId;
            armableTrapsList.Add(netId);
        }
    }

// NOTE: Needed to avoid an infinite loop in the above function.
    private void ResetArmableTraps()
    {
        for (var i = 0; i < armableTrapsList.Count; i++)
        {
            uint trapid = armableTrapsList[i];
            Trap trap = NetworkIdentity.spawned[trapid].GetComponent<Trap>();
            trap.ServerSetArmable(false);
        }
    }

    private void DisarmTraps()
    {
        for (var i = 0; i < trapsList.Length; i++)
        {
            Trap trap = trapsList[i].GetComponent<Trap>();
            trap.ServerDisarm();
        }
    }
}
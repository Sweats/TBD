using UnityEngine;
using Mirror;
using System.Collections;
using System.Collections.Generic;
public class ServerMary : MonoBehaviour
{
    private ServerMary() { }


    private Mary mary;

    private Coroutine energyRechargeRoutine;

    private Coroutine autoTeleportationRoutine;

    private Coroutine frenzyRoutine;

    private Coroutine attackCooldownRoutine;

    private GameObject[] maryPossibleTeleportationLocations;

    private List<Trap> oldTrapsList;

    public void RegisterNetworkHandlers()
    {
        NetworkServer.RegisterHandler<ServerClientGameMaryFrenzyRequest>(OnServerClientGameMaryFrenzyRequest);
        NetworkServer.RegisterHandler<ServerClientGameMaryTeleportRequest>(OnServerClientGameMaryTeleportRequest);
        EventManager.serverClientGameMaryJoinedEvent.AddListener(OnMaryJoinedGame);

    }

    private void OnMaryJoinedGame(uint netid)
    {
        this.mary = NetworkIdentity.spawned[netid].GetComponent<Mary>();
        maryPossibleTeleportationLocations = GameObject.FindGameObjectsWithTag(Tags.MARY_TELEPORT_LOCATION);
        oldTrapsList = new List<Trap>();
        autoTeleportationRoutine = StartCoroutine(RandomTeleportationRoutine());
        energyRechargeRoutine = StartCoroutine(EnergyRechargeRoutine());
    }

    private IEnumerator EnergyRechargeRoutine()
    {
        float maxEnergy = mary.ServerMaxEnergy();
        float minEnergy = mary.ServerMinEnergy();
        float minEnergyNeededToTeleport = mary.ServerEnergyNeededToTeleport();
        float energyNeededToFrenzy = mary.ServerEnergyNeededToFrenzy();
        float rechargeRate = mary.ServerEnergyRechargeRate();
        WaitForSeconds seconds = new WaitForSeconds(mary.ServerEnergyRechargeRate());

        while (mary.ServerEnergy() < maxEnergy)
        {
            float currentEnergy = mary.ServerEnergy();
            currentEnergy++;

            if (currentEnergy == minEnergyNeededToTeleport)
            {
                mary.netIdentity.connectionToClient.Send(new ClientServerGameMaryReadyToTeleportMessage { });
            }

            if (currentEnergy == energyNeededToFrenzy)
            {
                mary.netIdentity.connectionToClient.Send(new ClientServerGameMaryReadyToFrenzyMessage { });

            }

            mary.ServerSetEnergy(currentEnergy);

            yield return seconds;
        }

    }
    private IEnumerator RandomTeleportationRoutine()
    {
        while (true)
        {
            int min = mary.ServerMinTeleportationTimerInSeconds();
            int max = mary.ServerMaxTeleportationTimerInSeconds();
            int seconds = UnityEngine.Random.Range(min, max);
            yield return new WaitForSeconds(seconds);
            Vector3 pickedPosition = PickRandomTeleportationPosition();
            mary.connectionToClient.Send(new ClientServerGameMaryAutoTeleportMessage{newPosition = pickedPosition}); 
            ArmNewTraps(pickedPosition);
        }

    }

    private Vector3 PickRandomTeleportationPosition()
    {
        int randomNumber = UnityEngine.Random.Range(0, maryPossibleTeleportationLocations.Length);
        return maryPossibleTeleportationLocations[randomNumber].transform.position;
    }

    private IEnumerator FrenzyRoutine()
    {
        float currentEnergy = mary.ServerEnergy();
        WaitForSeconds seconds = new WaitForSeconds(mary.ServerEnergyConsumptionRate());

        while (currentEnergy > 0)
        {
            currentEnergy--;
            mary.ServerSetEnergy(currentEnergy);
            yield return seconds;
        }

        mary.ServerSetFrenzied(false);
        mary.ServerSetAttack(false);
        autoTeleportationRoutine = StartCoroutine(RandomTeleportationRoutine());
        energyRechargeRoutine = StartCoroutine(EnergyRechargeRoutine());
        Vector3 pickedPosition = PickRandomTeleportationPosition();
        mary.connectionToClient.Send(new ClientServerGameMaryAutoTeleportMessage {newPosition = pickedPosition});
        NetworkServer.SendToReady(new ClientServerGameMaryFrenzyOverMessage{maryId = mary.netIdentity.netId});
        ArmNewTraps(pickedPosition);
    }

    private IEnumerator AttackCooldownRoutine()
    {
        yield return new WaitForSeconds(mary.ServerAttackCooldown());
        mary.ServerSetAttack(true);
    }

    private void ArmNewTraps(Vector3 origin)
    {
        for (var i = 0; i < oldTrapsList.Count; i++)
        {
            Trap trap = oldTrapsList[i];
            trap.ServerDisarm();
        }

        oldTrapsList.Clear();
        float maryArmTrapDistance = mary.ServerArmTrapDistance();

        RaycastHit[] hitObjects = Physics.SphereCastAll(origin, maryArmTrapDistance, transform.forward, maryArmTrapDistance);

        for (var i = 0; i < hitObjects.Length; i++)
        {
            GameObject trapObject = hitObjects[i].collider.gameObject;

            if (trapObject.CompareTag(Tags.TRAP))
            {
                Trap trap = trapObject.GetComponent<Trap>();
                oldTrapsList.Add(trap);
                trap.ServerArm();

            }
        }
    }

    private void OnServerClientGameMaryTeleportRequest(NetworkConnection connection, ServerClientGameMaryTeleportRequest message)
    {
        uint netid = connection.identity.netId;

        Mary mary;

        bool isMary = NetworkIdentity.spawned[netid].TryGetComponent<Mary>(out mary);

        if (!isMary)
        {
            return;
        }

        float currentEnergy = mary.ServerEnergy();
        float energyNeededToTeleport = mary.ServerEnergyNeededToTeleport();

        if (currentEnergy < energyNeededToTeleport)
        {
            return;
        }

        float cost = mary.ServerTeleportationEnergyCost();
        float maxEnergy = mary.ServerMaxEnergy();

        mary.ServerSetEnergy(currentEnergy - cost);

        // NOTE: We need this because if the player teleports after the cap has been reached, then they won't be able to gain energy anymore!
        if (currentEnergy >= maxEnergy)
        {
            energyRechargeRoutine = StartCoroutine(EnergyRechargeRoutine());
        }


        Vector3 pickedPosition = PickRandomTeleportationPosition();
        mary.connectionToClient.Send(new ClientServerGameMaryAllowTeleportMessage {newPosition = pickedPosition});
        ArmNewTraps(pickedPosition);

        if (mary.ServerFrenzied())
        {
            return;
        }

        StopCoroutine(autoTeleportationRoutine);
        autoTeleportationRoutine = StartCoroutine(RandomTeleportationRoutine());
    }

    private void OnServerClientGameMaryAttackedSurvivor(NetworkConnection connection, ServerClientGameMaryAttackedSurvivorMessage message)
    {
        if (!NetworkIdentity.spawned.ContainsKey(message.requestedSurvivorId))
        {
            return;
        }

        Mary mary;

        bool isMary = connection.identity.TryGetComponent<Mary>(out mary);

        if (!isMary)
        {
            return;
        }

        if (!mary.ServerFrenzied())
        {
            return;
        }

        if (!mary.CanAttack())
        {
            return;
        }

        Survivor survivor;

        bool isVictimSurvivor = NetworkIdentity.spawned[message.requestedSurvivorId].TryGetComponent<Survivor>(out survivor);

        NetworkServer.SendToReady(new ClientServerGameMaryAttackedMessage{maryId = connection.identity.netId});

        if (!isVictimSurvivor)
        {
            return;
        }

        Vector3 maryPos = mary.transform.position;
        Vector3 survivorPos = survivor.transform.position;

        float distance = Vector3.Distance(maryPos, survivorPos);

        if (distance > mary.ServerAttackDistance())
        {
            return;
        }

        NetworkServer.SendToReady(new ClientServerGameSurvivorKilledMessage { survivorId = message.requestedSurvivorId });

    }

    private void OnServerClientGameMaryAttackedNothing(NetworkConnection connection, ServerClientGameMaryAttackedNothingMessage message)
    {
        Mary mary;

        bool isMary = connection.identity.TryGetComponent<Mary>(out mary);

        if (!isMary)
        {
            return;
        }

        if (!mary.ServerFrenzied())
        {
            return;
        }

        if (!mary.CanAttack())
        {
            return;
        }

        mary.ServerSetAttack(false);
        NetworkServer.SendToReady(new ClientServerGameMaryAttackedMessage { maryId = connection.identity.netId});
        attackCooldownRoutine = StartCoroutine(AttackCooldownRoutine());
    }

    private void OnServerClientGameMaryFrenzyRequest(NetworkConnection connection, ServerClientGameMaryFrenzyRequest message)
    {
        uint netid = connection.identity.netId;

        Mary mary;

        bool isMary = NetworkIdentity.spawned[netid].TryGetComponent<Mary>(out mary);

        if (!isMary)
        {
            return;
        }

        float currentEnergy = mary.ServerEnergy();

        float energyNeededToFrenzy = mary.ServerEnergyNeededToFrenzy();

        if (currentEnergy < energyNeededToFrenzy)
        {
            return;
        }

        NetworkServer.SendToReady(new ClientServerGameMaryFrenziedMessage {maryId = netid });
        mary.ServerSetFrenzied(true);
        mary.ServerSetAttack(true);
        StopCoroutine(autoTeleportationRoutine);
        StopCoroutine(energyRechargeRoutine);
        frenzyRoutine = StartCoroutine(FrenzyRoutine());

    }
}
using UnityEngine;
using Mirror;
using System.Collections;
using System.Collections.Generic;

public class ServerPhantom: MonoBehaviour
{
    private ServerPhantom(){}

    private Coroutine attackCooldownRoutine;

    private Coroutine survivorDetectionRoutine;


    private Phantom phantom;

    public void RegisterNetworkHandlers()
    {
        NetworkServer.RegisterHandler<ServerClientGamePhantomSwingAtNothingMessage>(OnServerClientGamePhantomAttackNothing);
        NetworkServer.RegisterHandler<ServerClientGamePhantomSwingAttackMessage>(OnServerClientGamePhantomAttackedSurvivor);
        EventManager.serverClientGamePhantomJoinedEvent.AddListener(OnServerClientGamePhantomJoinedEvent);
    }

    private void OnServerClientGamePhantomJoinedEvent(uint phantomId)
    {
        if (phantom == null)
        {
            phantom = NetworkIdentity.spawned[phantomId].GetComponent<Phantom>();
        }

        survivorDetectionRoutine = StartCoroutine(SurvivorDetectionRoutine());
    }

    private void OnServerClientGamePhantomAttackNothing(NetworkConnection connection, ServerClientGamePhantomSwingAtNothingMessage message)
    {
        Phantom phantom;

        bool isPhantom = connection.identity.TryGetComponent<Phantom>(out phantom);

        if (!isPhantom)
        {
            return;
        }

        if (!phantom.ServerCanAttack())
        {
            return;
        }

        phantom.ServerSetAttack(false);
        attackCooldownRoutine = StartCoroutine(AttackRoutine());
        uint phantomPlayerid = phantom.netIdentity.netId;
        NetworkServer.SendToReady(new ClientServerGamePhantomAttackedMessage{phantomId = phantomPlayerid});
    }

    private void OnServerClientGamePhantomAttackedSurvivor(NetworkConnection connection, ServerClientGamePhantomSwingAttackMessage message)
    {
        if (!NetworkIdentity.spawned.ContainsKey(message.requestedSurvivorId))
        {
            return;
        }

        Phantom phantom;

        bool isPhantom = connection.identity.TryGetComponent<Phantom>(out phantom);

        if (!isPhantom)
        {
            return;
        }

        if (!phantom.ServerCanAttack())
        {
            return;
        }

        Survivor survivor;

        bool isSurvivor = NetworkIdentity.spawned[message.requestedSurvivorId].TryGetComponent<Survivor>(out survivor);

        if (!isSurvivor)
        {
            return;
        }

        uint phantomPlayerId = connection.identity.netId;

        NetworkServer.SendToReady(new ClientServerGamePhantomAttackedMessage{phantomId = phantomPlayerId});

        Vector3 phantomPos = phantom.transform.position;
        Vector3 survivorPos = survivor.transform.position;

        float distance = Vector3.Distance(phantomPos, survivorPos);

        if (distance > phantom.ServerAttackDistance())
        {
            return;
        }

        NetworkServer.SendToReady(new ClientServerGameSurvivorKilledMessage{survivorId = message.requestedSurvivorId});
    }
    
    private IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(phantom.AttackCoolDownInSeconds());
        phantom.ServerSetAttack(true);

    }

    private IEnumerator SurvivorDetectionRoutine()
    {
        WaitForSeconds seconds = new WaitForSeconds(0.1f);

        while (true)
        {
            var connections = NetworkServer.connections.Keys;

            foreach(int key in connections)
            {
                int connectionId = key;

                Survivor survivor;

                bool isSurvivor = NetworkServer.connections[connectionId].identity.TryGetComponent<Survivor>(out survivor);

                if (!isSurvivor)
                {
                   continue; 
                }

                Vector3 survivorPosition = survivor.transform.position;
                Vector3 phantomPosition = phantom.transform.position;

                float distance = Vector3.Distance(survivorPosition, phantomPosition);

                uint survivorId = survivor.netIdentity.netId;

                if (distance > phantom.ServerDetectionDistance())
                {
                    if (survivor.ServerDetectedByPhantom())
                    {
                        survivor.ServerSetDetectedByPhantom(false);
                        phantom.netIdentity.connectionToClient.Send(new ClientServerGamePhantomSurvivorNoLongerDetected{noLongerDetectedSurvivorId = survivorId});
                    }

                }

                else
                {
                    if (!survivor.ServerDetectedByPhantom())
                    {
                        survivor.ServerSetDetectedByPhantom(true);
                        phantom.netIdentity.connectionToClient.Send(new ClientServerGamePhantomSurvivorDetectedMesasge{detectedSurvivorId = survivorId});
                    }
                }

            }

            yield return seconds;
        }

    }
}
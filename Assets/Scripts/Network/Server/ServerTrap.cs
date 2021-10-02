using Mirror;
using UnityEngine;
using System.Collections;

public class ServerTrap: MonoBehaviour
{
    private ServerTrap(){}

    private Coroutine survivorTrapRoutine;

    private bool insanityEnabled;
    public void OnServerSceneChanged(bool insanityEnabled)
    {
        this.insanityEnabled = insanityEnabled;
        survivorTrapRoutine = StartCoroutine(ServerSurvivorTrapRoutine());
    }

    public void RegisterNetworkHandlers()
    {
        NetworkServer.RegisterHandler<ServerClientGameHostRequestedToStartGameMessage>(OnServerClientGameHostStartedGame);
        EventManager.serverClientGameSurvivorsDeadEvent.AddListener(OnServerClientSurvivorsDeadEvent);
        EventManager.serverClientGameSurvivorsEscapedEvent.AddListener(OnServerClientGameSurvivorsEscapedEvent);
    }

    private void OnServerClientGameSurvivorsEscapedEvent()
    {
        StopCoroutine(survivorTrapRoutine);

    }

    private void OnServerClientSurvivorsDeadEvent()
    {
        StopCoroutine(survivorTrapRoutine);

    }

    private void OnServerClientGameHostStartedGame(NetworkConnection connection, ServerClientGameHostRequestedToStartGameMessage message)
    {
        survivorTrapRoutine = StartCoroutine(ServerSurvivorTrapRoutine());
    }



    private IEnumerator ServerSurvivorTrapRoutine()
    {
        while (true)
        {
            var keys = NetworkServer.connections.Keys;

            foreach(int key in keys)
            {
                int connectionId = key;

                if (!NetworkServer.connections.ContainsKey(connectionId))
                {
                    continue;
                }

                NetworkConnectionToClient connection = NetworkServer.connections[connectionId];
                Survivor survivor = connection.identity.GetComponent<Survivor>();

                // NOTE: Must be a monster.
                if (survivor == null)
                {
                    continue;
                }

                RaycastHit[] hitObjects = Physics.SphereCastAll(survivor.transform.position, survivor.TrapDistance(), survivor.transform.forward, survivor.TrapDistance());

                for (var i = 0; i < hitObjects.Length; i++)
                {
                    GameObject hitObject = hitObjects[i].collider.gameObject;

                    if (hitObject.CompareTag(Tags.TRAP))
                    {
                        Trap trap = hitObject.gameObject.GetComponent<Trap>();

                        if (trap.ServerArmed())
                        {
                            trap.ServerDisarm();
                            uint trapId = trap.netIdentity.netId;
                            NetworkServer.SendToAll(new ClientServerGameTrapTriggeredMessage{triggeredTrapId = trapId});

                        }

                        if (insanityEnabled)
                        {
                            survivor.SurvivorInsanity().Increment(trap.InsanityHitAmount());
                        }

                    }
                }

            }

            yield return new WaitForSeconds(0.5f);
        }
    }
}

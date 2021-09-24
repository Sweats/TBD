using Mirror;
using UnityEngine;
using System.Collections;

public class ServerTrap: MonoBehaviour
{
    private ServerTrap(){}

    private IEnumerator trapRoutine;
    private Coroutine survivorTrapRoutine;

    private bool insanityEnabled;
    public void OnServerSceneChanged(bool insanityEnabled)
    {
        this.insanityEnabled = insanityEnabled;
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

                        NetworkServer.SendToReady(new ClientServerGameSurvivorHitTrapMessage{});

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

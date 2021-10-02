using Mirror;
using UnityEngine;
using System.Collections;
public class ServerInsanity: MonoBehaviour
{
    private ServerInsanity(){}

    private float insanityRate;

    private bool insanityEnabled;
    private Coroutine insanityRoutine;

    public void OnServerSceneChanged(float insanityRate, bool insanityEnabled)
    {
        this.insanityRate = insanityRate;
        this.insanityEnabled = insanityEnabled;
    }

    public void RegisterNetworkHandlers()
    {
        NetworkServer.RegisterHandler<ServerClientGameHostRequestedToStartGameMessage>(OnServerClientGameHostStartedGame);
        EventManager.serverClientGameSurvivorsEscapedEvent.AddListener(OnServerClientGameSurvivorsEscapedEvent);
        EventManager.serverClientGameSurvivorsDeadEvent.AddListener(OnServerClientGameSurvivorsDeadEvent);

    }

    private void OnServerClientGameHostStartedGame(NetworkConnection connection, ServerClientGameHostRequestedToStartGameMessage message)
    {
        if (insanityEnabled)
        {
            insanityRoutine = StartCoroutine(ServerInsanityRoutine());
        }

    }

    private void OnServerClientGameSurvivorsEscapedEvent()
    {
        if (insanityEnabled)
        {
            StopCoroutine(insanityRoutine);
        }
    }

    private void OnServerClientGameSurvivorsDeadEvent()
    {
        if (insanityEnabled)
        {
            StopCoroutine(insanityRoutine);
        }

    }

    private IEnumerator ServerInsanityRoutine()
    {
        while (true)
        {
            var keys = NetworkServer.connections.Keys;

            // TODO: Use a regular for loop in here instead of a foreach?
            foreach(int key in keys)
            {
                int connectionId = key;

                if (!NetworkServer.connections.ContainsKey(connectionId))
                {
                    continue;
                }

                NetworkConnectionToClient connection = NetworkServer.connections[connectionId];

                Survivor survivor = connection.identity.GetComponent<Survivor>();

                if (survivor == null)
                {
                    continue;
                }

                Insanity insanity = survivor.SurvivorInsanity();

                if (insanity.Value() >= insanity.Max())
                {
                    continue;

                }

                insanity.Increment(1f);
            }

            yield return new WaitForSeconds(insanityRate);
        }

    }
}
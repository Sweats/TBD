using Mirror;
using UnityEngine;
using System.Collections;

public class ServerFlashlight: MonoBehaviour
{

    private Coroutine serverFlashlightRoutine;

    private ServerFlashlight(){}

    public void RegisterNetworkHandlers()
    {
        NetworkServer.RegisterHandler<ServerClientGameToggledFlashlightMessage>(OnServerClientGameToggledFlashlight);
        NetworkServer.RegisterHandler<ServerClientGameHostRequestedToStartGameMessage>(OnServerClientGameHostStartedGame);
        EventManager.serverClientGameSurvivorsEscapedEvent.AddListener(OnServerSurvivorsEscapedEvent);
        EventManager.serverClientGameSurvivorsDeadEvent.AddListener(OnServerSurvivorsDeadEvent);
    }


    private void OnServerSurvivorsEscapedEvent()
    {
        StopCoroutine(serverFlashlightRoutine);

    }

    private void OnServerSurvivorsDeadEvent()
    {
        StopCoroutine(serverFlashlightRoutine);
    }

    public void OnServerClientGameHostStartedGame(NetworkConnection connection, ServerClientGameHostRequestedToStartGameMessage message)
    {
        serverFlashlightRoutine = StartCoroutine(ServerFlashlightRoutine());

    }


    private IEnumerator ServerFlashlightRoutine()
    {
        while (true)
        {
            var keys  = NetworkServer.connections.Keys;

            foreach (int key in keys)
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

                if (!survivor.ServerFlashlightToggled())
                {
                    continue;
                }

                float currentCharge = survivor.FlashlightCharge();
                currentCharge -= survivor.ServerFlashlightDischargeRate();
                // NOTE: We don't have to send a message back here because Mirror will handle the syncvar and call our hook.
                survivor.ServerSetFlashlightCharge(currentCharge);

            }

            yield return new WaitForSeconds(1f);
        }

    }


    public void OnServerClientGameToggledFlashlight(NetworkConnection connection, ServerClientGameToggledFlashlightMessage message)
    {
        NetworkIdentity identity = connection.identity;

        Survivor survivor = identity.GetComponent<Survivor>();

        if (survivor == null)
        {
            return;
        }

        survivor.ServerToggleFlashlight();

    }
}
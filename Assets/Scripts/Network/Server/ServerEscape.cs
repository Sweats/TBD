using UnityEngine;
using Mirror;

public class ServerEscape: MonoBehaviour
{
    private ServerEscape(){}

    public void RegisterNetworkHandlers()
    {
        NetworkServer.RegisterHandler<ServerClientGameSurvivorEscapedMessage>(OnServerClientGameSurvivorEscaped);
        NetworkServer.RegisterHandler<ServerClientGameSurvivorNoLongerEscapedMessage>(OnServerClientGameNoLongerEscaped);

    }

    private void OnServerClientGameSurvivorEscaped(NetworkConnection connection, ServerClientGameSurvivorEscapedMessage message)
    {
        Survivor survivor = connection.identity.GetComponent<Survivor>();

        if (survivor == null)
        {
            return;
        }

        int escapedSurvivorsCount = 0;
        int survivorsCount = 0;

        survivor.ServerSetEscaped(true);

        var keys = NetworkServer.connections.Keys;

        foreach (int key in keys)
        {
            int connectionId = key;

            if (!NetworkServer.connections.ContainsKey(connectionId))
            {
                continue;
            }

            NetworkConnectionToClient clientConnection  = NetworkServer.connections[connectionId];

            Survivor foundSurvivor = clientConnection.identity.GetComponent<Survivor>();

            if (foundSurvivor == null)
            {
                continue;
            }

            if (foundSurvivor.Dead())
            {
                continue;
            }

            survivorsCount++;

            if (foundSurvivor.ServerEscaped())
            {
                escapedSurvivorsCount++;
            }
        }

        if (escapedSurvivorsCount == survivorsCount)
        {
            NetworkServer.SendToReady(new ClientServerGameSurvivorsEscapedMessage{});
        }

    }

    private void OnServerClientGameNoLongerEscaped(NetworkConnection connection, ServerClientGameSurvivorNoLongerEscapedMessage message)
    {
        Survivor survivor = connection.identity.GetComponent<Survivor>();

        if (survivor == null)
        {
            return;
        }

        survivor.ServerSetEscaped(false);

    }

}
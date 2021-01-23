using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


#region HOST

public class NetworkRoom : NetworkManager
{
    public override void OnServerConnect(NetworkConnection connection)
    {
        base.OnServerConnect(connection);
        int netId = connection.connectionId;
        EventManager.lobbyHostPlayerConnectedEvent.Invoke(Settings.PROFILE_NAME, netId);
    }

    public override void OnServerDisconnect(NetworkConnection connection)
    {
        base.OnServerDisconnect(connection);
        int netId = connection.connectionId;
        EventManager.lobbyHostPlayerDisconnectedEvent.Invoke(Settings.PROFILE_NAME, netId);
    }

#endregion

#region CLIENT

    public override void OnClientError(NetworkConnection connection, int errorCode)
    {
        base.OnClientError(connection, errorCode);
        EventManager.lobbyClientFailedToConnectToHostEvent.Invoke(errorCode);
    }

    public override void OnClientDisconnect(NetworkConnection connection)
    {
        base.OnClientDisconnect(connection);
        int errorCode = 0;
        EventManager.lobbyClientFailedToConnectToHostEvent.Invoke(errorCode);
    }

    public override void OnClientConnect(NetworkConnection connection)
    {
        base.OnClientConnect(connection);
        EventManager.lobbyClientPlayerConnectedToLobbyEvent.Invoke();
    }
}


#endregion

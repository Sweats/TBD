using UnityEngine;
using Mirror;
using System;


#region HOST

public class NetworkRoom : NetworkManager
{
    public override void OnServerConnect(NetworkConnection connection)
    {
        base.OnServerConnect(connection);
        //EventManager.lobbyHostPlayerConnectedEvent.Invoke(Settings.PROFILE_NAME, connection);
    }
    

    public override void OnServerDisconnect(NetworkConnection connection)
    {
        base.OnServerDisconnect(connection);
        //EventManager.lobbyHostPlayerDisconnectedEvent.Invoke(Settings.PROFILE_NAME, connection);
    }


#endregion

#region CLIENT

    public override void OnClientError(NetworkConnection connection, int errorCode)
    {
        base.OnClientError(connection, errorCode);
        //EventManager.lobbyClientFailedToConnectToHostEvent.Invoke(errorCode);
    }

    public override void OnClientDisconnect(NetworkConnection connection)
    {
        base.OnClientDisconnect(connection);
        int errorCode = 0;
        //EventManager.lobbyClientFailedToConnectToHostEvent.Invoke(errorCode);
    }

    public override void OnClientConnect(NetworkConnection connection)
    {
        base.OnClientConnect(connection);
        //EventManager.lobbyClientPlayerConnectedToLobbyEvent.Invoke();
    }

    public void Join(string hostname, string port)
    {
        string uriString = $"{hostname}:{port}";
        Uri uri = new Uri(uriString);
        StartClient(uri);
    }
}


#endregion

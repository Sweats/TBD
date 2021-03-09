using Mirror;
using kcp2k;
using UnityEngine;
using System.Collections.Generic;

public struct Lobby
{
    public string lobbyName;
    public byte players;
    public byte inLobby;
    public bool privateLobby;
    public int ping;
    public int id;
    public string hostname;
    public ushort port;
    public StageName stage;

    public Lobby(string name, bool isPrivate, int id)
    {
        this.lobbyName = name;
        this.privateLobby = isPrivate;
        this.inLobby = 1;
        this.players = 0;
        this.ping = 0;
        this.id = id;
        this.hostname = "localhost";
        this.port = 7777;
        this.stage = StageName.Lobby;
    }
}

//public enum DedicatedServerRequests: byte
//{
//    Servers = 0,
//    Add,
//    Remove,
//}
//

public class DarnedMasterServerManager : NetworkManager
{
    public static ushort PORT;
    public static bool EXITING = false;
    public static bool ENABLED = false;
    public static string HOSTNAME;
    public List<Lobby> lobbies;

    [Server]
    public override void OnStartServer()
    {
        base.OnStartServer();
        lobbies = new List<Lobby>();
        Log("Master server has started successfully!");
        NetworkServer.RegisterHandler<MasterServerLobbyRequestedToBeAddedMessage>(LobbyServerRequestedToBeAdded);
        NetworkServer.RegisterHandler<MasterServerRequestedToBeRemovedMessage>(LobbyServerRequestedToBeRemoved);
        NetworkServer.RegisterHandler<MasterServerClientRequestServerListMessage>(OnClientRequstedListOfServers);
    }

    [Server]
    public override void OnStopServer()
    {

    }

    private void PingServers()
    {
        for (var i = 0; i < lobbies.Count; i++)
        {

        }
    }

    [Server]
    private void OnClientRequstedListOfServers(NetworkConnection connection, MasterServerClientRequestServerListMessage message)
    {
        MasterServerLobbyListMessage newMessage = new MasterServerLobbyListMessage
        {
            lobbies = this.lobbies.ToArray()
        };

        Log("Recieved server list request. sending to client...");

        NetworkServer.SendToClientOfPlayer(connection.identity, newMessage);
    }

    [Server]
    private void LobbyServerRequestedToBeAdded(NetworkConnection connection, MasterServerLobbyRequestedToBeAddedMessage message)
    {
        string lobbyName = message.name;
        bool isPrivate = message.isPrivate;
        int generatedId = GenerateId();
        Lobby lobby = new Lobby(lobbyName, isPrivate, generatedId);
        lobby.hostname = connection.address;
        lobbies.Add(lobby);

        Log($"if this {connection.identity} prints, then the network identity of the connection is not null! yay!");

        ServerAddedOurLobbyMessage newMessage = new ServerAddedOurLobbyMessage
        {
            id = generatedId
        };

        NetworkServer.SendToClientOfPlayer(connection.identity, newMessage);
    }

    [Server]
    private void LobbyServerRequestedToBeRemoved(NetworkConnection connection, MasterServerRequestedToBeRemovedMessage message)
    {
        Log($"Got request to remove the lobby with an ID of {message.id}");

        for (var i = 0; i < lobbies.Count; i++)
        {
            int foundId = lobbies[i].id;

            if (foundId == message.id)
            {
                Log("Found the lobby. Removing...");
                lobbies.RemoveAt(i);
                break;
            }
        }
    }

    [Server]
    private int GenerateId()
    {
        int highestId = -1;

        for (var i = 0; i < lobbies.Count; i++)
        {
            int lobbyId = lobbies[i].id;

            if (lobbyId > highestId)
            {
                highestId = lobbyId;
            }
        }

        return highestId + 1;
    }


    [Server]
    public override void OnServerConnect(NetworkConnection connection)
    {
        base.OnServerConnect(connection);
        Log("Someone has connected to the master server!");
    }

    [Server]
    public override void OnServerDisconnect(NetworkConnection connection)
    {
        base.OnServerDisconnect(connection);
        NetworkServer.DestroyPlayerForConnection(connection);
        Log("Someone has disconnected from the master server!");
    }

    public static void Log(string text)
    {
        Debug.Log($"[Darned]: {text}");
    }

    [Client]
    public override void OnClientConnect(NetworkConnection connection)
    {
        base.OnClientConnect(connection);

        if (EXITING)
        {
            MasterServerRequestedToBeRemovedMessage message = new MasterServerRequestedToBeRemovedMessage
            {
                id = DarnedNetworkManager.ID
            };

            NetworkClient.Send(message);
            Log("Told the master server that this lobby no longer exits...");
            return;
        }

        if (DarnedNetworkManager.CLIENT_HOSTING_LOBBY || DarnedNetworkManager.DEDICATED_SERVER_HOSTING_LOBBY)
        {
            MasterServerLobbyRequestedToBeAddedMessage message = new MasterServerLobbyRequestedToBeAddedMessage
            {
                name = HostLobby.LOBBY_NAME,
                port = HostLobby.LOBBY_PORT,
                isPrivate = HostLobby.LOBBY_NAME != string.Empty
            };

            Log("Asking the master server to add us to the server list...");
            NetworkClient.Send(message);

        }

        else
        {
            Log("Asking the master server for the server listing...");
            MasterServerClientRequestServerListMessage message = new MasterServerClientRequestServerListMessage();
            NetworkClient.Send(message);

        }

    }

    [Client]
    public override void OnStartClient()
    {
        base.OnStartClient();
        NetworkClient.RegisterHandler<ServerAddedOurLobbyMessage>(OnServerAddedOurLobby);
        NetworkClient.RegisterHandler<MasterServerLobbyListMessage>(OnMasterServerSentLobbyList);
    }

    private void OnMasterServerSentLobbyList(NetworkConnection connection, MasterServerLobbyListMessage message)
    {
        Log("Got the server listing from the master server!");
        EventManager.masterServerSentLobbyListEvent.Invoke(message.lobbies);
    }

    [Client]
    private void OnServerAddedOurLobby(NetworkConnection connection, ServerAddedOurLobbyMessage message)
    {
        Debug.Log("OAEUTAHOEU");
        int id = message.id;
        EventManager.dedicatedServerReceivedIdEvent.Invoke(id);

    }

}

using UnityEngine;
using Mirror;
using System.Collections.Generic;
using System;

// NOTE: I know that I could have used SyncVars but I wanted more control on how and when things are synced. So I did it by hand.
// I also think it's really dumb that we can't use attributes like [Client] or [Server] inside a class that inherits from
// NetworkManager. Maybe I'm just bad at using Mirror.
// Had I used SyncVars, I would've had to seperate the code logic into 2 different files and that just makes things more confusing
// in my opinion.

// TODO; Host migration! 
//

public struct Player
{
    public string playerName;
    public Character character;
    public NetworkIdentity identity;
    public bool hosting;

    public Player(string name, Character character, NetworkIdentity identity)
    {
        this.playerName = name;
        this.character = character;
        this.identity = identity;
        this.hosting = false;
    }

    public void SetHosting(bool value)
    {
        this.hosting = value;
    }


    public bool Hosting()
    {
        return this.hosting;
    }
}

public class NetworkRoom : NetworkManager
{
    public static bool inLobby = true;

    public static List<Player> players;

    public static bool dedicatedServer = false;

    [SerializeField]
    private LobbyNetworkManager lobbyNetworkManager;

    [SerializeField]
    private StageNetworkManager stageNetworkManager;

    public override void OnStartServer()
    {
        players = new List<Player>();

        if (dedicatedServer)
        {
            Debug.Log("Dedicated server started succesfully!");
        }

        EventManager.serverLeftGameEvent.AddListener(OnServerStopped);
        NetworkServer.RegisterHandler<ServerPlayerJoinedMessage>(ServerPlayerJoined);
        NetworkServer.RegisterHandler<LobbyServerClientRequestedToStartGameMessage>(ServerOnClientRequestedToStartGame);

        lobbyNetworkManager.RegisterServerHandlers();
        stageNetworkManager.RegisterServerHandlers();

        if (inLobby)
        {
            lobbyNetworkManager.OnStartServer();
        }

        else
        {
            stageNetworkManager.OnStartServer();
        }

    }

    // NOTE: Called when the client that is acting as the server hits the exit button or back to title screen buttons.

    private void OnServerStopped()
    {
        StopHost();
    }

    public override void OnStopServer()
    {
        if (inLobby)
        {
            lobbyNetworkManager.OnStopServer();
        }

        else
        {
            stageNetworkManager.OnStopServer();
        }

    }

    private void ServerOnClientRequestedToStartGame(NetworkConnection connection, LobbyServerClientRequestedToStartGameMessage message)
    {
        uint id = connection.identity.netId;

        if (!lobbyNetworkManager.IsHost(id))
        {
            return;
        }

        inLobby = false;
        NetworkServer.SendToAll(new ClientServerChangeSceneMessage { newValue = NetworkRoom.inLobby });
        string sceneName = message.newSceneName;
        Debug.Log($"Server is switching to the new scene named {sceneName}");
        ServerChangeScene(sceneName);

    }

    public override void OnServerAddPlayer(NetworkConnection connection)
    {
        base.OnServerAddPlayer(connection);

    }

    public override void OnServerConnect(NetworkConnection connection)
    {
        Debug.Log("Server connected player!");

        if (inLobby)
        {
            lobbyNetworkManager.OnServerConnect(connection);

        }

        else
        {
            stageNetworkManager.OnServerConnect(connection);
        }

        base.OnServerConnect(connection);
    }

    public override void OnServerDisconnect(NetworkConnection connection)
    {
        if (inLobby)
        {
            lobbyNetworkManager.OnServerDisconnect(connection);
        }

        else
        {
            stageNetworkManager.OnServerDisconnect(connection);

            if (!inLobby && players.Count == 0)
            {
                Debug.Log("There are no more players left in the server. Going back to the lobby...");
                string lobbySceneName = Stages.Name(StageName.Menu);
                inLobby = true;
                ServerChangeScene(lobbySceneName);

            }
        }

        base.OnServerDisconnect(connection);
    }

    private void ServerPlayerJoined(NetworkConnection connection, ServerPlayerJoinedMessage message)
    {
        if (inLobby)
        {
            lobbyNetworkManager.OnPlayerJoined(connection, message);
        }

        else
        {
            stageNetworkManager.OnPlayerJoined(connection, message);
        }
    }


    public override void OnStartClient()
    {
        lobbyNetworkManager.RegisterClientHandlers();
        stageNetworkManager.RegisterClientHandlers();
        NetworkClient.RegisterHandler<ClientServerChangeSceneMessage>(ClientOnServerChangedScene);

        if (inLobby)
        {
            lobbyNetworkManager.OnStartClient();

        }

        else
        {
            stageNetworkManager.OnStartClient();
        }

        base.OnStartClient();
    }

    public override void OnClientError(NetworkConnection connection, int errorCode)
    {
        Debug.Log("Something went wrong with the server!");
        base.OnClientError(connection, errorCode);
    }

    public override void OnClientDisconnect(NetworkConnection connection)
    {
        StopClient();

        if (inLobby)
        {
            lobbyNetworkManager.OnClientDisconnect(connection);
        }

        else
        {
            stageNetworkManager.OnClientDisconnect(connection);
        }

        base.OnClientDisconnect(connection);
    }

    public override void OnClientSceneChanged(NetworkConnection connection)
    {
        if (inLobby)
        {
            Debug.Log("is client scene changed being called for the lobby?");

            lobbyNetworkManager.OnClientSceneChanged(connection);
        }

        else
        {
            stageNetworkManager.OnClientSceneChanged(connection);
        }

        base.OnClientSceneChanged(connection);
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        Debug.Log("server has changed the scene!");

        if (inLobby)
        {
            lobbyNetworkManager.OnServerSceneChanged(sceneName);
        }

        else
        {
            stageNetworkManager.OnServerSceneChanged(sceneName);

        }

        base.OnServerSceneChanged(sceneName);
    }

    private void ClientOnServerChangedScene(NetworkConnection connection, ClientServerChangeSceneMessage message)
    {
        Debug.Log("Received ClientServerChangeSceneMessage from the server!");
        NetworkRoom.inLobby = message.newValue;
    }
}


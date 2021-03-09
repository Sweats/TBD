using UnityEngine;
using Mirror;
using kcp2k;
using System;
using System.Collections.Generic;
using System.Collections;

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

public class DarnedNetworkManager : NetworkManager
{
    public static bool IN_LOBBY = true;

    public static int ID = -1; 

    public static List<Player> PLAYERS_IN_SERVER;

    public static bool CLIENT_HOSTING_LOBBY = false;

    public static string HOSTNAME = "localhost";

    public static ushort PORT;

    public static bool DEDICATED_SERVER_HOSTING_LOBBY = false;

    [SerializeField]
    private LobbyNetworkManager lobbyNetworkManager;

    [SerializeField]
    private StageNetworkManager stageNetworkManager;

    public override void OnStartServer()
    {
        SetPort(PORT);
        PLAYERS_IN_SERVER = new List<Player>();
        EventManager.serverLeftGameEvent.AddListener(OnServerStopped);
        NetworkServer.RegisterHandler<ServerPlayerJoinedMessage>(ServerPlayerJoined);
        NetworkServer.RegisterHandler<LobbyServerClientRequestedToStartGameMessage>(ServerOnClientRequestedToStartGame);

        lobbyNetworkManager.RegisterServerHandlers();
        stageNetworkManager.RegisterServerHandlers();

        if (IN_LOBBY)
        {
            lobbyNetworkManager.OnStartServer();
        }

        else
        {
            stageNetworkManager.OnStartServer();
        }

    }

    public static void Allocate()
    {
        GameObject darnedNetworkManagerObject = (GameObject)Resources.Load("Darned Network Manager");
        GameObject spawnedObject = Instantiate(darnedNetworkManagerObject);
    }

    public static void SetPort(ushort port)
    {
        KcpTransport transport = (KcpTransport)Transport.activeTransport;
        transport.Port = port;
    }

    // NOTE: Called when the client that is acting as the server hits the exit button or back to title screen buttons.

    private void OnServerStopped()
    {
        StopHost();
    }

    public override void OnStopServer()
    {
        if (IN_LOBBY)
        {
            lobbyNetworkManager.OnStopServer();
        }

        else
        {
            stageNetworkManager.OnStopServer();
        }

    }

    public override void OnApplicationQuit()
    {
        if (CLIENT_HOSTING_LOBBY)
        {
            StopHost();
        }

        else if (DEDICATED_SERVER_HOSTING_LOBBY)
        {
            StopServer();
        }

        base.OnApplicationQuit();
        // TODO: Make it so when the server terminates, we tell the master server that the server no longer exists.
    }

    private IEnumerator UpdateMasterServer()
    {
        Destroy(NetworkManager.singleton.gameObject);
        yield return null;
        GameObject loadedObject = (GameObject)Resources.Load("Darned Master Server Manager");
        GameObject spawnedObject = Instantiate(loadedObject);
        string address = "localhost:7777";
        Uri uri = new Uri(address);
        DarnedMasterServerManager.EXITING = true;
        NetworkManager.singleton.StartClient(uri);
    }

    private void ServerOnClientRequestedToStartGame(NetworkConnection connection, LobbyServerClientRequestedToStartGameMessage message)
    {
        uint id = connection.identity.netId;

        if (!lobbyNetworkManager.IsHost(id))
        {
            return;
        }

        IN_LOBBY = false;
        NetworkServer.SendToAll(new ClientServerChangeSceneMessage { newValue = DarnedNetworkManager.IN_LOBBY });
        string sceneName = message.newSceneName;
        Log($"[Darned]: Server is switching to the new scene named {sceneName}");
        ServerChangeScene(sceneName);

    }

    public override void OnServerAddPlayer(NetworkConnection connection)
    {
        base.OnServerAddPlayer(connection);

    }

    public override void OnServerConnect(NetworkConnection connection)
    {
        Log("Server connected player!");

        if (IN_LOBBY)
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
        if (IN_LOBBY)
        {
            lobbyNetworkManager.OnServerDisconnect(connection);
        }

        else
        {
            stageNetworkManager.OnServerDisconnect(connection);

            if (!IN_LOBBY && PLAYERS_IN_SERVER.Count == 0)
            {
                Log("There are no more players left in the server. Going back to the lobby...");
                string lobbySceneName = Stages.Name(StageName.Lobby);
                IN_LOBBY = true;
                ServerChangeScene(lobbySceneName);

            }
        }

        base.OnServerDisconnect(connection);
    }

    private void ServerPlayerJoined(NetworkConnection connection, ServerPlayerJoinedMessage message)
    {
        if (IN_LOBBY)
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

        if (IN_LOBBY)
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
        Log("Something went wrong with the server!");
        base.OnClientError(connection, errorCode);
    }

    public override void OnClientDisconnect(NetworkConnection connection)
    {
        StopClient();

        if (IN_LOBBY)
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
        if (IN_LOBBY)
        {
            Log("is client scene changed being called for the lobby?");

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
        Log("Server has changed the scene!");

        if (IN_LOBBY)
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
        Log("Received ClientServerChangeSceneMessage from the server!");
        DarnedNetworkManager.IN_LOBBY = message.newValue;
    }

    public static void Log(string text)
    {
        string newText = $"[Darned]: {text}";
        Debug.Log(newText);
    }
}


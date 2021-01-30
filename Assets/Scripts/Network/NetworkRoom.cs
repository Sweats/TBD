using UnityEngine;
using Mirror;
using System.Collections.Generic;



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

    public Player(string name, Character character, NetworkIdentity identity)
    {
        this.playerName = name;
        this.character = character;
        this.identity = identity;
    }
}

public class NetworkRoom : NetworkManager
{
    private bool inLobby = true;

    public static List<Player> players;

    [SerializeField]
    private LobbyNetworkManager lobbyNetworkManager;

    [SerializeField]
    private StageNetworkManager stageNetworkManager;

    public override void OnStartServer()
    {
        players = new List<Player>();

        NetworkServer.RegisterHandler<ServerPlayerJoinedMessage>(ServerPlayerJoined);

        lobbyNetworkManager.RegisterServerHandlers();
        stageNetworkManager.RegisterServerHandlers();

        EventManager.lobbyServerStartedGameEvent.AddListener(OnServerStartedGame);

        if (inLobby)
        {
            lobbyNetworkManager.OnStartServer();
        }

        else
        {
            stageNetworkManager.OnStartServer();
        }

    }

    public override void OnServerAddPlayer(NetworkConnection connection)
    {

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
        }
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

    private void OnServerStartedGame(string sceneName)
    {
        inLobby = false;
        ServerChangeScene(sceneName);
    }

    public override void OnStartClient()
    {
        lobbyNetworkManager.RegisterClientHandlers();
        stageNetworkManager.RegisterClientHandlers();

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

    public override void OnClientSceneChanged(NetworkConnection connection)
    {
        if (inLobby)
        {
            lobbyNetworkManager.OnClientSceneChanged(connection);
        }

        else
        {
            stageNetworkManager.OnClientSceneChanged(connection);
        }
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
    }
}


using Mirror;
using UnityEngine;
using System.Collections.Generic;

public struct Lobby
{
    public bool dedicated;
    public string name;
    public byte players;
    public byte inLobby;
    public bool isPrivate;
    public int ping;
    public int id;
    public string hostname;
    public ushort port;

    public string password;
    public StageName stage;
}

public class DarnedMasterServerManager : NetworkManager
{
    public static ushort PORT;
    public List<Lobby> lobbies;

    private MasterServer masterServer;

    private MasterClient masterClient;

    [Server]
    public override void OnStartServer()
    {
        base.OnStartServer();
        masterServer.RegisterNetworkHandlers();
        Log("Master server has started successfully!");
    }

    private void PingServers()
    {
        for (var i = 0; i < lobbies.Count; i++)
        {

        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        masterClient.RegisterNetworkHandlers();
    }

    public static void Log(string text)
    {
        Debug.Log($"[Darned]: {text}");
    }
}

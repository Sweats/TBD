using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TestNetworkManager : NetworkManager
{
    [SerializeField]
    private ServerDoor serverDoor;

    [SerializeField]
    private ServerTrap serverTrap;

    [SerializeField]
    private ServerKey serverKey;

    [SerializeField]
    private ServerInsanity serverInsanity;

    [SerializeField]
    private ServerLobby serverLobby;

    [SerializeField]
    private ServerStage serverStage;

    [SerializeField]
    private ServerEscape serverEscape;


    [SerializeField]
    private ServerBattery serverBattery;

    [SerializeField]
    private ServerFlashlight serverFlashlight;

    [SerializeField]
    private ServerLurker serverLurker;

    [SerializeField]
    private ClientLobby clientLobby;

    [SerializeField]
    private ClientBattery clientBattery;

    [SerializeField]
    private ClientDoor clientDoor;

    [SerializeField]
    private ClientKey clientKey;

    [SerializeField]
    private ClientStage clientStage;

    [SerializeField]
    private ClientEscape clientEscape;

    [SerializeField]
    private ClientLurker clientLurker;

    [SerializeField]
    private ClientTrap clientTrap;


    [SerializeField]
    private bool insanityEnabled;

    [SerializeField]
    private float insanityRate;

    public override void OnStartServer()
    {
        base.OnStartServer();
        serverDoor.RegisterNetworkHandlers();
        serverKey.RegisterNetworkHandlers();
        serverEscape.RegisterNetworkHandlers();
        serverInsanity.RegisterNetworkHandlers();
        serverLobby.RegisterNetworkHandlers();
        serverStage.RegisterNetworkHandlers();
        serverBattery.RegisterNetworkHandlers();
        serverFlashlight.RegisterNetworkHandlers();
        serverLurker.RegisterNetworkHandlers();
        serverTrap.RegisterNetworkHandlers();

        serverStage.OnServerSceneChanged();
        serverKey.OnServerSceneChanged();
        serverInsanity.OnServerSceneChanged(insanityRate, insanityEnabled);
        serverTrap.OnServerSceneChanged(insanityEnabled);
        serverLurker.OnServerSceneChanged();

        
    }

    public override void OnServerConnect(NetworkConnection connection)
    {
        base.OnServerConnect(connection);
        serverStage.OnServerConnect(connection);
        //NOTE: For testing only.
        NetworkClient.Send(new ServerClientGameHostRequestedToStartGameMessage{});
        
    }

    public override void OnServerDisconnect(NetworkConnection connection)
    {
        serverStage.OnServerDisconnect(connection);
        base.OnServerDisconnect(connection);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        clientBattery.RegisterNetworkHandlers();
        clientDoor.RegisterNetworkHandlers();
        clientKey.RegisterNetworkHandlers();
        clientLobby.RegisterNetworkHandlers();
        clientStage.RegisterNetworkHandlers();
        clientEscape.RegisterNetworkHandlers();
        clientLurker.RegisterNetworkHandlers();
        clientTrap.RegisterNetworkHandlers();


    }
}
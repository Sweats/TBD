using Mirror;
using UnityEngine;

public class DarnedNetworkManager: NetworkManager
{
    [SerializeField]
    private bool insanityOptionEnabled;

    private bool inLobby;

    [SerializeField]
    private int choosenKeyPath;
    private StageName stage;

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
    private ServerBattery serverBattery;

    [SerializeField]
    private ServerLurker serverLurker;

    [SerializeField]
    private ServerPhantom serverPhantom;

    [SerializeField]
    private ServerMary serverMary;

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
    private ClientLurker clientLurker;

    [SerializeField]
    private ClientPhantom clientPhantom;

    [SerializeField]
    private ClientMary clientMary;

    [SerializeField]
    private float survivorInsanityRate;

    private bool justStartedHosting; 

    public override void OnStartServer()
    {
        base.OnStartServer();
        justStartedHosting = true;
        serverDoor.RegisterNetworkHandlers();
        serverKey.RegisterNetworkHandlers();
        serverLobby.RegisterNetworkHandlers();
        serverStage.RegisterNetworkHandlers();
        serverBattery.RegisterNetworkHandlers();
        serverPhantom.RegisterNetworkHandlers();
        serverLurker.RegisterNetworkHandlers();
        serverMary.RegisterNetworkHandlers();
    }

    public override void OnServerConnect(NetworkConnection connection)
    {
        base.OnServerConnect(connection);

        if (inLobby)
        {
            serverLobby.OnServerConnect(connection);
        }

        else
        {
            serverStage.OnServerConnect(connection);
        }
    }

    public override void OnServerSceneChanged(string newSceneName)
    {
        base.OnServerSceneChanged(newSceneName);

        if (justStartedHosting)
        {
            justStartedHosting = false;
            serverLobby.OnServerJustStarted();
            return;
        }

        if (inLobby)
        {
            inLobby = false;
            serverTrap.OnServerSceneChanged(insanityOptionEnabled);
            serverInsanity.OnServerSceneChanged(survivorInsanityRate, insanityOptionEnabled);
            serverStage.OnServerSceneChanged();

        }

        else
        {
            inLobby = true;
            serverLobby.OnServerSceneChanged();
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        clientBattery.RegisterNetworkHandlers();
        clientDoor.RegisterNetworkHandlers();
        clientKey.RegisterNetworkHandlers();
        clientLobby.RegisterNetworkHandlers();
        clientStage.RegisterNetworkHandlers();
        clientMary.RegisterNetworkHandlers();
        clientLurker.RegisterNetworkHandlers();
        clientPhantom.RegisterNetworkHandlers();
    }
}
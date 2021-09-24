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
    private ClientLobby clientLobby;

    [SerializeField]
    private ClientBattery clientBattery;

    [SerializeField]
    private ClientDoor clientDoor;

    [SerializeField]
    private ClientKey clientKey;

    [SerializeField]
    private float survivorInsanityRate;


    public override void OnStartServer()
    {
        base.OnStartServer();
        serverDoor.RegisterNetworkHandlers();
        serverKey.RegisterNetworkHandlers();
        serverLobby.RegisterNetworkHandlers();
        serverStage.RegisterNetworkHandlers();
        serverBattery.RegisterNetworkHandlers();
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

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        clientLobby.RegisterNetworkHandlers();
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);

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


}
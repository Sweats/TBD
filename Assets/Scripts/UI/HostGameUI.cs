using UnityEngine;
using UnityEngine.UI;
using System;
using Mirror;

public class HostGameUI : MonoBehaviour
{

    [SerializeField]
    private InputField lobbyNameField;

    [SerializeField]
    private InputField passwordField;

    [SerializeField]
    private InputField portField;

    [SerializeField]
    private Canvas hostLobbyCanvas;

    [SerializeField]
    private MainMenuUI mainMenuUI;

    [SerializeField]
    private LobbyUI lobbyUI;

    private ushort port;

    private void Start()
    {
        this.enabled = false;
        EventManager.masterClientServerAddedClientHostEvent.AddListener(OnMasterServerAddedClientHost);
    }


    private void Update()
    {
        if (Keybinds.GetKey(Action.GuiReturn))
        {
            mainMenuUI.Show();
            Hide();

        }
    }

    private void Hide()
    {
        this.enabled = false;
        hostLobbyCanvas.enabled = false;

    }

    public void Show()
    {
        this.enabled = true;
        hostLobbyCanvas.enabled = true;
    }

    public void OnHostLobbyBackButtonClicked()
    {
        Hide();
        mainMenuUI.Show();
    }


    public void OnCreateLobbyButtonClicked()
    {
        string lobbyText = lobbyNameField.text;
        string password = passwordField.text;

        if (portField.text == string.Empty)
        {
            HostLobby.LOBBY_PORT = 7777;
        }

        else
        {
            HostLobby.LOBBY_PORT = port;
        }

        string lobbySceneName = Stages.Name(StageName.Lobby);
        HostLobby.LOBBY_NAME = lobbyText;
        HostLobby.LOBBY_PASSWORD = password;

        bool noMasterServer = true;

        if (noMasterServer)
        {
            CreateDarnedServer();

        }

        else
        {
            CreateDarnedServerAndAddToMasterServerList();
        }
    }

    private void CreateDarnedServerAndAddToMasterServerList()
    {
        GameObject masterServerManager = (GameObject)Resources.Load("Darned Master Server Manager");
        GameObject spawnedObject = Instantiate(masterServerManager);
        string address = "localhost:7777";
        Uri uri = new Uri(address);
        NetworkManager.singleton.StartClient();
        string name = HostLobby.LOBBY_NAME;
        string lobbyPassword = HostLobby.LOBBY_PASSWORD;
        NetworkClient.Send(new MasterServerClientHostRequestToBeAddedMessage{lobbyName = name, password = lobbyPassword});

    }

    private void CreateDarnedServer()
    {
        GameObject darnedNetworkManager = (GameObject)Resources.Load("Darned Network Manager");
        GameObject spawnedObject = Instantiate(darnedNetworkManager);
        string address = "localhost:7777";
        Uri uri = new Uri(address);
        NetworkManager.singleton.StartHost();
        string sceneToLoad = Stages.Name(StageName.Lobby);
        NetworkManager.singleton.ServerChangeScene(sceneToLoad);
    }

    private void OnMasterServerAddedClientHost(int id)
    {
        NetworkManager.singleton.StopClient();
        HostLobby.LOBBY_ID = id;
        Destroy(NetworkManager.singleton.gameObject);
        CreateDarnedServer();
    }


    public void OnPortFieldChanged(string text)
    {
        port = Convert.ToUInt16(text);
    }
}

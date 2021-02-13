using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Mirror;

public class JoinGameUI : MonoBehaviour
{
    [SerializeField]
    private Canvas joinGameCanvas;

    [SerializeField]
    private Button refreshAllButton;

    [SerializeField]
    private Button connectButton;

    [SerializeField]
    private Button backButton;

    [SerializeField]
    private Button directConnectButton;

    [SerializeField]
    private MainMenuUI mainMenuUI;

    [SerializeField]
    private DirectConnectUI directConnectUI;

    [SerializeField]
    private LobbyUI lobbyUI;

    [SerializeField]
    private Lobby[] lobbies;

    private void Update()
    {
        if (Keybinds.GetKey(Action.GuiReturn))
        {
            Hide();
            mainMenuUI.Show();
        }
    }

    private void Start()
    {
        this.enabled = false;
        EventManager.masterServerSentLobbyListEvent.AddListener(OnGetServers);

    }

    private void CreateMasterServerManager()
    {
        DarnedNetworkManager.CLIENT_HOSTING_LOBBY = false;
        GameObject dedicatedServerObject = (GameObject)Resources.Load("Darned Master Server Manager");
        GameObject spawnedGameobject = Instantiate(dedicatedServerObject);
        NetworkManager.singleton.StartClient();
    }

    private void OnGetServers(Lobby[] lobbies)
    {
        this.lobbies = lobbies;

        for (var i = 0; i < lobbies.Length; i++)
        {
            Lobby lobby = lobbies[i];
            string lobbyName = lobby.lobbyName;
            string hostName = lobby.hostname;
            byte players = lobby.players;
            bool isPrivate = lobby.privateLobby;
            int id = lobby.id;
            Debug.Log($"Lobby ID = {id}\nLobby Name = {lobbyName}\nIP = {hostName}\n Player Count = {players}\n isPrivate = {isPrivate}");
            Debug.Log("---------------------\n\n");
        }

    }

    public void Show()
    {
        CreateMasterServerManager();
        this.enabled = true;
        joinGameCanvas.enabled = true;

    }

    private void Hide()
    {
        NetworkManager.singleton.StopClient();
        Destroy(NetworkManager.singleton.gameObject);
        this.enabled = false;
        joinGameCanvas.enabled = false;
    }

    public void OnRefreshButtonClicked()
    {
        Debug.Log("Refresh all button clicked!");
        //lobbies = GetLobbies();

    }
    public void OnJoinGameBackButtonClicked()
    {
        Hide();
        mainMenuUI.Show();
    }

    public void OnJoinGameDirectConnectButtonClicked()
    {
        Hide();
        directConnectUI.Show();
    }

    public void OnConnectButtonClicked()
    {
        Debug.Log("Connect button clicked!");
        //lobbyUI.Show(false);
        Hide();

    }


    private List<Lobby> GetLobbies()
    {
        //TODO: Connect to the server and get the list of lobbies here and return it.
        return null;

    }

    private void UpdateLobbiesView()
    {
        for (var i = 0; i < lobbies.Length; i++)
        {

        }
    }
}

using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using Mirror;

public struct LobbyButton
{
    public Lobby lobby;
    public GameObject IdButton;
    public GameObject lobbyNameButton;
    public GameObject playerCountButton;
    public GameObject privateButton;
    public GameObject inLobbyButton;
    public GameObject stageButton;

    public LobbyButton(Lobby lobby, GameObject IdButton, GameObject lobbyNameButton, GameObject playerCountButton, GameObject privateButton, GameObject inLobbyButton, GameObject stageButton)
    {
        this.lobby = lobby;
        this.IdButton = IdButton;
        this.lobbyNameButton = lobbyNameButton;
        this.playerCountButton = playerCountButton;
        this.privateButton = privateButton;
        this.inLobbyButton = inLobbyButton;
        this.stageButton = stageButton;
    }
}

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
    private Lobby[] lobbies;

    [SerializeField]
    private GameObject buttonPrefab;

    [SerializeField]
    private GameObject buttonTemplate;

    [SerializeField]
    private UnityEngine.UIElements.ScrollView scrollView;

    private List<LobbyButton> lobbyButtonList;

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
        lobbyButtonList = new List<LobbyButton>();
        this.enabled = false;
        EventManager.masterServerClientSentUsLobbyListEvent.AddListener(OnGetServers);

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
        DestroyLobbyList();

        for (var i = 0; i < lobbies.Length; i++)
        {
            Lobby lobby = lobbies[i];
            string lobbyName = lobby.name;
            string hostName = lobby.hostname;
            byte players = lobby.players;
            bool isPrivate = lobby.isPrivate;
            int id = lobby.id;
            StageName stage = lobby.stage;
            AddLobbyToList(lobby);
        }

    }

    private void DestroyButtons(LobbyButton lobbyButton)
    {
        Destroy(lobbyButton.IdButton.gameObject);
        Destroy(lobbyButton.lobbyNameButton.gameObject);
        Destroy(lobbyButton.playerCountButton.gameObject);
        Destroy(lobbyButton.privateButton.gameObject);
        Destroy(lobbyButton.inLobbyButton.gameObject);
        Destroy(lobbyButton.stageButton.gameObject);

    }

    private void AddLobbyToList(Lobby lobby)
    {
        GameObject idButton = Instantiate(buttonPrefab);
        idButton.transform.SetParent(buttonTemplate.transform.parent);
        GameObject lobbyNameButton = Instantiate(buttonPrefab);
        lobbyNameButton.transform.SetParent(buttonTemplate.transform.parent);
        GameObject playerCountButton  = Instantiate(buttonPrefab);
        playerCountButton.transform.SetParent(buttonTemplate.transform.parent);
        GameObject privateButton = Instantiate(buttonPrefab);
        privateButton.transform.SetParent(buttonTemplate.transform.parent);
        GameObject inLobbyButton = Instantiate(buttonPrefab);
        inLobbyButton.transform.SetParent(buttonTemplate.transform.parent);
        GameObject stageButton = Instantiate(buttonPrefab);
        stageButton.transform.SetParent(buttonTemplate.transform.parent);

        idButton.GetComponentInChildren<Text>().text = $"{lobby.id}";
        lobbyNameButton.GetComponentInChildren<Text>().text = lobby.name;
        playerCountButton.GetComponentInChildren<Text>().text = $"{lobby.players}/5";
        privateButton.GetComponentInChildren<Text>().text = $"{lobby.isPrivate}";
        inLobbyButton.GetComponentInChildren<Text>().text = $"{lobby.inLobby}";
        stageButton.GetComponentInChildren<Text>().text = Stages.Name(lobby.stage);

        LobbyButton lobbyButton = new LobbyButton(lobby, idButton, lobbyNameButton, playerCountButton, privateButton, inLobbyButton, stageButton);
        lobbyButtonList.Add(lobbyButton);

    }

    public void Show()
    {
        CreateMasterServerManager();
        this.enabled = true;
        joinGameCanvas.enabled = true;
    }

    private void DestroyLobbyList()
    {
        for (var i = 0; i < lobbyButtonList.Count; i++)
        {
            DestroyButtons(lobbyButtonList[i]);
        }

        lobbyButtonList.Clear();
    }

    private void Hide()
    {
        DestroyLobbyList();

        if (NetworkManager.singleton != null)
        {
            NetworkManager.singleton.StopClient();
            Destroy(NetworkManager.singleton.gameObject);
        }

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
        Hide();

    }

    private void UpdateLobbiesView()
    {
        for (var i = 0; i < lobbies.Length; i++)
        {

        }
    }
}

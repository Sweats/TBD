﻿using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;
using kcp2k;
using System;
using System.Text;
using System.Collections;

public enum Character : byte
{
    Random = 0,
    Chad,
    Alice,
    Jesus,
    Jamal,
    Lurker,
    Phantom,
    Mary,
    Fallen,
    Spectator,
    Empty
}

//public enum Option: byte
//{
//    Insanity = 0,
//    AllRandom,
//    AllowSpectator,
//    Gamemode,
//    Stage,
//}


public class LobbyUI : MonoBehaviour
{
    [SerializeField]
    private Texture aliceIcon;

    [SerializeField]
    private Texture chadIcon;

    [SerializeField]
    private Texture jesusIcon;

    [SerializeField]
    private Texture jamalIcon;

    [SerializeField]
    private Texture randomCharacterIcon;

    [SerializeField]
    private Texture emptyLobbySlotIcon;

    [SerializeField]
    private Texture lurkerIcon;

    [SerializeField]
    private Texture phantomIcon;

    [SerializeField]
    private Texture maryIcon;

    [SerializeField]
    private Texture fallenIcon;

    [SerializeField]
    private Texture hoveredOverCharacterTexture;

    [SerializeField]
    private Canvas lobbyCanvas;

    //    [SerializeField]
    //    private JoinGameUI joinGameUI;
    //
    //    [SerializeField]
    //    private HostGameUI hostGameUI;
    //
    [SerializeField]
    private Color buttonTextColor;

    [SerializeField]
    private Button chooseCharacterButton;

    [SerializeField]
    private Button startGameButton;

    [SerializeField]
    private Button leaveLobbyButton;

    [SerializeField]
    private Button chadButton;

    [SerializeField]
    private Button aliceButton;

    [SerializeField]
    private Button jesusButton;

    [SerializeField]
    private Button jamalButton;

    [SerializeField]
    private Button lurkerButton;

    [SerializeField]
    private Button phantomButton;

    [SerializeField]
    private Button maryButton;

    [SerializeField]
    private Button fallenButton;

    [SerializeField]
    private Button randomCharacterButton;

    [SerializeField]
    private Button kickPlayerOneButton;

    [SerializeField]
    private Button kickPlayerTwoButton;

    [SerializeField]
    private Button kickPlayerThreeButton;

    [SerializeField]
    private Button kickPlayerFourButton;

    [SerializeField]
    private Button kickPlayerFiveButton;

    [SerializeField]
    private Text playerOneNameText;

    [SerializeField]
    private Text playerTwoNameText;

    [SerializeField]
    private Text playerThreeNameText;

    [SerializeField]
    private Text playerFourNameText;

    [SerializeField]
    private Text playerFiveNameText;

    [SerializeField]
    private Toggle insanityToggle;

    [SerializeField]
    private Toggle allRandomToggle;

    [SerializeField]
    private Toggle allowSpectatorToggle;

    [SerializeField]
    private RawImage playerOneLobbyIcon;

    [SerializeField]
    private RawImage playerTwoLobbyIcon;

    [SerializeField]
    private RawImage playerThreeLobbyIcon;

    [SerializeField]
    private RawImage playerFourLobbyIcon;

    [SerializeField]
    private RawImage playerFiveLobbyIcon;

    [SerializeField]
    private Image selectCharacterPanel;

    [SerializeField]
    private Text selectCharacterText;

    [SerializeField]
    [Tooltip("This list must match the the stage listing in the file Stages.cs")]
    private Dropdown stageDropdown;

    [SerializeField]
    private Dropdown gameModeDropdown;

    [SerializeField]
    private InputField chatMessageBoxInput;

    [SerializeField]
    private TMP_InputField chatMessageBox;


    private const int STAGE_TEMPLATE = 0;
    private const int STAGE_TEMPLATE_MARY = 1;
    private const int STAGE_TEMPLATE_FALLEN = 2;
    private const int STAGE_TEMPLATE_LURKER = 3;
    private const int STAGE_TEMPLATE_PHANTOM = 4;

    private const int PLAYER_ONE = 0;
    private const int PLAYER_TWO = 1;
    private const int PLAYER_THREE = 2;
    private const int PLAYER_FOUR = 3;
    private const int PLAYER_FIVE = 4;

    private bool selectingCharacter;

    private const int NORMAL_MODE = 0;
    private const int LIFE_MODE = 1;

    private bool insanityEnabled;
    private StageName selectedStage;

    private int playerOneCharacter;
    private int playerTwoCharacter;
    private int playerThreeCharacter;
    private int playerFourCharacter;
    private int playerFiveCharacter;

    private StringBuilder chatStringBuilder;

    private bool hostingLobby;

    private bool hostingOnDedicatedServer;

    private void Start()
    {
        if (DarnedNetworkManager.DEDICATED_SERVER_HOSTING_LOBBY && NetworkManager.singleton == null)
        {
            StartDedicatedServer();
            return;
        }

        else if (DarnedNetworkManager.CLIENT_HOSTING_LOBBY && NetworkManager.singleton == null)
        {
            StartHost();
            this.hostingLobby = true;
            this.hostingOnDedicatedServer = false;
            SetUpHostLobbyControls();
        }

        else if (!DarnedNetworkManager.CLIENT_HOSTING_LOBBY && !DarnedNetworkManager.DEDICATED_SERVER_HOSTING_LOBBY && NetworkManager.singleton == null)
        {
            CreateNetworkManager();
            this.hostingLobby = false;
            this.hostingOnDedicatedServer = false;
            SetUpClientLobbyControls();
            string address = $"{DarnedNetworkManager.HOSTNAME}:{DarnedNetworkManager.PORT}";
            Uri uri = new Uri(address);
            NetworkManager.singleton.StartClient(uri);

        }

        this.enabled = true;
        hostingLobby = DarnedNetworkManager.CLIENT_HOSTING_LOBBY;
        chatStringBuilder = new StringBuilder();
        EventManager.lobbyClientHostChangedAllowSpectatorEvent.AddListener(OnAllowSpectatorOptionUpdated);
        EventManager.lobbyClientHostChangedStageEvent.AddListener(OnStageSelectionUpdated);
        EventManager.lobbyClientHostChangedGamemodeEvent.AddListener(OnGameModeSelectionUpdated);
        EventManager.lobbyClientHostChangedAllRandomEvent.AddListener(OnAllRandomOptionUpdated);
        EventManager.lobbyClientHostChangedInsanityOptionEvent.AddListener(OnInsanityOptionUpdated);
        EventManager.lobbyClientPlayerJoinedEvent.AddListener(OnPlayerJoinedLobby);
        EventManager.lobbyClientPlayerLeftEvent.AddListener(OnPlayerLeftLobby);
        EventManager.lobbyClientPlayerChangedCharacterEvent.AddListener(OnPlayerSlotUpdated);
        EventManager.lobbyYouHaveBeenKickedEvent.AddListener(OnKickedFromLobby);
        EventManager.lobbyClientPlayerSentChatMessageEvent.AddListener(OnPlayerRecievedChatMessage);
        EventManager.lobbyClientServerAssignedYouHostEvent.AddListener(OnServerAssignedYouHost);
        EventManager.lobbyClientServerPickedNewHostEvent.AddListener(OnServerAssignedPlayerHost);

    }

    private void StartDedicatedServer()
    {
        this.hostingLobby = false;
        this.hostingOnDedicatedServer = true;
        EventManager.dedicatedServerReceivedIdEvent.AddListener(OnDedicatedServerRecievedId);
        string configName = "darned_lobby_server_configuration.yaml";

        if (!DedicatedServerConfiguration.Exists(configName))
        {
            DarnedNetworkManager.Log("Exiting...");
            Application.Quit();
            return;
        }

        DedicatedServerConfiguration serverConfiguration = DedicatedServerConfiguration.Load(configName);
        string serverName = serverConfiguration.Name();
        HostLobby.LOBBY_NAME = serverName;
        ushort port = serverConfiguration.Port();
        HostLobby.LOBBY_PORT = port;
        string password = serverConfiguration.Password();
        HostLobby.LOBBY_PASSWORD = password;
        bool isPrivate = password != string.Empty;
        DarnedNetworkManager.PORT = port;
        DarnedNetworkManager.Log($"Loaded server configuration:\n\nServer Name = {serverName}\nPort = {port}\nPrivate Lobby = {isPrivate}");
        GameObject loadedObject = (GameObject)Resources.Load("Darned Master Server Manager");
        GameObject spawnedObject = Instantiate(loadedObject);
        string address = "localhost:7777";
        Uri uri = new Uri(address);
        NetworkManager.singleton.StartClient(uri);
    }

    private void StartHost()
    {
        EventManager.dedicatedServerReceivedIdEvent.AddListener(OnDedicatedServerRecievedId);
        GameObject loadedObject = (GameObject)Resources.Load("Darned Master Server Manager");
        GameObject spawnedObject = Instantiate(loadedObject);
        string address = "localhost:7777";
        Uri uri = new Uri(address);
        NetworkManager.singleton.StartClient(uri);
    }

    //NOTE: Called when the master server sends us back the ID it generated.
    private void OnDedicatedServerRecievedId(int id)
    {
        StartCoroutine(StartServer(id));
    }

    private IEnumerator StartServer(int id)
    {
        DarnedNetworkManager.ID = id;
        NetworkManager.singleton.StopClient();
        Destroy(NetworkManager.singleton.gameObject);
        yield return null;
        DarnedNetworkManager.Log("The master server has added us to the list! Starting the dedicated server...");
        CreateNetworkManager();
        DarnedNetworkManager.PORT = HostLobby.LOBBY_PORT;

        if (DarnedNetworkManager.DEDICATED_SERVER_HOSTING_LOBBY)
        {
            NetworkManager.singleton.StartServer();
        }

        else if (DarnedNetworkManager.CLIENT_HOSTING_LOBBY)
        {
            NetworkManager.singleton.StartHost();
        }

        DarnedNetworkManager.Log("Successfully started the server!");

    }

    private void CreateNetworkManager()
    {
        GameObject dedicatedServerObject = (GameObject)Resources.Load("Darned Network Manager");
        GameObject spawnedGameobject = Instantiate(dedicatedServerObject);
        DontDestroyOnLoad(spawnedGameobject);
    }

    private void DisableHostControls()
    {
        startGameButton.interactable = false;
        startGameButton.enabled = false;
        gameModeDropdown.interactable = false;
        stageDropdown.interactable = false;
        insanityToggle.interactable = false;
        allRandomToggle.interactable = false;
        allowSpectatorToggle.interactable = false;

        gameModeDropdown.GetComponent<Image>().enabled = false;
        gameModeDropdown.GetComponentInChildren<Text>().color = Color.white;
        stageDropdown.GetComponent<Image>().enabled = false;
        stageDropdown.GetComponentInChildren<Text>().color = Color.white;

    }

    public void OnPointerEnterButton(Button button)
    {
        if (!button.enabled || !button.interactable)
        {
            return;
        }

        button.GetComponentInChildren<Text>().color = buttonTextColor;
    }

    public void OnPointerExitButton(Button button)
    {
        if (!button.enabled || !button.interactable)
        {
            return;
        }

        button.GetComponentInChildren<Text>().color = Color.white;
    }

    // NOTE: If the user gets here then they must be a client and the server.
    public void OnKickedButtonClicked(int playerNumber)
    {
        EventManager.lobbyServerKickedEvent.Invoke(playerNumber);
    }

    private void SetUpClientLobbyControls()
    {
        startGameButton.interactable = false;
        startGameButton.enabled = false;
        gameModeDropdown.interactable = false;
        stageDropdown.interactable = false;
        insanityToggle.interactable = false;
        allRandomToggle.interactable = false;
        allowSpectatorToggle.interactable = false;

        kickPlayerOneButton.interactable = false;
        kickPlayerOneButton.GetComponentInChildren<Text>().color = Color.clear;
        kickPlayerTwoButton.interactable = false;
        kickPlayerTwoButton.GetComponentInChildren<Text>().color = Color.clear;
        kickPlayerThreeButton.interactable = false;
        kickPlayerThreeButton.GetComponentInChildren<Text>().color = Color.clear;
        kickPlayerFourButton.interactable = false;
        kickPlayerFourButton.GetComponentInChildren<Text>().color = Color.clear;
        kickPlayerFiveButton.interactable = false;
        kickPlayerFiveButton.GetComponentInChildren<Text>().color = Color.clear;

        playerOneNameText.text = string.Empty;
        playerTwoNameText.text = string.Empty;
        playerThreeNameText.text = string.Empty;
        playerFourNameText.text = string.Empty;
        playerFiveNameText.text = string.Empty;

        gameModeDropdown.GetComponent<Image>().enabled = false;
        gameModeDropdown.GetComponentInChildren<Text>().color = Color.white;
        stageDropdown.GetComponent<Image>().enabled = false;
        stageDropdown.GetComponentInChildren<Text>().color = Color.white;

    }

    // NOTE: Called when a client starts a lobby for the first time. Ignored for dedicated servers.
    private void SetUpHostLobbyControls()
    {
        startGameButton.interactable = true;
        startGameButton.enabled = true;
        gameModeDropdown.interactable = true;
        stageDropdown.interactable = true;
        insanityToggle.interactable = true;
        allRandomToggle.interactable = true;
        allowSpectatorToggle.interactable = true;

        kickPlayerOneButton.interactable = false;
        kickPlayerOneButton.GetComponentInChildren<Text>().color = Color.clear;
        kickPlayerTwoButton.interactable = false;
        kickPlayerTwoButton.GetComponentInChildren<Text>().color = Color.clear;
        kickPlayerThreeButton.interactable = false;
        kickPlayerThreeButton.GetComponentInChildren<Text>().color = Color.clear;
        kickPlayerFourButton.interactable = false;
        kickPlayerFourButton.GetComponentInChildren<Text>().color = Color.clear;
        kickPlayerFiveButton.interactable = false;
        kickPlayerFiveButton.GetComponentInChildren<Text>().color = Color.clear;


        playerOneNameText.text = Settings.PROFILE_NAME;
        playerTwoNameText.text = string.Empty;
        playerThreeNameText.text = string.Empty;
        playerFourNameText.text = string.Empty;
        playerFiveNameText.text = string.Empty;

        gameModeDropdown.GetComponent<Image>().enabled = true;
        gameModeDropdown.GetComponentInChildren<Text>().color = Color.gray;
        stageDropdown.GetComponent<Image>().enabled = true;
        stageDropdown.GetComponentInChildren<Text>().color = Color.gray;
    }

    // NOTE: Called when the server assigns the client as the new host of the lobby.
    // We don't need to modify anything else in here because the UI should be up to date with what the server has in memory.
    private void SetUpNewHostControls(int index)
    {
        startGameButton.interactable = true;
        startGameButton.enabled = true;
        gameModeDropdown.interactable = true;
        stageDropdown.interactable = true;
        insanityToggle.interactable = true;
        allRandomToggle.interactable = true;
        allowSpectatorToggle.interactable = true;

        gameModeDropdown.GetComponent<Image>().enabled = true;
        gameModeDropdown.GetComponentInChildren<Text>().color = Color.gray;
        stageDropdown.GetComponent<Image>().enabled = true;
        stageDropdown.GetComponentInChildren<Text>().color = Color.gray;

        // NOTE: Now we enable the kick buttons. Easy way to do this is to check for the player strings that have already been set.

        if (playerOneNameText.text != string.Empty)
        {
            kickPlayerOneButton.interactable = true;
            kickPlayerOneButton.GetComponentInChildren<Text>().color = Color.white;
        }

        else if (playerTwoNameText.text != string.Empty)
        {
            kickPlayerTwoButton.interactable = true;
            kickPlayerTwoButton.GetComponentInChildren<Text>().color = Color.white;

        }

        else if (playerThreeNameText.text != string.Empty)
        {
            kickPlayerThreeButton.interactable = true;
            kickPlayerThreeButton.GetComponentInChildren<Text>().color = Color.white;

        }

        else if (playerFourNameText.text != string.Empty)
        {
            kickPlayerFourButton.interactable = true;
            kickPlayerFourButton.GetComponentInChildren<Text>().color = Color.white;

        }

        else if (playerFiveNameText.text != string.Empty)
        {
            kickPlayerFiveButton.interactable = true;
            kickPlayerFiveButton.GetComponentInChildren<Text>().color = Color.white;
        }

        UpdateHostLabel(index);

    }

    private void UpdateHostLabel(int index)
    {
        switch (index)
        {
            case PLAYER_ONE:
                playerOneNameText.text = $"{playerOneNameText.text} (Host)";
                break;
            case PLAYER_TWO:
                playerTwoNameText.text = $"{playerTwoNameText.text} (Host)";
                break;
            case PLAYER_THREE:
                playerThreeNameText.text = $"{playerThreeNameText.text} (Host)";
                break;
            case PLAYER_FOUR:
                playerFourNameText.text = $"{playerFourNameText.text} (Host)";
                break;
            case PLAYER_FIVE:
                playerFiveNameText.text = $"{playerFiveNameText.text} (Host)";
                break;
        }
    }

    private void ExitScene()
    {
        if (hostingLobby)
        {
            NetworkManager.singleton.StopHost();
            DarnedNetworkManager.Log("You are no longer hosting the server!");
        }

        else
        {
            NetworkManager.singleton.StopClient();
        }

        Destroy(NetworkManager.singleton.gameObject);

        Stages.Load(StageName.Menu);
    }

    private void OnPlayerRecievedChatMessage(string text, string clientName)
    {
        string newText = $"{clientName}: {text}";
        chatStringBuilder.AppendLine(newText);
        chatMessageBox.text = chatStringBuilder.ToString();
    }

    #region HOST_OPTIONS

    public void OnInsanityEnabledCheckboxClicked(bool newOption)
    {
        NetworkClient.Send(new LobbyServerClientRequestedChangeInsanityMessage { newValue = newOption });
    }

    public void OnAllowSpectatorCheckboxClicked(bool newOption)
    {
        NetworkClient.Send(new LobbyServerClientRequestedChangeAllowSpectatorMessage { newValue = newOption });
    }


    public void OnAllRandomCheckboxClicked(bool newOption)
    {
        NetworkClient.Send(new LobbyServerClientRequestedChangeAllRandomMessage { newValue = newOption });
    }

    public void OnStageDropdownChanged(int newOption)
    {
        NetworkClient.Send(new LobbyServerClientRequestedChangeStageMessage { newValue = newOption });
    }

    public void OnGameModeDropdownChanged(int newOption)
    {
        NetworkClient.Send(new LobbyServerClientRequestedChangeGamemodeMessage { newValue = newOption });
    }

    public void OnVoiceChatDropdownChanged()
    {

    }

    public void OnStartGameButtonClicked()
    {
        // NOTE: When updating the stage dropdown in the Unity Editor, make sure it matches the enum
        // and the dictionary in the file Stages.cs.
        StageName name = (StageName)stageDropdown.value;
        string sceneName = Stages.Name(name);
        NetworkClient.Send(new LobbyServerClientRequestedToStartGameMessage { newSceneName = sceneName });
    }

    #endregion

    #region CLIENT
    public void OnChooseCharacterButtonClicked()
    {
        //DisableControls();
        EnableSelectCharacterControls();

    }

    //[Client]
    private void Update()
    {
        if (Keybinds.GetKey(Action.GuiReturn))
        {
            if (selectingCharacter)
            {
                DisableSelectCharacterControls();
                //EnableControls();
                return;
            }

            ExitScene();
        }

        else if (Keybinds.GetKey(Action.Enter))
        {
            Debug.Log("Hit the enter button!");

            if (chatMessageBoxInput.text != string.Empty)
            {
                string chatMessageText = chatMessageBoxInput.text;
                NetworkClient.Send(new LobbyServerPlayerChatMessage { clientName = Settings.PROFILE_NAME, text = chatMessageText });
                chatMessageBoxInput.text = string.Empty;
                chatMessageBoxInput.Select();
            }
        }
    }



    public void OnJamalButtonClicked()
    {
        NetworkClient.Send(new LobbyServerClientChangedCharacterMessage { newValue = Character.Jamal });
        DisableSelectCharacterControls();
        //EnableControls();
    }


    public void OnAliceButtonClicked()
    {
        NetworkClient.Send(new LobbyServerClientChangedCharacterMessage { newValue = Character.Alice });
        DisableSelectCharacterControls();
        //EnableControls();
    }

    public void OnChadButtonClicked()
    {
        NetworkClient.Send(new LobbyServerClientChangedCharacterMessage { newValue = Character.Chad });
        DisableSelectCharacterControls();
        //EnableControls();
    }

    public void OnJesusButtonClicked()
    {
        NetworkClient.Send(new LobbyServerClientChangedCharacterMessage { newValue = Character.Jesus });
        DisableSelectCharacterControls();
        //EnableControls();
    }

    public void OnLurkerButtonClicked()
    {
        NetworkClient.Send(new LobbyServerClientChangedCharacterMessage { newValue = Character.Lurker });
        DisableSelectCharacterControls();
        //EnableControls();
    }


    public void OnPhantomButtonClicked()
    {
        NetworkClient.Send(new LobbyServerClientChangedCharacterMessage { newValue = Character.Phantom });
        DisableSelectCharacterControls();
        //EnableControls();
    }


    public void OnMaryButtonClicked()
    {
        NetworkClient.Send(new LobbyServerClientChangedCharacterMessage { newValue = Character.Mary });
        DisableSelectCharacterControls();
        //EnableControls();
    }


    public void OnFallenButtonClicked()
    {
        NetworkClient.Send(new LobbyServerClientChangedCharacterMessage { newValue = Character.Fallen });
        DisableSelectCharacterControls();
        //EnableControls();
    }

    public void OnRandomCharacterButtonClicked()
    {
        NetworkClient.Send(new LobbyServerClientChangedCharacterMessage { newValue = Character.Random });
        DisableSelectCharacterControls();
        //EnableControls();
    }


    public void OnLeaveLobbyButtonClicked()
    {
        ExitScene();
    }

    private void Reset()
    {
        playerOneLobbyIcon.texture = emptyLobbySlotIcon;
        playerTwoLobbyIcon.texture = emptyLobbySlotIcon;
        playerThreeLobbyIcon.texture = emptyLobbySlotIcon;
        playerFourLobbyIcon.texture = emptyLobbySlotIcon;
        startGameButton.interactable = true;
        stageDropdown.interactable = true;
        gameModeDropdown.interactable = true;
        allRandomToggle.interactable = true;
        allowSpectatorToggle.interactable = true;
        insanityToggle.interactable = true;
        DisableSelectCharacterControls();
    }

    private void EnableSelectCharacterControls()
    {
        selectingCharacter = true;
        jamalButton.enabled = true;
        jamalButton.image.enabled = true;
        aliceButton.enabled = true;
        aliceButton.image.enabled = true;
        chadButton.enabled = true;
        chadButton.image.enabled = true;
        jesusButton.enabled = true;
        jesusButton.image.enabled = true;
        randomCharacterButton.enabled = true;
        randomCharacterButton.image.enabled = true;
        lurkerButton.enabled = true;
        lurkerButton.image.enabled = true;
        phantomButton.enabled = true;
        phantomButton.image.enabled = true;
        maryButton.enabled = true;
        maryButton.image.enabled = true;
        fallenButton.enabled = true;
        fallenButton.image.enabled = true;
        selectCharacterPanel.enabled = true;
        selectCharacterText.enabled = true;

    }

    private void DisableSelectCharacterControls()
    {
        selectingCharacter = false;
        jamalButton.enabled = false;
        jamalButton.image.enabled = false;
        aliceButton.enabled = false;
        aliceButton.image.enabled = false;
        chadButton.enabled = false;
        chadButton.image.enabled = false;
        jesusButton.enabled = false;
        jesusButton.image.enabled = false;
        randomCharacterButton.enabled = false;
        randomCharacterButton.image.enabled = false;
        lurkerButton.enabled = false;
        lurkerButton.image.enabled = false;
        phantomButton.enabled = false;
        phantomButton.image.enabled = false;
        maryButton.enabled = false;
        maryButton.image.enabled = false;
        fallenButton.enabled = false;
        fallenButton.image.enabled = false;
        selectCharacterPanel.enabled = false;
        selectCharacterText.enabled = false;

    }

    private Texture GetCharacterTexture(Character character)
    {
        switch (character)
        {
            case Character.Jamal:
                return jamalIcon;
            case Character.Alice:
                return aliceIcon;
            case Character.Chad:
                return chadIcon;
            case Character.Jesus:
                return jesusIcon;
            case Character.Lurker:
                return lurkerIcon;
            case Character.Phantom:
                return phantomIcon;
            case Character.Mary:
                return maryIcon;
            case Character.Fallen:
                return fallenIcon;
            case Character.Random:
                return randomCharacterIcon;
            case Character.Empty:
                return emptyLobbySlotIcon;
            default:
                return emptyLobbySlotIcon;
        }
    }


    private void OnPlayerSlotUpdated(Character character, int index)
    {
        Texture characterTexture = GetCharacterTexture(character);
        Debug.Log($"Player changed character! Index = {index}: New character is {character}");

        switch (index)
        {
            case PLAYER_ONE:
                playerOneLobbyIcon.texture = characterTexture;
                break;

            case PLAYER_TWO:
                playerTwoLobbyIcon.texture = characterTexture;
                break;

            case PLAYER_THREE:
                playerThreeLobbyIcon.texture = characterTexture;
                break;

            case PLAYER_FOUR:
                playerFourLobbyIcon.texture = characterTexture;
                break;

            case PLAYER_FIVE:
                playerFiveLobbyIcon.texture = characterTexture;
                break;
        }
    }

    private void OnPlayerLeftLobby(string playerName, int index)
    {
        switch (index)
        {
            case PLAYER_ONE:
                playerOneLobbyIcon.texture = emptyLobbySlotIcon;
                playerOneNameText.text = string.Empty;

                if (hostingLobby || hostingOnDedicatedServer)
                {
                    kickPlayerOneButton.interactable = false;
                    kickPlayerOneButton.GetComponentInChildren<Text>().color = Color.clear;
                }

                break;

            case PLAYER_TWO:
                playerTwoLobbyIcon.texture = emptyLobbySlotIcon;
                playerTwoNameText.text = string.Empty;

                if (hostingLobby || hostingOnDedicatedServer)
                {
                    kickPlayerTwoButton.interactable = false;
                    kickPlayerTwoButton.GetComponentInChildren<Text>().color = Color.clear;
                }

                break;

            case PLAYER_THREE:
                playerThreeLobbyIcon.texture = emptyLobbySlotIcon;
                playerThreeNameText.text = string.Empty;

                if (hostingLobby || hostingOnDedicatedServer)
                {
                    kickPlayerThreeButton.interactable = false;
                    kickPlayerThreeButton.GetComponentInChildren<Text>().color = Color.clear;
                }

                break;

            case PLAYER_FOUR:
                playerFourLobbyIcon.texture = emptyLobbySlotIcon;
                playerFourNameText.text = string.Empty;

                if (hostingLobby || hostingOnDedicatedServer)
                {
                    kickPlayerFourButton.interactable = false;
                    kickPlayerFourButton.GetComponentInChildren<Text>().color = Color.clear;
                }

                break;

            case PLAYER_FIVE:
                playerFiveLobbyIcon.texture = emptyLobbySlotIcon;
                playerFiveNameText.text = string.Empty;

                if (hostingLobby || hostingOnDedicatedServer)
                {
                    kickPlayerFiveButton.interactable = false;
                    kickPlayerFiveButton.GetComponentInChildren<Text>().color = Color.clear;
                }
                break;
        }
    }

    private void OnPlayerJoinedLobby(string playerName, int index)
    {
        switch (index)
        {
            case PLAYER_ONE:
                playerOneLobbyIcon.texture = randomCharacterIcon;
                playerOneNameText.text = playerName;

                if (hostingLobby || hostingOnDedicatedServer)
                {
                    kickPlayerOneButton.interactable = true;
                    kickPlayerOneButton.GetComponentInChildren<Text>().color = Color.white;
                }

                break;

            case PLAYER_TWO:
                playerTwoLobbyIcon.texture = randomCharacterIcon;
                playerTwoNameText.text = playerName;

                if (hostingLobby || hostingOnDedicatedServer)
                {
                    kickPlayerTwoButton.interactable = true;
                    kickPlayerTwoButton.GetComponentInChildren<Text>().color = Color.white;
                }

                break;

            case PLAYER_THREE:
                playerThreeLobbyIcon.texture = randomCharacterIcon;
                playerThreeNameText.text = playerName;

                if (hostingLobby || hostingOnDedicatedServer)
                {
                    kickPlayerThreeButton.interactable = true;
                    kickPlayerThreeButton.GetComponentInChildren<Text>().color = Color.white;
                }


                break;
            case PLAYER_FOUR:
                playerFourLobbyIcon.texture = randomCharacterIcon;
                playerFourNameText.text = playerName;

                if (hostingLobby || hostingOnDedicatedServer)
                {
                    kickPlayerFourButton.interactable = true;
                    kickPlayerFourButton.GetComponentInChildren<Text>().color = Color.white;
                }

                break;

            case PLAYER_FIVE:
                playerFiveLobbyIcon.texture = randomCharacterIcon;
                playerFiveNameText.text = playerName;

                if (hostingLobby || hostingOnDedicatedServer)
                {
                    kickPlayerFiveButton.interactable = true;
                    kickPlayerFiveButton.GetComponentInChildren<Text>().color = Color.white;
                }

                break;
        }
    }

    private void OnStageSelectionUpdated(int newValue)
    {
        stageDropdown.value = (int)newValue;
    }

    private void OnKickedFromLobby()
    {
        NetworkManager.singleton.StopClient();
        ExitScene();
    }

    private void OnServerAssignedYouHost(int index)
    {
        // NOTE: Don't use the hostingLobby variable because that is if the client is also the server.
        // In this case we are the client but the real host is the dedicated server.

        this.hostingOnDedicatedServer = true;
        SetUpNewHostControls(index);
    }

    private void OnServerAssignedPlayerHost(string playerName, int index)
    {
        UpdateHostLabel(index);
    }

    private void OnGameModeSelectionUpdated(int newValue)
    {
        gameModeDropdown.value = newValue;
    }

    private void OnInsanityOptionUpdated(bool newValue)
    {
        insanityToggle.isOn = newValue;
    }

    private void OnAllRandomOptionUpdated(bool newValue)
    {
        allRandomToggle.isOn = newValue;
    }

    private void OnAllowSpectatorOptionUpdated(bool newValue)
    {
        allowSpectatorToggle.isOn = newValue;
    }

    public bool Hosting()
    {
        return hostingLobby;
    }

}

#endregion

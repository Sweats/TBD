using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using Mirror;

[System.Serializable]
public struct LobbyPlayer
{
    public int clientId;
    public int choosenCharacter;
    public string lobbyPlayerName;
}

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

    [SerializeField]
    private JoinGameUI joinGameUI;

    [SerializeField]
    private HostGameUI hostGameUI;

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

    private const int CHAD = 0;
    private const int ALICE = 1;
    private const int JESUS = 2;
    private const int JAMAL = 3;
    private const int LURKER = 4;
    private const int PHANTOM = 5;
    private const int MARY = 6;
    private const int FALLEN = 7;
    private const int RANDOM = -1;
    private const int UNKNOWN = -2;

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

    [SerializeField]
    private NetworkManager networkManager;

    private bool hostingLobby;

    // NOTE: Put connectionId's in here. -1 means no client is using the slot
    [SerializeField]
    private LobbyPlayer[] players;

    private void Start()
    {
        players = new LobbyPlayer[]
        {
            new LobbyPlayer(){clientId = -1, choosenCharacter = UNKNOWN},
            new LobbyPlayer(){clientId = -1, choosenCharacter = UNKNOWN},
            new LobbyPlayer(){clientId = -1, choosenCharacter = UNKNOWN},
            new LobbyPlayer(){clientId = -1, choosenCharacter = UNKNOWN},
            new LobbyPlayer(){clientId = -1, choosenCharacter = UNKNOWN},
        };

        this.enabled = false;
        EventManager.lobbyHostPlayerConnectedEvent.AddListener(OnLobbyHostPlayerConnected);
        EventManager.lobbyHostPlayerDisconnectedEvent.AddListener(OnLobbyHostPlayerDisconnected);
    }


    private void OnLobbyHostPlayerDisconnected(string playerName, int netId)
    {
        if (!hostingLobby)
        {
            return;
        }
        // NOTE: 0 always seems to be the host. No need to sync at this point.
        if (netId == 0)
        {
            return;
        }

        for (var i = 0; i < players.Length; i++)
        {
            int id = players[i].clientId;

            if (id == netId)
            {
                players[i].clientId = -1;
                players[i].choosenCharacter = UNKNOWN;
                SyncCharacters();
                break;
            }
        }
    }

    private void OnLobbyHostPlayerConnected(string playerName, int netId)
    {
        if (!hostingLobby)
        {
            return;
        }
        // NOTE: 0 always seems to be the host. No need to sync at this point.
        // However we will set up the UI for the host player.
        if (netId == 0)
        {
            players[PLAYER_ONE].clientId = netId;
            players[PLAYER_ONE].choosenCharacter = RANDOM;
            playerOneLobbyIcon.texture = randomCharacterIcon;
            return;
        }

        for (var i = 0; i < players.Length; i++)
        {
            int id = players[i].clientId;

            if (id == -1)
            {
                players[i].clientId = netId;
                players[i].choosenCharacter = RANDOM;
                break;
            }
        }

        SyncCharacters();
        SyncOptions();
    }

    public void Show(bool hosting)
    {
        this.enabled = true;
        hostingLobby = hosting;
        Debug.Log($"Hosting = {hostingLobby}");

        if (!hostingLobby)
        {
            SetUpClient();
        }

        else
        {
            SetUpServer();
        }

        lobbyCanvas.enabled = true;
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

        gameModeDropdown.GetComponent<Image>().enabled = false;
        gameModeDropdown.GetComponentInChildren<Text>().color = Color.white;
        stageDropdown.GetComponent<Image>().enabled = false;
        stageDropdown.GetComponentInChildren<Text>().color = Color.white;

    }

    private void SetUpHostLobbyControls()
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
    }

    private void Hide()
    {
        networkManager.StopHost();
        this.enabled = false;
        Reset();
        lobbyCanvas.enabled = false;
        Debug.Log("You are no longer hosting a game!");
    }

    #region HOST_OPTIONS

    public void OnInsanityEnabledCheckboxClicked(bool newValue)
    {
        //NOTE: Have to add this here because this callback function gets invoked when the client gets the message and updates
        //the GUI. We don't want to try to call NetworkServer.SendToAll() because if we do then it will throw warnings.
        if (hostingLobby)
        {
            HostChangedInsanityOptionSettingMessage message = new HostChangedInsanityOptionSettingMessage
            {
                insanityEnabled = newValue

            };

            NetworkServer.SendToAll(message);
        }
    }

    public void OnAllowSpectatorCheckboxClicked(bool newValue)
    {
        //NOTE: Have to add this here because this callback function gets invoked when the client gets the message and updates
        //the GUI. We don't want to try to call NetworkServer.SendToAll() because if we do then it will throw warnings.
        if (hostingLobby)
        {
            HostChangedAllowSpectatorOptionMessage message = new HostChangedAllowSpectatorOptionMessage
            {
                allowSpectator = newValue
            };

            NetworkServer.SendToAll(message);
        }
    }


    public void OnAllRandomCheckboxClicked(bool newValue)
    {
        //NOTE: Have to add this here because this callback function gets invoked when the client gets the message and updates
        //the GUI. We don't want to try to call NetworkServer.SendToAll() because if we do then it will throw warnings.
        if (hostingLobby)
        {
            HostChangedAllRandomOptionMessage message = new HostChangedAllRandomOptionMessage
            {
                allRandomEnabled = newValue
            };

            NetworkServer.SendToAll(message);
        }
    }

    public void OnStageDropdownChanged(int newStage)
    {
        //NOTE: Have to add this here because this callback function gets invoked when the client gets the message and updates
        //the GUI. We don't want to try to call NetworkServer.SendToAll() because if we do then it will throw warnings.
        if (hostingLobby)
        {
            HostChangedStageSettingMessage message = new HostChangedStageSettingMessage
            {
                newStageName = (StageName)newStage
            };

            NetworkServer.SendToAll(message);
        }
    }


    public void OnGameModeDropdownChanged(int newGameMode)
    {
        //NOTE: Have to add this here because this callback function gets invoked when the client gets the message and updates
        //the GUI. We don't want to try to call NetworkServer.SendToAll() because if we do then it will throw warnings.
        if (hostingLobby)
        {
            HostChangedGameModeSettingMessage message = new HostChangedGameModeSettingMessage
            {
                gameMode = newGameMode
            };

            NetworkServer.SendToAll(message);
        }
    }

    public void OnVoiceChatDropdownChanged()
    {

    }

    public void OnStartGameButtonClicked()
    {
        // NOTE: When updating the stage dropdown in the Unity Editor, make sure it matches the enum
        // and the dictionary in the file Stages.cs.
        StageName name = (StageName)stageDropdown.value;
        Stages.Load(name);
    }

    #endregion

    #region CLIENT
    public void OnChooseCharacterButtonClicked()
    {
        DisableControls();
        EnableSelectCharacterControls();

    }

    private void OnClientConnectedToLobby()
    {

    }

    //[Client]
    private void Update()
    {
        if (Keybinds.GetKey(Action.GuiReturn))
        {
            if (selectingCharacter)
            {
                DisableSelectCharacterControls();
                EnableControls();
                return;
            }

            Hide();

            if (hostingLobby)
            {
                NetworkServer.SendToAll(new HostDisconnectedMessage());
                networkManager.StopHost();
                hostGameUI.Show();
                Debug.Log("You are no longer hosting the game!");
            }

            else
            {
                joinGameUI.Show();
                //NetworkClient.Send(new LobbyHostClientDisconnectedMessage { playerName = Settings.PROFILE_NAME });
            }

        }
    }


    //[Client]
    private void DisableControls()
    {
        leaveLobbyButton.enabled = false;
        startGameButton.enabled = false;
        chooseCharacterButton.enabled = false;
        leaveLobbyButton.GetComponentInChildren<Text>().color = Color.grey;
        startGameButton.GetComponentInChildren<Text>().color = Color.grey;
        chooseCharacterButton.GetComponentInChildren<Text>().color = Color.grey;
    }

    //[Client]
    private void EnableControls()
    {
        leaveLobbyButton.enabled = true;
        startGameButton.enabled = true;
        chooseCharacterButton.enabled = true;
        leaveLobbyButton.GetComponentInChildren<Text>().color = Color.white;
        startGameButton.GetComponentInChildren<Text>().color = Color.white;
        chooseCharacterButton.GetComponentInChildren<Text>().color = Color.white;
    }

    //[Client]
    public void OnJamalButtonClicked()
    {
        NetworkClient.Send(new LobbyClientCharacterChangedMessage { character = JAMAL });
        DisableSelectCharacterControls();
        EnableControls();
    }


    //[Client]
    public void OnAliceButtonClicked()
    {
        NetworkClient.Send(new LobbyClientCharacterChangedMessage { character = ALICE });
        DisableSelectCharacterControls();
        EnableControls();
    }

    //[Client]
    public void OnChadButtonClicked()
    {
        NetworkClient.Send(new LobbyClientCharacterChangedMessage { character = CHAD });
        DisableSelectCharacterControls();
        EnableControls();
    }

    //[Client]
    public void OnJesusButtonClicked()
    {
        NetworkClient.Send(new LobbyClientCharacterChangedMessage { character = JESUS });
        DisableSelectCharacterControls();
        EnableControls();
    }

    //[Client]
    public void OnLurkerButtonClicked()
    {
        NetworkClient.Send(new LobbyClientCharacterChangedMessage { character = LURKER });
        DisableSelectCharacterControls();
        EnableControls();
    }


    //[Client]
    public void OnPhantomButtonClicked()
    {
        NetworkClient.Send(new LobbyClientCharacterChangedMessage { character = PHANTOM });
        DisableSelectCharacterControls();
        EnableControls();
    }


    ////[Client]
    public void OnMaryButtonClicked()
    {
        NetworkClient.Send(new LobbyClientCharacterChangedMessage { character = MARY });
        DisableSelectCharacterControls();
        EnableControls();
    }


    //[Client]
    public void OnFallenButtonClicked()
    {
        NetworkClient.Send(new LobbyClientCharacterChangedMessage { character = FALLEN });
        DisableSelectCharacterControls();
        EnableControls();
    }

    //[Client]
    public void OnRandomCharacterButtonClicked()
    {
        NetworkClient.Send(new LobbyClientCharacterChangedMessage { character = RANDOM });
        DisableSelectCharacterControls();
        EnableControls();
    }


    //[Client]
    public void OnLeaveLobbyButtonClicked()
    {
        if (hostingLobby)
        {
            NetworkServer.SendToAll(new HostDisconnectedMessage());
            hostGameUI.Show();
        }

        else
        {
            joinGameUI.Show();

        }

        Hide();
    }

    private void Reset()
    {
        playerOneLobbyIcon.texture = emptyLobbySlotIcon;
        playerTwoLobbyIcon.texture = emptyLobbySlotIcon;
        playerThreeLobbyIcon.texture = emptyLobbySlotIcon;
        playerFourLobbyIcon.texture = emptyLobbySlotIcon;

        for (var i = 0; i < players.Length; i++)
        {
            players[i].choosenCharacter = UNKNOWN;
        }
    }

    /*
    private void OnMouseOverSelectCharacterButton(Button button)
    {
        button.GetComponent<Image>().texture = hoveredOverCharacterTexture;

    }

    private void OnMouseLeftCharacterSelectChadButton(Button button)
    {
        button.GetComponent<Image>().texture = chadIcon;

    }

    private void OnMouseLeftCharacterSelectAliceButton(Button button)
    {
        button.GetComponent<Image>().texture = aliceIcon;

    }

    private void OnMouseLeftCharacterSelectJamalutton(Button button)
    {
        button.GetComponent<Image>().texture = jamalIcon;

    }

    private void OnMouseLeftCharacterSelectJesusButton(Button button)
    {
        button.GetComponent<Image>().texture = jesusIcon;

    }
    */

    //[Client]
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

    //[Client]
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

    // NOTE: Server.
    private void OnLobbyHostClientCharacterChanged(NetworkConnection connection, LobbyClientCharacterChangedMessage message)
    {
        int playerNumber = GetPlayerNumber(connection.connectionId);
        int character = message.character;
        Debug.Log($"Detected a new change: playerNumber = {playerNumber} and character = {character}");
        players[playerNumber].choosenCharacter = character;
        SyncCharacters();

    }

    private int GetPlayerNumber(int clientId)
    {
        int result = -1;

        for (var i = 0; i < players.Length; i++)
        {
            int id = players[i].clientId;

            if (id == clientId)
            {
                result = i;
                break;
            }
        }

        return result;
    }

    private void OnHostChangedStage(NetworkConnection connection, HostChangedStageSettingMessage message)
    {
        stageDropdown.value = (int)message.newStageName;
        Debug.Log($"StageName = {message.newStageName}");
    }


    private void OnHostDisconnected(NetworkConnection connection, HostDisconnectedMessage message)
    {
        Hide();
        joinGameUI.Show();
    }

    private void OnHostChangedInsanityOption(NetworkConnection connection, HostChangedInsanityOptionSettingMessage message)
    {
        bool newValue = message.insanityEnabled;
        insanityToggle.isOn = newValue;
        Debug.Log($"TEST NEW VALUE FOR INSANITY IS {newValue}");
    }

    private void OnHostChangedGamemodeOption(NetworkConnection connection, HostChangedGameModeSettingMessage message)
    {
        gameModeDropdown.value = message.gameMode;

        switch (message.gameMode)
        {
            case NORMAL_MODE:
                Debug.Log("Gamemode switched to NORMAL_MODE! ");
                break;
            case LIFE_MODE:
                Debug.Log("Gamemode switched to LIFE_MODE! ");
                break;
            default:
                Debug.Log("Gamemode switched to something unknown!");
                break;

        }
    }

    private void OnHostChangedAllowSpectatorOption(NetworkConnection connection, HostChangedAllowSpectatorOptionMessage message)
    {
        allowSpectatorToggle.isOn = message.allowSpectator;
        Debug.Log("Allow spectator updated!");
    }

    private void OnHostChangedAllRandomOption(NetworkConnection connection, HostChangedAllRandomOptionMessage message)
    {
        allRandomToggle.isOn = message.allRandomEnabled;
        Debug.Log("All random update!");

    }

    //NOTE: Server.
    private void SyncCharacters()
    {
        for (var i = 0; i < players.Length; i++)
        {
            switch (i)
            {
                case PLAYER_ONE:
                    NetworkServer.SendToAll(new LobbyHostClientCharacterChangedMessage { character = players[PLAYER_ONE].choosenCharacter, playerNumber = PLAYER_ONE });
                    break;
                case PLAYER_TWO:
                    NetworkServer.SendToAll(new LobbyHostClientCharacterChangedMessage { character = players[PLAYER_TWO].choosenCharacter, playerNumber = PLAYER_TWO });
                    break;
                case PLAYER_THREE:
                    NetworkServer.SendToAll(new LobbyHostClientCharacterChangedMessage { character = players[PLAYER_THREE].choosenCharacter, playerNumber = PLAYER_THREE });
                    break;
                case PLAYER_FOUR:
                    NetworkServer.SendToAll(new LobbyHostClientCharacterChangedMessage { character = players[PLAYER_FOUR].choosenCharacter, playerNumber = PLAYER_FOUR });
                    break;
                case PLAYER_FIVE:
                    NetworkServer.SendToAll(new LobbyHostClientCharacterChangedMessage { character = players[PLAYER_FIVE].choosenCharacter, playerNumber = PLAYER_FIVE });
                    break;
                default:
                    break;
            }
        }
    }

    private void SyncOptions()
    {
        NetworkServer.SendToAll(new HostChangedGameModeSettingMessage { gameMode = gameModeDropdown.value });
        NetworkServer.SendToAll(new HostChangedInsanityOptionSettingMessage { insanityEnabled = insanityToggle.isOn });
        NetworkServer.SendToAll(new HostChangedStageSettingMessage { newStageName = (StageName)stageDropdown.value });
        NetworkServer.SendToAll(new HostChangedAllRandomOptionMessage { allRandomEnabled = allRandomToggle.isOn });
        NetworkServer.SendToAll(new HostChangedAllowSpectatorOptionMessage { allowSpectator = allowSpectatorToggle.isOn });
    }

    //NOTE: Called on client.
    private void OnPlayerCharacterChanged(NetworkConnection connection, LobbyHostClientCharacterChangedMessage message)
    {
        int playerNumber = message.playerNumber;
        int character = message.character;
        Texture characterTexture = GetCharacterTexture(character);

        switch (playerNumber)
        {
            case PLAYER_ONE:
                playerOneLobbyIcon.texture = characterTexture;
                playerOneCharacter = character;
                break;
            case PLAYER_TWO:
                playerTwoLobbyIcon.texture = characterTexture;
                playerTwoCharacter = character;
                break;
            case PLAYER_THREE:
                playerThreeLobbyIcon.texture = characterTexture;
                playerThreeCharacter = character;
                break;
            case PLAYER_FOUR:
                playerFourLobbyIcon.texture = characterTexture;
                playerFourCharacter = character;
                break;
            case PLAYER_FIVE:
                playerFiveLobbyIcon.texture = characterTexture;
                playerFiveCharacter = character;
                break;
        }
    }

    private Texture GetCharacterTexture(int character)
    {
        switch (character)
        {
            case JAMAL:
                return jamalIcon;
            case ALICE:
                return aliceIcon;
            case CHAD:
                return chadIcon;
            case JESUS:
                return jesusIcon;
            case LURKER:
                return lurkerIcon;
            case PHANTOM:
                return phantomIcon;
            case MARY:
                return maryIcon;
            case FALLEN:
                return fallenIcon;
            case RANDOM:
                return randomCharacterIcon;
            case UNKNOWN:
                return emptyLobbySlotIcon;
            default:
                return emptyLobbySlotIcon;
        }
    }
    private void SetUpClient()
    {
        SetUpClientLobbyControls();
        NetworkClient.RegisterHandler<HostChangedStageSettingMessage>(OnHostChangedStage);
        NetworkClient.RegisterHandler<HostChangedInsanityOptionSettingMessage>(OnHostChangedInsanityOption);
        NetworkClient.RegisterHandler<HostChangedGameModeSettingMessage>(OnHostChangedGamemodeOption);
        NetworkClient.RegisterHandler<LobbyHostClientCharacterChangedMessage>(OnPlayerCharacterChanged);
        NetworkClient.RegisterHandler<HostChangedAllowSpectatorOptionMessage>(OnHostChangedAllowSpectatorOption);
        NetworkClient.RegisterHandler<HostChangedAllRandomOptionMessage>(OnHostChangedAllRandomOption);
        NetworkClient.RegisterHandler<HostDisconnectedMessage>(OnHostDisconnected);
    }

    private void SetUpServer()
    {
        SetUpHostLobbyControls();
        networkManager.StartHost();
        NetworkServer.RegisterHandler<LobbyClientCharacterChangedMessage>(OnLobbyHostClientCharacterChanged);

        NetworkClient.RegisterHandler<HostChangedStageSettingMessage>(OnHostChangedStage);
        NetworkClient.RegisterHandler<HostChangedInsanityOptionSettingMessage>(OnHostChangedInsanityOption);
        NetworkClient.RegisterHandler<HostChangedGameModeSettingMessage>(OnHostChangedGamemodeOption);
        NetworkClient.RegisterHandler<HostChangedAllowSpectatorOptionMessage>(OnHostChangedAllowSpectatorOption);
        NetworkClient.RegisterHandler<HostChangedAllRandomOptionMessage>(OnHostChangedAllRandomOption);
        NetworkClient.RegisterHandler<LobbyHostClientCharacterChangedMessage>(OnPlayerCharacterChanged);
        Debug.Log("You are now hosting a game!");
    }

    public bool Hosting()
    {
        return hostingLobby;
    }
}

#endregion

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using Mirror;
using System;

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
    Unknown,
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
        this.enabled = false;
        EventManager.lobbyHostPlayerChangedCharacterEvent.AddListener(OnPlayerSlotUpdated);
        EventManager.lobbyPlayerLeftLobbyEvent.AddListener(OnPlayerLeftLobby);
        EventManager.lobbyPlayerJoinedLobbyEvent.AddListener(OnPlayerJoinedLobby);

        EventManager.lobbyClientHostChangedAllowSpectatorEvent.AddListener(OnAllowSpectatorOptionUpdated);
        EventManager.lobbyClientHostChangedStageEvent.AddListener(OnStageSelectionUpdated);
        EventManager.lobbyClientHostChangedGamemodeEvent.AddListener(OnGameModeSelectionUpdated);
        EventManager.lobbyClientHostChangedAllRandomEvent.AddListener(OnAllRandomOptionUpdated);
        EventManager.lobbyClientHostChangedInsanityOptionEvent.AddListener(OnInsanityOptionUpdated);
    }

    public void Show(bool hosting)
    {
        this.enabled = true;
        this.lobbyCanvas.enabled = true;
        this.hostingLobby = hosting;

        if (hosting)
        {
            SetUpHostLobbyControls();
        }

        else
        {
            SetUpClientLobbyControls();
        }
    }

    public void OnKickedButtonClicked(int playerNumber)
    {
        //lobbyNetworkHandler.CmdKickPlayer(player);
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

    private void Hide()
    {
        if (hostingLobby)
        {
            networkManager.StopHost();
            Debug.Log("You are no longer hosting a game!");
        }

        this.enabled = false;
        Reset();
        lobbyCanvas.enabled = false;
    }

    #region HOST_OPTIONS

    public void OnInsanityEnabledCheckboxClicked(bool newValue)
    {
        if (hostingLobby)
        {
            EventManager.lobbyHostChangedInsanityOptionEvent.Invoke(newValue);
        }
    }

    public void OnAllowSpectatorCheckboxClicked(bool newValue)
    {
        if (hostingLobby)
        {
            EventManager.lobbyHostChangedAllowSpectatorEvent.Invoke(newValue);
        }
    }


    public void OnAllRandomCheckboxClicked(bool newValue)
    {
        if (hostingLobby)
        {
            EventManager.lobbyHostChangedAllRandomOptionEvent.Invoke(newValue);
        }
    }

    public void OnStageDropdownChanged(int newValue)
    {
        if (hostingLobby)
        {
            EventManager.lobbyHostChangedStageEvent.Invoke(newValue);
        }
    }

    public void OnGameModeDropdownChanged(int newValue)
    {
        if (hostingLobby)
        {
            EventManager.lobbyHostChangedGamemodeEvent.Invoke(newValue);
        }
    }

    public void OnVoiceChatDropdownChanged()
    {
        if (hostingLobby)
        {

        }

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

            Hide();

            if (hostingLobby)
            {
                networkManager.StopHost();
                hostGameUI.Show();
                Debug.Log("You are no longer hosting the game!");
            }

            else
            {
                joinGameUI.Show();
            }
        }
    }



    //[Client]
    public void OnJamalButtonClicked()
    {
        EventManager.lobbyYouChangedCharacterEvent.Invoke(Character.Jamal);
        DisableSelectCharacterControls();
        //EnableControls();
    }


    //[Client]
    public void OnAliceButtonClicked()
    {
        EventManager.lobbyYouChangedCharacterEvent.Invoke(Character.Alice);
        DisableSelectCharacterControls();
        //EnableControls();
    }

    //[Client]
    public void OnChadButtonClicked()
    {
        EventManager.lobbyYouChangedCharacterEvent.Invoke(Character.Chad);
        DisableSelectCharacterControls();
        //EnableControls();
    }

    //[Client]
    public void OnJesusButtonClicked()
    {
        EventManager.lobbyYouChangedCharacterEvent.Invoke(Character.Jesus);
        DisableSelectCharacterControls();
        //EnableControls();
    }

    //[Client]
    public void OnLurkerButtonClicked()
    {
        EventManager.lobbyYouChangedCharacterEvent.Invoke(Character.Lurker);
        DisableSelectCharacterControls();
        //EnableControls();
    }


    //[Client]
    public void OnPhantomButtonClicked()
    {
        EventManager.lobbyYouChangedCharacterEvent.Invoke(Character.Phantom);
        DisableSelectCharacterControls();
        //EnableControls();
    }


    ////[Client]
    public void OnMaryButtonClicked()
    {
        EventManager.lobbyYouChangedCharacterEvent.Invoke(Character.Mary);
        DisableSelectCharacterControls();
        //EnableControls();
    }


    //[Client]
    public void OnFallenButtonClicked()
    {
        EventManager.lobbyYouChangedCharacterEvent.Invoke(Character.Fallen);
        DisableSelectCharacterControls();
        //EnableControls();
    }

    //[Client]
    public void OnRandomCharacterButtonClicked()
    {
        EventManager.lobbyYouChangedCharacterEvent.Invoke(Character.Random);
        DisableSelectCharacterControls();
        //EnableControls();
    }


    //[Client]
    public void OnLeaveLobbyButtonClicked()
    {
        if (hostingLobby)
        {
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
    }

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


    private void OnPlayerSlotUpdated(Character character, string playerName, int index)
    {
        Texture characterTexture = GetCharacterTexture(character);
        Debug.Log($"Player changed character! Index = {index}: New character is {character}");

        switch (index)
        {
            case PLAYER_ONE:
                playerOneLobbyIcon.texture = characterTexture;
                playerOneNameText.text = playerName;
                break;
            case PLAYER_TWO:
                playerTwoLobbyIcon.texture = characterTexture;
                playerTwoNameText.text = playerName;
                break;
            case PLAYER_THREE:
                playerThreeLobbyIcon.texture = characterTexture;
                playerThreeNameText.text = playerName;
                break;
            case PLAYER_FOUR:
                playerFourLobbyIcon.texture = characterTexture;
                playerFourNameText.text = playerName;
                break;
            case PLAYER_FIVE:
                playerFiveLobbyIcon.texture = characterTexture;
                playerFiveNameText.text = playerName;
                break;
        }
    }

    private void OnPlayerLeftLobby(int index, string playerName)
    {
        switch (index)
        {
            case PLAYER_ONE:
                playerOneLobbyIcon.texture = emptyLobbySlotIcon;
                playerOneNameText.text = string.Empty;
                kickPlayerOneButton.interactable = false;
                kickPlayerOneButton.GetComponentInChildren<Text>().color = Color.clear;
                break;
            case PLAYER_TWO:
                playerTwoLobbyIcon.texture = emptyLobbySlotIcon;
                playerTwoNameText.text = string.Empty;
                kickPlayerTwoButton.interactable = false;
                kickPlayerTwoButton.GetComponentInChildren<Text>().color = Color.clear;
                break;
            case PLAYER_THREE:
                playerThreeLobbyIcon.texture = emptyLobbySlotIcon;
                playerThreeNameText.text = string.Empty;
                kickPlayerThreeButton.interactable = false;
                kickPlayerThreeButton.GetComponentInChildren<Text>().color = Color.clear;
                break;
            case PLAYER_FOUR:
                playerFourLobbyIcon.texture = emptyLobbySlotIcon;
                playerFourNameText.text = string.Empty;
                kickPlayerFourButton.interactable = false;
                kickPlayerFourButton.GetComponentInChildren<Text>().color = Color.clear;
                break;
            case PLAYER_FIVE:
                playerFiveLobbyIcon.texture = emptyLobbySlotIcon;
                playerFiveNameText.text = string.Empty;
                kickPlayerFiveButton.interactable = false;
                kickPlayerFiveButton.GetComponentInChildren<Text>().color = Color.clear;
                break;
        }
    }

    private void OnPlayerJoinedLobby(int index, string playerName)
    {
        switch (index)
        {
            case PLAYER_ONE:
                playerOneLobbyIcon.texture = randomCharacterIcon;
                playerOneNameText.text = playerName;
                kickPlayerOneButton.interactable = true;
                kickPlayerOneButton.GetComponentInChildren<Text>().color = Color.white;
                break;
            case PLAYER_TWO:
                playerTwoLobbyIcon.texture = emptyLobbySlotIcon;
                playerTwoNameText.text = playerName;
                kickPlayerTwoButton.interactable = true;
                kickPlayerTwoButton.GetComponentInChildren<Text>().color = Color.white;
                break;
            case PLAYER_THREE:
                playerThreeLobbyIcon.texture = emptyLobbySlotIcon;
                playerThreeNameText.text = playerName;
                kickPlayerThreeButton.interactable = true;
                kickPlayerThreeButton.GetComponentInChildren<Text>().color = Color.white;
                break;
            case PLAYER_FOUR:
                playerFourLobbyIcon.texture = emptyLobbySlotIcon;
                playerFourNameText.text = playerName;
                kickPlayerFourButton.interactable = true;
                kickPlayerFourButton.GetComponentInChildren<Text>().color = Color.white;
                break;
            case PLAYER_FIVE:
                playerFiveLobbyIcon.texture = emptyLobbySlotIcon;
                playerFiveNameText.text = playerName;
                kickPlayerFiveButton.interactable = true;
                kickPlayerFiveButton.GetComponentInChildren<Text>().color = Color.white;
                break;
        }
    }

    private void OnStageSelectionUpdated(int newValue)
    {
        Debug.Log("Stage selection changed!");
        stageDropdown.value = (int)newValue;
    }

    private void OnGameModeSelectionUpdated(int newValue)
    {
        Debug.Log("Gamemode selection changed!");
        gameModeDropdown.value = newValue;
    }

    private void OnInsanityOptionUpdated(bool newValue)
    {
        Debug.Log("Insanity option changed!");

        if (!hostingLobby)
        {
            insanityToggle.isOn = newValue;
        }
    }

    private void OnAllRandomOptionUpdated(bool newValue)
    {
        Debug.Log("All random option changed!");
        allRandomToggle.isOn = newValue;
    }

    private void OnAllowSpectatorOptionUpdated(bool newValue)
    {
        Debug.Log("Allow spectator changed!");
        allowSpectatorToggle.isOn = newValue;
    }

    public bool Hosting()
    {
        return hostingLobby;
    }
}

#endregion

using UnityEngine;
using UnityEngine.UI;


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
    private RawImage playerOneLobbyIcon;

    [SerializeField]
    private RawImage playerTwoLobbyIcon;

    [SerializeField]
    private RawImage playerThreeLobbyIcon;

    [SerializeField]
    private RawImage playerFourLobbyIcon;

    [SerializeField]
    private Image selectCharacterPanel;

    [SerializeField]
    private Text selectCharacterText;

    [SerializeField]
    private Dropdown stageDropdown;

    private const int ALICE = 0;
    private const int JAMAL = 1;
    private const int CHAD = 2;
    private const int JESUS = 3;

    private const int STAGE_TEMPLATE = 0;
    private const int STAGE_TEMPLATE_MARY = 1;
    private const int STAGE_TEMPLATE_FALLEN = 2;
    private const int STAGE_TEMPLATE_LURKER = 3;
    private const int STAGE_TEMPLATE_PHANTOM = 4;

    private bool hosting;

    private bool selectingCharacter;

    private void Start()
    {
        this.enabled = false;
    }

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

            if (hosting)
            {
                hostGameUI.Show();
            }

            else
            {
                joinGameUI.Show();
            }


        }
    }


    public void Show(bool isHosting)
    {
        hosting = isHosting;
        this.enabled = true;
        lobbyCanvas.enabled = true;

    }

    private void Hide()
    {
        this.enabled = false;
        lobbyCanvas.enabled = false;
        Reset();
    }

#region HOST_OPTIONS

    public void OnInsanityEnabledCheckboxClicked()
    {

    }


    public void OnAllowSpectatorCheckboxClicked()
    {

    }


    public void OnAllRandomCheckboxClicked()
    {

    }


    public void OnGameModeDropdownChanged()
    {

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

    public void OnChooseCharacterButtonClicked()
    {
        DisableControls();
        EnableSelectCharacterControls();

    }

    private void DisableControls()
    {
        leaveLobbyButton.enabled = false;
        startGameButton.enabled = false;
        chooseCharacterButton.enabled = false;
        leaveLobbyButton.GetComponentInChildren<Text>().color = Color.grey;
        startGameButton.GetComponentInChildren<Text>().color = Color.grey;
        chooseCharacterButton.GetComponentInChildren<Text>().color = Color.grey;
    }

    private void EnableControls()
    {
        leaveLobbyButton.enabled = true;
        startGameButton.enabled = true;
        chooseCharacterButton.enabled = true;
        leaveLobbyButton.GetComponentInChildren<Text>().color = Color.white;
        startGameButton.GetComponentInChildren<Text>().color = Color.white;
        chooseCharacterButton.GetComponentInChildren<Text>().color = Color.white;
    }

    public void OnJamalButtonClicked()
    {
        playerOneLobbyIcon.texture = jamalIcon;
        DisableSelectCharacterControls();
        EnableControls();
    }

    public void OnAliceButtonClicked()
    {
        playerOneLobbyIcon.texture = aliceIcon;
        DisableSelectCharacterControls();
        EnableControls();
    }

    public void OnChadButtonClicked()
    {
        playerOneLobbyIcon.texture = chadIcon;
        DisableSelectCharacterControls();
        EnableControls();
    }

    public void OnJesusButtonClicked()
    {
        playerOneLobbyIcon.texture = jesusIcon;
        DisableSelectCharacterControls();
        EnableControls();
    }

    public void OnLurkerButtonClicked()
    {
        playerOneLobbyIcon.texture = lurkerIcon;
        DisableSelectCharacterControls();
        EnableControls();
    }


    public void OnPhantomButtonClicked()
    {
        playerOneLobbyIcon.texture = phantomIcon;
        DisableSelectCharacterControls();
        EnableControls();
    }


    public void OnMaryButtonClicked()
    {
        playerOneLobbyIcon.texture = maryIcon;
        DisableSelectCharacterControls();
        EnableControls();
    }


    public void OnFallenButtonClicked()
    {
        playerOneLobbyIcon.texture = fallenIcon;
        DisableSelectCharacterControls();
        EnableControls();
    }

    public void OnRandomCharacterButtonClicked()
    {
        playerOneLobbyIcon.texture = randomCharacterIcon;
        DisableSelectCharacterControls();
        EnableControls();
    }


    public void OnLeaveLobbyButtonClicked()
    {
        if (hosting)
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

    private void OnPersonDisconnected()
    {
        // TODO: Get the id of the player and then change it to the X icon.
        return;

    }

}

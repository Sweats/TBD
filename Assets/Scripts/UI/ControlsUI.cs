using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public struct ButtonStruct
{
    public Button button;
    public Action action;
    public string defaultKey;

}

public class ControlsUI : MonoBehaviour
{

    [SerializeField]
    private Color buttonTextColor;

    [SerializeField]
    private Button moveForwardKeyButton;

    [SerializeField]
    private Button moveBackwardKeyButton;

    [SerializeField]
    private Button moveLeftKeyButton;

    [SerializeField]
    private Button moveRightKeyButton;

    [SerializeField]
    private Button sprintKeyButton;

    [SerializeField]
    private Button transformKeyButton;

    [SerializeField]
    private Button walkKeyButton;

    [SerializeField]
    private Button crouchKeyButton;

    [SerializeField]
    private Button startKeyButton;

    [SerializeField]
    private Button pauseKeyButton;

    [SerializeField]
    private Button playerStatsKeyButton;

    [SerializeField]
    private Button grabKeyButton;

    [SerializeField]
    private Button switchFlashlightKeyButton;

    [SerializeField]
    private Button attackKeyButton;

    [SerializeField]
    private Button teleportKeyButton;

    [SerializeField]
    private Button spectateNextButton;

    [SerializeField]
    private Button voicechatKeyButton;

    [SerializeField]
    private Button guiAcceptKeyButton;

    [SerializeField]
    private Button guiReturnKeyButton;

    [SerializeField]
    private Button defaultsButton;

    [SerializeField]
    private Button backButton;

    [SerializeField]
    private Canvas keybindingsCanvas;

    [SerializeField]
    private Slider mouseSensitivySlider;

    [SerializeField]
    private Toggle invertXToggle;

    [SerializeField]
    private Toggle invertYToggle;

    [SerializeField]
    private OptionsUI optionsUI;

    private IEnumerator buttonRoutine;

    private Action lastSettingPressed;

    private ButtonStruct[] buttonStructs;

    private const string MOUSE_SENSITIVITY_KEY = "mouse_sensitivity";
    private const string INVERT_X_KEY = "invert_x";
    private const string INVERT_Y_KEY = "invert_y";

    private const string TEST_KEY = "test";

    private const int ACTION_NONE = -1;

    // Apparently changing the toggle via code causes the callback to be called so we need to use this variable to stop that from happening
    // when the scene is launched.
    private bool initialized;

    private void Update()
    {
        if (Keybinds.GetKey(Action.GuiReturn))
        {
            optionsUI.Show();
            Hide();
        }
    }

    private void Awake()
    {
        Keybinds.Init();
    }

    private void Start()
    {
        this.enabled = false;
        SetUpButtons();

        if (!PlayerPrefs.HasKey(Enum.GetName(typeof(Action), Action.GuiReturn)))
        {
            // We don't have to worry about setting text for the buttons because I will set that in the inspector.
            GenerateSettings();
            initialized = true;
            return;
        }


        LoadSettings();
        initialized = true;
    }

    private void SetUpButtons()
    {

        buttonStructs = new ButtonStruct[]
        {
            new ButtonStruct() { button = moveRightKeyButton, action = Action.MoveRight, defaultKey =  "D" },
            new ButtonStruct(){button = moveLeftKeyButton, action = Action.MoveLeft, defaultKey = "A" },
            new ButtonStruct(){button = moveBackwardKeyButton, action = Action.MoveBack, defaultKey = "S"},
            new ButtonStruct(){button = moveForwardKeyButton, action = Action.MoveForward, defaultKey = "W"},
            new ButtonStruct(){button = sprintKeyButton, action = Action.Sprint, defaultKey = "Shift"},
            new ButtonStruct(){button = transformKeyButton, action = Action.Transform, defaultKey = "E"},
            new ButtonStruct(){button = walkKeyButton, action = Action.Walk, defaultKey = "LeftAlt"},
            new ButtonStruct(){button = crouchKeyButton, action = Action.Crouch, defaultKey = "LeftControl"},
            new ButtonStruct(){button = startKeyButton, action = Action.Start, defaultKey = "KeypadEnter"},
            new ButtonStruct(){button = pauseKeyButton, action = Action.Pause, defaultKey = "Escape"},
            new ButtonStruct(){button = playerStatsKeyButton, action = Action.PlayerStats, defaultKey = "Tab"},
            new ButtonStruct(){button = grabKeyButton, action = Action.Grab, defaultKey = "Mouse0"},
            new ButtonStruct(){button = switchFlashlightKeyButton, action = Action.SwitchFlashlight, defaultKey = "Mouse1"},

            new ButtonStruct(){button = attackKeyButton, action = Action.Attack, defaultKey = "Mouse0"},
            new ButtonStruct(){button = teleportKeyButton, action = Action.Teleport, defaultKey = "E"},
            new ButtonStruct(){button = spectateNextButton, action = Action.SpectateNext, defaultKey = "Mouse1"},
            new ButtonStruct(){button = voicechatKeyButton, action = Action.VoiceChat, defaultKey = "C"},
            new ButtonStruct(){button = guiAcceptKeyButton, action = Action.GuiAccept, defaultKey = "Mouse0"},
            new ButtonStruct(){button = guiReturnKeyButton, action = Action.GuiReturn, defaultKey = "Escape"}


        };

        // A lot easier to do this here rather than the insepector in my opinion.
        // The power of copy and pasting and vim :) 
        moveBackwardKeyButton.onClick.AddListener(OnMoveBackButtonClick);
        moveForwardKeyButton.onClick.AddListener(OnMoveForwardButtonClick);
        moveLeftKeyButton.onClick.AddListener(OnMoveLeftButtonClick);
        moveRightKeyButton.onClick.AddListener(OnMoveRightButtonClicked);
        sprintKeyButton.onClick.AddListener(OnSprintKeyButton);
        transformKeyButton.onClick.AddListener(OnTransformKeyButton);
        walkKeyButton.onClick.AddListener(OnWalkKeyButton);
        crouchKeyButton.onClick.AddListener(OnCrouchKeyButton);
        startKeyButton.onClick.AddListener(OnStartKeyButton);
        pauseKeyButton.onClick.AddListener(OnPauseKeyButton);
        playerStatsKeyButton.onClick.AddListener(OnPlayerStatsButtonClicked);
        grabKeyButton.onClick.AddListener(OnGrabKeyButton);
        switchFlashlightKeyButton.onClick.AddListener(OnSwitchFlashlightKeyButton);
        attackKeyButton.onClick.AddListener(OnAttackKeyButton);
        teleportKeyButton.onClick.AddListener(OnTeleportKeyButton);
        spectateNextButton.onClick.AddListener(OnSpectateNextKeyButton);
        voicechatKeyButton.onClick.AddListener(OnVoiceChatKeyButton);
        guiAcceptKeyButton.onClick.AddListener(OnGUIAcceptKeyButton);
        guiReturnKeyButton.onClick.AddListener(OnGUIReturnKeyButton);
        defaultsButton.onClick.AddListener(OnDefaultsButtonClicked);
        backButton.onClick.AddListener(OnControlsBackButtonClicked);
    }

    #region ButtonCallbacks
    private void OnMoveForwardButtonClick()
    {
        lastSettingPressed = Action.MoveForward;
        UpdateButtons();

    }

    private void OnMoveLeftButtonClick()
    {
        lastSettingPressed = Action.MoveLeft;
        UpdateButtons();

    }

    private void OnMoveRightButtonClicked()
    {
        lastSettingPressed = Action.MoveRight;
        UpdateButtons();

    }


    private void OnMoveBackButtonClick()
    {
        lastSettingPressed = Action.MoveBack;
        UpdateButtons();

    }
    private void OnSprintKeyButton()
    {

        lastSettingPressed = Action.Sprint;
        UpdateButtons();
    }

    private void OnTransformKeyButton()
    {

        lastSettingPressed = Action.Transform;
        UpdateButtons();
    }

    private void OnWalkKeyButton()
    {

        lastSettingPressed = Action.Walk;
        UpdateButtons();
    }

    private void OnCrouchKeyButton()
    {
        lastSettingPressed = Action.Crouch;
        UpdateButtons();

    }

    private void OnPlayerStatsButtonClicked()
    {
        lastSettingPressed = Action.PlayerStats;
        UpdateButtons();

    }

    private void OnStartKeyButton()
    {
        lastSettingPressed = Action.Start;
        UpdateButtons();

    }

    private void OnPauseKeyButton()
    {
        lastSettingPressed = Action.Pause;
        UpdateButtons();

    }

    private void OnGrabKeyButton()
    {
        lastSettingPressed = Action.Grab;
        UpdateButtons();

    }

    private void OnSwitchFlashlightKeyButton()
    {
        lastSettingPressed = Action.SwitchFlashlight;
        UpdateButtons();

    }
    private void OnAttackKeyButton()
    {
        lastSettingPressed = Action.Attack;
        UpdateButtons();

    }

    private void OnTeleportKeyButton()
    {
        lastSettingPressed = Action.Teleport;
        UpdateButtons();
    }

    private void OnSpectateNextKeyButton()
    {
        lastSettingPressed = Action.SpectateNext;
        UpdateButtons();
    }

    private void OnVoiceChatKeyButton()
    {
        lastSettingPressed = Action.VoiceChat;
        UpdateButtons();
    }

    private void OnGUIAcceptKeyButton()
    {
        lastSettingPressed = Action.GuiAccept;
        UpdateButtons();
    }

    private void OnGUIReturnKeyButton()
    {
        lastSettingPressed = Action.GuiReturn;
        UpdateButtons();
    }

    private void OnControlsBackButtonClicked()
    {
        optionsUI.Show();
        Hide();
    }

    #endregion

    private void UpdateButtons()
    {
        for (var i = 0; i < buttonStructs.Length; i++)
        {
            Action action = buttonStructs[i].action;
            Button button = buttonStructs[i].button;
            button.enabled = false;

            if (action == lastSettingPressed)
            {
                button.GetComponentInChildren<Text>().color = Color.red;
                buttonRoutine = ButtonRoutine(button);
                StartCoroutine(buttonRoutine);

            }

            else
            {
                button.GetComponentInChildren<Text>().color = Color.grey;

            }
        }
    }


    private void ResetButtonControls()
    {
        Cursor.lockState = CursorLockMode.None;
        lastSettingPressed = (Action)ACTION_NONE;

        for (var i = 0; i < buttonStructs.Length; i++)
        {
            Button button = buttonStructs[i].button;
            button.enabled = true;
            button.GetComponentInChildren<Text>().color = Color.white;
        }
    }

    private IEnumerator ButtonRoutine(Button clickedButton)
    {
        Cursor.lockState = CursorLockMode.Locked;
        KeyCode[] keyCodes = (KeyCode[])Enum.GetValues(typeof(KeyCode));

        bool keyCodeFound = false;

        while (true)
        {
            if (Input.anyKeyDown)
            {
                for (var i = 0; i < keyCodes.Length; i++)
                {
                    KeyCode keyCode = keyCodes[i];

                    if (Input.GetKey(keyCode))
                    {
                        Text clickedButtonText = clickedButton.GetComponentInChildren<Text>();
                        clickedButtonText.color = Color.white;
                        string keyCodeString = Enum.GetName(typeof(KeyCode), keyCode);
                        clickedButtonText.text = keyCodeString;
                        Keybinds.actions[lastSettingPressed] = keyCode;
                        keyCodeFound = true;
                        //NOTE: I wanted to just do a yield break here but it hangs the Unity Editor.
                        break;
                    }
                }

                if (keyCodeFound)
                {
                    break;
                }

            }

            yield return null;
        }

        ResetButtonControls();
        Debug.Log("ButtonRoutine stopped.");
    }

    public void Show()
    {
        this.enabled = true;
        keybindingsCanvas.enabled = true;
    }

    private void Hide()
    {
        SaveKeybindsConfig();
        keybindingsCanvas.enabled = false;
        this.enabled = false;
    }

    private void OnDefaultsButtonClicked()
    {
        Keybinds.SetDefaults();
        PlayerPrefs.SetInt(INVERT_X_KEY, 0);
        PlayerPrefs.SetInt(INVERT_Y_KEY, 0);
        PlayerPrefs.SetFloat(MOUSE_SENSITIVITY_KEY, 5);

        Settings.MOUSE_SENSITIVITY = 5;
        Settings.INVERT_X = 0;
        Settings.INVERT_Y = 0;

        UpdateMiscControls();
        PlayerPrefs.Save();

        for (var i = 0; i < buttonStructs.Length; i++)
        {
            buttonStructs[i].button.GetComponentInChildren<Text>().text = buttonStructs[i].defaultKey;
        }

    }

    private void GenerateSettings()
    {
        Keybinds.SetDefaults();

        PlayerPrefs.SetInt(INVERT_X_KEY, 0);
        PlayerPrefs.SetInt(INVERT_Y_KEY, 0);
        PlayerPrefs.SetFloat(MOUSE_SENSITIVITY_KEY, 5);

        Settings.MOUSE_SENSITIVITY = 5;
        Settings.INVERT_X = 0;
        Settings.INVERT_Y = 0;

        UpdateMiscControls();

        foreach (KeyValuePair<Action, KeyCode> pair in Keybinds.actions)
        {
            string keyName = Enum.GetName(typeof(Action), pair.Key);
            PlayerPrefs.SetInt(keyName, (int)pair.Value);
        }

        PlayerPrefs.Save();
    }


    private void LoadSettings()
    {
        var actionNames = Enum.GetNames(typeof(Action));
        var actions = Enum.GetValues(typeof(Action));

        for (var i = 0; i < actions.Length; i++)
        {
            string keyName = actionNames[i];
            Action action = (Action)actions.GetValue(i);
            KeyCode loadedKeycode = (KeyCode)PlayerPrefs.GetInt(keyName);
            Keybinds.actions[action] = loadedKeycode;
            UpdateButtonControl(action, loadedKeycode);
        }

        Settings.MOUSE_SENSITIVITY = PlayerPrefs.GetFloat(MOUSE_SENSITIVITY_KEY);
        Settings.INVERT_X = PlayerPrefs.GetInt(INVERT_X_KEY);
        Settings.INVERT_Y = PlayerPrefs.GetInt(INVERT_Y_KEY);

        UpdateMiscControls();

    }

    private void UpdateButtonControl(Action action, KeyCode newKeyCode)
    {
        for (var i = 0; i < buttonStructs.Length; i++)
        {
            Button button = buttonStructs[i].button;
            Action foundAction = buttonStructs[i].action;

            if (action == foundAction)
            {
                string newText = Enum.GetName(typeof(KeyCode), newKeyCode);
                button.GetComponentInChildren<Text>().text = newText;
                break;
            }
        }
    }

    private void UpdateMiscControls()
    {
        mouseSensitivySlider.value = Settings.MOUSE_SENSITIVITY;

        if (Settings.INVERT_X == 1)
        {
            invertXToggle.isOn = true;
        }

        else
        {
            invertXToggle.isOn = false;
        }


        if (Settings.INVERT_Y == 1)
        {
            invertYToggle.Select();
        }

        else
        {
            invertYToggle.isOn = false;
        }

    }

    private void SaveKeybindsConfig()
    {
        foreach (KeyValuePair<Action, KeyCode> pair in Keybinds.actions)
        {
            string keyName = Enum.GetName(typeof(Action), pair.Key);
            PlayerPrefs.SetInt(keyName, (int)pair.Value);
        }

        PlayerPrefs.SetFloat(MOUSE_SENSITIVITY_KEY, Settings.MOUSE_SENSITIVITY);
        PlayerPrefs.SetInt(INVERT_X_KEY, Settings.INVERT_X);
        PlayerPrefs.SetInt(INVERT_Y_KEY, Settings.INVERT_Y);

        PlayerPrefs.Save();
    }
}


using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private Canvas keybindingsCanvas;

    [SerializeField]
    private Slider mouseSensitivySlider;

    [SerializeField]
    private Toggle invertXToggle;

    [SerializeField]
    private Toggle invertYToggle;

    private Action lastSettingPressed;
    // used to save the key binds to the config
    private Dictionary<Action, Button> buttonDict;

    private const string MOUSE_SENSITIVITY_KEY = "mouse_sensitivity";
    private const string INVERT_X_KEY = "invert_x";
    private const string INVERT_Y_KEY = "invert_y";

    private const int ACTION_NONE = -1;

    // Apparently changing the toggle via code causes the callback to be called so we need to use this variable to stop that from happening
    // when the scene is launched.
    private bool initialized;


    void Update()
    {
        if (Input.anyKeyDown && (int)lastSettingPressed != ACTION_NONE && Cursor.lockState == CursorLockMode.Locked)
        {
            KeyCode[] keyCodes = (KeyCode[])Enum.GetValues(typeof(KeyCode));

            for (var i = 0; i < keyCodes.Length; i++)
            {
                KeyCode currentKeyCode = keyCodes[i];

                if (Input.GetKeyDown(keyCodes[i]))
                {
                    Keybinds.actions[lastSettingPressed] = currentKeyCode;
                    Text text = buttonDict[lastSettingPressed].GetComponentInChildren<Text>();
                    text.color = Color.white;
                    text.text = Enum.GetName(typeof(KeyCode), currentKeyCode);
                    Cursor.lockState = CursorLockMode.Confined;
                    lastSettingPressed = (Action)ACTION_NONE;
                    break;
                }
            }
        }
    }


    void Start()
    {
        initialized = false;
        buttonDict = new Dictionary<Action, Button>();
        buttonDict[Action.MoveForward] = moveForwardKeyButton;
        buttonDict[Action.MoveLeft] = moveLeftKeyButton;
        buttonDict[Action.MoveBack] = moveBackwardKeyButton;
        buttonDict[Action.MoveRight] = moveRightKeyButton;
        buttonDict[Action.Sprint] = sprintKeyButton;
        buttonDict[Action.Walk] = walkKeyButton;
        buttonDict[Action.Crouch] = crouchKeyButton;
        buttonDict[Action.Start] = startKeyButton;
        buttonDict[Action.Pause] = pauseKeyButton;
        buttonDict[Action.PlayerStats] = playerStatsKeyButton;
        buttonDict[Action.Transform] = transformKeyButton;
        buttonDict[Action.Grab] = grabKeyButton;
        buttonDict[Action.SwitchFlashlight] = switchFlashlightKeyButton;
        buttonDict[Action.Attack] = attackKeyButton;
        buttonDict[Action.Teleport] = teleportKeyButton;
        buttonDict[Action.SpectateNext] = spectateNextButton;
        buttonDict[Action.VoiceChat] = voicechatKeyButton;
        buttonDict[Action.GuiAccept] = guiAcceptKeyButton;
        buttonDict[Action.GUiReturn] = guiReturnKeyButton;
        lastSettingPressed = (Action)ACTION_NONE;

        if (!PlayerPrefs.HasKey(Enum.GetName(typeof(Action), Action.GUiReturn)))
        {
            // We don't have to worry about setting text for the buttons because I will set that in the inspector.
            GenerateSettings();
            initialized = true;
            return;
        }


        LoadSettings();
        initialized = true;

    }

    public void Show()
    {
        keybindingsCanvas.enabled = true;
    }

    public void Hide()
    {
        SaveKeybindsConfig();
        keybindingsCanvas.enabled = false;
    }


    private void GenerateSettings()
    {
        SetDefaultBindings();
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
            buttonDict[action].GetComponentInChildren<Text>().text = Enum.GetName(typeof(KeyCode), loadedKeycode);
        }

        Survivor.mouseSensitivity = PlayerPrefs.GetFloat(MOUSE_SENSITIVITY_KEY);
        Survivor.invertX = PlayerPrefs.GetInt(INVERT_X_KEY);
        Survivor.invertY = PlayerPrefs.GetInt(INVERT_Y_KEY);

        UpdateMiscControls();
    }

    private void UpdateMiscControls()
    {
        mouseSensitivySlider.value = Survivor.mouseSensitivity;

        if (Survivor.invertX == 1)
        {
            invertXToggle.isOn = true;
        }

        else
        {
            invertXToggle.isOn = false;
        }


        if (Survivor.invertY == 1)
        {
            invertYToggle.Select();
        }

        else
        {
            invertYToggle.isOn = false;
        }

    }

    private void SetDefaultBindings()
    {
        Keybinds.actions[Action.MoveForward] = KeyCode.W;
        Keybinds.actions[Action.MoveBack] = KeyCode.S;
        Keybinds.actions[Action.MoveLeft] = KeyCode.A;
        Keybinds.actions[Action.MoveRight] = KeyCode.D;
        Keybinds.actions[Action.Sprint] = KeyCode.LeftShift;
        Keybinds.actions[Action.Transform] = KeyCode.E;
        Keybinds.actions[Action.Walk] = KeyCode.LeftAlt;
        Keybinds.actions[Action.Crouch] = KeyCode.LeftControl;
        Keybinds.actions[Action.Start] = KeyCode.KeypadEnter;
        Keybinds.actions[Action.Pause] = KeyCode.Escape;
        Keybinds.actions[Action.PlayerStats] = KeyCode.Tab;
        Keybinds.actions[Action.Grab] = KeyCode.Mouse0;
        Keybinds.actions[Action.SwitchFlashlight] = KeyCode.Mouse1;
        Keybinds.actions[Action.Attack] = KeyCode.Mouse0;
        Keybinds.actions[Action.Teleport] = KeyCode.Mouse1;
        Keybinds.actions[Action.SpectateNext] = KeyCode.Mouse0;
        Keybinds.actions[Action.VoiceChat] = KeyCode.C;
        Keybinds.actions[Action.GuiAccept] = KeyCode.Mouse0;
        Keybinds.actions[Action.GUiReturn] = KeyCode.Escape;



        PlayerPrefs.SetInt(INVERT_X_KEY, 0);
        PlayerPrefs.SetInt(INVERT_Y_KEY, 0);
        PlayerPrefs.SetFloat(MOUSE_SENSITIVITY_KEY, 5);

        Survivor.mouseSensitivity = 5;
        Survivor.invertX = 0;
        Survivor.invertY = 0;

        UpdateMiscControls();

        PlayerPrefs.Save();
    }

    public void OnMoveForwardButtonClick()
    {
        lastSettingPressed = Action.MoveForward;
        Cursor.lockState = CursorLockMode.Locked;
        buttonDict[lastSettingPressed].GetComponentInChildren<Text>().color = Color.red;
    }


    public void OnMoveLeftButtonClick()
    {
        lastSettingPressed = Action.MoveLeft;
        Cursor.lockState = CursorLockMode.Locked;
        buttonDict[lastSettingPressed].GetComponentInChildren<Text>().color = Color.red;

    }


    public void OnMoveBackButtonClick()
    {
        lastSettingPressed = Action.MoveBack;
        Cursor.lockState = CursorLockMode.Locked;
        buttonDict[lastSettingPressed].GetComponentInChildren<Text>().color = Color.red;

    }


    public void OnMoveRightButtonClick()
    {

        lastSettingPressed = Action.MoveRight;
        Cursor.lockState = CursorLockMode.Locked;
        buttonDict[lastSettingPressed].GetComponentInChildren<Text>().color = Color.red;

    }


    public void OnSprintButtonClick()
    {
        lastSettingPressed = Action.Sprint;
        Cursor.lockState = CursorLockMode.Locked;
        buttonDict[lastSettingPressed].GetComponentInChildren<Text>().color = Color.red;

    }


    public void OnWalkButtonClick()
    {
        lastSettingPressed = Action.Walk;
        Cursor.lockState = CursorLockMode.Locked;
        buttonDict[lastSettingPressed].GetComponentInChildren<Text>().color = Color.red;

    }


    public void OnCrouchButtonClick()
    {
        lastSettingPressed = Action.Crouch;
        Cursor.lockState = CursorLockMode.Locked;
        buttonDict[lastSettingPressed].GetComponentInChildren<Text>().color = Color.red;

    }


    public void OnStartButtonCilck()
    {
        lastSettingPressed = Action.Start;
        Cursor.lockState = CursorLockMode.Locked;
        buttonDict[lastSettingPressed].GetComponentInChildren<Text>().color = Color.red;

    }


    public void OnPauseButtonClick()
    {
        lastSettingPressed = Action.Pause;
        Cursor.lockState = CursorLockMode.Locked;
        buttonDict[lastSettingPressed].GetComponentInChildren<Text>().color = Color.red;

    }


    public void OnPlayerstatsButtonClick()
    {
        lastSettingPressed = Action.PlayerStats;
        Cursor.lockState = CursorLockMode.Locked;
        buttonDict[lastSettingPressed].GetComponentInChildren<Text>().color = Color.red;

    }



    public void OnGrabButtonClick()
    {
        lastSettingPressed = Action.Grab;
        Cursor.lockState = CursorLockMode.Locked;
        buttonDict[lastSettingPressed].GetComponentInChildren<Text>().color = Color.red;

    }


    public void OnSwitchFlashlightButtonClick()
    {
        lastSettingPressed = Action.SwitchFlashlight;
        Cursor.lockState = CursorLockMode.Locked;
        buttonDict[lastSettingPressed].GetComponentInChildren<Text>().color = Color.red;
    }


    public void OnTransformButtonClick()
    {
        lastSettingPressed = Action.Transform;
        Cursor.lockState = CursorLockMode.Locked;
        buttonDict[lastSettingPressed].GetComponentInChildren<Text>().color = Color.red;

    }


    public void OnAttackButtonClick()
    {
        lastSettingPressed = Action.Attack;
        Cursor.lockState = CursorLockMode.Locked;
        buttonDict[lastSettingPressed].GetComponentInChildren<Text>().color = Color.red;

    }


    public void OnTeleportButtonClick()
    {

        lastSettingPressed = Action.Teleport;
        Cursor.lockState = CursorLockMode.Locked;
        buttonDict[lastSettingPressed].GetComponentInChildren<Text>().color = Color.red;

    }


    public void OnSpectateNextButtonClick()
    {

        lastSettingPressed = Action.SpectateNext;
        Cursor.lockState = CursorLockMode.Locked;
        buttonDict[lastSettingPressed].GetComponentInChildren<Text>().color = Color.red;
    }


    public void OnVoiceChatButtonCilck()
    {

        lastSettingPressed = Action.VoiceChat;
        Cursor.lockState = CursorLockMode.Locked;
        buttonDict[lastSettingPressed].GetComponentInChildren<Text>().color = Color.red;
    }


    public void guiAcceptButtonClick()
    {
        lastSettingPressed = Action.GuiAccept;
        Cursor.lockState = CursorLockMode.Locked;
        buttonDict[lastSettingPressed].GetComponentInChildren<Text>().color = Color.red;
    }


    public void guiReturnButtonClick()
    {
        lastSettingPressed = Action.GUiReturn;
        Cursor.lockState = CursorLockMode.Locked;
        buttonDict[lastSettingPressed].GetComponentInChildren<Text>().color = Color.red;

    }

    public void OnMouseEnterButton(Button button)
    {
        Text text = button.GetComponentInChildren<Text>();
        text.color = buttonTextColor;
    }


    public void OnMouseLeftButton(Button button)
    {
        // Hack to get around the fact that this gets called after we click on a button.
        // We want the button text to be highlighed in red after the user clicks on it.
        if (lastSettingPressed == (Action)ACTION_NONE)
        {
            Text text = button.GetComponentInChildren<Text>();
            text.color = Color.white;
        }

    }
    public void OnLoadDefaultsButton()
    {
        initialized = false;
        lastSettingPressed = (Action)ACTION_NONE;
        SetDefaultBindings();
        ResetActionButtons();
        initialized = true;
    }


    private void ResetActionButtons()
    {
        foreach (KeyValuePair<Action, KeyCode> pair in Keybinds.actions)
        {
            Action action = pair.Key;
            KeyCode keycode = pair.Value;
            string enumName = Enum.GetName(typeof(Action), action);
            PlayerPrefs.SetInt(enumName, (int)keycode);

            foreach (KeyValuePair<Action, Button> secondPair in buttonDict)
            {
                Action secondAction = secondPair.Key;

                if (action == secondAction)
                {
                    string keyCodeString = Enum.GetName(typeof(KeyCode), keycode);
                    buttonDict[action].GetComponentInChildren<Text>().text = keyCodeString;
                    break;
                }
            }
        }
    }


    public void OnMouseSensitivySliderChanged(float value)
    {
        Survivor.mouseSensitivity = value;
    }

    public void OnInvertXToggle(bool value)
    {
        if (!initialized)
        {
            return;
        }

        if (value)
        {
            Survivor.invertX = 1;
        }

        else
        {
            Survivor.invertX = 0;
        }
    }

    public void OnInvertYToggle(bool value)
    {
        if (!initialized)
        {
            return;
        }

        if (value)
        {
            Survivor.invertY = 1;
        }

        else
        {
            Survivor.invertY = 0;
        }
    }


    private void SaveKeybindsConfig()
    {
        foreach (KeyValuePair<Action, KeyCode> pair in Keybinds.actions)
        {
            string keyName = Enum.GetName(typeof(Action), pair.Key);
            PlayerPrefs.SetInt(keyName, (int)pair.Value);
        }

        PlayerPrefs.SetFloat(MOUSE_SENSITIVITY_KEY, Survivor.mouseSensitivity);
        PlayerPrefs.SetInt(INVERT_X_KEY, Survivor.invertX);
        PlayerPrefs.SetInt(INVERT_Y_KEY, Survivor.invertY);

        PlayerPrefs.Save();
    }
}


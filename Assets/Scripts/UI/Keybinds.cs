using System.Collections.Generic;
using UnityEngine;

public enum Action
{
    MoveForward = 0,
    MoveBack,
    MoveLeft,
    MoveRight,
    Sprint,
    Transform,
    Walk,
    Crouch,
    Start,
    Pause,
    PlayerStats,
    Grab,
    SwitchFlashlight,
    Attack,
    Teleport,
    SpectateNext,
    VoiceChat,
    GuiAccept,
    GUiReturn
}

public class Keybinds : MonoBehaviour
{
    public static Dictionary<Action, KeyCode> actions;

    private void Start()
    {
        actions = new Dictionary<Action, KeyCode>();
    }

    public static bool GetKey(Action action, bool onKeyUp = false)
    {
        if (!onKeyUp)
        {
            return UnityEngine.Input.GetKeyDown(actions[action]);
        }

        else
        {
            return UnityEngine.Input.GetKeyUp(actions[action]);
        }
    }


    public static bool Get(Action action)
    {
        return UnityEngine.Input.GetKey(actions[action]);
    }

    public static void Set(Action action, KeyCode keyCode)
    {
        actions[action] = keyCode;
    }

    private static void testfunction()
    {

    }
}


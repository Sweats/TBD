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
    Enter,
    Pause,
    PlayerStats,
    Grab,
    SwitchFlashlight,
    Attack,
    Teleport,
    SpectateNext,
    VoiceChat,
    GuiAccept,
    GuiReturn
}

public static class Keybinds
{
    public static Dictionary<Action, KeyCode> actions;

    //NOTE: Called when the ControlUI class is initialized.
    public static void Init()
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

    public static void SetDefaults()
    {
        Keybinds.actions[Action.MoveForward] = KeyCode.W;
        Keybinds.actions[Action.MoveBack] = KeyCode.S;
        Keybinds.actions[Action.MoveLeft] = KeyCode.A;
        Keybinds.actions[Action.MoveRight] = KeyCode.D;
        Keybinds.actions[Action.Sprint] = KeyCode.LeftShift;
        Keybinds.actions[Action.Transform] = KeyCode.E;
        Keybinds.actions[Action.Walk] = KeyCode.LeftAlt;
        Keybinds.actions[Action.Crouch] = KeyCode.LeftControl;
        Keybinds.actions[Action.Enter] = KeyCode.Return;
        Keybinds.actions[Action.Pause] = KeyCode.Escape;
        Keybinds.actions[Action.PlayerStats] = KeyCode.Tab;
        Keybinds.actions[Action.Grab] = KeyCode.Mouse0;
        Keybinds.actions[Action.SwitchFlashlight] = KeyCode.Mouse1;
        Keybinds.actions[Action.Attack] = KeyCode.Mouse0;
        Keybinds.actions[Action.Teleport] = KeyCode.Mouse1;
        Keybinds.actions[Action.SpectateNext] = KeyCode.Mouse0;
        Keybinds.actions[Action.VoiceChat] = KeyCode.C;
        Keybinds.actions[Action.GuiAccept] = KeyCode.Mouse0;
        Keybinds.actions[Action.GuiReturn] = KeyCode.Escape;
    }


    public static bool Get(Action action)
    {
        return UnityEngine.Input.GetKey(actions[action]);
    }

    public static void Set(Action action, KeyCode keyCode)
    {
        actions[action] = keyCode;
    }
}


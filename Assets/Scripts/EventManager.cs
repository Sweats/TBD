using UnityEngine.Events;

public static class EventManager
{
    public static SurvivorPickedUpBatteryEvent survivorPickedUpBatteryEvent = new SurvivorPickedUpBatteryEvent();

    public static SurvivorAlreadyHaveKeyEvent SurvivorAlreadyHaveKeyEvent = new SurvivorAlreadyHaveKeyEvent();

    public static SurvivorDeathEvent survivorDeathEvent = new SurvivorDeathEvent();

    public static SurvivorFailedToPickUpBatteryEvent survivorFailedToPickUpBatteryEvent = new SurvivorFailedToPickUpBatteryEvent();

    public static SurvivorsEscapedStageEvent survivorsEscapedStageEvent = new SurvivorsEscapedStageEvent();

    public static SurvivorTriggeredTrapEvent survivorTriggeredTrapEvent = new SurvivorTriggeredTrapEvent();

    public static SurvivorToggleFlashlightEvent survivorToggledFlashlightEvent;

    public static SurvivorPickedUpKeyEvent survivorPickedUpKeyEvent = new SurvivorPickedUpKeyEvent();

    public static SurvivorSendChatMessageEvent survivorSendChatMessageEvent = new SurvivorSendChatMessageEvent();

    public static SurvivorOpenedChatEvent survivorOpenedChatEvent = new SurvivorOpenedChatEvent();

    public static SurvivorClosedChatEvent survivorClosedChatEvent = new SurvivorClosedChatEvent();

    public static SurvivorFailedToUnlockDoorEvent survivorFailedToUnlockDoorEvent = new SurvivorFailedToUnlockDoorEvent();

    public static SurvivorUnlockDoorEvent survivorUnlockDoorEvent = new SurvivorUnlockDoorEvent();


    public static SurvivorOpenedPlayerStats survivorOpenedPlayerStats = new SurvivorOpenedPlayerStats();

    public static SurvivorClosedPlayerStats survivorClosedPlayerStats = new SurvivorClosedPlayerStats();

    public static PlayerConnectedEvent playerConnectedEvent = new PlayerConnectedEvent();

    public static PlayerOpenedConsoleEvent playerOpenedConsoleEvent = new PlayerOpenedConsoleEvent();

    public static PlayerDisconnectedEvent playerDisconnectedEvent = new PlayerDisconnectedEvent();
    public static MonsterWonEvent monsterWonEvent = new MonsterWonEvent();

    public static FailedToLoadStageEvent failedToLoadStageEvent = new FailedToLoadStageEvent();

    public static PlayerClosedConsoleEvent playerClosedConsoleEvent = new PlayerClosedConsoleEvent();

    public static PlayerOpenedPauseMenuEvent playerOpenedPauseMenuEvent = new PlayerOpenedPauseMenuEvent();

    public static PlayerClosedPauseMenuEvent playerClosedPauseMenuEvent = new PlayerClosedPauseMenuEvent();

    public static LurkerChangedFormEvent lurkerChangedFormEvent = new LurkerChangedFormEvent();

    public static LurkerReadyToGoIntoPhysicalFormEvent lurkerReadyToGoIntoPhysicalFormEvent = new LurkerReadyToGoIntoPhysicalFormEvent();

}



#region Survivor_Events


[System.Serializable]
public class SurvivorDeathEvent : UnityEvent<Survivor> { }

[System.Serializable]
public class SurvivorUnlockDoorEvent : UnityEvent<Survivor, Key, Door> { }

[System.Serializable]
public class SurvivorFailedToUnlockDoorEvent : UnityEvent<Door> { }

[System.Serializable]
public class SurvivorPickedUpKeyEvent : UnityEvent<Survivor, Key> { }

[System.Serializable]
public class SurvivorPickedUpBatteryEvent : UnityEvent<Survivor, Battery> { }

[System.Serializable]
public class SurvivorFailedToPickUpBatteryEvent : UnityEvent { }

[System.Serializable]
public class SurvivorStartSprintingEvent : UnityEvent<Survivor> { }

[System.Serializable]
public class SurvivorStopSprintingEvent : UnityEvent<Survivor> { }

[System.Serializable]
public class SurvivorToggleFlashlightEvent : UnityEvent<Survivor> { }

[System.Serializable]
public class SurvivorAlreadyHaveKeyEvent : UnityEvent { }

[System.Serializable]
public class SurvivorSendChatMessageEvent : UnityEvent<ChatMessage> { }



[System.Serializable]
public class SurvivorMovingEvent : UnityEvent<bool> { }

[System.Serializable]
public class SurvivorStopMovingEvent : UnityEvent { }

#endregion

#region INSANITY

[System.Serializable]
public class InsanityEffectEvent : UnityEvent { }


#endregion

#region GAME_MESSAGES

[System.Serializable]

#endregion


#region TRAP_EVENTS

public class SurvivorTriggeredTrapEvent : UnityEvent<Survivor, Trap> { }

[System.Serializable]
public class MonsterArmedTrapEvent : UnityEvent<Trap> { }

#endregion


#region STAGE_EVENTS

[System.Serializable]
public class SurvivorsEscapedStageEvent : UnityEvent { }


[System.Serializable]
public class FailedToLoadStageEvent : UnityEvent<string> { }

public class MonsterWonEvent : UnityEvent { }


#endregion

#region LURKER_EVENTS

public class LurkerChangedFormEvent : UnityEvent<bool> { }


public class LurkerReadyToGoIntoPhysicalFormEvent : UnityEvent { }


#endregion


#region LOBBY_EVENTS

//[System.Serializable]
//public class ReturnToLobbyEvent: UnityEvent {}

#endregion


#region NETWORK_EVENTS


[System.Serializable]
public class PlayerConnectedEvent : UnityEvent<Survivor> { }

[System.Serializable]
public class PlayerDisconnectedEvent : UnityEvent<Survivor> { }

#endregion



#region WINDOW_EVENTS

public class PlayerOpenedConsoleEvent : UnityEvent { }

public class PlayerClosedConsoleEvent : UnityEvent { }

public class PlayerOpenedPauseMenuEvent : UnityEvent { }

public class PlayerClosedPauseMenuEvent : UnityEvent { }

public class SurvivorOpenedChatEvent : UnityEvent { }

public class SurvivorClosedChatEvent : UnityEvent { }

public class SurvivorOpenedPlayerStats : UnityEvent { }

public class SurvivorClosedPlayerStats : UnityEvent { }

#endregion

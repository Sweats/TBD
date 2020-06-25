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
    
    public static SurvivorSendChatMessageEvent survivorSendChatMessageEvent;
    
    public static SurvivorOpenedChatEvent survivorOpenedChatEvent = new SurvivorOpenedChatEvent();
    
    public static SurvivorClosedChatEvent survivorClosedChatEvent = new SurvivorClosedChatEvent();
    
    public static SurvivorFailedToUnlockDoorEvent survivorFailedToUnlockDoorEvent = new SurvivorFailedToUnlockDoorEvent();
    
    public static SurvivorUnlockDoorEvent survivorUnlockDoorEvent = new SurvivorUnlockDoorEvent();


    public static SurvivorOpenedPlayerStats survivorOpenedPlayerStats = new SurvivorOpenedPlayerStats();

    public static SurvivorClosedPlayerStats survivorClosedPlayerStats = new SurvivorClosedPlayerStats();

    public static PlayerConnectedEvent playerConnectedEvent = new PlayerConnectedEvent();

    public static PlayerDisconnectedEvent playerDisconnectedEvent = new PlayerDisconnectedEvent();
    public static MonsterWonEvent monsterWonEvent = new MonsterWonEvent();
}


    
#region Survivor_Events


[System.Serializable]
public class SurvivorDeathEvent: UnityEvent<Survivor> {}

[System.Serializable]
public class SurvivorUnlockDoorEvent: UnityEvent<Survivor, Key, Door> {}

[System.Serializable]
public class SurvivorFailedToUnlockDoorEvent: UnityEvent<Door> {}

[System.Serializable]
public class SurvivorPickedUpKeyEvent: UnityEvent<Survivor, Key> {}

[System.Serializable]
public class SurvivorPickedUpBatteryEvent: UnityEvent<Survivor, Battery> {}

[System.Serializable]
public class SurvivorFailedToPickUpBatteryEvent: UnityEvent {}

[System.Serializable]
public class SurvivorStartSprintingEvent: UnityEvent<Survivor> {}

[System.Serializable]
public class  SurvivorStopSprintingEvent: UnityEvent<Survivor> {}

[System.Serializable]
public class SurvivorToggleFlashlightEvent: UnityEvent<Survivor> {}

[System.Serializable]
public class SurvivorAlreadyHaveKeyEvent: UnityEvent {}

[System.Serializable]
public class SurvivorOpenedChatEvent: UnityEvent {}

[System.Serializable]
public class SurvivorClosedChatEvent: UnityEvent {}

[System.Serializable]
public class SurvivorSendChatMessageEvent: UnityEvent<Survivor> {}

[System.Serializable]
public class SurvivorOpenedPlayerStats: UnityEvent {}

[System.Serializable]
public class SurvivorClosedPlayerStats: UnityEvent {}


[System.Serializable]
public class SurvivorMovingEvent: UnityEvent<bool> {}

[System.Serializable]
public class SurvivorStopMovingEvent: UnityEvent {}

#endregion

#region INSANITY

[System.Serializable]
public class InsanityEffectEvent: UnityEvent {}


#endregion

#region GAME_MESSAGES

[System.Serializable]

#endregion


#region TRAP_EVENTS

public class SurvivorTriggeredTrapEvent: UnityEvent<Survivor, Trap>{}

[System.Serializable]
public class MonsterArmedTrapEvent: UnityEvent<Trap> {}

#endregion


#region STAGE_EVENTS

[System.Serializable]
public class SurvivorsEscapedStageEvent: UnityEvent {}

public class MonsterWonEvent: UnityEvent {}


#endregion

#region LURKER_EVENTS

[System.Serializable]
public class LurkerChangedFormEvent: UnityEvent<bool> {}

#endregion


#region LOBBY_EVENTS

//[System.Serializable]
//public class ReturnToLobbyEvent: UnityEvent {}

#endregion


#region NETWORK_EVENTS


[System.Serializable]
public class PlayerConnectedEvent: UnityEvent<Survivor> {}


[System.Serializable]
public class PlayerDisconnectedEvent: UnityEvent<Survivor> {}

#endregion
using UnityEngine.Events;

#region Survivor_Events

[System.Serializable]
public class SurvivorDeathEvent: UnityEvent<Survivor> {}

[System.Serializable]
public class SurvivorUnlockDoorEvent: UnityEvent<Survivor, Key, Door> {}

[System.Serializable]
public class SurvivorClickedOnDoorEvent: UnityEvent<Survivor, Door> {}

[System.Serializable]
public class SurvivorClickedOnKeyEvent: UnityEvent<Survivor, KeyObject> {}

[System.Serializable]
public class SurvivorFailedToUnlockDoorEvent: UnityEvent<Door> {}


[System.Serializable]
public class SurvivorClickedOnBatteryEvent: UnityEvent<Survivor, Battery> {}


[System.Serializable]
public class SurviorGrabbedKeyEvent: UnityEvent<Survivor, Key> {}

[System.Serializable]
public class SurvivorPickedUpBatteryEvent: UnityEvent<Survivor, Battery> {}

[System.Serializable]
public class SurvivorFailedToPickUpbBatteryEvent: UnityEvent {}

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
public class SurvivorClosedChat: UnityEvent {}

[System.Serializable]
public class SurvivorSendChatMessage: UnityEvent {}

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

#endregion

#region LURKER_EVENTS

[System.Serializable]
public class LurkerChangedFormEvent: UnityEvent<bool> {}


#endregion



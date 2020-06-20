
using UnityEngine.Events;

#region Survivor_Events

[System.Serializable]
public class SurvivorDeathEvent: UnityEvent<Survivor> {}

[System.Serializable]
public class SurvivorUnlockDoorEvent: UnityEvent<Survivor, Key, Door> {}

[System.Serializable]
public class SurvivorFailedToUnlockDoorEvent: UnityEvent<Door> {}

[System.Serializable]
public class SurvivorPickedUpKeyEvent: UnityEvent<Survivor, KeyObject> {}

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
public class SurvivorTriggeredTrapEvent: UnityEvent<Survivor, Trap>{}

[System.Serializable]
public class SurvivorAlreadyHaveKeyEvent: UnityEvent {}

#endregion




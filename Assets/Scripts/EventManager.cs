using UnityEngine.Events;

public static class EventManager
{

    public static SurvivorPickedUpBatteryEvent survivorPickedUpBatteryEvent = new SurvivorPickedUpBatteryEvent();

    public static SurvivorAlreadyHaveKeyEvent survivorAlreadyHasKeyEvent = new SurvivorAlreadyHaveKeyEvent();

    public static SurvivorDeathEvent survivorDeathEvent = new SurvivorDeathEvent();

    public static SurvivorFailedToPickUpBatteryEvent survivorFailedToPickUpBatteryEvent = new SurvivorFailedToPickUpBatteryEvent();

    public static SurvivorsEscapedStageEvent survivorsEscapedStageEvent = new SurvivorsEscapedStageEvent();

    public static SurvivorTriggeredTrapEvent survivorTriggeredTrapEvent = new SurvivorTriggeredTrapEvent();

    public static SurvivorToggleFlashlightEvent survivorToggledFlashlightEvent;

    public static PlayerPickedUpKeyEvent playerPickedUpKeyEvent = new PlayerPickedUpKeyEvent();

    public static LobbyHostPlayerConnectedEvent lobbyHostPlayerConnectedEvent = new LobbyHostPlayerConnectedEvent();

    public static LobbyHostPlayerDisconnectedEvent lobbyHostPlayerDisconnectedEvent = new LobbyHostPlayerDisconnectedEvent();

    public static LobbyHostBeginHostingEvent lobbyHostBeginHostingEvent = new LobbyHostBeginHostingEvent();

    public static LobbyClientFailedToConnectToHostEvent lobbyClientFailedToConnectToHostEvent = new LobbyClientFailedToConnectToHostEvent();


    public static LobbyClientPlayerConnectedToLobbyEvent lobbyClientPlayerConnectedToLobbyEvent = new LobbyClientPlayerConnectedToLobbyEvent();

    public static LobbyClientPlayerDisconnectFromLobbyEvent lobbyClientPlayerDisconnectFromLobbyEvent = new LobbyClientPlayerDisconnectFromLobbyEvent();

    public static LobbyOtherPlayerConnectEvent lobbyOtherPlayerConnectEvent = new LobbyOtherPlayerConnectEvent();

    public static LobbyOtherPlayerDisconnectedEvent lobbyOtherPlayerDisconnectedEvent = new LobbyOtherPlayerDisconnectedEvent();

    public static PlayerSentChatMessageEvent playerSentChatMessageEvent = new PlayerSentChatMessageEvent();

    public static PlayerRecievedChatMessageEvent playerRecievedChatMessageEvent = new PlayerRecievedChatMessageEvent();

    public static SurvivorFailedToUnlockDoorEvent survivorFailedToUnlockDoorEvent = new SurvivorFailedToUnlockDoorEvent();

    public static SurvivorUnlockDoorEvent survivorUnlockDoorEvent = new SurvivorUnlockDoorEvent();

    public static PlayerConnectedEvent playerConnectedEvent = new PlayerConnectedEvent();

    public static PlayerDisconnectedEvent playerDisconnectedEvent = new PlayerDisconnectedEvent();

    public static PlayerChangedNameEvent playerChangedNameEvent = new PlayerChangedNameEvent();

    public static PlayerClientChangedNameEvent playerClientChangedNameEvent = new PlayerClientChangedNameEvent();

    public static MonsterWonEvent monsterWonEvent = new MonsterWonEvent();

    public static FailedToLoadStageEvent failedToLoadStageEvent = new FailedToLoadStageEvent();

    public static LurkerChangedFormEvent lurkerChangedFormEvent = new LurkerChangedFormEvent();

    public static LurkerReadyToGoIntoPhysicalFormEvent lurkerReadyToGoIntoPhysicalFormEvent = new LurkerReadyToGoIntoPhysicalFormEvent();

    public static MonsterSpawnedInStageEvent monsterSpawnedInStageEvent = new MonsterSpawnedInStageEvent();

    public static MaryReadyToFrenzyEvent maryReadyToFrenzyEvent = new MaryReadyToFrenzyEvent();

    public static MaryReadyToTeleportEvent maryReadyToTeleportEvent = new MaryReadyToTeleportEvent();

    public static InvalidLobbyNameEvent invalidLobbyNameEvent = new InvalidLobbyNameEvent();
}


#region Survivor_Events


[System.Serializable]
public class SurvivorDeathEvent : UnityEvent<string> { }

[System.Serializable]
public class SurvivorUnlockDoorEvent : UnityEvent<string, string, string> { }

[System.Serializable]
public class SurvivorFailedToUnlockDoorEvent : UnityEvent<Door> { }


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

//TODO. Figure out if we need this or not.
public class FlashlightEvent : UnityEvent<Flashlight> { }

[System.Serializable]
public class SurvivorAlreadyHaveKeyEvent : UnityEvent { }

[System.Serializable]
public class PlayerSentChatMessageEvent : UnityEvent<string, string> { }

public class PlayerRecievedChatMessageEvent : UnityEvent<string, string> { }


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

public class SurvivorTriggeredTrapEvent : UnityEvent<Trap> { }

[System.Serializable]
public class MonsterArmedTrapEvent : UnityEvent<Trap> { }

#endregion


#region STAGE_EVENTS

[System.Serializable]
public class SurvivorsEscapedStageEvent : UnityEvent { }


[System.Serializable]
public class FailedToLoadStageEvent : UnityEvent<string> { }

public class MonsterWonEvent : UnityEvent { }


public class MonsterSpawnedInStageEvent : UnityEvent<int> { }

public class LurkerSpawnedInStageEvent : UnityEvent { }

public class PhantomSpawnedInStageEvent : UnityEvent { }

public class MarySpawendInStageEvent : UnityEvent { }

public class FallenSpawnedInStageEvent : UnityEvent { }


#endregion

#region LURKER_EVENTS

public class LurkerChangedFormEvent : UnityEvent<bool> { }


public class LurkerReadyToGoIntoPhysicalFormEvent : UnityEvent { }


#endregion


#region MARY_EVENTS

public class MaryReadyToFrenzyEvent : UnityEvent { }

public class MaryReadyToTeleportEvent : UnityEvent { }

#endregion


#region LOBBY_EVENTS

#endregion


#region UI_EVENTS

public class InvalidLobbyNameEvent : UnityEvent { }

#endregion


#region NETWORK_EVENTS


[System.Serializable]
public class PlayerConnectedEvent : UnityEvent<string> { }

[System.Serializable]
public class PlayerDisconnectedEvent : UnityEvent<string> { }

public class PlayerChangedNameEvent: UnityEvent<string, string>{}

public class PlayerClientChangedNameEvent: UnityEvent<string, string>{}

public class PlayerPickedUpKeyEvent : UnityEvent<string, string> { }


#region LOBBY

//NOTE: Host events.

public class LobbyHostPlayerConnectedEvent: UnityEvent<string, int>{}

public class LobbyHostPlayerDisconnectedEvent: UnityEvent<string, int>{}

public class LobbyHostBeginHostingEvent: UnityEvent{}

//NOTE: Client events.
public class LobbyClientFailedToConnectToHostEvent: UnityEvent<int>{}

public class LobbyClientPlayerConnectedToLobbyEvent: UnityEvent{}

public class LobbyClientPlayerDisconnectFromLobbyEvent: UnityEvent{}

public class LobbyOtherPlayerConnectEvent: UnityEvent<string>{};

public class LobbyOtherPlayerDisconnectedEvent: UnityEvent<string, int>{}

#endregion

#endregion


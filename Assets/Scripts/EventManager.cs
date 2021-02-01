using UnityEngine.Events;
using Mirror;

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

    public static ServerAskedYouToPickCharacterEvent serverAskedYouToPickCharacterEvent = new ServerAskedYouToPickCharacterEvent();

    public static ServerLeftGameEvent serverLeftGameEvent = new ServerLeftGameEvent();

    public static PlayerJoinedEvent playerJoinedEvent = new PlayerJoinedEvent();

    public static StageClientServerDisconnectdEvent stageClientServerDisconnectdEvent = new StageClientServerDisconnectdEvent();



    public static LobbyServerChangedInsanityOptionEvent lobbyServerChangedInsanityOptionEvent = new LobbyServerChangedInsanityOptionEvent();

#region LOBBY_SERVER

    public static LobbyServerKickedEvent lobbyServerKickedEvent = new LobbyServerKickedEvent();

    public static LobbyServerChangedAllRandomEvent lobbyServerChangedAllRandomEvent = new LobbyServerChangedAllRandomEvent();
    
    public static LobbyServerChangedAllowSpecatorEvent lobbyServerChangedAllowSpectatorEvent = new LobbyServerChangedAllowSpecatorEvent(); 

    public static LobbyServerChangedStageEvent lobbyServerChangedStageEvent = new LobbyServerChangedStageEvent();

    public static LobbyServerChangedGamemodeEvent lobbyServerChangedGamemodeEvent = new LobbyServerChangedGamemodeEvent();

    public static LobbyServerStartedGameEvent lobbyServerStartedGameEvent = new LobbyServerStartedGameEvent();


#endregion

#region LOBBY_CLIENT

    //NOTE: Lobby client.

    public static LobbyClientKickedEvent lobbyClientKickedEvent = new LobbyClientKickedEvent();

    public static LobbyClientHostChangedGamemodeEvent lobbyClientHostChangedGamemodeEvent = new LobbyClientHostChangedGamemodeEvent();

    public static LobbyYouChangedCharacterEvent lobbyYouChangedCharacterEvent = new LobbyYouChangedCharacterEvent();

    public static LobbyClientPlayerJoinedEvent lobbyClientPlayerJoinedEvent = new LobbyClientPlayerJoinedEvent();

    public static LobbyClientPlayerLeftEvent lobbyClientPlayerLeftEvent = new LobbyClientPlayerLeftEvent();

    public static LobbyClientPlayerChangedCharacterEvent lobbyClientPlayerChangedCharacterEvent = new LobbyClientPlayerChangedCharacterEvent();

    public static LobbyClientPlayerSentChatMessageEvent lobbyClientPlayerSentChatMessageEvent = new LobbyClientPlayerSentChatMessageEvent();

    public static LobbyClientServerDisconnectedEvent lobbyClientServerDisconnectedEvent = new LobbyClientServerDisconnectedEvent();

    public static LobbyClientHostChangedStageEvent lobbyClientHostChangedStageEvent = new LobbyClientHostChangedStageEvent();

    public static LobbyClientHostChangedAllowSpectatorEvent lobbyClientHostChangedAllowSpectatorEvent = new LobbyClientHostChangedAllowSpectatorEvent();

    public static LobbyClientHostChangedAllRandomEvent lobbyClientHostChangedAllRandomEvent = new LobbyClientHostChangedAllRandomEvent();


    public static LobbyClientHostChangedInsanityOptionEvent lobbyClientHostChangedInsanityOptionEvent = new LobbyClientHostChangedInsanityOptionEvent();

    public static LobbyYouHaveBeenKickedEvent lobbyYouHaveBeenKickedEvent = new LobbyYouHaveBeenKickedEvent();

#endregion

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

public class ServerAskedYouToPickCharacterEvent: UnityEvent<Character[]>{}

public class ServerLeftGameEvent: UnityEvent{}

public class PlayerJoinedEvent: UnityEvent<string>{}

public class StageClientServerDisconnectdEvent: UnityEvent{}

#endregion


#region LOBBY_SERVER_EVENTS

//NOTE: Host events.

public class LobbyHostBeginHostingEvent: UnityEvent{}

public class LobbyClientKickedEvent: UnityEvent<string, int>{}

public class LobbyServerKickedEvent: UnityEvent<int>{}

public class LobbyYouHaveBeenKickedEvent: UnityEvent{}

public class LobbyYouChangedCharacterEvent: UnityEvent<Character>{}

public class LobbyServerChangedInsanityOptionEvent: UnityEvent<bool>{}

public class LobbyServerChangedAllRandomEvent: UnityEvent<bool>{}

public class LobbyServerChangedAllowSpecatorEvent: UnityEvent<bool>{}

public class LobbyServerChangedStageEvent: UnityEvent<int>{}

public class LobbyServerChangedGamemodeEvent: UnityEvent<int>{}

public class LobbyServerStartedGameEvent: UnityEvent<string>{}

public class LobbyClientHostChangedInsanityOptionEvent: UnityEvent<bool>{}

public class LobbyClientHostChangedAllRandomEvent: UnityEvent<bool>{}

public class LobbyClientHostChangedAllowSpectatorEvent: UnityEvent<bool>{}

public class LobbyClientHostChangedStageEvent: UnityEvent<int>{}

public class LobbyClientHostChangedGamemodeEvent: UnityEvent<int>{}

public class LobbyClientPlayerJoinedEvent: UnityEvent<string, int>{}

public class LobbyClientPlayerLeftEvent: UnityEvent<string, int>{}

public class LobbyClientPlayerChangedCharacterEvent: UnityEvent<Character, int>{}

public class LobbyClientPlayerSentChatMessageEvent: UnityEvent<string, string>{}

public class LobbyClientServerDisconnectedEvent: UnityEvent{}




#endregion


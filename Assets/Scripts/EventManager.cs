using UnityEngine.Events;

public static class EventManager
{
    #region STAGE_CLIENT
    public static ClientServerGameSurvivorDeathEvent clientServerGameSurvivorDeathEvent = new ClientServerGameSurvivorDeathEvent();

    public static ClientServerGameSurvivorsEscapedEvent survivorsEscapedStageEvent = new ClientServerGameSurvivorsEscapedEvent();

    public static ClientServerGameSurvivorPickedUpKeyEvent clientServerGameSurvivorPickedUpKeyEvent = new ClientServerGameSurvivorPickedUpKeyEvent();

    public static ClientServerGameAlreadyHaveKeyEvent clientServerGameAlreadyHaveKeyEvent = new ClientServerGameAlreadyHaveKeyEvent();

    public static ClientServerGameAskedYouToPickCharacterEvent clientServerGameAskedYouToPickCharacterEvent = new ClientServerGameAskedYouToPickCharacterEvent();

    public static ClientServerGamePlayerJoinedEvent clientServerGamePlayerJoinedEvent = new ClientServerGamePlayerJoinedEvent();

    public static ClientServerGamePlayerDisconnectEvent ClientServerGamePlayerDisconnectEvent = new ClientServerGamePlayerDisconnectEvent();

    public static ClientServerGameYouPickedUpBatteryEvent clientServerGameYouPickedUpBatteryEvent = new ClientServerGameYouPickedUpBatteryEvent();

    public static ClientServerGameRejectBatteryPickupEvent clientServerGameRejectBatteryPickupEvent = new ClientServerGameRejectBatteryPickupEvent();

    public static ClientServerGamePlayerSentChatMessageEvent clientServerGamePlayerSentChatMessageEvent = new ClientServerGamePlayerSentChatMessageEvent();

    public static ClientServerGameSurvivorUnlockedDoorEvent clientServerGameSurvivorUnlockedDoorEvent = new ClientServerGameSurvivorUnlockedDoorEvent();

    public static ClientServerGameFaliedToUnlockDoorEvent clientServerGameFaliedToUnlockDoorEvent = new ClientServerGameFaliedToUnlockDoorEvent();

    public static ClientServerGamePlayerConnectedEvent clientServerGamePlayerConnectedEvent = new ClientServerGamePlayerConnectedEvent();

    public static ClientServerGamePlayerDisconnectedEvent clientServerGamePlayerDisconnectedEvent = new ClientServerGamePlayerDisconnectedEvent();

    public static ClientServerGamePlayerChangedNameEvent clientServerGamePlayerChangedNameEvent = new ClientServerGamePlayerChangedNameEvent();

    public static ClientServerGameMonsterWonEvent clientServerGameMonsterWonEvent = new ClientServerGameMonsterWonEvent();

    public static ClientServerGameLurkerReadyToGoIntoPhysicalFormEvent clientServerGameLurkerReadyToGoIntoPhysicalFormEvent = new ClientServerGameLurkerReadyToGoIntoPhysicalFormEvent();

    public static ClientServerGameMaryReadyToFrenzyEvent ClientServerGameMaryReadyToFrenzyEvent = new ClientServerGameMaryReadyToFrenzyEvent();

    public static ClientServerGameMaryReadyToTeleportEvent clientServerGameMaryReadyToTeleportEvent = new ClientServerGameMaryReadyToTeleportEvent();

#endregion

#region LOBBY_CLIENT

    //NOTE: Lobby client.

    public static ClientServerLobbyClientKickedEvent clientServerLobbyClientKickedEvent = new ClientServerLobbyClientKickedEvent();

    public static ClientServerLobbyHostChangedGamemodeEvent clientServerLobbyHostChangedGamemodeEvent = new ClientServerLobbyHostChangedGamemodeEvent();

    public static ClientServerLobbyPlayerJoinedEvent clientServerLobbyPlayerJoinedEvent = new ClientServerLobbyPlayerJoinedEvent();

    public static ClientServerLobbyPlayerDisconnectEvent clientServerLobbyPlayerDisconnectEvent = new ClientServerLobbyPlayerDisconnectEvent();

    public static ClientServerLobbyPlayerChangedCharacterEvent clientServerLobbyPlayerChangedCharacterEvent = new ClientServerLobbyPlayerChangedCharacterEvent();

    public static ClientServerLobbyPlayerSentChatMessageEvent clientServerLobbyPlayerSentChatMessageEvent = new ClientServerLobbyPlayerSentChatMessageEvent();

    public static ClientServerLobbyServerAssignedYouHostEvent clientServerLobbyServerAssignedYouHostEvent = new ClientServerLobbyServerAssignedYouHostEvent();


    public static ClientServerLobbyServerPickedNewHostEvent clientServerLobbyServerPickedNewHostEvent = new ClientServerLobbyServerPickedNewHostEvent();

    public static ClientServerLobbyHostChangedStageEvent clientServerLobbyHostChangedStageEvent = new ClientServerLobbyHostChangedStageEvent();

    public static ClientServerLobbyHostChangedAllowSpectatorEvent clientServerLobbyHostChangedAllowSpectatorEvent = new ClientServerLobbyHostChangedAllowSpectatorEvent();

    public static ClientServerLobbyHostChangedAllRandomEvent clientServerLobbyHostChangedAllRandomEvent = new ClientServerLobbyHostChangedAllRandomEvent();


    public static ClientServerLobbyHostChangedInsanityOptionEvent clientServerLobbyHostChangedInsanityOptionEvent = new ClientServerLobbyHostChangedInsanityOptionEvent();

    public static ClientServerLobbyHostKickedYouEvent clientServerLobbyHostKickedYouEvent = new ClientServerLobbyHostKickedYouEvent();

#endregion
    public static MasterServerClientSentUsLobbyListEvent masterServerClientSentUsLobbyListEvent = new MasterServerClientSentUsLobbyListEvent();


}


#region CLIENT_GAME_EVENTS


public class ClientServerGameSurvivorDeathEvent : UnityEvent<string> { }

public class ClientServerGameSurvivorUnlockedDoorEvent : UnityEvent<string, string, string> { }

public class ClientServerGameFaliedToUnlockDoorEvent: UnityEvent<float, float, float>{}

public class ClientServerGameRejectBatteryPickupEvent: UnityEvent{}

public class ClientServerGameYouPickedUpBatteryEvent: UnityEvent{}

public class ClientServerGamePlayerSentChatMessageEvent : UnityEvent<string, string> { }

#endregion

#region STAGE_EVENTS

public class ClientServerGameSurvivorsEscapedEvent : UnityEvent { }

public class ClientServerGameMonsterWonEvent : UnityEvent { }

#endregion

#region LURKER_EVENTS

public class LurkerChangedFormEvent : UnityEvent<bool> { }


public class ClientServerGameLurkerReadyToGoIntoPhysicalFormEvent : UnityEvent { }


#endregion


#region MARY_EVENTS

public class ClientServerGameMaryReadyToFrenzyEvent : UnityEvent { }

public class ClientServerGameMaryReadyToTeleportEvent : UnityEvent { }

#endregion


#region LOBBY_EVENTS

#endregion


#region UI_EVENTS

public class InvalidLobbyNameEvent : UnityEvent { }

#endregion


#region NETWORK_EVENTS


public class ClientServerGamePlayerConnectedEvent : UnityEvent<string> { }

public class ClientServerGamePlayerDisconnectedEvent : UnityEvent<string> { }

public class ClientServerGamePlayerChangedNameEvent: UnityEvent<string, string>{}

public class PlayerClientChangedNameEvent: UnityEvent<string, string>{}

public class ClientServerGameSurvivorPickedUpKeyEvent : UnityEvent<string, string> { }

public class ClientServerGameAlreadyHaveKeyEvent: UnityEvent{}

public class ClientServerGameAskedYouToPickCharacterEvent: UnityEvent<Character[]>{}

public class ServerLeftGameEvent: UnityEvent{}


public class ClientServerGamePlayerJoinedEvent: UnityEvent<string>{}

public class ClientServerGamePlayerDisconnectEvent: UnityEvent{}



#endregion


#region LOBBY_SERVER_EVENTS

//NOTE: Host events.

public class LobbyHostBeginHostingEvent: UnityEvent{}

public class ClientServerLobbyClientKickedEvent: UnityEvent<string, int>{}

public class LobbyServerKickedEvent: UnityEvent<int>{}

public class ClientServerLobbyHostKickedYouEvent: UnityEvent{}

public class LobbyYouChangedCharacterEvent: UnityEvent<Character>{}

public class LobbyServerChangedInsanityOptionEvent: UnityEvent<bool>{}

public class LobbyServerChangedAllRandomEvent: UnityEvent<bool>{}

public class LobbyServerChangedAllowSpecatorEvent: UnityEvent<bool>{}

public class LobbyServerChangedStageEvent: UnityEvent<int>{}

public class LobbyServerChangedGamemodeEvent: UnityEvent<int>{}

public class LobbyServerStartedGameEvent: UnityEvent<string>{}

public class ClientServerLobbyHostChangedInsanityOptionEvent: UnityEvent<bool>{}

public class ClientServerLobbyHostChangedAllRandomEvent: UnityEvent<bool>{}

public class ClientServerLobbyHostChangedAllowSpectatorEvent: UnityEvent<bool>{}

public class ClientServerLobbyHostChangedStageEvent: UnityEvent<int>{}

public class ClientServerLobbyHostChangedGamemodeEvent: UnityEvent<int>{}

public class ClientServerLobbyPlayerJoinedEvent: UnityEvent<string, int>{}

public class ClientServerLobbyPlayerDisconnectEvent: UnityEvent<string, int>{}

public class ClientServerLobbyPlayerChangedCharacterEvent: UnityEvent<Character, int>{}

public class ClientServerLobbyPlayerSentChatMessageEvent: UnityEvent<string, string>{}

public class LobbyClientServerDisconnectedEvent: UnityEvent{}

public class ClientServerLobbyServerPickedNewHostEvent: UnityEvent<string, int>{}

public class ClientServerLobbyServerAssignedYouHostEvent: UnityEvent<int>{}

public class DedicatedServerReceivedIdEvent: UnityEvent<int>{}

public class MasterServerClientSentUsLobbyListEvent: UnityEvent<Lobby[]>{}

public class ClientServerPickedNewHostEvent: UnityEvent<string>{}

#endregion


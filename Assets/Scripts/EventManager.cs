using UnityEngine.Events;

public static class EventManager
{
    #region STAGE_CLIENT
    public static ClientServerGameSurvivorDeathEvent clientServerGameSurvivorKilledEvent = new ClientServerGameSurvivorDeathEvent();

    public static ClientServerGameSurvivorsEscapedEvent clientServerGameSurvivorsEscapedEvent = new ClientServerGameSurvivorsEscapedEvent();

    public static ClientServerGameSurvivorsDeadEvent clientServerGameSurvivorsDeadEvent = new ClientServerGameSurvivorsDeadEvent();

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

    public static ClientServerGameLurkerTrapArmedEvent clientServerGameLurkerTrapArmedEvent = new ClientServerGameLurkerTrapArmedEvent();

    public static ClientServerGameLurkerArmableTrapsEvent clientServerGameLurkerArmableTrapsEvent = new ClientServerGameLurkerArmableTrapsEvent();

    public static ClientServerGameMaryReadyToFrenzyEvent clientServerGameMaryReadyToFrenzyEvent = new ClientServerGameMaryReadyToFrenzyEvent();

    public static ClientServerGameMaryFrenzyOverEvent clientServerGameMaryFrenzyOverEvent = new ClientServerGameMaryFrenzyOverEvent();

    public static ClientServerGameMaryReadyToTeleportEvent clientServerGameMaryReadyToTeleportEvent = new ClientServerGameMaryReadyToTeleportEvent();

    public static ClientServerGameHostStartedGameEvent clientServerGameHostStartedGameEvent = new ClientServerGameHostStartedGameEvent();

    public static ClientServerGameMaryServerTeleportedYouEvent clientServerGameMaryServerTeleportedYouEvent = new ClientServerGameMaryServerTeleportedYouEvent{};

    public static ClientServerGameMaryFrenziedEvent clientServerGameMaryFrenziedEvent = new ClientServerGameMaryFrenziedEvent();

#endregion

#region STAGE_SERVER

    public static ServerClientGameSurvivorsEscapedEvent serverClientGameSurvivorsEscapedEvent = new ServerClientGameSurvivorsEscapedEvent();

    public static ServerClientGameSurvivorsDeadEvent serverClientGameSurvivorsDeadEvent = new ServerClientGameSurvivorsDeadEvent();

    public static ServerClientGameLurkerJoinedEvent serverClientGameLurkerJoinedEvent = new ServerClientGameLurkerJoinedEvent();

    public static ServerClientGameMaryJoinedEvent serverClientGameMaryJoinedEvent = new ServerClientGameMaryJoinedEvent();

    public static ServerClientGamePhantomJoinedEvent serverClientGamePhantomJoinedEvent = new ServerClientGamePhantomJoinedEvent();

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
    public static MasterClientServerSentUsLobbyListEvent masterServerClientSentUsLobbyListEvent = new MasterClientServerSentUsLobbyListEvent();

    public static MasterClientServerAddedClientHostEvent masterClientServerAddedClientHostEvent = new MasterClientServerAddedClientHostEvent();

    public static MasterClientServerAddedDedicatedServerEvent masterClientServerAddedDedicatedServerEvent = new MasterClientServerAddedDedicatedServerEvent();


}


#region CLIENT_GAME_EVENTS


public class ClientServerGameSurvivorDeathEvent : UnityEvent<string> { }

public class ClientServerGameSurvivorUnlockedDoorEvent : UnityEvent<string, string, string> { }

public class ClientServerGameFaliedToUnlockDoorEvent: UnityEvent<float, float, float>{}

public class ClientServerGameRejectBatteryPickupEvent: UnityEvent{}

public class ClientServerGameYouPickedUpBatteryEvent: UnityEvent{}

public class ClientServerGamePlayerSentChatMessageEvent : UnityEvent<string> { }

public class ClientServerGameSurvivorsEscapedEvent: UnityEvent{}

public class ClientServerGameSurvivorsDeadEvent: UnityEvent{}

public class ClientServerGameMonsterWonEvent : UnityEvent { }

public class ClientServerGameLurkerReadyToGoIntoPhysicalFormEvent : UnityEvent { }

public class ClientServerGameLurkerTrapArmedEvent: UnityEvent{}

public class ClientServerGameLurkerArmableTrapsEvent: UnityEvent<uint[]>{}

#endregion

#region SERVER_GAME_EVENTS
public class ServerClientGameSurvivorsEscapedEvent: UnityEvent{}

public class ServerClientGameSurvivorsDeadEvent: UnityEvent{}

public class ServerClientGameLurkerJoinedEvent: UnityEvent<uint>{}

public class ServerClientGameMaryJoinedEvent: UnityEvent<uint>{}

public class ServerClientGamePhantomJoinedEvent: UnityEvent<uint>{}

#endregion

#region MARY_EVENTS

public class ClientServerGameMaryReadyToFrenzyEvent : UnityEvent { }

public class ClientServerGameMaryFrenzyOverEvent: UnityEvent{}

public class ClientServerGameMaryReadyToTeleportEvent : UnityEvent { }

public class ClientServerGameHostStartedGameEvent: UnityEvent{}

public class ClientServerGameMaryServerTeleportedYouEvent: UnityEvent<float, float, float>{}

public class ClientServerGameMaryFrenziedEvent: UnityEvent{}

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

public class ClientServerLobbyPlayerSentChatMessageEvent: UnityEvent<string>{}

public class LobbyClientServerDisconnectedEvent: UnityEvent{}

public class ClientServerLobbyServerPickedNewHostEvent: UnityEvent<string, int>{}

public class ClientServerLobbyServerAssignedYouHostEvent: UnityEvent<int>{}

public class DedicatedServerReceivedIdEvent: UnityEvent<int>{}

public class MasterClientServerSentUsLobbyListEvent: UnityEvent<Lobby[]>{}

public class MasterClientServerAddedClientHostEvent: UnityEvent<int>{}
public class MasterClientServerAddedDedicatedServerEvent: UnityEvent<int>{}

public class ClientServerPickedNewHostEvent: UnityEvent<string>{}

#endregion
#endregion

using Mirror;
using UnityEngine;

// NOTE: These are the messages that the server/host will get from the clients. The implementation for all of this will all happen in DarnedNetworkManager.cs
#region SERVER_MESSAGES

// If a client sends us this, then that means they toggled their flashlight. 
public struct ServerClientGameToggledFlashlightMessage: NetworkMessage
{
    public bool toggled;
}

// If a client sends us this, then that means that they clicked on a battery and we need to make sure that they are allowed to do that. Checks must be done on the server side.
public struct ServerClientGameClickedOnBatteryMessage: NetworkMessage
{
    public uint requestedBatteryId;
}

// If a client sends us this, then that means that they clicked on a door and we need to tell them whether or not they can unlock it. Checks must be done on the server side.
public struct ServerClientGameClickedOnDoorMessage: NetworkMessage
{
    public uint requestedDoorID;
}

// If a client sends us this, then that means they requested to change the stage for the server. Only a host should be able to do this.
public struct ServerClientLobbyRequestedToChangeStageMessage: NetworkMessage
{
    public StageName requestedStageValue;
}

// If a client sends us this, then that means they requested to change the insanity option in the lobby. Only a host should be able to do this.
public struct ServerClientLobbyRequestedToChangeInsanityMessage: NetworkMessage
{
    public bool requestedInsanityValue;
}

public struct ServerClientLobbyRequestedToChangeAllowSpectatorMessage: NetworkMessage
{
    public bool requestedAllowSpectatorValue;
    
}

// If a client sends us this, then that means a client requested to change the All Random option in the lobby. Only a host should be able to do this.
public struct ServerClientLobbyRequestedToChangeAllRandomMessage: NetworkMessage
{
    public bool requestedAllRandomValue;
}

public struct ServerClientLobbyRequestedUnavailableCharactersMessage: NetworkMessage
{

}

// If a client sends us this, then that means a client requested to change their character. Checks will be done on the server side.
public struct ServerClientLobbyRequestedCharacterChangeMessage: NetworkMessage
{
    public Character requestedCharacter;
}

// If a client sends us this, then that means that a client requested to start the game. Only a host should be able do to this.
public struct ServerClientLobbyRequestedToStartGameMessage: NetworkMessage
{

}

public struct ServerClientLobbyHostRequestToKickPlayerMessage: NetworkMessage
{
    public NetworkIdentity playerIdentity;
}

public struct ServerClientLobbyRequestedToChangeGamemodeMessage: NetworkMessage
{
    public int requestedGamemodeValue;
}

// If a client sends us this, then that means that a client rquested to pick up a key. Checks should be done on the server side.
public struct ServerClientGameClickedOnKeyMessage: NetworkMessage
{
    public uint requestedKeyId;
}

public struct ServerClientLobbyRequestedToChangeVoiceChatMessage: NetworkMessage
{
    public bool requestedVoiceChatValue;

}

public struct ServerClientGamePlayerPickedCharacterMessage: NetworkMessage
{
    public Character pickedCharacter;
}

public struct ServerClientPlayerSentChatMessage: NetworkMessage
{
    public string playerName;
    public string text;
}

public struct ServerClientGameSurvivorEscapedMessage: NetworkMessage
{

}

public struct ServerClientGameSurvivorNoLongerEscapedMessage: NetworkMessage
{

}

#endregion

//NOTE: These messages are what clients will get from the host/server when the server sends it. The logic for all of these are implemented in DarnedNetworkManager.cs.
#region CLIENT_MESSAGES


public struct ClientServerLobbyHostChangedInsanityOption: NetworkMessage
{
    public bool newInsanityValue;
}

public struct ClientServerLobbyHostChangedVoiceChatMessage: NetworkMessage
{
    public bool newVoiceChatValue;
}

// If the server sends us this, then that means that the server rejected our character change request because another client has already chosen that character.
public struct ClientServerLobbyCharacterAlreadyTakenMessage: NetworkMessage
{

}

// If the server sends us this, then that means the server told us that we already have the key (same mask) we clicked on.
public struct ClientServerGameAlreadyHaveKeyMessage: NetworkMessage
{

}

public struct ClientServerLobbyPlayerChangedCharacterMessage: NetworkMessage
{
    // Who is the new character that the player picked?
    public Character newCharacter;
    // Index of the person who picked the new character
    public byte playerIndexInLobby;

}

public struct ClientServerLobbyHostChangedStageMessage: NetworkMessage
{
    public StageName newStage;
}

public struct ClientServerLobbyHostChangedAllRandomOption: NetworkMessage
{
    public bool newAllRandomValue;
}

public struct ClientServerLobbyHostChangedAllowSpectatorOptinon: NetworkMessage
{
    public bool newAllowSpectatorValue;
}

public struct ClientServerLobbyHostChangedGamemodeOption: NetworkMessage
{
    public int newGamemodeValue;
}

public struct ClientServerLobbyPlayerDisconnectedMessage: NetworkMessage
{
    public string disconnectedClientName;
    public int disconnectedClientIndex;
}

public struct ClientServerLobbyHostKickedYouMessage: NetworkMessage
{

}

public struct ClientServerLobbyHostKickedPlayerMessage: NetworkMessage
{
    public string kickedClientName;
    public int index;

}

// If the server sends this, then that means that the server said that we cannot unlock that door because it failed its required checks.
public struct ClientServerGameDoorFailedToUnlockMessage: NetworkMessage
{
    // Where should we play the sound?
    public Vector3 position;

}

// If the server sends us is, then that means the server said that the door is now unlocked.
public struct ClientServerGameDoorUnlockedMessage: NetworkMessage
{
    // Who unlocked the door?
    public string playerName;
    // What kind of door is it?
    public string doorName;
}

// If the server sends us this, then that means the server  has allowed a fellow survivor to pick up a key.
public struct ClientServerGameSurvivorPickedUpKeyMessage: NetworkMessage
{
    // Who picked up a key?
    public string playerName;
    // What kind of key is it?
    public string keyName;
}

// If the server sends us this, then that means the server has allowed us to pick up the key we clicked on.
public struct ClientServerGameYouPickedUpKeyMessage: NetworkMessage
{
    // What kind of key is it?
    public string keyName;

}

// If the server sends us this, then that means the server has allowed us to pick up the battery we clicked on.
public struct ClientServerGameYouPickedUpBatteryMessage: NetworkMessage
{

}

// If the server sends us this, then that means the server made us host because the old host of the lobby has left.
public struct ClientServerLobbyMadeYouHostMessage: NetworkMessage
{
    public int index;

}

// If the server sends us this, then that means the server made someone else an new host because the old host has left.
public struct ClientServerLobbyMadeSomeoneHostMessage: NetworkMessage
{
    // Who is the new host?
    public string newHostName;
    public int index;
}

// If the server sends us this, then that means it has rejected our request to pick up a battery to recharge our flashlight.
public struct ClientServerGameRejectedBatteryPickupMessage: NetworkMessage
{

}

// If the server sends us this, then that means someone picked up a battery to recharge their flashlight.
public struct ClientServerGameBatteryPickedUp: NetworkMessage
{
    public int batteryID;
}

// If the server sends us this, then that means that a fellow survivor has toggled their flashlight.
public struct ClientServerGamePlayerToggledFlashlight: NetworkMessage
{

}

// If the server sends us this, then that means that the server decided to let us know the latest value of a survivors flashlight. This will only happen a little bit to avoid network spam.
public struct ClientServerGamePlayerUpdatedFlashlightValue: NetworkMessage
{
    public float newValue;
}

public struct ClientServerGameSurvivorHitTrapMessage: NetworkMessage
{

}

public struct ClientServerPlayerConnectedMessage: NetworkMessage
{
    public string name;

}

public struct ClientServerLobbyPlayerJoinedMessage: NetworkMessage
{
    public string clientName;
    public int index;

}
public struct ClientServerLobbyPlayerSentChatMessage: NetworkMessage
{
    public string clientName;
    public string text;
}

public struct ClientServerGameSurvivorsEscapedMessage: NetworkMessage
{

}

public struct ClientServerLobbyUnavailableCharactersMessage: NetworkMessage
{
    public Character[] unavailableCharacters;
}
public struct ClientServerGamePickCharacterMessage: NetworkMessage
{
    public Character[] unavailableCharacters;
}

#endregion

#region MASTER_SERVER

public struct MasterServerClientRequestServerListMessage: NetworkMessage
{

}

public struct MasterServerDedicatedServerRequestToBeAddedMessage: NetworkMessage
{
    public string lobbyName;
    public string password;
    public bool isPrivate;

}

public struct MasterServerClientHostRequestToBeAddedMessage: NetworkMessage
{
    public string lobbyName;
    public string password;
    public bool isPrivate;

}

public struct MasterServerClientHostRequestToBeRemovedMessage: NetworkMessage
{
    public int clientHostServerId;

}

public struct MasterServerDedicatedServerRequestToBeRemovedMessage: NetworkMessage
{
    public int dedicatedServerId;

}

#endregion

#region MASTER_CLIENT

public struct MasterClientServerAddedClientHostServerMessage: NetworkMessage
{

}

// IF the master server sends us this, then that means it added our dedicated server do the master server list
public struct MasterClientServerAddedDedicatedServerMessage: NetworkMessage
{
    // The id that the master server gave us. Store this for when the dedicated server disconnects and requests to be removed from the server listing.
    public int id;

}

public struct MasterClientServerSentServerListingMessage: NetworkMessage
{
    public Lobby[] lobbiesOnServer;

}

#endregion

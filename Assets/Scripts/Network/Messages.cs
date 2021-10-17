using Mirror;
using UnityEngine;

// NOTE: These are the messages that the server/host will get from the clients. The implementation for all of this will all happen in DarnedNetworkManager.cs
#region SERVER_MESSAGES

// If a client sends us this, then that means they toggled their flashlight. 
public struct ServerClientGameToggledFlashlightMessage: NetworkMessage
{

}

public struct ServerClientGameHostRequestedToStartGameMessage: NetworkMessage
{

}

// If a client sends us this, then that means that they clicked on a battery and we need to make sure that they are allowed to do that. Checks must be done on the server side.
public struct ServerClientGameClickedOnBatteryMessage: NetworkMessage
{
    public uint requestedBatteryId;
}

public struct ServerClientGamePlayerChangedProfileNameMessage: NetworkMessage
{
    public string oldProfileName;
    public string newProfileName;
}

// If a client sends us this, then that means that they clicked on a door and we need to tell them whether or not they can unlock it. Checks must be done on the server side.
public struct ServerClientGameClickedOnDoorMessage: NetworkMessage
{
    public uint requestedDoorID;
}


public struct ServerClientGameDoorBumpedIntoMessage: NetworkMessage
{
    public uint requestedDoorID;

    public Vector3 moveDirection;

}
public struct ServerClientGamePlayerSentChatMessage: NetworkMessage
{
    public string chatText;
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

public struct ServerClientLobbyPlayerJoinedMessage: NetworkMessage
{

}

public struct ServerClientGamePlayerSpectatorJoinedMessage: NetworkMessage
{

}

public struct ServerClientGamePlayerPickedCharacterMessage: NetworkMessage
{
    public Character pickedCharacter;
}

public struct ServerClientLobbyPlayerSentChatMessage: NetworkMessage
{
    public string text;
}

public struct ServerClientGameSurvivorEscapedMessage: NetworkMessage
{

}

public struct ServerClientGameSurvivorNoLongerEscapedMessage: NetworkMessage
{

}


public struct ServerClientGameLurkerRequestToChangeFormMessage: NetworkMessage
{

}


public struct ServerClientGameLurkerSwingAttackMessage: NetworkMessage
{
    public uint requestedTargetId;
}

public struct ServerClientGameLurkerSwingAtNothingMessage: NetworkMessage
{

}

public struct ServerClientGameLurkerRequestToArmTrapMessage: NetworkMessage
{
    public uint requestedTrapId;

}

public struct ServerClientGamePhantomSwingAtNothingMessage: NetworkMessage
{

}

public struct ServerClientGamePhantomSwingAttackMessage: NetworkMessage
{
    public uint requestedSurvivorId;

}

public struct ServerClientGameLurkerJoinedMessage: NetworkMessage
{

}

public struct ServerClientGameMaryJoinedMessage: NetworkMessage
{

}

public struct ServerClientGameMaryTeleportRequest: NetworkMessage
{

}

public struct ServerClientGameMaryFrenzyRequest: NetworkMessage
{

}

public struct ServerClientGameMaryAttackedSurvivorMessage: NetworkMessage
{
    public uint requestedSurvivorId;
}

public struct ServerClientGameMaryAttackedNothingMessage: NetworkMessage
{

}

public struct ServerClientGamePhantomJoinedMessage: NetworkMessage
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

public struct ClientServerGamePlayerChangedProfileNameMessage: NetworkMessage
{
    public string oldName;
    public string newName;

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
    public int playerIndexInLobby;

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
    // The door id. We need this to play the sound associated with the door itself.
    public uint doorId;
}

public struct ClientServerGameSurvivorKilledMessage: NetworkMessage
{
    public uint survivorId;

}

// If the server sends us is, then that means the server said that the door is now unlocked.
public struct ClientServerGameDoorUnlockedMessage: NetworkMessage
{

    // The door id. We need this to play the sound associated with the door itself.
    public uint doorId;
    // Who unlocked the door?
    public string playerName;
    // The name of the key that unlocked the door.
    public string keyName;
}

public struct ClientServerGameTrapTriggeredMessage: NetworkMessage
{
    public uint triggeredTrapId;

}

// If the server sends us this, then that means the server  has allowed a fellow survivor to pick up a key.
public struct ClientServerGameSurvivorPickedUpKeyMessage: NetworkMessage
{
    // net ID of the key. Needed to send to client so they know which sound to play for the key
    public uint keyId;

    // net id of the survivor
    public uint survivorId;
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


public struct ClientServerGameLurkerTrapArmedMessage: NetworkMessage
{
    public uint trapId;
}

public struct ClientServerGameLurkerAttackedMessage: NetworkMessage
{
    public uint lurkerId;

}

public struct ClientServerGamePhantomAttackedMessage: NetworkMessage
{
    public uint phantomId;

}

public struct ClientServerGamePhantomSurvivorDetectedMesasge: NetworkMessage
{
    public uint detectedSurvivorId;

}

public struct ClientServerGamePhantomSurvivorNoLongerDetected: NetworkMessage
{
    public uint noLongerDetectedSurvivorId;
}

public struct ClientServerGameMaryReadyToFrenzyMessage: NetworkMessage
{

}

public struct ClientServerGameMaryAttackedMessage: NetworkMessage
{
    public uint maryId;

}

public struct ClientServerGameMaryReadyToTeleportMessage: NetworkMessage
{

}

public struct ClientServerGameMaryAutoTeleportMessage: NetworkMessage
{
    public Vector3 newPosition;
}

public struct ClientServerGameMaryFrenzyOverMessage: NetworkMessage
{
    public uint maryId;
}

public struct ClientServerGameMaryAllowTeleportMessage: NetworkMessage
{
    public Vector3 newPosition;
}

public struct ClientServerGameMaryFrenziedMessage: NetworkMessage
{
    public uint maryId;

}



public struct ClientServerGameLurkerArmableTrapsMessage: NetworkMessage
{
    public uint[] armableTraps;
}

public struct ClientServerGameLurkerAllowGhostToPhysicalFormChangeMessage: NetworkMessage
{

}

public struct ClientServerGameLurkerAllowPhysicalToGhostFormChangeMessage: NetworkMessage
{

}

public struct ClientServerGameLurkerReadyToGoIntoPhysicalFormMessage: NetworkMessage
{

}


public struct ClientServerGameLurkerForceBackIntoGhostFormMessage: NetworkMessage
{

}


public struct ClientServerGamePlayerConnectedMessage: NetworkMessage
{
    public string name;

}

public struct ClientServerGamePlayerJoinedMessage: NetworkMessage
{
    public string name;

}

public struct ClientServerGamePlayerDisconnectedMessage: NetworkMessage
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
    public string chatMessage;
}


public struct ClientServerGameSurvivorsEscapedMessage: NetworkMessage
{

}

public struct ClientServerGameSurvivorsDeadMessage: NetworkMessage
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

public struct ClientServerGameHostStartedGameMessage: NetworkMessage
{

}

public struct ClientServerGamePlayerSentChatMessage: NetworkMessage
{
    public string chatMessage;

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
    // The id that the master server gave us. Store this for when the dedicated server disconnects and requests to be removed from the server listing.
    public int id;

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

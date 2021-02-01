using Mirror;

public struct PlayerChatMessage
{
    public string survivorName;
    public string text;
}

public struct PlayerUnlockedDoorMessage
{
    public string playerName;
    public string doorName;
    public string keyName;
}

public struct PlayerPickedUpKeyMessage
{
    public string playerName;
    public string keyName;
}

public struct PlayerConnectMessage
{
    public string playerName;
}

public struct PlayerJoinMessage
{
    public string playerName;
}

public struct PlayerDisconnectMessage
{
    public string playerName;
}

public struct PlayerAddKeyToInventoryMessage
{
    public KeyObject key;
}

#region LOBBY

// NOTE: This is kind of dumb but whatever. It's a work around for now. I may change this.
//
public struct LobbyClientCharacterChangedMessage: NetworkMessage
{
    public int character;
}

public struct LobbyHostClientCharacterChangedMessage: NetworkMessage
{
    public int character;
    public int playerNumber;
    public string playerName;
}

public struct PlayerOneCharacterChanged : NetworkMessage
{
    public int character;
}
public struct PlayerTwoCharacterChanged : NetworkMessage
{
    public int character;
}
public struct PlayerThreeCharacterChanged : NetworkMessage
{
    public int character;
}
public struct PlayerFourCharacterChanged : NetworkMessage
{
    public int character;
}
public struct PlayerFiveCharacterChanged : NetworkMessage
{
    public int character;
}



public struct LobbyOtherPlayerConnectedMessage: NetworkMessage
{
    public string playerName;
}

public struct LobbyOtherPlayerDisconnectedMessage: NetworkMessage
{
    public string playerName;
}

public struct LobbyHostClientDisconnectedMessage: NetworkMessage
{
    public string playerName;
}

public struct LobbyHostClientConnected: NetworkMessage
{
    public string playerName;
}

public struct HostKickedPlayerMessage: NetworkMessage
{
    public int lobbyPlayerNumber;
    public string lobbyPlayerName;

}


#region LOBBY_SERVER

public struct LobbyServerClientChangedCharacterMessage: NetworkMessage
{
    public Character newValue;
}


#endregion

#region LOBBY_CLIENT

public struct LobbyClientChangedAllRandomMessage: NetworkMessage
{
    public bool newOption;

}

public struct LobbyClientKickedMessage: NetworkMessage
{
    public string kickedClientName;
    public int index;
}

public struct LobbyServerKickedMessage: NetworkMessage
{
    public string kickedClientName;
    public int index;
}

public struct LobbyClientYouHaveBeenKickedMessage: NetworkMessage
{

}
public struct LobbyClientChangedAllowSpectatorOptionMessage: NetworkMessage
{
    public bool newOption;
}

public struct LobbyClientChangedStageMessage: NetworkMessage
{
    public int newOption;

}

public struct LobbyClientChangedInsanityOptionMessage: NetworkMessage
{
    public bool newOption;
}

public struct LobbyClientChangedGamemodeOptionMessage: NetworkMessage
{
    public int newOption;

}

public struct LobbyClientPlayerJoinedMessage: NetworkMessage
{
    public string clientName;
    public int index;
}

public struct LobbyClientPlayerLeftMessage: NetworkMessage
{
    public string clientName;
    public int index;
}

public struct LobbyClientPlayerChangedCharacterMessage: NetworkMessage
{
    public Character newCharacter;
    public int index;
}

public struct ServerPlayerSentChatMessage : NetworkMessage
{
    public string playerName;
    public string text;
}

public struct ClientPlayerSentChatMessage : NetworkMessage
{
    public string playerName;
    public string text;
}

public struct ServerPlayerChangedProfileNameMessage: NetworkMessage
{
    public string oldName;
    public string newName;
}

public struct ClientPlayerChangedProfileNameMessage: NetworkMessage
{
    public string oldName;
    public string newName;
}

public struct ClientPickCharacterMessage: NetworkMessage
{
    public Character [] availableCharacters;
}

public struct ServerClientPickedCharacterMessage: NetworkMessage
{
    public Character pickedCharacter;
}

public struct ServerPlayerJoinedMessage: NetworkMessage
{
    public string clientName;
    public NetworkIdentity clientIdentity;
}

public struct ClientPlayerJoinedMessage: NetworkMessage
{
    public string clientName;
}

public struct ClientPlayerDisconnectedMessage: NetworkMessage
{
    public string clientName;
}

public struct LobbyServerPlayerChatMessage: NetworkMessage
{
    public string clientName;
    public string text;
}

public struct LobbyClientPlayerChatMessage: NetworkMessage
{
    public string clientName;
    public string text;
}

public struct ServerClientLoadedSceneMessage: NetworkMessage
{

}

public struct ClientServerDisconnectedMessage: NetworkMessage
{

}

public struct ClientServerChangeSceneMessage: NetworkMessage
{
    public bool newValue;
}

#endregion

#endregion

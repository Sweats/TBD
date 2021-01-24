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

public struct HostChangedStageSettingMessage: NetworkMessage
{
    public StageName newStageName;
}

public struct HostDisconnectedMessage: NetworkMessage
{

}

public struct HostChangedInsanityOptionSettingMessage: NetworkMessage
{
    public bool insanityEnabled;
}

public struct HostChangedGameModeSettingMessage: NetworkMessage
{
    public int gameMode;
}

public struct HostChangedAllRandomOptionMessage: NetworkMessage
{
    public bool allRandomEnabled;
}

public struct HostChangedAllowSpectatorOptionMessage: NetworkMessage
{
    public bool allowSpectator;
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



#endregion


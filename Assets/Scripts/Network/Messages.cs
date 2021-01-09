public struct PlayerChatMessage
{
    public string survivorName;
    public string text;
}

public struct PlayerPickedUpKeyMessage
{
    public string playerName;
    public string keyName;
}

public struct PlayerUnlockedDoorMessage
{
    public string playerName;
    public string doorName;
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

#region LOBBY

public struct PlayerChangedCharacterMessage
{
    public int clientID;
    public int character;
}

public struct HostChangedStageSettingMessage
{
    public int newSetting;
}

#endregion


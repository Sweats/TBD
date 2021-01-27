using UnityEngine;
using Mirror;
using System.Collections.Generic;


#region SERVER

public struct LobbyPlayerStruct
{
    public string clientName;
    public Character character;
    public NetworkIdentity identity;

}

// NOTE: I know that I could have used SyncVars but I wanted more control on how and when things are synced. So I did it by hand.
// I also think it's really dumb that we can't use attributes like [Client] or [Server] inside a class that inherits from
// NetworkManager. Maybe I'm just bad at using Mirror.
// Had I used SyncVars, I would've had to seperate the code logic into 2 different files and that just makes things more confusing
// in my opinion.

// TODO; Host migration! 

public class NetworkRoom : NetworkManager
{
    private List<LobbyPlayerStruct> players;

    private bool allRandom;

    private bool insanityOption;

    private bool allowSpectator;

    private int selectedStage;

    private int selectedGamemode;
         
    public override void OnStartServer()
    {
        players = new List<LobbyPlayerStruct>();

        NetworkServer.RegisterHandler<LobbyServerPlayerJoinedMessage>(ServerOnPlayerJoined);
        NetworkServer.RegisterHandler<LobbyServerClientChangedCharacterMessage>(ServerOnClientChangedCharacter);

        EventManager.lobbyServerKickedEvent.AddListener(ServerOnKickedPlayerEvent);
        EventManager.lobbyServerChangedStageEvent.AddListener(ServerOnChangedStageEvent);
        EventManager.lobbyServerChangedGamemodeEvent.AddListener(ServerOnChangedGamemodeEvent);
        EventManager.lobbyServerChangedAllRandomEvent.AddListener(ServerOnChangedAllRandomEvent);
        EventManager.lobbyServerChangedAllowSpectatorEvent.AddListener(ServerOnChangedAllowSpectatorEvent);
        EventManager.lobbyServerChangedInsanityOptionEvent.AddListener(ServerOnChangedInsanityOptionEvent);
        base.OnStartServer();
    }

    public override void OnServerDisconnect(NetworkConnection connection)
    {
        uint connectionId = connection.identity.netId;
        int foundIndex = GetIndexOfPlayer(connectionId);

        if (foundIndex != -1)
        {
            LobbyPlayerStruct player = players[foundIndex];
            string name = player.clientName;
            players.RemoveAt(foundIndex);
            NetworkServer.SendToAll(new LobbyClientPlayerLeftMessage { clientName = name, index = foundIndex });
        }

        base.OnServerDisconnect(connection);
    }

    private void ServerOnChangedStageEvent(int newValue)
    {
        if (NetworkServer.localClientActive)
        {
            selectedStage = newValue;
            NetworkServer.SendToAll(new LobbyClientChangedStageMessage { newOption = newValue });
        }
    }

    private void ServerOnChangedGamemodeEvent(int newValue)
    {
        if (NetworkServer.localClientActive)
        {
            selectedGamemode = newValue;
            NetworkServer.SendToAll(new LobbyClientChangedGamemodeOptionMessage { newOption = newValue });

        }
    }

    private void ServerOnChangedAllRandomEvent(bool newValue)
    {
        if (NetworkServer.localClientActive)
        {
            allRandom = newValue;
            NetworkServer.SendToAll(new LobbyClientChangedAllRandomMessage { newOption = newValue });
        }
    }

    private void ServerOnChangedAllowSpectatorEvent(bool newValue)
    {
        if (NetworkServer.localClientActive)
        {
            allowSpectator = newValue;
            NetworkServer.SendToAll(new LobbyClientChangedAllowSpectatorOptionMessage { newOption = newValue });

        }
    }

    private void ServerOnChangedInsanityOptionEvent(bool newValue)
    {
        if (NetworkServer.localClientActive)
        {
            insanityOption = newValue;
            NetworkServer.SendToAll(new LobbyClientChangedInsanityOptionMessage { newOption = newValue });
        }
    }

    private void ServerOnPlayerJoined(NetworkConnection connection, LobbyServerPlayerJoinedMessage message)
    {
        Debug.Log($"A client has sent us their name! it is {message.clientName}. Their netId is {message.clientIdentity.netId}!");
        string name = message.clientName;
        NetworkIdentity clientIdentity = message.clientIdentity;
        players.Add(new LobbyPlayerStruct { clientName = name, character = Character.Random, identity = clientIdentity });
        int foundPlayerIndex = GetIndexOfPlayer(clientIdentity.netId);

        NetworkServer.SendToAll(new LobbyClientPlayerJoinedMessage { clientName = name, index = foundPlayerIndex });
        SyncUpdatesToNewClient(clientIdentity);
    }

    private void ServerOnKickedPlayerEvent(int playerNumber)
    {
        // NOTE: If statement used just in case if someone wants to try to kick as a non host.
        if (NetworkServer.localClientActive)
        {
            // NOTE: I don't think this will work properly and I should definitely test this.
            LobbyPlayerStruct player = players[playerNumber];
            string name = player.clientName;
            NetworkIdentity identity = player.identity;
            Debug.Log($"Kick attempted! playerNumber = {playerNumber}, clientName = {name}, netId = {identity.netId}");
            NetworkServer.SendToClientOfPlayer(identity, new LobbyClientYouHaveBeenKickedMessage { });
            players.RemoveAt(playerNumber);
            NetworkServer.SendToAll(new LobbyClientKickedMessage { kickedClientName = name, index = playerNumber });
            NetworkServer.SendToAll(new LobbyClientPlayerChangedCharacterMessage{newCharacter = Character.Empty, index = playerNumber});

        }
    }

    private void SyncUpdatesToNewClient(NetworkIdentity newClientIdentity)
    {
        for (var i = 0; i < players.Count; i++)
        {
            LobbyPlayerStruct player = players[i];
            string lobbyPlayerName = player.clientName;
            Character lobbyPlayerCharacter = player.character;
            int foundIndex = i;
            NetworkServer.SendToClientOfPlayer(newClientIdentity, new LobbyClientPlayerJoinedMessage { clientName = lobbyPlayerName, index = foundIndex });
            NetworkServer.SendToClientOfPlayer(newClientIdentity, new LobbyClientPlayerChangedCharacterMessage { newCharacter = lobbyPlayerCharacter, index = foundIndex });

        }

        NetworkServer.SendToClientOfPlayer(newClientIdentity, new LobbyClientChangedStageMessage{newOption = selectedStage});
        NetworkServer.SendToClientOfPlayer(newClientIdentity, new LobbyClientChangedGamemodeOptionMessage{newOption = selectedGamemode});
        NetworkServer.SendToClientOfPlayer(newClientIdentity, new LobbyClientChangedAllowSpectatorOptionMessage{newOption = allowSpectator});
        NetworkServer.SendToClientOfPlayer(newClientIdentity, new LobbyClientChangedAllRandomMessage{newOption = allRandom});
        NetworkServer.SendToClientOfPlayer(newClientIdentity, new LobbyClientChangedInsanityOptionMessage{newOption = insanityOption});
    }

    private int GetIndexOfPlayer(uint id)
    {
        int index = -1;

        for (var i = 0; i < players.Count; i++)
        {
            uint foundId = players[i].identity.netId;

            if (foundId == id)
            {
                index = i;
                break;
            }
        }

        return index;
    }

    private void ServerOnClientChangedCharacter(NetworkConnection connection, LobbyServerClientChangedCharacterMessage message)
    {
        Character character = message.newValue;
        uint id = connection.identity.netId;
        int foundIndex = GetIndexOfPlayer(id);
        LobbyPlayerStruct player = players[foundIndex];
        player.character = character;
        players[foundIndex] = player;
        NetworkServer.SendToAll(new LobbyClientPlayerChangedCharacterMessage { newCharacter = character, index = foundIndex });
    }

    #endregion

    #region CLIENT

    public override void OnStartClient()
    {
        NetworkClient.RegisterHandler<LobbyClientChangedStageMessage>(OnHostChangedStage);
        NetworkClient.RegisterHandler<LobbyClientChangedGamemodeOptionMessage>(OnHostChangedGameModeOption);
        NetworkClient.RegisterHandler<LobbyClientChangedAllRandomMessage>(OnHostChangedAllRandomOption);
        NetworkClient.RegisterHandler<LobbyClientChangedInsanityOptionMessage>(OnHostChangedInsanityOption);
        NetworkClient.RegisterHandler<LobbyClientChangedAllowSpectatorOptionMessage>(OnHostChangedAllowSpectatorOption);
        NetworkClient.RegisterHandler<LobbyClientPlayerJoinedMessage>(OnClientPlayerJoinedLobby);
        NetworkClient.RegisterHandler<LobbyClientPlayerLeftMessage>(OnClientPlayerLeftLobby);
        NetworkClient.RegisterHandler<LobbyClientPlayerChangedCharacterMessage>(OnClientPlayerChangedCharacter);
        NetworkClient.RegisterHandler<LobbyClientYouHaveBeenKickedMessage>(OnClientYouHaveBeenKickedMessage);
        NetworkClient.RegisterHandler<LobbyClientKickedMessage>(OnClientKickedMessage);
        base.OnStartClient();
    }

    private void OnHostChangedStage(NetworkConnection connection, LobbyClientChangedStageMessage message)
    {
        int newValue = message.newOption;
        Debug.Log("Detected change in stage option!");
        EventManager.lobbyClientHostChangedStageEvent.Invoke(newValue);
    }

    private void OnHostChangedGameModeOption(NetworkConnection connection, LobbyClientChangedGamemodeOptionMessage message)
    {
        int newValue = message.newOption;
        Debug.Log("Detected change in gamemode option!");
        EventManager.lobbyClientHostChangedGamemodeEvent.Invoke(newValue);
    }

    private void OnHostChangedAllRandomOption(NetworkConnection connection, LobbyClientChangedAllRandomMessage message)
    {
        bool newValue = message.newOption;
        Debug.Log("Detected change in random option!");
        EventManager.lobbyClientHostChangedAllRandomEvent.Invoke(newValue);
    }

    private void OnHostChangedInsanityOption(NetworkConnection connection, LobbyClientChangedInsanityOptionMessage message)
    {
        bool newValue = message.newOption;
        Debug.Log("Detected change in insanity option!");
        EventManager.lobbyClientHostChangedInsanityOptionEvent.Invoke(newValue);
    }

    private void OnHostChangedAllowSpectatorOption(NetworkConnection connection, LobbyClientChangedAllowSpectatorOptionMessage message)
    {
        Debug.Log("Detected change in spectator option!");
        bool newValue = message.newOption;
        EventManager.lobbyClientHostChangedAllowSpectatorEvent.Invoke(newValue);
    }

    private void OnClientPlayerJoinedLobby(NetworkConnection connection, LobbyClientPlayerJoinedMessage message)
    {
        string name = message.clientName;
        int index = message.index;
        EventManager.lobbyClientPlayerJoinedEvent.Invoke(name, index);
    }

    private void OnClientPlayerLeftLobby(NetworkConnection connection, LobbyClientPlayerLeftMessage message)
    {
        string name = message.clientName;
        int index = message.index;
        EventManager.lobbyClientPlayerLeftEvent.Invoke(name, index);
    }

    private void OnClientPlayerChangedCharacter(NetworkConnection connection, LobbyClientPlayerChangedCharacterMessage message)
    {
        Character character = message.newCharacter;
        int index = message.index;
        EventManager.lobbyClientPlayerChangedCharacterEvent.Invoke(character, index);
    }

    private void OnClientYouHaveBeenKickedMessage(NetworkConnection connection, LobbyClientYouHaveBeenKickedMessage message)
    {
        EventManager.lobbyYouHaveBeenKickedEvent.Invoke();
    }

    private void OnClientKickedMessage(NetworkConnection connection, LobbyClientKickedMessage message)
    {
        string name = message.kickedClientName;
        int index = message.index;
        EventManager.lobbyClientKickedEvent.Invoke(name, index);

    }
}


#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

#region SERVER

public class LobbyNetworkManager : MonoBehaviour
{
    private bool allRandom;

    private bool insanityOption;

    private bool allowSpectator;

    private int selectedStage;

    private int selectedGamemode;

    public void OnStartServer()
    {
        EventManager.lobbyServerKickedEvent.AddListener(ServerOnKickedPlayerEvent);
        EventManager.lobbyServerChangedStageEvent.AddListener(ServerOnChangedStageEvent);
        EventManager.lobbyServerChangedGamemodeEvent.AddListener(ServerOnChangedGamemodeEvent);
        EventManager.lobbyServerChangedAllRandomEvent.AddListener(ServerOnChangedAllRandomEvent);
        EventManager.lobbyServerChangedAllowSpectatorEvent.AddListener(ServerOnChangedAllowSpectatorEvent);
        EventManager.lobbyServerChangedInsanityOptionEvent.AddListener(ServerOnChangedInsanityOptionEvent);
    }


    public void OnStopServer()
    {

    }

    public void RegisterServerHandlers()
    {
        NetworkServer.RegisterHandler<LobbyServerClientChangedCharacterMessage>(ServerOnClientChangedCharacter);
        NetworkServer.RegisterHandler<LobbyServerPlayerChatMessage>(ServerOnPlayerSentChatMessage);
    }

    public void OnServerConnect(NetworkConnection connection)
    {
        GameObject lobbyPlayer = (GameObject)Resources.Load("Lobby Player");
        GameObject spawnedlobbyPlayer = Instantiate(lobbyPlayer);
        NetworkServer.AddPlayerForConnection(connection, spawnedlobbyPlayer);
    }


    public void OnServerSceneChanged(string sceneName)
    {

    }

    public void OnServerDisconnect(NetworkConnection connection)
    {
        List<Player> players = NetworkRoom.players;
        uint connectionId = connection.identity.netId;
        int foundIndex = GetIndexOfPlayer(connectionId);

        if (foundIndex != -1)
        {
            Player player = players[foundIndex];
            string name = player.playerName;
            players.RemoveAt(foundIndex);
            NetworkServer.SendToAll(new LobbyClientPlayerLeftMessage { clientName = name, index = foundIndex });
        }
    }

    private void ServerOnChangedStageEvent(int newValue)
    {
        if (NetworkServer.localClientActive)
        {
            selectedStage = newValue;
            NetworkServer.SendToAll(new LobbyClientChangedStageMessage { newOption = newValue });
        }
    }

    private void ServerOnPlayerSentChatMessage(NetworkConnection connection, LobbyServerPlayerChatMessage message)
    {
        string chatMessageText = message.text;
        string playerName = message.clientName;
        NetworkServer.SendToAll(new LobbyClientPlayerChatMessage { clientName = playerName, text = chatMessageText });
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

    // NOTE: Called when the player takes over a LobbyPlayer object.

    public void ServerOnKickedPlayerEvent(int playerNumber)
    {
        List<Player> players = NetworkRoom.players;

        // NOTE: If statement used just in case if someone wants to try to kick as a non host.
        if (NetworkServer.localClientActive)
        {
            // NOTE: I don't think this will work properly and I should definitely test this.
            Player player = players[playerNumber];
            string name = player.playerName;
            NetworkIdentity identity = player.identity;
            Debug.Log($"Kick attempted! playerNumber = {playerNumber}, clientName = {name}, netId = {identity.netId}");
            NetworkServer.SendToClientOfPlayer(identity, new LobbyClientYouHaveBeenKickedMessage { });
            players.RemoveAt(playerNumber);
            NetworkServer.SendToAll(new LobbyClientKickedMessage { kickedClientName = name, index = playerNumber });
            NetworkServer.SendToAll(new LobbyClientPlayerChangedCharacterMessage { newCharacter = Character.Empty, index = playerNumber });

        }
    }

    private void SyncUpdatesToNewClient(NetworkIdentity newClientIdentity)
    {
        List<Player> players = NetworkRoom.players;

        for (var i = 0; i < players.Count; i++)
        {
            Player player = players[i];
            string lobbyPlayerName = player.playerName;
            Character lobbyPlayerCharacter = player.character;
            int foundIndex = i;
            NetworkServer.SendToClientOfPlayer(newClientIdentity, new LobbyClientPlayerJoinedMessage { clientName = lobbyPlayerName, index = foundIndex });
            NetworkServer.SendToClientOfPlayer(newClientIdentity, new LobbyClientPlayerChangedCharacterMessage { newCharacter = lobbyPlayerCharacter, index = foundIndex });

        }

        NetworkServer.SendToClientOfPlayer(newClientIdentity, new LobbyClientChangedStageMessage { newOption = selectedStage });
        NetworkServer.SendToClientOfPlayer(newClientIdentity, new LobbyClientChangedGamemodeOptionMessage { newOption = selectedGamemode });
        NetworkServer.SendToClientOfPlayer(newClientIdentity, new LobbyClientChangedAllowSpectatorOptionMessage { newOption = allowSpectator });
        NetworkServer.SendToClientOfPlayer(newClientIdentity, new LobbyClientChangedAllRandomMessage { newOption = allRandom });
        NetworkServer.SendToClientOfPlayer(newClientIdentity, new LobbyClientChangedInsanityOptionMessage { newOption = insanityOption });
    }

    private int GetIndexOfPlayer(uint id)
    {
        List<Player> players = NetworkRoom.players;

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
        Player player = NetworkRoom.players[foundIndex];
        player.character = character;
        NetworkRoom.players[foundIndex] = player;
        NetworkServer.SendToAll(new LobbyClientPlayerChangedCharacterMessage { newCharacter = character, index = foundIndex });
    }

    public void OnPlayerJoined(NetworkConnection connection, ServerPlayerJoinedMessage message)
    {
        Debug.Log($"A client has sent us their name! it is {message.clientName}. Their netId is {message.clientIdentity.netId}!");
        string name = message.clientName;
        NetworkIdentity clientIdentity = message.clientIdentity;
        NetworkRoom.players.Add(new Player(name, Character.Random, clientIdentity));
        int foundPlayerIndex = GetIndexOfPlayer(clientIdentity.netId);
        NetworkServer.SendToAll(new LobbyClientPlayerJoinedMessage { clientName = name, index = foundPlayerIndex });
        SyncUpdatesToNewClient(clientIdentity);
    }

    #endregion

    #region CLIENT

    public void OnStartClient()
    {

    }

    public void OnClientDisconnect(NetworkConnection connection)
    {
        //EventManager.lobbyClientServerDisconnectedEvent.Invoke();
    }


    public void OnClientSceneChanged(NetworkConnection connection)
    {

    }

    public void RegisterClientHandlers()
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
        NetworkClient.RegisterHandler<LobbyClientPlayerChatMessage>(OnClientPlayerChatMessage);

    }

    private void OnClientPlayerChatMessage(NetworkConnection connection, LobbyClientPlayerChatMessage message)
    {
        string text = message.text;
        string clientName = message.clientName;
        EventManager.lobbyClientPlayerSentChatMessageEvent.Invoke(text, clientName);

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

    #endregion
}

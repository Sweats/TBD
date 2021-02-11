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

    }


    public void OnStopServer()
    {

    }

    public void RegisterServerHandlers()
    {
        NetworkServer.RegisterHandler<LobbyServerClientChangedCharacterMessage>(ServerOnClientChangedCharacter);
        NetworkServer.RegisterHandler<LobbyServerPlayerChatMessage>(ServerOnPlayerSentChatMessage);
        NetworkServer.RegisterHandler<LobbyServerClientRequestedKickMessage>(ServerOnClientRequestedKickMessage);
        NetworkServer.RegisterHandler<LobbyServerClientRequestedChangeStageMessage>(ServerOnClientRequestedToChangeStage);
        NetworkServer.RegisterHandler<LobbyServerClientRequestedChangeGamemodeMessage>(ServerOnClientRequestedToChangeGamemode);
        NetworkServer.RegisterHandler<LobbyServerClientRequestedChangeInsanityMessage>(ServerOnClientRequestedToChangeInsanity);
        NetworkServer.RegisterHandler<LobbyServerClientRequestedChangeAllRandomMessage>(ServerOnClientRequestedToChangeAllRandom);
        NetworkServer.RegisterHandler<LobbyServerClientRequestedChangeAllowSpectatorMessage>(ServerOnClientRequestedToChangeAllowSpectator);
    }

    public void OnServerConnect(NetworkConnection connection)
    {
        List<Player> players = NetworkRoom.PLAYERS_IN_SERVER;
        GameObject lobbyPlayer = (GameObject)Resources.Load("Lobby Player");
        GameObject spawnedlobbyPlayer = Instantiate(lobbyPlayer);
        NetworkServer.AddPlayerForConnection(connection, spawnedlobbyPlayer);
    }


    public void OnServerSceneChanged(string sceneName)
    {

    }

    public void OnServerDisconnect(NetworkConnection connection)
    {
        List<Player> players = NetworkRoom.PLAYERS_IN_SERVER;

        // NOTE: We need this check here in case when the host disconnects when there are clients connected to this host.
        if (connection.identity != null)
        {
            uint connectionId = connection.identity.netId;
            int foundIndex = GetIndexOfPlayer(connectionId);

            if (foundIndex != -1)
            {
                Player player = players[foundIndex];
                string name = player.playerName;
                bool hosting = player.Hosting();
                players.RemoveAt(foundIndex);
                NetworkServer.DestroyPlayerForConnection(connection);

                if (hosting && players.Count > 0)
                {
                    Debug.Log("Old host has left the server! Picking a new host...");
                    PickNewHost();
                }

                NetworkServer.SendToAll(new LobbyClientPlayerLeftMessage { clientName = name, index = foundIndex });
            }
        }
    }

    private void ServerOnClientRequestedToChangeStage(NetworkConnection connection, LobbyServerClientRequestedChangeStageMessage message)
    {
        Debug.Log("Client requested to change the stage!");
        uint id = connection.identity.netId;

        if (!IsHost(id))
        {
            return;
        }

        selectedStage = message.newValue;
        NetworkServer.SendToAll(new LobbyClientChangedStageMessage { newOption = message.newValue });
    }




    private void ServerOnClientRequestedToChangeInsanity(NetworkConnection connection, LobbyServerClientRequestedChangeInsanityMessage message)
    {
        uint id = connection.identity.netId;
        Debug.Log("Client requested to change the insanity!");

        if (!IsHost(id))
        {
            return;
        }

        insanityOption = message.newValue;
        NetworkServer.SendToAll(new LobbyClientChangedInsanityOptionMessage { newOption = message.newValue });

    }

    private void ServerOnClientRequestedToChangeAllowSpectator(NetworkConnection connection, LobbyServerClientRequestedChangeAllowSpectatorMessage message)
    {
        Debug.Log("Client requested to change the spectator!");
        uint id = connection.identity.netId;

        if (!IsHost(id))
        {
            Debug.Log("The player is not the host!");
            return;
        }

        Debug.Log("Server has granted the player to change the allow spectator option");

        allowSpectator = message.newValue;

        NetworkServer.SendToAll(new LobbyClientChangedAllowSpectatorOptionMessage { newOption = message.newValue });


    }

    private void ServerOnClientRequestedToChangeAllRandom(NetworkConnection connection, LobbyServerClientRequestedChangeAllRandomMessage message)
    {
        Debug.Log("Client requested to change the all random!");
        uint id = connection.identity.netId;

        if (!IsHost(id))
        {
            return;
        }

        allRandom = message.newValue;

        NetworkServer.SendToAll(new LobbyClientChangedAllRandomMessage { newOption = message.newValue });

    }

    private void ServerOnClientRequestedToChangeGamemode(NetworkConnection connection, LobbyServerClientRequestedChangeGamemodeMessage message)
    {
        Debug.Log("Client requested to change the game mode! ");
        uint id = connection.identity.netId;

        if (!IsHost(id))
        {
            return;
        }

        selectedGamemode = message.newValue;

        NetworkServer.SendToAll(new LobbyClientChangedGamemodeOptionMessage { newOption = message.newValue });
    }

    public bool IsHost(uint id)
    {
        int index = GetIndexOfPlayer(id);
        Player host = NetworkRoom.PLAYERS_IN_SERVER[index];

        if (host.Hosting())
        {
            return true;
        }

        return false;
    }


    private void PickNewHost()
    {
        List<Player> players = NetworkRoom.PLAYERS_IN_SERVER;
        int randomNumber = Random.Range(0, players.Count);
        NetworkRoom.PLAYERS_IN_SERVER[randomNumber].SetHosting(true);
        NetworkIdentity newHostIdentity = NetworkRoom.PLAYERS_IN_SERVER[randomNumber].identity;
        NetworkRoom.Log($"Picking a new host with the net identity of {newHostIdentity.netId}");
        NetworkServer.SendToClientOfPlayer(newHostIdentity, new LobbyClientServerAssignedYouHostMessage());

        for (var i = 0; i < players.Count; i++)
        {
            // NOTE: Skip the new host.
            if (randomNumber == i)
            {
                continue;
            }

            Player player = players[i];
            NetworkIdentity playerIdentity = player.identity;
            string playerName = player.playerName;
            NetworkServer.SendToClientOfPlayer(playerIdentity, new LobbyClientServerPickedNewHostMessage { clientName = playerName, index = randomNumber });
        }

    }


    private void ServerOnPlayerSentChatMessage(NetworkConnection connection, LobbyServerPlayerChatMessage message)
    {
        string chatMessageText = message.text;
        string playerName = message.clientName;
        NetworkServer.SendToAll(new LobbyClientPlayerChatMessage { clientName = playerName, text = chatMessageText });
    }

    private void ServerOnClientRequestedKickMessage(NetworkConnection connection, LobbyServerClientRequestedKickMessage message)
    {
        List<Player> players = NetworkRoom.PLAYERS_IN_SERVER;
        uint netId = connection.identity.netId;
        int requestedPlayerIndex = GetIndexOfPlayer(netId);
        Player requestee = players[requestedPlayerIndex];

        if (!requestee.Hosting())
        {
            return;
        }

        int playerIndexToKick = message.index;
        Player kickedPlayer = players[playerIndexToKick];
        NetworkIdentity identity = kickedPlayer.identity;
        players.RemoveAt(playerIndexToKick);
        Debug.Log($"Kick attempted! playerNumber = {playerIndexToKick}, clientName = {name}, netId = {identity.netId}");
        NetworkServer.SendToClientOfPlayer(identity, new LobbyClientYouHaveBeenKickedMessage { });
        NetworkServer.SendToAll(new LobbyClientKickedMessage { kickedClientName = name, index = playerIndexToKick });
        NetworkServer.SendToAll(new LobbyClientPlayerChangedCharacterMessage { newCharacter = Character.Empty, index = playerIndexToKick });
    }

    private void SyncUpdatesToNewClient(NetworkIdentity newClientIdentity)
    {
        List<Player> players = NetworkRoom.PLAYERS_IN_SERVER;

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
        List<Player> players = NetworkRoom.PLAYERS_IN_SERVER;

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
        uint id = connection.identity.netId;
        int foundIndex = GetIndexOfPlayer(id);
        Player player = NetworkRoom.PLAYERS_IN_SERVER[foundIndex];
        Character character = message.newValue;
        player.character = character;
        NetworkRoom.PLAYERS_IN_SERVER[foundIndex] = player;
        NetworkServer.SendToAll(new LobbyClientPlayerChangedCharacterMessage { newCharacter = character, index = foundIndex });
    }

    public void OnPlayerJoined(NetworkConnection connection, ServerPlayerJoinedMessage message)
    {
        List<Player> players = NetworkRoom.PLAYERS_IN_SERVER;

        Debug.Log($"A client has sent us their name! it is {message.clientName}. Their netId is {message.clientIdentity.netId}!");
        string name = message.clientName;
        NetworkIdentity clientIdentity = message.clientIdentity;
        Player player = new Player(name, Character.Random, clientIdentity);

        if (players.Count == 0)
        {
            player.SetHosting(true);
            NetworkRoom.Log($"The value of the hosting  player is {player.Hosting()}");
            NetworkServer.SendToClientOfPlayer(player.identity, new LobbyClientServerAssignedYouHostMessage());
            NetworkRoom.Log($"Made {message.clientName} the host of this lobby.");
        }

        players.Add(player);
        int newPlayerIndex = players.Count - 1;
        Debug.Log($"Length of NetworkRoom.players is {NetworkRoom.PLAYERS_IN_SERVER.Count}");
        NetworkServer.SendToAll(new LobbyClientPlayerJoinedMessage { clientName = name, index = newPlayerIndex });
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
        NetworkClient.RegisterHandler<LobbyClientServerPickedNewHostMessage>(OnClientServerSeletedNewHost);
        NetworkClient.RegisterHandler<LobbyClientServerAssignedYouHostMessage>(OnClientServerAssignedYouHost);

    }

    private void OnClientServerSeletedNewHost(NetworkConnection connection, LobbyClientServerPickedNewHostMessage message)
    {
        string newHostName = message.clientName;
        int index = message.index;
        EventManager.lobbyClientServerPickedNewHostEvent.Invoke(newHostName, index);
    }

    private void OnClientServerAssignedYouHost(NetworkConnection connection, LobbyClientServerAssignedYouHostMessage message)
    {
        int index = message.index;
        EventManager.lobbyClientServerAssignedYouHostEvent.Invoke(index);
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

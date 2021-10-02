using UnityEngine;
using Mirror;

public class ClientLobby: MonoBehaviour
{
    private ClientLobby(){}

    public void RegisterNetworkHandlers()
    {
        NetworkClient.RegisterHandler<ClientServerLobbyHostChangedAllowSpectatorOptinon>(OnClientServerLobbyHostChangedAllowSpectator);
        NetworkClient.RegisterHandler<ClientServerLobbyHostChangedAllRandomOption>(OnClientServerLobbyHostChangedAllRandom);
        NetworkClient.RegisterHandler<ClientServerLobbyHostChangedGamemodeOption>(OnClientServerLobbyHostChangedGamemode);
        NetworkClient.RegisterHandler<ClientServerLobbyHostChangedInsanityOption>(OnClientServerLobbyHostChangedInsanityOption);
        NetworkClient.RegisterHandler<ClientServerLobbyHostChangedStageMessage>(OnClientServerLobbyHostChangedStage);
        NetworkClient.RegisterHandler<ClientServerLobbyHostChangedVoiceChatMessage>(OnClientServerLobbyHostChangedVoiceChatOption);
        NetworkClient.RegisterHandler<ClientServerLobbyPlayerChangedCharacterMessage>(OnClientServerLobbyPlayerChangedCharacter);
        NetworkClient.RegisterHandler<ClientServerLobbyPlayerDisconnectedMessage>(OnClientServerLobbyPlayerDisconnected);
        NetworkClient.RegisterHandler<ClientServerLobbyPlayerJoinedMessage>(OnClientServerLobbyPlayerJoined);
        NetworkClient.RegisterHandler<ClientServerLobbyHostKickedPlayerMessage>(OnClientServerLobbyHostKickedPlayer);
        NetworkClient.RegisterHandler<ClientServerLobbyHostKickedYouMessage>(OnClientServerLobbyHostKickedYou);

    }

    private void OnClientServerLobbyMadeSomeoneHost(NetworkConnection connection, ClientServerLobbyMadeSomeoneHostMessage message)
    {
        string newHostName = message.newHostName;
        int index = message.index;
        EventManager.clientServerLobbyServerPickedNewHostEvent.Invoke(newHostName, index);
    }

    private void OnClientServerLobbyMaadeYouHost(NetworkConnection connection, ClientServerLobbyMadeYouHostMessage message)
    {
        int index = message.index;
        EventManager.clientServerLobbyServerAssignedYouHostEvent.Invoke(index);
    }

    private void OnClientServerLobbyPlayerSentChat(NetworkConnection connection, ClientServerLobbyPlayerSentChatMessage message)
    {
        string text = message.chatMessage;
        EventManager.clientServerLobbyPlayerSentChatMessageEvent.Invoke(text);

    }

    private void OnClientServerLobbyHostChangedStage(NetworkConnection connection, ClientServerLobbyHostChangedStageMessage message)
    {
        int newValue = (int)message.newStage;
        Debug.Log("Detected change in stage option!");
        EventManager.clientServerLobbyHostChangedStageEvent.Invoke(newValue);
    }

    private void OnClientServerLobbyHostChangedGamemode(NetworkConnection connection, ClientServerLobbyHostChangedGamemodeOption message)
    {
        int newValue = message.newGamemodeValue;
        Debug.Log("Detected change in gamemode option!");
        EventManager.clientServerLobbyHostChangedGamemodeEvent.Invoke(newValue);
    }

    private void OnClientServerLobbyHostChangedAllRandom(NetworkConnection connection, ClientServerLobbyHostChangedAllRandomOption message)
    {
        bool newValue = message.newAllRandomValue;
        Debug.Log("Detected change in random option!");
        EventManager.clientServerLobbyHostChangedAllRandomEvent.Invoke(newValue);
    }

    private void OnClientServerLobbyHostChangedInsanityOption(NetworkConnection connection, ClientServerLobbyHostChangedInsanityOption message)
    {
        bool newValue = message.newInsanityValue;
        Debug.Log("Detected change in insanity option!");
        EventManager.clientServerLobbyHostChangedInsanityOptionEvent.Invoke(newValue);
    }

    private void OnClientServerLobbyHostChangedAllowSpectator(NetworkConnection connection, ClientServerLobbyHostChangedAllowSpectatorOptinon message)
    {
        Debug.Log("Detected change in spectator option!");
        bool newValue = message.newAllowSpectatorValue;
        EventManager.clientServerLobbyHostChangedAllowSpectatorEvent.Invoke(newValue);
    }

    private void OnClientServerLobbyHostChangedVoiceChatOption(NetworkConnection connection, ClientServerLobbyHostChangedVoiceChatMessage message)
    {
        Debug.Log("Detected change in voice chat option!");
        bool newValue = message.newVoiceChatValue;

    }

    private void OnClientServerLobbyPlayerJoined(NetworkConnection connection, ClientServerLobbyPlayerJoinedMessage message)
    {
        string name = message.clientName;
        int index = message.index;
        EventManager.clientServerLobbyPlayerJoinedEvent.Invoke(name, index);
    }

    private void OnClientServerLobbyPlayerDisconnected(NetworkConnection connection, ClientServerLobbyPlayerDisconnectedMessage message)
    {
        string name = message.disconnectedClientName;
        int index = message.disconnectedClientIndex;
        EventManager.clientServerLobbyPlayerDisconnectEvent.Invoke(name, index);
    }

    private void OnClientServerLobbyPlayerChangedCharacter(NetworkConnection connection, ClientServerLobbyPlayerChangedCharacterMessage message)
    {
        Character character = message.newCharacter;
        int index = message.playerIndexInLobby;
        EventManager.clientServerLobbyPlayerChangedCharacterEvent.Invoke(character, index);
    }

    private void OnClientServerLobbyHostKickedYou(NetworkConnection connection, ClientServerLobbyHostKickedYouMessage message)
    {
        EventManager.clientServerLobbyHostKickedYouEvent.Invoke();
    }

    private void OnClientServerLobbyHostKickedPlayer(NetworkConnection connection, ClientServerLobbyHostKickedPlayerMessage message)
    {
        string name = message.kickedClientName;
        int index = message.index;
        EventManager.clientServerLobbyClientKickedEvent.Invoke(name, index);
    }
}
using Mirror;
using UnityEngine;

public class ClientStage: MonoBehaviour
{
    private ClientStage(){}

    public void RegisterNetworkHandlers()
    {
        NetworkClient.RegisterHandler<ClientServerGamePickCharacterMessage>(OnClientServerGamePickCharacterMessage);
        NetworkClient.RegisterHandler<ClientServerGameHostStartedGameMessage>(OnClientServerGameHostStartedGameMessage);
        NetworkClient.RegisterHandler<ClientServerGamePlayerChangedProfileNameMessage>(OnClientServerGamePlayerChangedProfileNameMessage);
        NetworkClient.RegisterHandler<ClientServerGameSurvivorsDeadMessage>(OnClientServerGameSurvivorsDeadMessage);
        NetworkClient.RegisterHandler<ClientServerGamePlayerJoinedMessage>(OnClientServerJoinedGameMessage);
        NetworkClient.RegisterHandler<ClientServerGamePlayerConnectedMessage>(OnClientServerGamePlayerConnectedMessage);
        NetworkClient.RegisterHandler<ClientServerGamePlayerDisconnectedMessage>(OnClienServerGamePlayerDisconnectedMessage);
        NetworkClient.RegisterHandler<ClientServerGameSurvivorKilledMessage>(OnClientServerGameSurviorKilled);
    }

    private void OnClientServerGamePlayerConnectedMessage(NetworkConnection connection, ClientServerGamePlayerConnectedMessage message)
    {
        EventManager.clientServerGamePlayerConnectedEvent.Invoke(message.name);
    }

    private void OnClienServerGamePlayerDisconnectedMessage(NetworkConnection connection, ClientServerGamePlayerDisconnectedMessage message)
    {
        EventManager.clientServerGamePlayerDisconnectedEvent.Invoke(message.name);

    }

    private void OnClientServerGamePlayerChangedProfileNameMessage(NetworkConnection connection, ClientServerGamePlayerChangedProfileNameMessage message)
    {
        EventManager.clientServerGamePlayerChangedNameEvent.Invoke(message.oldName, message.newName);
    }

    private void OnClientServerJoinedGameMessage(NetworkConnection connection, ClientServerGamePlayerJoinedMessage message)
    {
        EventManager.clientServerGamePlayerJoinedEvent.Invoke(message.name);
    }

    private void OnClientServerGameHostStartedGameMessage(NetworkConnection connection, ClientServerGameHostStartedGameMessage message)
    {
        Debug.Log("THE HOST HAS STARTED THE GAME!");
        EventManager.clientServerGameHostStartedGameEvent.Invoke();

    }

    private void OnClientServerGamePickCharacterMessage(NetworkConnection connection, ClientServerGamePickCharacterMessage message)
    {
        Character[] unavailableCharacters = message.unavailableCharacters;
        EventManager.clientServerGameAskedYouToPickCharacterEvent.Invoke(unavailableCharacters);
    }

    private void OnClientServerGameSurvivorsDeadMessage(NetworkConnection connection, ClientServerGameSurvivorsDeadMessage message)
    {
        EventManager.clientServerGameSurvivorsDeadEvent.Invoke();

    }

    private void OnClientServerGameSurviorKilled(NetworkConnection connection, ClientServerGameSurvivorKilledMessage message)
    {
        Survivor survivor = NetworkIdentity.spawned[message.survivorId].GetComponent<Survivor>();
        survivor.ClientPlayDeathSound();
        EventManager.clientServerGameSurvivorKilledEvent.Invoke(survivor.Name());
    }

}
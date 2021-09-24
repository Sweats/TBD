using Mirror;

public class ClientStage
{
    private ClientStage(){}

    public void RegisterNetworkHandlers()
    {
        NetworkClient.RegisterHandler<ClientServerGamePickCharacterMessage>(OnClientServerGamePickCharacterMessage);

    }

    private void OnClientServerGamePickCharacterMessage(NetworkConnection connection, ClientServerGamePickCharacterMessage message)
    {
        Character[] unavailableCharacters = message.unavailableCharacters;
        EventManager.clientServerGameAskedYouToPickCharacterEvent.Invoke(unavailableCharacters);
    }

}
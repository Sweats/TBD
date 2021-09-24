using Mirror;
public class ClientKey
{
    private ClientKey(){}

    public void RegisterNetworkHandlers()
    {
        NetworkClient.RegisterHandler<ClientServerGameAlreadyHaveKeyMessage>(OnClientServerGameAlreadyHaveKey);
        NetworkClient.RegisterHandler<ClientServerGameSurvivorPickedUpKeyMessage>(OnClientServerGameSurvivorPickedUpKey);

    }


    private void OnClientServerGameAlreadyHaveKey(NetworkConnection connection, ClientServerGameAlreadyHaveKeyMessage message)
    {
        EventManager.clientServerGameAlreadyHaveKeyEvent.Invoke();

    }

    private void OnClientServerGameSurvivorPickedUpKey(NetworkConnection connection, ClientServerGameSurvivorPickedUpKeyMessage message)
    {
        string keyName = message.keyName;
        string survivorName = message.playerName;
        EventManager.clientServerGameSurvivorPickedUpKeyEvent.Invoke(keyName, survivorName);

    }
}
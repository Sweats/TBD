using Mirror;
public class ClientChat
{
    private ClientChat(){}

    public void RegisterNetworkHandlers()
    {
        NetworkClient.RegisterHandler<ClientServerGamePlayerSentChatMessage>(OnClientServerGamePlayerSentChatMessage);

    }


    private void OnClientServerGamePlayerSentChatMessage(NetworkConnection connection, ClientServerGamePlayerSentChatMessage message)
    {
        EventManager.clientServerGamePlayerSentChatMessageEvent.Invoke(message.chatMessage);
    }
}
using Mirror;
public class ServerChat
{
    private ServerChat(){}

    public void RegisterNetworkHandlers()
    {
        NetworkServer.RegisterHandler<ServerClientGamePlayerSentChatMessage>(OnServerClientGamePlayerSentChatMessage);

    }

    private void OnServerClientGamePlayerSentChatMessage(NetworkConnection connection, ServerClientGamePlayerSentChatMessage message)
    {
        Survivor survivor = connection.identity.GetComponent<Survivor>();
        string playerName = string.Empty;
        string text = message.chatText;

        // NOTE: Only a survivor should be allowed to see and send chat messages.
        if (survivor != null)
        {
            playerName = survivor.Name();
            string finalMessage = $"{playerName}: {text}";
            NetworkServer.SendToReady<ClientServerGamePlayerSentChatMessage>(new ClientServerGamePlayerSentChatMessage{chatMessage = finalMessage} );
            return;
        }
    }
}
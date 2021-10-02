using Mirror;
using UnityEngine;

public class ClientEscape: MonoBehaviour
{
    public void RegisterNetworkHandlers()
    {
        NetworkClient.RegisterHandler<ClientServerGameSurvivorsEscapedMessage>(OnClientServerGameSurvivorsEscapedMessage);

    }

    private void OnClientServerGameSurvivorsEscapedMessage(NetworkConnection connection, ClientServerGameSurvivorsEscapedMessage message)
    {
        EventManager.clientServerGameSurvivorsEscapedEvent.Invoke();

    }

}
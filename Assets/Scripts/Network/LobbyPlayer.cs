using Mirror;

public class LobbyPlayer : NetworkBehaviour
{
    // NOTE: We have to do this here because apparently the net identity is not set inside the NetworkManager OnServerConnect() function.

    public override void OnStartClient()
    {
        if (isLocalPlayer)
        {
            NetworkClient.Send(new LobbyServerPlayerJoinedMessage { clientName = Settings.PROFILE_NAME, clientIdentity = netIdentity });
            base.OnStartClient();
        }
    }
}

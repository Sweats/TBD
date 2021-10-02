using Mirror;

public class MasterClient
{
    private MasterClient(){}

    public void RegisterNetworkHandlers()
    {
        NetworkClient.RegisterHandler<MasterClientServerSentServerListingMessage>(OnMasterClientServerSentLobbyListing);
        NetworkClient.RegisterHandler<MasterClientServerAddedClientHostServerMessage>(OnMasterClientServerClientHostAddedServer);
        NetworkClient.RegisterHandler<MasterClientServerAddedDedicatedServerMessage>(OnMasterClientServerAddDedicatedServer);
    }

    private void OnMasterClientServerSentLobbyListing(NetworkConnection connection, MasterClientServerSentServerListingMessage message)
    {
        EventManager.masterServerClientSentUsLobbyListEvent.Invoke(message.lobbiesOnServer);

    }

    private void OnMasterClientServerClientHostAddedServer(NetworkConnection connection, MasterClientServerAddedClientHostServerMessage message)
    {
        EventManager.masterClientServerAddedClientHostEvent.Invoke(message.id);
    }


    private void OnMasterClientServerAddDedicatedServer(NetworkConnection connection, MasterClientServerAddedDedicatedServerMessage message)
    {
        EventManager.masterClientServerAddedDedicatedServerEvent.Invoke(message.id);
    }
}
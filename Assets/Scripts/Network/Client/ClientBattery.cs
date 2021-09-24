using Mirror;

public class ClientBattery
{
    private ClientBattery(){}

    public void RegisterNetworkHandlers()
    {
        NetworkClient.RegisterHandler<ClientServerGameRejectedBatteryPickupMessage>(OnClientServerGameFailedToPickUpBattery);
        NetworkClient.RegisterHandler<ClientServerGameYouPickedUpBatteryMessage>(OnClientServerGameYouPickedUpBattery);

    }

    private void OnClientServerGameFailedToPickUpBattery(NetworkConnection connection, ClientServerGameRejectedBatteryPickupMessage message)
    {
        EventManager.clientServerGameRejectBatteryPickupEvent.Invoke();

    }

    private void OnClientServerGameYouPickedUpBattery(NetworkConnection connection, ClientServerGameYouPickedUpBatteryMessage message)
    {
        EventManager.clientServerGameYouPickedUpBatteryEvent.Invoke();

    }
}

using Mirror;
using UnityEngine;

public class ClientDoor
{
    private ClientDoor(){}

    public void RegisterNetworkHandlers()
    {
        NetworkClient.RegisterHandler<ClientServerGameDoorFailedToUnlockMessage>(OnClientServerGameFailedToUnlockDoor);
    }


    private void OnClientServerGameFailedToUnlockDoor(NetworkConnection connection, ClientServerGameDoorFailedToUnlockMessage message)
    {
        Vector3 position = message.position;
        float x = position.x;
        float y = position.y;
        float z = position.z;
        EventManager.clientServerGameFaliedToUnlockDoorEvent.Invoke(x, y, z);

    }
}
using Mirror;
using UnityEngine;

public class ClientDoor : MonoBehaviour
{
    private ClientDoor(){}

    public void RegisterNetworkHandlers()
    {
        NetworkClient.RegisterHandler<ClientServerGameDoorFailedToUnlockMessage>(OnClientServerGameFailedToUnlockDoor);
        NetworkClient.RegisterHandler<ClientServerGameDoorUnlockedMessage>(OnClientServerGameDoorUnlocked);
    }


    private void OnClientServerGameFailedToUnlockDoor(NetworkConnection connection, ClientServerGameDoorFailedToUnlockMessage message)
    {
        uint doorId = message.doorId;

        Door door = NetworkIdentity.spawned[doorId].GetComponent<Door>();

        if (door != null)
        {
            Vector3 position = door.transform.position;
            door.ClientPlayLockedSound();
        }
    }

    private void OnClientServerGameDoorUnlocked(NetworkConnection connection, ClientServerGameDoorUnlockedMessage message)
    {
        uint doorId = message.doorId;

        Door door = NetworkIdentity.spawned[doorId].GetComponent<Door>();

        if (door != null)
        {
            Vector3 position = door.transform.position;
            door.ClientPlayUnlockedSound();
            EventManager.clientServerGameSurvivorUnlockedDoorEvent.Invoke(message.playerName, message.keyName, door.Name());
        }

    }
}
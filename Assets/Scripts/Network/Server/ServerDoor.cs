using Mirror;
using UnityEngine;

public class ServerDoor
{
    private ServerDoor(){}

    public void RegisterNetworkHandlers()
    {
        NetworkServer.RegisterHandler<ServerClientGameClickedOnDoorMessage>(OnServerClientPlayerClickedOnDoor);

    }

    private void OnServerClientPlayerClickedOnDoor(NetworkConnection connection, ServerClientGameClickedOnDoorMessage message)
    {
        //NOTE: Seeing as we trust the client to send us the correct netid, a malicious client could crash the server by sending net ids that don't exist.
        if (!NetworkIdentity.spawned.ContainsKey(message.requestedDoorID))
        {
            return;
        }

        Survivor survivor = connection.identity.GetComponent<Survivor>();

        // NOTE: The player is a monster.
        if (survivor == null)
        {
            ClientServerGameDoorFailedToUnlockMessage clientServerGameDoorFailedToUnlockMessage = new ClientServerGameDoorFailedToUnlockMessage{};
            NetworkServer.SendToReady(clientServerGameDoorFailedToUnlockMessage);
            return;
        }

        Door door = NetworkIdentity.spawned[message.requestedDoorID].GetComponent<Door>();

        //NOTE: Seeing as we trust the client to send us the correct netid, a malicious client could crash the server by sending net ids that don't match a door.
        if (door == null)
        {
            return;
        }

        Vector3 doorPos = door.transform.position;
        Vector3 survivorPos = survivor.transform.position;

        float distance = Vector3.Distance(doorPos, survivorPos);

        if (distance > survivor.GrabDistance())
        {
            return;
        }

        Key[] keys = survivor.Items().Keys();
        bool unlocked = false;

        for (var i = 0; i < keys.Length; i++)
        {
            Key key = keys[i];
            
            if (key.Mask() == door.UnlockMask())
            {
                unlocked = true;
                door.Unlock();
                string survivorName = survivor.Name();
                string doorName = door.Name();

                ClientServerGameDoorUnlockedMessage clientServerGameDoorUnlockedMessage = new ClientServerGameDoorUnlockedMessage()
                {
                    doorName  = doorName,
                    playerName = survivorName
                };


                NetworkServer.SendToReady(message);
                break;
            }
        }

        if (!unlocked)
        {
            ClientServerGameDoorFailedToUnlockMessage clientServerGameDoorFailedToUnlockMessage = new ClientServerGameDoorFailedToUnlockMessage{};
            NetworkServer.SendToReady(clientServerGameDoorFailedToUnlockMessage);
        }
    }
}
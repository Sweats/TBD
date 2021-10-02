using Mirror;
using UnityEngine;

public class ServerDoor: MonoBehaviour
{
    private ServerDoor(){}

    public void RegisterNetworkHandlers()
    {
        NetworkServer.RegisterHandler<ServerClientGameClickedOnDoorMessage>(OnServerClientPlayerClickedOnDoor);
        NetworkServer.RegisterHandler<ServerClientGameDoorBumpedIntoMessage>(OnServerClientGameDoorBumpedInto);

    }

    private void OnServerClientPlayerClickedOnDoor(NetworkConnection connection, ServerClientGameClickedOnDoorMessage message)
    {
        //NOTE: Seeing as we trust the client to send us the correct netid, a malicious client could crash the server by sending net ids that don't exist.
        if (!NetworkIdentity.spawned.ContainsKey(message.requestedDoorID))
        {
            return;
        }

        Survivor survivor = connection.identity.GetComponent<Survivor>();

        Door door = NetworkIdentity.spawned[message.requestedDoorID].GetComponent<Door>();
        //NOTE: Seeing as we trust the client to send us the correct netid, a malicious client could crash the server by sending net ids that does not match a door.
        if (door == null)
        {
            return;
        }

        if (door.ServerUnlocked())
        {
            return;
        }

        // NOTE: The player is a monster.
        if (survivor == null)
        {
            ClientServerGameDoorFailedToUnlockMessage clientServerGameDoorFailedToUnlockMessage = new ClientServerGameDoorFailedToUnlockMessage
            {
                doorId = door.netIdentity.netId,
            };

            NetworkServer.SendToReady(clientServerGameDoorFailedToUnlockMessage);
            return;
        }

        Vector3 doorPos = door.transform.position;
        Vector3 survivorPos = survivor.transform.position;

        float distance = Vector3.Distance(doorPos, survivorPos);

        if (distance > survivor.GrabDistance())
        {
            return;
        }

        Key[] keys = survivor.Items().ServerKeys();
        bool unlocked = false;

        for (var i = 0; i < keys.Length; i++)
        {
            Key key = keys[i];
            
            if (key.Mask() == door.UnlockMask())
            {
                unlocked = true;
                door.ServerUnlock();
                string survivorName = survivor.Name();
                string doorName = door.Name();

                ClientServerGameDoorUnlockedMessage clientServerGameDoorUnlockedMessage = new ClientServerGameDoorUnlockedMessage()
                {
                    doorId = door.netIdentity.netId,
                    playerName = survivorName,
                    keyName = key.Name()
                };


                NetworkServer.SendToReady(clientServerGameDoorUnlockedMessage);
                break;
            }
        }

        if (!unlocked)
        {
            ClientServerGameDoorFailedToUnlockMessage clientServerGameDoorFailedToUnlockMessage = new ClientServerGameDoorFailedToUnlockMessage
            {
                doorId = door.netIdentity.netId,
            };

            NetworkServer.SendToReady(clientServerGameDoorFailedToUnlockMessage);
        }
    }

    private void OnServerClientGameDoorBumpedInto(NetworkConnection connection, ServerClientGameDoorBumpedIntoMessage message)
    {
        uint id = message.requestedDoorID;

        if (!NetworkIdentity.spawned.ContainsKey(id))
        {
            return;
        }

        Door door = NetworkIdentity.spawned[id].GetComponent<Door>();

        if (door == null)
        {
            return;
        }

        if (!door.ServerUnlocked())
        {
            return;
        }

        //NOTE: Mirror will sync the movement for us
        float doorPushStrength = door.PushStrength();
        Vector3 velocity = doorPushStrength * message.moveDirection;
        door.AddForce(velocity);
    }
}
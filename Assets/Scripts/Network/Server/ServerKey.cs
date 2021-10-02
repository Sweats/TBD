using Mirror;
using UnityEngine;
using System.Collections.Generic;

public class ServerKey: MonoBehaviour
{
    private ServerKey(){}

    private int forcedPathID;

    private int choosenKeyPath;

    public void RegisterNetworkHandlers()
    {
        NetworkServer.RegisterHandler<ServerClientGameClickedOnKeyMessage>(OnServerClientGamePlayerClickedOnKey);

    }

    public void OnServerSceneChanged()
    {
        ServerChooseKeyPath();
        ServerSpawnKeys();

    }

    private void ServerChooseKeyPath()
    {
        if (forcedPathID <= -1)
        {
            int maxPaths = ServerGetMaxPaths();
            choosenKeyPath = UnityEngine.Random.Range(0, maxPaths);
        }

        else
        {
            choosenKeyPath = forcedPathID;
        }

    }

    private int ServerGetMaxPaths()
    {
        return 0;
    }

    private void ServerSpawnKeys()
    {
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag(Tags.KEY_SPAWN_POINT);
        List<int> maskIgnoreList = new List<int>();

        for (var i = 0; i < spawnPoints.Length; i++)
        {
            KeysAtSpawnPoint[] keysAtSpawnPoint = spawnPoints[i].GetComponent<KeySpawnPoint>().SpawnableKeysAtPoint();

            for (var j = 0; j < keysAtSpawnPoint.Length; j++)
            {
                KeysAtSpawnPoint spawnableKeys = keysAtSpawnPoint[j];

                if (spawnableKeys.pathID != choosenKeyPath)
                {
                    continue;
                }

                bool ignore = ServerIgnore(maskIgnoreList, spawnableKeys.mask);

                if (ignore)
                {
                    Debug.Log($"Key with mask {spawnableKeys.mask} has already spawned! Skipping...");
                    continue;
                }

                Vector3 spawnLocation = ServerGetRandomSpawnLocation(spawnPoints, spawnableKeys.mask);
                maskIgnoreList.Add(spawnableKeys.mask);
                ServerSpawnKey(spawnableKeys, spawnLocation);
            }
        }
    }

    private bool ServerIgnore(List<int> ignoreList, int maskID)
    {
        bool found = false;

        for (var i = 0; i < ignoreList.Count; i++)
        {
            int ignoreListMask = ignoreList[i];

            if (maskID == ignoreListMask)
            {
                found = true;
                break;
            }
        }

        return found;
    }


    private Vector3 ServerGetRandomSpawnLocation(GameObject[] spawnPoints, int maskIdToSpawn)
    {
        List<Vector3> potentialKeySpawns = new List<Vector3>();

        for (var i = 0; i < spawnPoints.Length; i++)
        {
            KeySpawnPoint spawnPointObject = spawnPoints[i].GetComponent<KeySpawnPoint>();
            KeysAtSpawnPoint[] keysAtSpawnPoint = spawnPointObject.SpawnableKeysAtPoint();

            for (var j = 0; j < keysAtSpawnPoint.Length; j++)
            {
                KeysAtSpawnPoint spawnableKeys = keysAtSpawnPoint[j];

                if (spawnableKeys.pathID == choosenKeyPath && spawnableKeys.mask == maskIdToSpawn)
                {
                    Vector3 spawnPointToAdd = spawnPoints[i].GetComponent<KeySpawnPoint>().transform.position;
                    potentialKeySpawns.Add(spawnPointToAdd);
                    break;

                }
            }
        }

        int randomNumber = UnityEngine.Random.Range(0, potentialKeySpawns.Count);
        return potentialKeySpawns[randomNumber];

    }



    private void ServerSpawnKey(KeysAtSpawnPoint spawner, Vector3 spawnPointPosition)
    {
        string keyName = spawner.keyName;
        int pathID = spawner.pathID;
        int mask = spawner.mask;
        KeyType type = spawner.type;
        GameObject keyPrefab = (GameObject)Resources.Load("Key");
        KeyObject key = Instantiate(keyPrefab, spawnPointPosition, Quaternion.identity).GetComponent<KeyObject>();
        key.SetMask(mask);
        key.SetName(keyName);
        key.SetType(type);
        NetworkServer.Spawn(key.gameObject);
    }

    private void OnServerClientGamePlayerClickedOnKey(NetworkConnection connection, ServerClientGameClickedOnKeyMessage message)
    {
        if (!NetworkIdentity.spawned.ContainsKey(message.requestedKeyId))
        {
            return;
        }

        Survivor survivor = connection.identity.GetComponent<Survivor>();

        if (survivor == null)
        {
            return;
        }

        KeyObject keyObject = NetworkIdentity.spawned[message.requestedKeyId].GetComponent<KeyObject>();

        if (keyObject == null)
        {
            return;
        }

        Vector3 keyPos = keyObject.transform.position;
        Vector3 survivorPos = survivor.transform.position;
        float distance = Vector3.Distance(keyPos, survivorPos);

        if (distance > survivor.GrabDistance())
        {
            return;
        }

        Key[] keys = survivor.Items().ServerKeys();

        int foundKeyMask = keyObject.Mask();
        KeyType type = keyObject.Type();
        bool alreadyHasKey = false;

        for (var i = 0; i < keys.Length; i++)
        {
            Key keyInInventory = keys[i];

            if (keyInInventory.Mask() == foundKeyMask)
            {
                alreadyHasKey = true;
                break;
            }

        }

        if (alreadyHasKey)
        {
            ClientServerGameAlreadyHaveKeyMessage clientServerGameAlreadyHaveKeyMessage = new ClientServerGameAlreadyHaveKeyMessage{};
            connection.identity.connectionToClient.Send(clientServerGameAlreadyHaveKeyMessage);
        }

        else
        {
            ClientServerGameSurvivorPickedUpKeyMessage clientServerGameSurvivorPickedUpKeyMessage = new ClientServerGameSurvivorPickedUpKeyMessage
            {
                keyId = keyObject.netIdentity.netId,
                survivorId = survivor.netIdentity.netId
            };

            Key newKey = new Key(keyObject.Name(), keyObject.Mask(), keyObject.Type());
            survivor.Items().ServerAdd(newKey);
            NetworkServer.SendToReady(clientServerGameSurvivorPickedUpKeyMessage);
            // NOTE: Normally we would destroy it but we just hide it on the client instead because we can't destroy the key itself while also letting the client play the sound for it.

        }
    }

}
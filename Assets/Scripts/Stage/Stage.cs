using UnityEngine;
using System.Collections.Generic;
using Mirror;

// NOTE: Not sure how this class will be used exactly. Maybe it will handle global events or something.
public class Stage : NetworkManager
{
    [SerializeField]
    private int batterySpawnChance;

    [SerializeField]
    private int forcedPathID = -1;

    [SerializeField]
    private int choosenKeyPath;

    [SerializeField]
    private GameObject batteryPrefab;

    private SurvivorSpawnPoint[] survivorSpawnPoints;

    private List<Survivor> survivors;

    private const int MONSTER_LURKER = 0;
    private const int MONSTER_PHANTOM = 1;
    private const int MONSTER_MARY = 2;
    private const int MONSTER_FALLEN = 3;

    private const int SURVIVOR_CHAD = 0;
    private const int SURVIVOR_ALICE = 1;
    private const int SURVIVOR_JESUS = 2;
    private const int SURVIVOR_JAMAL = 3;

    private int playerCount;

    private const int MAX_PLAYER_COUNT = 5;

    private void Start()
    {

    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        SetSurvivorSpawnPoints();
        ChoosePath();
        SpawnKeys();
        survivors = new List<Survivor>();
    }

    // NOTE: Called when a new client connects to the server.
    // NOTE: We need to do the following...
    // 1. See if new client can even join the server.
    //      If so, offer them available characters.
    //
    public override void OnServerConnect(NetworkConnection connection)
    {
        if (playerCount == 4)
        {
            //NOTE: Can we reject a client from here?
        }

        playerCount++;
        SpawnSurvivor(connection);
        base.OnServerConnect(connection);
    }

    public override void OnServerAddPlayer(NetworkConnection connection)
    {
        //Debug.Log($"{connection.identity.netId}");
    }

    // NOTE: Called when a client disconnects from the the server.
    public override void OnServerDisconnect(NetworkConnection connection)
    {
        string playerName = connection.identity.gameObject.GetComponent<Survivor>().Name();
        EventManager.playerDisconnectedEvent.Invoke(playerName);
        base.OnServerDisconnect(connection);

    }

    [Server]
    private void SpawnSurvivor(NetworkConnection connection)
    {
        for (var i = 0; i < survivorSpawnPoints.Length; i++)
        {
            SurvivorSpawnPoint survivorSpawnPoint = survivorSpawnPoints[i];

            if (survivorSpawnPoint.Used())
            {
                continue;
            }

            // We found a spawn point.
            GameObject chadPrefab = (GameObject)Resources.Load("Chad");
            GameObject chadSurvivor = Instantiate(chadPrefab, survivorSpawnPoint.gameObject.transform.position, Quaternion.identity);
            Survivor newSurvivor = chadSurvivor.GetComponent<Survivor>();
            survivors.Add(newSurvivor);
            NetworkServer.AddPlayerForConnection(connection, chadSurvivor);
            survivorSpawnPoint.SetUsed();
            break;
        }

    }

    [Server]
    private void SetSurvivorSpawnPoints()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(Tags.SURVIVOR_SPAWN_POINT);
        List<SurvivorSpawnPoint> survivorSpawnPointsList = new List<SurvivorSpawnPoint>();

        for (var i = 0; i < gameObjects.Length; i++)
        {
            SurvivorSpawnPoint spawnPoint = gameObjects[i].GetComponent<SurvivorSpawnPoint>();
            survivorSpawnPointsList.Add(spawnPoint);
        }

        survivorSpawnPoints = survivorSpawnPointsList.ToArray();
    }


    [Server]
    private void ChoosePath()
    {
        if (forcedPathID <= -1)
        {
            int maxPaths = GetMaxPaths();
            choosenKeyPath = Random.Range(0, maxPaths);
        }

        else
        {
            choosenKeyPath = forcedPathID;
        }

    }

    [Server]
    private void SpawnKeys()
    {
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag(Tags.KEY_SPAWN_POINT);
        List<int> maskIgnoreList = new List<int>();
        //List<KeySpawnPoint> spawnerList = new List<KeySpawnPoint>();

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

                bool ignore = Ignore(maskIgnoreList, spawnableKeys.mask);

                if (ignore)
                {
                    Debug.Log($"Key with mask {spawnableKeys.mask} has already spawned! Skipping...");
                    continue;
                }

                //spawnerList.Add(spawnPoints[i]);
                Vector3 spawnLocation = GetRandomSpawnLocation(spawnPoints, spawnableKeys.mask);
                maskIgnoreList.Add(spawnableKeys.mask);
                SpawnKey(spawnableKeys, spawnLocation);
            }
        }
    }


    [Server]
    private bool Ignore(List<int> ignoreList, int maskID)
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


    [Server]
    private Vector3 GetRandomSpawnLocation(GameObject[] spawnPoints, int maskIdToSpawn)
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

        int randomNumber = Random.Range(0, potentialKeySpawns.Count);
        return potentialKeySpawns[randomNumber];

    }


    [Server]
    private void SpawnKey(KeysAtSpawnPoint spawner, Vector3 spawnPointPosition)
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

    [Server]
    private int GetMaxPaths()
    {
        return 0;
    }
}

    /*
NOTE: When we get more prefabs, uncomment this.
    private GameObject DetermineKeyType(KeyType type)
    {
        switch (type)
        {
            case KeyType.Rusty:
                return (GameObject)Resources.Load("Key");
            default:
                break;
        }

    }
    */
    

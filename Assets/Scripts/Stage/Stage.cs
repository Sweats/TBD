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
    private GameObject keyPrefab;

    [SerializeField]
    private GameObject batteryPrefab;


    private ObjectSpawnPoint[] _objectSpawnPoints;

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

    public override void OnStartServer()
    {
        base.OnStartServer();
        survivors = new List<Survivor>();
        SetObjectSpawnPoints();
        SetSurvivorSpawnPoints();
        ChoosePath();
        SpawnKeys();
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
        }

    }


    /*
     [Server]
     private bool IsASurvivorAvailable()
     {
         bool available = true;

         for (var i = 0; i < survivorPrefabs.Length; i++)
         {
             Survivor survivor = survivorPrefabs[i];

             if (survivor.Dead() || survivor.Disconnected())
             {
                 continue;
             }

             available = false;
         }

         return available;
     }


     */


    /*
    [Server]
    private void SpawnSurvivor(int survivorIndex)
    {
        bool anySurvivorsLeft = false;

        for (var i = 0; i < survivors.Length; i++)
        {
            Survivor survivor = survivors[i];

            if (survivor.Dead() || survivor.Disconnected())
            {
                continue;
            }

            anySurvivorsLeft = true;

            for (var j = 0; j < survivorSpawnPoints.Length; j++)
            {
                SurvivorSpawnPoint survivorSpawnPoint = survivorSpawnPoints[j];

                if (survivorSpawnPoint.Used())
                {
                    continue;
                }

                Survivor newSurvivor = Instantiate(survivors[i], survivorSpawnPoint.gameObject.transform.position, Quaternion.identity);
                NetworkServer.Spawn(newSurvivor);
            }
        }

        if (!anySurvivorsLeft)
        {
            //TODO: Offer the survivor the chance to spectate.
        }
    }
    */

    /*
    [Server]
    private void SpawnSurvivors()
    {
        // NOTE: Not sure if I want it so survivors are randomly spawned on certain spawn points or give each one its own unique spawn point.
        //
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(Tags.SURVIVOR_SPAWN_POINT);
    }
    */


    /*
    [Server]
    private void SpawnMonster(int monster)
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(Tags.MONSTER_SPAWN_POINT);
        int choice = Random.Range(0, gameObjects.Length);

        switch (monster)
        {
            case MONSTER_LURKER:
                GameObject lurker = Instantiate(lurkerPrefab, gameObjects[choice].transform.position, Quaternion.identity);
                NetworkServer.Spawn(lurker.gameObject);
                break;
            case MONSTER_PHANTOM:
                GameObject phantom = Instantiate(phantomPrefab, gameObjects[choice].transform.position, Quaternion.identity);
                NetworkServer.Spawn(phantomPrefab.gameObject);
                break;
            case MONSTER_MARY:
                GameObject mary = Instantiate(maryPrefab, gameObjects[choice].transform.position, Quaternion.identity);
                NetworkServer.Spawn(maryPrefab.gameObject);
                break;
            //case MONSTER_FALLEN:
            //    fallenPrefab = Instantiate(fallenPrefab, gameObjects[choice].transform.position, Quaternion.identity );
            //    break;
            default:
                break;
        }
    }
    */

    [Server]
    private void SetObjectSpawnPoints()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(Tags.OBJECT_SPAWN_POINT);
        List<ObjectSpawnPoint> objectSpawnPoints = new List<ObjectSpawnPoint>();

        for (var i = 0; i < gameObjects.Length; i++)
        {
            ObjectSpawnPoint spawnPoint = gameObjects[i].GetComponent<ObjectSpawnPoint>();
            objectSpawnPoints.Add(spawnPoint);
        }

        _objectSpawnPoints = objectSpawnPoints.ToArray();
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
        List<int> ignoreMaskList = new List<int>();

        for (var i = 0; i < _objectSpawnPoints.Length; i++)
        {
            ObjectSpawnPoint spawnPoint = _objectSpawnPoints[i];
            Key[] keys = spawnPoint.Keys();

            for (var j = 0; j < keys.Length; j++)
            {
                Key key = keys[j];
                int keyPathID = key.PathID();
                int maskID = key.Mask();

                // TODO: How should we make it so we can have multiple keys of the same mask on the stage at a time?
                // Only keys with a unique mask can be spawned. Any duplicates will be ignored.

                bool ignore = ShouldIgnore(ignoreMaskList, maskID);

                if (keyPathID == choosenKeyPath && !ignore)
                {
                    ignoreMaskList.Add(maskID);
                    SpawnKey(key);

                }
            }
        }
    }


    [Server]
    private bool ShouldIgnore(List<int> ignoreMaskList, int maskID)
    {
        bool found = false;

        for (var i = 0; i < ignoreMaskList.Count; i++)
        {
            int foundIgnoreMaskID = ignoreMaskList[i];

            if (maskID == foundIgnoreMaskID)
            {
                found = true;
                break;
            }
        }


        return found;
    }

    [Server]
    private void SpawnKey(Key key)
    {
        Debug.Log($"Key spawned mask = {key.Mask()} ");
        ObjectSpawnPoint[] potentialSpawnPoints = GetSpawnPointsForKey(key);
        int randomNumber = Random.Range(0, potentialSpawnPoints.Length);
        ObjectSpawnPoint pickedSpawnPoint = potentialSpawnPoints[randomNumber];
        GameObject spawnedKeyObject = Instantiate(keyPrefab, pickedSpawnPoint.transform.position, Quaternion.identity);
        //NOTE: Maybe there is a cleaner way to do this.
        spawnedKeyObject.GetComponent<KeyObject>().SetKey(key);
        NetworkServer.Spawn(spawnedKeyObject);
    }

    [Server]
    private ObjectSpawnPoint[] GetSpawnPointsForKey(Key key)
    {
        List<ObjectSpawnPoint> objectSpawnPoints = new List<ObjectSpawnPoint>();

        for (var i = 0; i < _objectSpawnPoints.Length; i++)
        {
            ObjectSpawnPoint foundObjectSpawnPoint = _objectSpawnPoints[i];
            Key[] keys = _objectSpawnPoints[i].Keys();

            for (var j = 0; j < keys.Length; j++)
            {
                Key foundKey = keys[j];
                int foundKeyMaskID = foundKey.Mask();
                int foundKeyPathID = foundKey.PathID();

                int keyPathID = key.PathID();
                int keyMask = key.Mask();

                if (foundKeyMaskID == keyMask && keyPathID == foundKeyPathID)
                {
                    objectSpawnPoints.Add(foundObjectSpawnPoint);
                }
            }
        }

        return objectSpawnPoints.ToArray();

    }


    [Server]
    private int GetMaxPaths()
    {
        return 0;
    }
}

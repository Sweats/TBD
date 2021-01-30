using UnityEngine;
using System.Collections.Generic;
using Mirror;
using System;

public struct Player
{
    public string playerName;
    public Character character;
    public NetworkIdentity identity;

    public Player(string name, Character character, NetworkIdentity identity)
    {
        this.playerName = name;
        this.character = character;
        this.identity = identity;
    }
}

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

    private List<Player> players;

    private Character playerOneCharacter;
    private Character playerTwoCharacter;
    private Character playerThreeCharacter;
    private Character playerFourCharacter;
    private Character playerFiveCharacter;

    private Character monsterCharacter = Character.Unknown;

    private bool monsterCharacterAvailable = true;

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

        players = new List<Player>();
        ServerSetSurvivorSpawnPoints();
        ServerChoosePath();
        ServerSpawnKeys();

        NetworkServer.RegisterHandler<ServerPlayerSentChatMessage>(ServerOnPlayerSentChatMessage);
        NetworkServer.RegisterHandler<ServerPlayerChangedProfileNameMessage>(ServerOnPlayerChangedProfileNameChanged);
        NetworkServer.RegisterHandler<ServerClientPickedCharacterMessage>(ServerClientPickedCharacter);
        NetworkServer.RegisterHandler<ServerPlayerJoinedPlayerMessage>(OnPlayerJoined);

    }

    // NOTE: Called after the player spawns in with the spectator object.
    [Server]
    private void OnPlayerJoined(NetworkConnection connection, ServerPlayerJoinedPlayerMessage message)
    {
        NetworkIdentity identity = message.identity;
        string playerName = message.clientName;
        Debug.Log($"Player name of the joined player is {playerName}");
        players.Add(new Player(playerName, Character.Unknown, identity));
        NetworkServer.SendToAll(new ClientPlayerJoinedMessage { clientName = playerName });
        Character[] availableCharactersArray = ServerAvailableCharacters();
        NetworkServer.SendToClientOfPlayer(identity, new ClientPickCharacterMessage { availableCharacters = availableCharactersArray });
    }

    public override void OnServerConnect(NetworkConnection connection)
    {
        base.OnServerConnect(connection);

        if (playerCount == 4)
        {
            //NOTE: Can we reject a client from here?
        }

        playerCount++;
        //ServerSpawnSurvivor(connection);
        GameObject playerSpectatorObject = (GameObject)Resources.Load("PlayerSpectator");
        GameObject spawnedSpectator = Instantiate(playerSpectatorObject, new Vector3(0, 200, 0), Quaternion.identity);
        NetworkServer.AddPlayerForConnection(connection, spawnedSpectator);
    }

    public override void OnClientConnect(NetworkConnection connection)
    {
        base.OnClientConnect(connection);

        NetworkClient.RegisterHandler<ClientPlayerSentChatMessage>(ClientOnPlayerSentChatMessage);
        //NetworkClient.RegisterHandler<ClientPlayerChangedProfileNameMessage>(ClientOnPlayerChangedProfleName);
        NetworkClient.RegisterHandler<ClientPickCharacterMessage>(ClientPickCharacter);
        NetworkClient.RegisterHandler<ClientPlayerJoinedMessage>(ClientPlayerJoined);
        NetworkClient.RegisterHandler<ClientPlayerDisconnectedMessage>(OnClientPlayerDisconnected);

    }

    // NOTE: Called when a client disconnects from the the server.
    public override void OnServerDisconnect(NetworkConnection connection)
    {
        base.OnServerDisconnect(connection);

        uint id = connection.identity.netId;
        int playerDisconnectedIndex = 0;

        for (var i = 0; i < players.Count; i++)
        {
            uint foundId = players[i].identity.netId;

            if (id == foundId)
            {
                string playerName = players[i].playerName;
                playerDisconnectedIndex = i;
                NetworkServer.SendToAll(new ClientPlayerDisconnectedMessage { clientName = playerName });
                break;
            }
        }

        Character character = players[playerDisconnectedIndex].character;

        if ((byte)character > 4)
        {
            monsterCharacterAvailable = true;
        }
    }

    [Server]
    private void ServerSpawnSurvivor(NetworkConnection connection, GameObject survivor)
    {
        GameObject playerSpectatorObject = connection.identity.gameObject;
        for (var i = 0; i < survivorSpawnPoints.Length; i++)
        {
            SurvivorSpawnPoint survivorSpawnPoint = survivorSpawnPoints[i];

            if (survivorSpawnPoint.Used())
            {
                Debug.Log("Skipping survivor spawn point");
                continue;
            }

            // We found a spawn point.
            GameObject spawnedSurvivor = Instantiate(survivor, survivorSpawnPoint.gameObject.transform.position, Quaternion.identity);
            NetworkServer.ReplacePlayerForConnection(connection, spawnedSurvivor);
            NetworkServer.Destroy(playerSpectatorObject);
            survivorSpawnPoint.SetUsed();
            break;
        }

    }

    [Server]
    private void ServerSetSurvivorSpawnPoints()
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
    private void ServerChoosePath()
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

    [Server]
    private void ServerSpawnKeys()
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

                bool ignore = ServerIgnore(maskIgnoreList, spawnableKeys.mask);

                if (ignore)
                {
                    Debug.Log($"Key with mask {spawnableKeys.mask} has already spawned! Skipping...");
                    continue;
                }

                //spawnerList.Add(spawnPoints[i]);
                Vector3 spawnLocation = ServerGetRandomSpawnLocation(spawnPoints, spawnableKeys.mask);
                maskIgnoreList.Add(spawnableKeys.mask);
                ServerSpawnKey(spawnableKeys, spawnLocation);
            }
        }
    }


    [Server]
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


    [Server]
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


    [Server]
    private Character[] ServerAvailableCharacters()
    {
        List<Character> availableCharacters = new List<Character>();
        int count = 0;

        bool chadUsed = false, aliceUsed = false, jamalUsed = false, jesusUsed = false;

        for (var i = 0; i < players.Count; i++)
        {
            Character character = players[i].character;

            switch (character)
            {
                case Character.Chad:
                    chadUsed = true;
                    break;
                case Character.Jamal:
                    jamalUsed = true;
                    break;
                case Character.Jesus:
                    jesusUsed = true;
                    break;
                case Character.Alice:
                    aliceUsed = true;
                    break;
            }
        }

        if (monsterCharacterAvailable)
        {
            availableCharacters.Add(monsterCharacter);
        }

        if (!jamalUsed)
        {
            count++;
            availableCharacters.Add(Character.Jamal);
        }

        if (!aliceUsed)
        {
            count++;
            availableCharacters.Add(Character.Alice);
        }

        if (!chadUsed)
        {
            count++;
            availableCharacters.Add(Character.Chad);
        }

        if (!jesusUsed)
        {
            count++;
            availableCharacters.Add(Character.Jesus);
        }

        if (monsterCharacter == Character.Unknown)
        {
            availableCharacters.Add(Character.Fallen);
            availableCharacters.Add(Character.Lurker);
            availableCharacters.Add(Character.Phantom);
            availableCharacters.Add(Character.Mary);
        }

        //TODO: Test this.
        if (count < 4 || monsterCharacterAvailable)
        {
            availableCharacters.Add(Character.Random);
        }

        return availableCharacters.ToArray();
    }


    [Server]
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

    [Server]
    private int ServerGetMaxPaths()
    {
        return 0;
    }

    [Server]
    private void ServerOnPlayerSentChatMessage(NetworkConnection connection, ServerPlayerSentChatMessage message)
    {
        string clientName = message.playerName;
        string clientText = message.text;
        NetworkServer.SendToAll(new ClientPlayerSentChatMessage { playerName = clientName, text = clientText });
    }

    [Server]
    private void ServerOnPlayerChangedProfileNameChanged(NetworkConnection connection, ServerPlayerChangedProfileNameMessage message)
    {
        string oldProfileName = message.oldName;
        string newProfileName = message.newName;
        NetworkServer.SendToAll(new ClientPlayerChangedProfileNameMessage { newName = newProfileName, oldName = oldProfileName });

    }

    [Client]
    private void ClientOnPlayerChangedProfleName(NetworkConnection connection, ClientPlayerChangedProfileNameMessage message)
    {
        string oldProfileName = message.oldName;
        string newProfileName = message.newName;
        Debug.Log("Detected profile message from the server");
        EventManager.playerChangedNameEvent.Invoke(oldProfileName, newProfileName);
    }

    [Client]
    private void ClientOnPlayerSentChatMessage(NetworkConnection connection, ClientPlayerSentChatMessage message)
    {
        string clientName = message.playerName;
        string clientText = message.text;
        Debug.Log("Detected chat message from the server");
        EventManager.playerRecievedChatMessageEvent.Invoke(clientName, clientText);
    }

    [Client]
    private void ClientPickCharacter(NetworkConnection connection, ClientPickCharacterMessage message)
    {
        Character[] characters = message.availableCharacters;
        Debug.Log("Server asked to is pick a character");
        EventManager.serverAskedYouToPickCharacterEvent.Invoke(characters);
    }

    [Client]
    private void OnClientPlayerDisconnected(NetworkConnection connection, ClientPlayerDisconnectedMessage message)
    {
        string name = message.clientName;
        Debug.Log("Client disconnected");
        EventManager.playerDisconnectedEvent.Invoke(name);
    }

    [Client]
    private void ClientPlayerJoined(NetworkConnection connection, ClientPlayerJoinedMessage message)
    {
        string name = message.clientName;
        Debug.Log("Player joined");
        EventManager.playerJoinedEvent.Invoke(name);
    }

    [Server]
    private void ServerClientPickedCharacter(NetworkConnection connection, ServerClientPickedCharacterMessage message)
    {
        Character pickedCharacter = message.pickedCharacter;
        Debug.Log($"player picked character {pickedCharacter}");

        uint netId = connection.identity.netId;

        for (var i = 0; i < players.Count; i++)
        {
            uint foundId = players[i].identity.netId;

            if (foundId == netId)
            {
                Player player = players[i];
                player.character = pickedCharacter;
                players[i] = player;
                break;
            }
        }

        // NOTE: If the value itself is greater than 4 then it must be a monster.

        if ((byte)pickedCharacter > 4)
        {
            GameObject monster = Monster(pickedCharacter);
            ServerSpawnMonster(connection, monster);
        }

        else
        {
            GameObject survivor = Survivor(pickedCharacter);
            ServerSpawnSurvivor(connection, survivor);
        }

    }

    [Server]
    private void ServerSpawnMonster(NetworkConnection connection, GameObject monster)
    {
        GameObject playerGameObject = connection.identity.gameObject;
        GameObject[] monsterSpawnPoints = GameObject.FindGameObjectsWithTag(Tags.MONSTER_SPAWN_POINT);
        int randomNumber = UnityEngine.Random.Range(0, monsterSpawnPoints.Length);
        GameObject pickedSpawnPoint = monsterSpawnPoints[randomNumber];
        GameObject spawnedMonster = Instantiate(monster, pickedSpawnPoint.transform.position, Quaternion.identity);
        NetworkServer.ReplacePlayerForConnection(connection, spawnedMonster);
        NetworkServer.Destroy(playerGameObject);
    }

    [Server]
    private GameObject Survivor(Character character)
    {
        switch (character)
        {
            case Character.Chad:
                return (GameObject)Resources.Load("Chad");
            case Character.Alice:
                return (GameObject)Resources.Load("Alice");
            case Character.Jamal:
                return (GameObject)Resources.Load("Jamal");
            case Character.Jesus:
                return (GameObject)Resources.Load("Jesus");
            default:
                return null;
        }
    }

    [Server]
    private GameObject Monster(Character character)
    {
        monsterCharacterAvailable = false;

        switch (character)
        {
            case Character.Lurker:
                return (GameObject)Resources.Load("Lurker");
            case Character.Phantom:
                return (GameObject)Resources.Load("Phantom");
            case Character.Mary:
                return (GameObject)Resources.Load("Mary");
            case Character.Fallen:
                return (GameObject)Resources.Load("Fallen");
            default:
                return null;
        }
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



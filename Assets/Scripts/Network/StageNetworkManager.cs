using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class StageNetworkManager : MonoBehaviour
{
    [SerializeField]
    private int batterySpawnChance;

    [SerializeField]
    private int forcedPathID = -1;

    [SerializeField]
    private int choosenKeyPath;

    private SurvivorSpawnPoint[] survivorSpawnPoints;

    private Character playerOneCharacter;
    private Character playerTwoCharacter;
    private Character playerThreeCharacter;
    private Character playerFourCharacter;
    private Character playerFiveCharacter;

    private Character monsterCharacter = Character.Empty;

    private bool monsterCharacterAvailable;

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

    public void OnStartServer()
    {
        Debug.Log("OnStartServer() called!");
    }

    // NOTE: Called on any client that disconnects from the server.

    public void OnServerDisconnect()
    {

    }

    public void OnStopServer()
    {
        Debug.Log("OnStopServer() called!");
        Stages.Load(StageName.Menu);
    }

    public void RegisterServerHandlers()
    {
        NetworkServer.RegisterHandler<ServerPlayerSentChatMessage>(ServerOnPlayerSentChatMessage);
        NetworkServer.RegisterHandler<ServerPlayerChangedProfileNameMessage>(ServerOnPlayerChangedProfileNameChanged);
        NetworkServer.RegisterHandler<ServerClientPickedCharacterMessage>(ServerClientPickedCharacter);
        NetworkServer.RegisterHandler<ServerClientLoadedSceneMessage>(ServerOnClientLoadedScene);
    }

    // NOTE: Called after the player spawns in with the spectator object.


    public void OnServerConnect(NetworkConnection connection)
    {
        Debug.Log("OnServerConnect() called!");

        if (playerCount == 4)
        {
            //NOTE: Can we reject a client from here?
        }

        playerCount++;

        GameObject playerSpectatorObject = (GameObject)Resources.Load("PlayerSpectator");
        GameObject spawnedSpectator = Instantiate(playerSpectatorObject, new Vector3(0, 200, 0), Quaternion.identity);
        NetworkServer.AddPlayerForConnection(connection, spawnedSpectator);
    }

    public void OnPlayerJoined(NetworkConnection connection, ServerPlayerJoinedMessage message)
    {
        NetworkIdentity identity = message.clientIdentity;
        string playerName = message.clientName;
        Debug.Log($"Player name of the joined player is {playerName}");
        NetworkRoom.players.Add(new Player(playerName, Character.Spectator, identity));
        NetworkServer.SendToAll(new ClientPlayerJoinedMessage { clientName = playerName });
        Character[] unavailableCharactersArray = ServerUnavailableCharacters();
        NetworkServer.SendToClientOfPlayer(identity, new ClientPickCharacterMessage { availableCharacters = unavailableCharactersArray });
    }

    public void OnServerDisconnect(NetworkConnection connection)
    {
        List<Player> players = NetworkRoom.players;
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

        if ((byte)character >= 5)
        {
            monsterCharacterAvailable = true;
        }

        NetworkRoom.players.RemoveAt(playerDisconnectedIndex);
        NetworkServer.DestroyPlayerForConnection(connection);
    }

    public void OnServerSceneChanged(string sceneName)
    {
        ServerSetSurvivorSpawnPoints();
        ServerChoosePath();
        ServerSpawnKeys();
    }


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


    private int ServerGetMaxPaths()
    {
        return 0;
    }

    private void ServerOnPlayerSentChatMessage(NetworkConnection connection, ServerPlayerSentChatMessage message)
    {
        string clientName = message.playerName;
        string clientText = message.text;
        NetworkServer.SendToAll(new ClientPlayerSentChatMessage { playerName = clientName, text = clientText });
    }

    private void ServerOnPlayerChangedProfileNameChanged(NetworkConnection connection, ServerPlayerChangedProfileNameMessage message)
    {
        string oldProfileName = message.oldName;
        string newProfileName = message.newName;
        NetworkServer.SendToAll(new ClientPlayerChangedProfileNameMessage { newName = newProfileName, oldName = oldProfileName });

    }

    private void ClientOnPlayerChangedProfleName(NetworkConnection connection, ClientPlayerChangedProfileNameMessage message)
    {
        string oldProfileName = message.oldName;
        string newProfileName = message.newName;
        Debug.Log("Detected profile message from the server");
        EventManager.playerChangedNameEvent.Invoke(oldProfileName, newProfileName);
    }

    private void ClientOnPlayerSentChatMessage(NetworkConnection connection, ClientPlayerSentChatMessage message)
    {
        string clientName = message.playerName;
        string clientText = message.text;
        Debug.Log("Detected chat message from the server");
        EventManager.playerRecievedChatMessageEvent.Invoke(clientName, clientText);
    }

    private void ClientPickCharacter(NetworkConnection connection, ClientPickCharacterMessage message)
    {
        Character[] characters = message.availableCharacters;
        Debug.Log("Server asked to is pick a character");
        EventManager.serverAskedYouToPickCharacterEvent.Invoke(characters);
    }

    private void OnClientPlayerDisconnected(NetworkConnection connection, ClientPlayerDisconnectedMessage message)
    {
        string name = message.clientName;
        Debug.Log("Client disconnected");
        EventManager.playerDisconnectedEvent.Invoke(name);
    }

    private void ClientPlayerJoined(NetworkConnection connection, ClientPlayerJoinedMessage message)
    {
        string name = message.clientName;
        Debug.Log("Player joined");
        EventManager.playerJoinedEvent.Invoke(name);
    }


    private void ServerClientPickedCharacter(NetworkConnection connection, ServerClientPickedCharacterMessage message)
    {
        Character pickedCharacter = message.pickedCharacter;
        List<Player> players = NetworkRoom.players;
        Debug.Log($"player picked character {pickedCharacter}");
        uint netId = connection.identity.netId;
        ServerSpawnPlayer(connection, pickedCharacter, true);
    }

    // NOTE: Called only when players are joining the scene from the lobby. OnClientSceneChanged() only gets called when the server
    // changes the scene.
    // Logic for when clients join the server when the server is NOT in a lobby is handeled in OnServerConnect(). 

    private void ServerOnClientLoadedScene(NetworkConnection connection, ServerClientLoadedSceneMessage message)
    {
        List<Player> players = NetworkRoom.players;
        uint netId = connection.identity.netId;

        for (var i = 0; i < players.Count; i++)
        {
            uint foundNetId = players[i].identity.netId;

            if (foundNetId == netId)
            {
                Character playerCharacter = players[i].character;
                Debug.Log($"Player character is {playerCharacter}");
                ServerSpawnPlayer(connection, playerCharacter, false);
            }
        }
    }

    private void ServerSpawnPlayer(NetworkConnection connection, Character character, bool joiningMidGame)
    {
        if (character == Character.Random)
        {
            Character[] availableCharacters = ServerUnavailableCharacters();
            int randomIndex = UnityEngine.Random.Range(0, availableCharacters.Length);
            Character pickedCharacter = availableCharacters[randomIndex];
            Debug.Log($"Randomly picked character is {pickedCharacter}!");

            if ((byte)pickedCharacter >= 5)
            {
                ServerSpawnMonster(connection, pickedCharacter, joiningMidGame);
            }

            else
            {
                ServerSpawnSurvivor(connection, pickedCharacter, joiningMidGame);
            }

            ServerUpdatePlayerSlot(connection.identity.netId, pickedCharacter);
        }

        else
        {
            if ((byte)character >= 5)
            {
                ServerSpawnMonster(connection, character, joiningMidGame);
            }

            else
            {
                ServerSpawnSurvivor(connection, character, joiningMidGame);
            }

            ServerUpdatePlayerSlot(connection.identity.netId, character);
        }
    }

    private Character[] ServerUnavailableCharacters()
    {
        List<Player> players = NetworkRoom.players;
        List<Character> characters = new List<Character>();

        for (var i = 0; i < players.Count; i++)
        {
            Character playerCharacter = players[i].character;

            if (playerCharacter == Character.Spectator)
            {
                continue;
            }

            Debug.Log($"{playerCharacter} is the same as {monsterCharacter}?");

            if (playerCharacter == monsterCharacter)
            {
                characters.Add(Character.Mary);
                characters.Add(Character.Fallen);
                characters.Add(Character.Lurker);
                characters.Add(Character.Phantom);
                continue;
            }

            characters.Add(playerCharacter);
        }

        return characters.ToArray();
    }

    private void ServerSpawnMonster(NetworkConnection connection, Character monster, bool joiningMidGame)
    {
        this.monsterCharacter = monster;
        this.monsterCharacterAvailable = false;
        GameObject[] monsterSpawnPoints = GameObject.FindGameObjectsWithTag(Tags.MONSTER_SPAWN_POINT);
        int randomNumber = UnityEngine.Random.Range(0, monsterSpawnPoints.Length);
        GameObject monsterObject = Monster(monster);
        GameObject pickedSpawnPoint = monsterSpawnPoints[randomNumber];
        GameObject spawnedMonster = Instantiate(monsterObject, pickedSpawnPoint.transform.position, Quaternion.identity);

        if (joiningMidGame)
        {
            GameObject playerGameObject = connection.identity.gameObject;
            NetworkServer.ReplacePlayerForConnection(connection, spawnedMonster);
            NetworkServer.Destroy(playerGameObject);
        }

        else
        {
            NetworkServer.AddPlayerForConnection(connection, spawnedMonster);
        }
    }

    private void ServerSpawnSurvivor(NetworkConnection connection, Character survivorCharacter, bool joiningMidGame)
    {
        for (var i = 0; i < survivorSpawnPoints.Length; i++)
        {
            SurvivorSpawnPoint survivorSpawnPoint = survivorSpawnPoints[i];

            // We found a spawn point.
            if (survivorSpawnPoint.Used())
            {
                Debug.Log("Skipping survivor spawn point");
                continue;
            }

            survivorSpawnPoint.SetUsed();
            GameObject survivorObject = Survivor(survivorCharacter);
            GameObject spawnedSurvivor = Instantiate(survivorObject, survivorSpawnPoint.gameObject.transform.position, Quaternion.identity);
            if (joiningMidGame)
            {
                GameObject playerSpectatorObject = connection.identity.gameObject;
                NetworkServer.ReplacePlayerForConnection(connection, spawnedSurvivor);
                NetworkServer.Destroy(playerSpectatorObject);
            }

            else
            {
                NetworkServer.AddPlayerForConnection(connection, spawnedSurvivor);
            }

            break;
        }

    }

    private void ServerUpdatePlayerSlot(uint netId, Character pickedCharacter)
    {
        List<Player> players = NetworkRoom.players;

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
    }

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

    private GameObject Monster(Character character)
    {
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

    public void OnClientConnect(NetworkConnection connection)
    {
        Debug.Log("OnClientConnect() called!");
    }

    public void OnClientDisconnect(NetworkConnection connection)
    {
        //EventManager.stageClientServerDisconnectdEvent.Invoke();
        Stages.Load(StageName.Menu);
    }

    public void RegisterClientHandlers()
    {
        NetworkClient.RegisterHandler<ClientPlayerSentChatMessage>(ClientOnPlayerSentChatMessage);
        //NetworkClient.RegisterHandler<ClientPlayerChangedProfileNameMessage>(ClientOnPlayerChangedProfleName);
        NetworkClient.RegisterHandler<ClientPickCharacterMessage>(ClientPickCharacter);
        NetworkClient.RegisterHandler<ClientPlayerJoinedMessage>(ClientPlayerJoined);
        NetworkClient.RegisterHandler<ClientPlayerDisconnectedMessage>(OnClientPlayerDisconnected);
        //NetworkClient.RegisterHandler<ClientServerDisconnectedMessage>(OnClientPlayerDisconnected);
    }

    public void OnClientSceneChanged(NetworkConnection connection)
    {
        Debug.Log("CLIENT CONNECTED TO THE NEW SCENE ON THE SERVER!");
        NetworkClient.Send(new ServerClientLoadedSceneMessage());
    }

    public void OnStartClient()
    {

    }

}


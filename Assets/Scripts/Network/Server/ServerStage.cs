using UnityEngine;
using Mirror;
using System.Collections.Generic;

public class ServerStage : MonoBehaviour
{
    private ServerStage() { }

    private SurvivorSpawnPoint[] survivorSpawnPoints;

    private Character monsterCharacterForGame = Character.Empty;

    // NOTE: Called when we are going from lobby -> stage.
    public void OnServerSceneChanged()
    {
        ServerSetSurvivorSpawnPoints();
        SpawnPlayersFromLobby();
    }

    public void RegisterNetworkHandlers()
    {
        NetworkServer.RegisterHandler<ServerClientGamePlayerPickedCharacterMessage>(ServerClientGameOnPlayerPickedCharacterMessage);
        NetworkServer.RegisterHandler<ServerClientGameHostRequestedToStartGameMessage>(OnServerClientGameHostStartedGame);
        NetworkServer.RegisterHandler<ServerClientGamePlayerChangedProfileNameMessage>(OnServerClientGamePlayerChangedProfileName);
        NetworkServer.RegisterHandler<ServerClientGameLurkerJoinedMessage>(OnServerClientGameLurkerJoined);
        NetworkServer.RegisterHandler<ServerClientGamePhantomJoinedMessage>(OnServerClientGamePhantomJoined);
        NetworkServer.RegisterHandler<ServerClientGamePlayerSpectatorJoinedMessage>(OnServerClientGamePlayerSpectatorConnected);
        NetworkServer.RegisterHandler<ServerClientGameMaryJoinedMessage>(OnServerClientGameMaryJoined);

    }

    private void OnServerClientGamePlayerSpectatorConnected(NetworkConnection connection, ServerClientGamePlayerSpectatorJoinedMessage message)
    {
        Character[] unavailableCharactersOnServer = ServerUnavailableCharacters();
        PlayerSpectator spectator = connection.identity.GetComponent<PlayerSpectator>();
        string connectedPlayerName = spectator.ServerName();
        connection.identity.connectionToClient.Send(new ClientServerGamePickCharacterMessage { unavailableCharacters = unavailableCharactersOnServer });
        NetworkServer.SendToReady(new ClientServerGamePlayerConnectedMessage { name = connectedPlayerName });

    }

    private void OnServerClientGameLurkerJoined(NetworkConnection connection, ServerClientGameLurkerJoinedMessage message)
    {
        Lurker lurker;

        bool isLurker = connection.identity.TryGetComponent<Lurker>(out lurker);

        if (!isLurker)
        {
            return;
        }

        monsterCharacterForGame = Character.Lurker;

        EventManager.serverClientGameLurkerJoinedEvent.Invoke(lurker.netIdentity.netId);

    }

    private void OnServerClientGameMaryJoined(NetworkConnection connection, ServerClientGameMaryJoinedMessage message)
    {
        Mary mary;

        bool isMary = connection.identity.TryGetComponent<Mary>(out mary);

        if (!isMary)
        {
            return;
        }

        monsterCharacterForGame = Character.Mary;

        uint id = connection.identity.netId;

        EventManager.serverClientGameMaryJoinedEvent.Invoke(id);

    }

    private void OnServerClientGamePhantomJoined(NetworkConnection connection, ServerClientGamePhantomJoinedMessage message)
    {
        Phantom phantom;

        bool isPhantom = connection.identity.TryGetComponent<Phantom>(out phantom);

        if (!isPhantom)
        {
            return;
        }

        uint netid = phantom.netIdentity.netId;

        monsterCharacterForGame = Character.Phantom;

        EventManager.serverClientGamePhantomJoinedEvent.Invoke(netid);
    }

    private void OnServerClientGamePlayerChangedProfileName(NetworkConnection connection, ServerClientGamePlayerChangedProfileNameMessage message)
    {
        Survivor survivor = connection.identity.GetComponent<Survivor>();

        if (survivor == null)
        {
            return;
        }

        survivor.ServerSetName(message.newProfileName);

        // NOTE: Is there a point in setting the player name for the monster?

        NetworkServer.SendToReady(new ClientServerGamePlayerChangedProfileNameMessage { oldName = message.oldProfileName, newName = message.newProfileName });

    }

    private void OnServerClientGameHostStartedGame(NetworkConnection connection, ServerClientGameHostRequestedToStartGameMessage message)
    {
        NetworkServer.SendToReady(new ClientServerGameHostStartedGameMessage { });
    }

    private void ServerClientGameOnPlayerPickedCharacterMessage(NetworkConnection connection, ServerClientGamePlayerPickedCharacterMessage message)
    {
        Character pickedCharacter = message.pickedCharacter;

        PlayerSpectator spectator = connection.identity.GetComponent<PlayerSpectator>();

        if (spectator == null)
        {
            return;
        }


        string playerName = spectator.ServerName();

        if ((byte)pickedCharacter >= 5)
        {
            ServerSpawnMonster(connection, pickedCharacter);
        }

        else
        {
            ServerSpawnSurvivorMidGame(connection, pickedCharacter);
        }

        NetworkServer.SendToReady(new ClientServerGamePlayerJoinedMessage { name = playerName });

    }

    // NOTE: Only called when someone joins a game mid match.
    public void OnServerConnect(NetworkConnection connection)
    {
        SpawnPlayerSpectator(connection);
        Character[] character = ServerUnavailableCharacters();
        connection.identity.connectionToClient.Send(new ClientServerGamePickCharacterMessage { unavailableCharacters = character });
    }

    public void OnServerDisconnect(NetworkConnection connection)
    {
        Survivor survivor;
        string disconnectedName = string.Empty;

        bool isSurvivor = connection.identity.TryGetComponent<Survivor>(out survivor);

        if (isSurvivor)
        {
            disconnectedName = survivor.Name();
        }

        Mary mary;

        bool isMary = connection.identity.TryGetComponent<Mary>(out mary);

        if (isMary)
        {
            disconnectedName = mary.Name();
        }

        Lurker lurker;

        bool isLurker = connection.identity.TryGetComponent<Lurker>(out lurker);

        if (isLurker)
        {
            disconnectedName = lurker.Name();

        }

        Phantom phantom;

        bool isPhantom = connection.identity.TryGetComponent<Phantom>(out phantom);

        if (isPhantom)
        {
            disconnectedName = phantom.ServerName();
        }

        NetworkServer.SendToReady(new ClientServerGamePlayerDisconnectedMessage { name = disconnectedName });
    }

    private void SpawnPlayerSpectator(NetworkConnection connection)
    {
        GameObject playerSpectatorObject = (GameObject)Resources.Load("PlayerSpectator");
        GameObject spawnedSpectator = Instantiate(playerSpectatorObject, new Vector3(0, 200, 0), Quaternion.identity);
        NetworkServer.AddPlayerForConnection(connection, spawnedSpectator);

    }

    private Character[] ServerUnavailableCharacters()
    {
        List<Character> unAvailableCharacters = new List<Character>();

        var keys = NetworkServer.connections.Keys;

        if (monsterCharacterForGame == Character.Phantom)
        {
            unAvailableCharacters.Add(Character.Lurker);
            unAvailableCharacters.Add(Character.Fallen);
            unAvailableCharacters.Add(Character.Mary);

        }

        else if (monsterCharacterForGame == Character.Lurker)
        {
            unAvailableCharacters.Add(Character.Fallen);
            unAvailableCharacters.Add(Character.Phantom);
            unAvailableCharacters.Add(Character.Mary);
        }

        else if (monsterCharacterForGame == Character.Mary)
        {
            unAvailableCharacters.Add(Character.Fallen);
            unAvailableCharacters.Add(Character.Phantom);
            unAvailableCharacters.Add(Character.Lurker);
        }

        foreach (int key in keys)
        {
            int connectionId = key;

            NetworkConnectionToClient connection = NetworkServer.connections[connectionId];

            Survivor survivor = connection.identity.GetComponent<Survivor>();

            if (survivor != null)
            {
                unAvailableCharacters.Add(survivor.SurvivorCharacter());
                continue;
            }


            Mary mary = connection.identity.GetComponent<Mary>();

            if (mary != null)
            {
                monsterCharacterForGame = Character.Mary;
                unAvailableCharacters.Add(Character.Mary);
                continue;
            }

            Lurker lurker = connection.identity.GetComponent<Lurker>();

            if (lurker != null)
            {
                monsterCharacterForGame = Character.Lurker;
                unAvailableCharacters.Add(Character.Lurker);
                continue;

            }

            Phantom phantom = connection.identity.GetComponent<Phantom>();

            if (phantom != null)
            {
                monsterCharacterForGame = Character.Phantom;
                unAvailableCharacters.Add(Character.Phantom);
                continue;
            }

        }

        return unAvailableCharacters.ToArray();

    }

    private void SpawnPlayersFromLobby()
    {
        var keys = NetworkServer.connections.Keys;

        foreach (int key in keys)
        {
            int connectionId = key;
            NetworkConnectionToClient connection = NetworkServer.connections[key];

            LobbyPlayer player = connection.identity.GetComponent<LobbyPlayer>();

            if (player == null)
            {
                return;
            }

            Character selectedCharacter = player.SelectedCharacter();

            if ((byte)selectedCharacter <= 5)
            {
                ServerSpawnSurvivorLobby(connection, player.SelectedCharacter());
            }

            else
            {
                ServerSpawnMonster(connection, player.SelectedCharacter());
            }
        }
    }

    private void ServerSpawnMonster(NetworkConnection connection, Character monster)
    {
        GameObject[] monsterSpawnPoints = GameObject.FindGameObjectsWithTag(Tags.MONSTER_SPAWN_POINT);
        int randomNumber = UnityEngine.Random.Range(0, monsterSpawnPoints.Length);
        GameObject monsterObject = Monster(monster);
        GameObject pickedSpawnPoint = monsterSpawnPoints[randomNumber];
        GameObject spawnedMonster = Instantiate(monsterObject, pickedSpawnPoint.transform.position, Quaternion.identity);
        monsterCharacterForGame = monster;

        GameObject playerGameObject = connection.identity.gameObject;
        NetworkServer.ReplacePlayerForConnection(connection, spawnedMonster);
        NetworkServer.Destroy(playerGameObject);
    }

    private void ServerSpawnSurvivorMidGame(NetworkConnection connection, Character survivorCharacter)
    {
        for (var i = 0; i < survivorSpawnPoints.Length; i++)
        {
            SurvivorSpawnPoint survivorSpawnPoint = survivorSpawnPoints[i];

            // We found a spawn point.
            if (survivorSpawnPoint.Used())
            {
                continue;
            }

            survivorSpawnPoint.SetUsed();
            GameObject survivorObject = Survivor(survivorCharacter);
            GameObject spawnedSurvivor = Instantiate(survivorObject, survivorSpawnPoint.gameObject.transform.position, Quaternion.identity);
            PlayerSpectator playerSpectator = connection.identity.GetComponent<PlayerSpectator>();
            string playerName = playerSpectator.ServerName();
            NetworkServer.ReplacePlayerForConnection(connection, spawnedSurvivor);
            Survivor spawnedSurvivorPlayer = spawnedSurvivor.GetComponent<Survivor>();
            spawnedSurvivorPlayer.SetCharacter(survivorCharacter);
            spawnedSurvivorPlayer.ServerSetName(playerName);
            NetworkServer.Destroy(playerSpectator.gameObject);
            break;
        }
    }

    private void ServerSpawnSurvivorLobby(NetworkConnection connection, Character survivorCharacter)
    {
        for (var i = 0; i < survivorSpawnPoints.Length; i++)
        {
            SurvivorSpawnPoint survivorSpawnPoint = survivorSpawnPoints[i];

            // We found a spawn point.
            if (survivorSpawnPoint.Used())
            {
                continue;
            }

            survivorSpawnPoint.SetUsed();
            GameObject survivorObject = Survivor(survivorCharacter);
            GameObject spawnedSurvivor = Instantiate(survivorObject, survivorSpawnPoint.gameObject.transform.position, Quaternion.identity);
            LobbyPlayer lobbyPlayer = connection.identity.GetComponent<LobbyPlayer>();
            string playerName = lobbyPlayer.Name();
            NetworkServer.ReplacePlayerForConnection(connection, spawnedSurvivor);
            Survivor spawnedSurvivorPlayer = spawnedSurvivor.GetComponent<Survivor>();
            spawnedSurvivorPlayer.ServerSetName(playerName);
            spawnedSurvivorPlayer.SetCharacter(survivorCharacter);
            NetworkServer.Destroy(lobbyPlayer.gameObject);
            break;
        }

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

    private GameObject Survivor(Character character)
    {
        switch (character)
        {
            case Character.Chad:
                return (GameObject)Resources.Load("Chad");
            case Character.Karen:
                return (GameObject)Resources.Load("Karen");
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


}
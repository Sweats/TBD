using UnityEngine;
using Mirror;
using System.Collections.Generic;

public class ServerStage: MonoBehaviour
{
    private ServerStage(){}

    private SurvivorSpawnPoint[] survivorSpawnPoints;

    // NOTE: Called when we are going from lobby -> stage.
    public void OnServerSceneChanged()
    {
        ServerSetSurvivorSpawnPoints();
        SpawnPlayersFromLobby();
    }

    public void RegisterNetworkHandlers()
    {
        NetworkServer.RegisterHandler<ServerClientGamePlayerPickedCharacterMessage>(ServerClientGameOnPlayerPickedCharacterMessage);

    }

    private void ServerClientGameOnPlayerPickedCharacterMessage(NetworkConnection connection, ServerClientGamePlayerPickedCharacterMessage message)
    {

        Character pickedCharacter = message.pickedCharacter;

        if ((byte)pickedCharacter >= 5)
        {
            ServerSpawnMonster(connection, pickedCharacter);
        }

        else
        {
            ServerSpawnSurvivor(connection, pickedCharacter);
        }
    }

    // NOTE: Only called when someone joins a game mid match.
    public void OnServerConnect(NetworkConnection connection)
    {
        SpawnPlayerSpectator(connection);
        Character [] character = ServerUnavailableCharacters();
        connection.identity.connectionToClient.Send(new ClientServerGamePickCharacterMessage{unavailableCharacters = character});
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

        foreach (int key in keys)
        {
            int connectionId = key;

            NetworkConnectionToClient connection = NetworkServer.connections[connectionId];

            Survivor survivor = connection.identity.GetComponent<Survivor>();

            if (survivor != null)
            {
                unAvailableCharacters.Add(survivor.PlayerCharacter());
                continue;
            }

            Mary mary = connection.identity.GetComponent<Mary>();

            if (mary != null)
            {
                unAvailableCharacters.Add(Character.Lurker);
                unAvailableCharacters.Add(Character.Fallen);
                unAvailableCharacters.Add(Character.Mary);
                unAvailableCharacters.Add(Character.Phantom);
                unAvailableCharacters.Add(Character.Mary);
                continue;
            }

            Lurker lurker = connection.identity.GetComponent<Lurker>();

            if (lurker != null)
            {
                unAvailableCharacters.Add(Character.Lurker);
                unAvailableCharacters.Add(Character.Fallen);
                unAvailableCharacters.Add(Character.Mary);
                unAvailableCharacters.Add(Character.Mary);
                continue;

            }

            Phantom phantom = connection.identity.GetComponent<Phantom>();

            if (phantom != null)
            {
                unAvailableCharacters.Add(Character.Lurker);
                unAvailableCharacters.Add(Character.Fallen);
                unAvailableCharacters.Add(Character.Mary);
                unAvailableCharacters.Add(Character.Mary);
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
                ServerSpawnSurvivor(connection, player.SelectedCharacter());
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

        GameObject playerGameObject = connection.identity.gameObject;
        NetworkServer.ReplacePlayerForConnection(connection, spawnedMonster);
        NetworkServer.Destroy(playerGameObject);
    }

    private void ServerSpawnSurvivor(NetworkConnection connection, Character survivorCharacter)
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
            GameObject lobbyPlayerObject = connection.identity.gameObject;
            NetworkServer.ReplacePlayerForConnection(connection, spawnedSurvivor);
            NetworkServer.Destroy(lobbyPlayerObject);
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


}
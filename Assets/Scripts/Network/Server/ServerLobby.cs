using Mirror;
using UnityEngine;  
using System.Collections.Generic;
public class ServerLobby: MonoBehaviour
{
    private ServerLobby(){}

    private bool insanityOptionEnabled;

    private bool allRandomEnabled;

    private bool voiceChatEnabled;

    private bool allowSpectatorEnabled;

    private StageName selectedStage;


    public void RegisterNetworkHandlers()
    {
        NetworkServer.RegisterHandler<ServerClientLobbyRequestedUnavailableCharactersMessage>(ServerClientLobbyOnRequestedUnavailableCharacters);
        NetworkServer.RegisterHandler<ServerClientLobbyRequestedCharacterChangeMessage>(ServerClientLobbyOnRequestedToChangeCharacter);
        NetworkServer.RegisterHandler<ServerClientLobbyRequestedToChangeInsanityMessage>(ServerClientLobbyOnRequestedToChangeInsanityOption);
        NetworkServer.RegisterHandler<ServerClientLobbyRequestedToChangeAllRandomMessage>(ServerClientLobbyOnRequestedToChangeAllRandomOption);
        NetworkServer.RegisterHandler<ServerClientLobbyRequestedToChangeVoiceChatMessage>(ServerClientLobbyOnRequestedToChangeVoiceChatOption);
        NetworkServer.RegisterHandler<ServerClientLobbyRequestedToChangeStageMessage>(ServerClientLobbyOnRequestedToChangeStageOption);
        NetworkServer.RegisterHandler<ServerClientLobbyRequestedToStartGameMessage>(ServerClientLobbyOnRequestedToStartGame);

    }

    // NOTE: Called when we are going from a stage to the lobby.
    public void OnServerSceneChanged()
    {
        var keys = NetworkServer.connections.Keys;

        foreach(int key in keys)
        {
            int connectionId = key;

            NetworkConnectionToClient connection = NetworkServer.connections[connectionId];
            //TODO: Make this a lot better. Maybe make it so monsters inherit from a base monster class that basically does nothing except to avoid all of these if else statements?
            Survivor oldSurvivorObject = connection.identity.GetComponent<Survivor>();

            if (oldSurvivorObject != null)
            {
                Character survivorCharacter = oldSurvivorObject.ServerPlayerCharacter();
                RespawnAsLobbyPlayer(connection, survivorCharacter);
                NetworkServer.Destroy(oldSurvivorObject.gameObject);
                continue;

            }

            Mary oldMaryObject = connection.identity.GetComponent<Mary>();

            if (oldMaryObject != null)
            {
                RespawnAsLobbyPlayer(connection, Character.Mary);
                NetworkServer.Destroy(oldMaryObject.gameObject);
                continue;

            }

            Phantom oldPhantomObject = connection.identity.GetComponent<Phantom>();

            if (oldPhantomObject != null)
            {
                RespawnAsLobbyPlayer(connection, Character.Phantom);
                NetworkServer.Destroy(oldPhantomObject.gameObject);
                continue;
            }

            Lurker oldLurkerObject = connection.identity.GetComponent<Lurker>();

            if (oldLurkerObject != null)
            {
                RespawnAsLobbyPlayer(connection, Character.Lurker);
                NetworkServer.Destroy(oldLurkerObject.gameObject);
                continue;
            }
        }

    }
    public void OnServerConnect(NetworkConnection connection)
    {
        GameObject spawnedLobbyPlayer = (GameObject)Resources.Load("LobbyPlayer");
        LobbyPlayer player = Instantiate(spawnedLobbyPlayer, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<LobbyPlayer>();
        NetworkServer.AddPlayerForConnection(connection, player.gameObject);
        player.SetCharacter(Character.Random);
    }


    private void RespawnAsLobbyPlayer(NetworkConnection connection, Character character)
    {
        GameObject spawnedLobbyPlayer = (GameObject)Resources.Load("LobbyPlayer");
        LobbyPlayer player = Instantiate(spawnedLobbyPlayer, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<LobbyPlayer>();
        NetworkServer.ReplacePlayerForConnection(connection, player.gameObject);
        player.SetCharacter(character);
    }

    // NOTE: We send a list of unavailable characters back to the client to show which characters they can pick.
    private void ServerClientLobbyOnRequestedUnavailableCharacters(NetworkConnection connection, ServerClientLobbyRequestedUnavailableCharactersMessage message)
    {
        LobbyPlayer playerWhoRequstedCharacterChange = connection.identity.GetComponent<LobbyPlayer>();

        if (playerWhoRequstedCharacterChange == null)
        {
            return;
        }

        Character[] unavailableCharactersOnServer = UnavailableCharacters();

        connection.identity.connectionToClient.Send(new ClientServerLobbyUnavailableCharactersMessage{unavailableCharacters = unavailableCharactersOnServer});
    }

    private void ServerClientLobbyOnRequestedToChangeCharacter(NetworkConnection connection, ServerClientLobbyRequestedCharacterChangeMessage message)
    {
        LobbyPlayer playerWhoRequstedCharacterChange = connection.identity.GetComponent<LobbyPlayer>();

        if (playerWhoRequstedCharacterChange == null)
        {
            return;
        }

        Character[] unavailableCharactersOnServer = UnavailableCharacters();
        bool taken = false;

        for (var i = 0; i < unavailableCharactersOnServer.Length; i++)
        {
            if (unavailableCharactersOnServer[i] == message.requestedCharacter)
            {
                taken = true;
                break;
            }

        }

        if (taken)
        {
            connection.identity.connectionToClient.Send(new ClientServerLobbyCharacterAlreadyTakenMessage{});
        }

        playerWhoRequstedCharacterChange.SetCharacter(message.requestedCharacter);

    }

    private Character[] UnavailableCharacters()
    {
        var keys = NetworkServer.connections.Keys;

        List<Character> unavailableCharacters = new List<Character>();

        foreach (int key in keys)
        {
            int connectionId = key;

            LobbyPlayer lobbyPlayer = NetworkServer.connections[connectionId].identity.GetComponent<LobbyPlayer>();

            if (lobbyPlayer == null)
            {
                continue;
            }

            Character selectedCharacter = lobbyPlayer.SelectedCharacter();

            if ((byte)selectedCharacter >= 5)
            {
                unavailableCharacters.Add(Character.Fallen);
                unavailableCharacters.Add(Character.Lurker);
                unavailableCharacters.Add(Character.Phantom);
                unavailableCharacters.Add(Character.Mary);
            }

            switch (selectedCharacter)
            {
                case Character.Alice:
                unavailableCharacters.Add(Character.Alice);
                break;
                case Character.Chad:
                unavailableCharacters.Add(Character.Chad);
                break;
                case Character.Jamal:
                unavailableCharacters.Add(Character.Jamal);
                break;
                case Character.Jesus:
                unavailableCharacters.Add(Character.Jesus);
                break;
                default:
                break;
            }
        }

        return unavailableCharacters.ToArray();
    }

    private void ServerClientLobbyOnRequestedToChangeAllRandomOption(NetworkConnection connection, ServerClientLobbyRequestedToChangeAllRandomMessage message)
    {
        LobbyPlayer player = connection.identity.GetComponent<LobbyPlayer>();

        if (player == null)
        {
            return;
        }

        if (!player.Hosting())
        {
            return;
        }

        NetworkServer.SendToReady(new ClientServerLobbyHostChangedAllRandomOption{newAllRandomValue = message.requestedAllRandomValue});

    }

    private void ServerClientLobbyOnRequestedToChangeVoiceChatOption(NetworkConnection connection, ServerClientLobbyRequestedToChangeVoiceChatMessage message)
    {
        LobbyPlayer player = connection.identity.GetComponent<LobbyPlayer>();

        if (player == null)
        {
            return;
        }

        if (!player.Hosting())
        {
            return;
        }


        voiceChatEnabled = message.requestedVoiceChatValue;

        NetworkServer.SendToReady(new ClientServerLobbyHostChangedVoiceChatMessage{newVoiceChatValue = message.requestedVoiceChatValue});

    }

    private void ServerClientLobbyOnRequestedToChangeInsanityOption(NetworkConnection connection, ServerClientLobbyRequestedToChangeInsanityMessage message)
    {
        LobbyPlayer player = connection.identity.GetComponent<LobbyPlayer>();

        if (player == null)
        {
            return;
        }

        if (!player.Hosting())
        {
            return;
        }

        insanityOptionEnabled = message.requestedInsanityValue;

        NetworkServer.SendToReady(new ClientServerLobbyHostChangedInsanityOption{newInsanityValue = insanityOptionEnabled});
    }

    private void ServerClientLobbyOnRequestedToChangeStageOption(NetworkConnection connection, ServerClientLobbyRequestedToChangeStageMessage message)
    {
        switch (message.requestedStageValue)
        {
            case StageName.Template_Lurker:
            break;
            default:
            return;
        }

        LobbyPlayer player = connection.identity.GetComponent<LobbyPlayer>();

        if (!player.Hosting())
        {
            return;
        }


        NetworkServer.SendToReady(new ClientServerLobbyHostChangedStageMessage{newStage = message.requestedStageValue});
    }


    private void ServerClientLobbyOnRequestedToChangeSpectatorOption(NetworkConnection connection, ServerClientLobbyRequestedToChangeAllowSpectatorMessage message)
    {
        LobbyPlayer player = connection.identity.GetComponent<LobbyPlayer>();

        if (player == null)
        {
            return;
        }

        if (!player.Hosting())
        {
            return;
        }

        allowSpectatorEnabled = message.requestedAllowSpectatorValue;

        NetworkServer.SendToReady(new ClientServerLobbyHostChangedAllowSpectatorOptinon{newAllowSpectatorValue = allowSpectatorEnabled});
    }

    private void ServerClientLobbyOnRequestedToStartGame(NetworkConnection connection, ServerClientLobbyRequestedToStartGameMessage message)
    {
        LobbyPlayer player = connection.identity.GetComponent<LobbyPlayer>();

        if (player == null)
        {
            return;
        }

        if (!player.Hosting())
        {
            return;
        }

        string newSceneName = Stages.Name(selectedStage);

        NetworkManager.singleton.ServerChangeScene(newSceneName);
    }

    private void ServerClientLobbyOnRequestedToChangeGamemade(NetworkConnection connection, ServerClientLobbyRequestedToChangeGamemodeMessage message)
    {
        LobbyPlayer player = connection.identity.GetComponent<LobbyPlayer>();

        if (player == null)
        {
            return;
        }

        if (!player.Hosting())
        {
            return;
        }

        NetworkServer.SendToReady(new ClientServerLobbyHostChangedGamemodeOption{newGamemodeValue = message.requestedGamemodeValue});

    }

}


using System;
using UnityEngine;
using Mirror;

public struct LobbyPlayer
{
    public string lobbyPlayerName;
    public Character choosenCharacter;
    public NetworkIdentity identity;
}

public class LobbyNetworkPlayer : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnHostChangedGameModeOption))]
    private int gameMode;

    [SyncVar(hook = nameof(OnHostChangedStage))]
    private int stageName;

    [SyncVar(hook = nameof(OnHostChangedInsanityOption))]
    private bool insanityOptionEnabled;

    [SyncVar(hook = nameof(OnHostChangedAllRandomOption))]
    private bool allRandomEnabled;

    [SyncVar(hook = nameof(OnHostChangedAllowSpectatorOption))]
    private bool allowSpectatorEnabled;
//
//    [SyncVar](hook = nameof(OnPlayerChangedCharacter))
//    private Character playerOneChracter;
//    [SyncVar](hook = nameof(OnPlayerChangedCharacter))
//    private Character playerTwoCharacter;
//    [SyncVar](hook = nameof(OnPlayerChangedCharacter))
//    private Character playerThreeCharacter;
//    [SyncVar](hook = nameof(OnPlayerChangedCharacter))
//    private Character playerFourCharacter;
//    [SyncVar](hook = nameof(OnPlayerChangedCharacter))
//    private Character playerFiveCharacter;
//
    [SerializeField]
    private readonly SyncList<LobbyPlayer> players = new SyncList<LobbyPlayer>();

    [Server]
    private void Start()
    {
        players.Add(new LobbyPlayer { lobbyPlayerName = Settings.PROFILE_NAME, choosenCharacter = Character.Random, identity = netIdentity });
    }

    public override void OnStartLocalPlayer()
    {
        if (!isServer)
        {
            CmdSetUpNewClient(Settings.PROFILE_NAME);
        }

        EventManager.lobbyYouChangedCharacterEvent.AddListener(OnYouChangedCharacterEvent);
        EventManager.lobbyHostChangedStageEvent.AddListener(HostChangedStage);
        EventManager.lobbyHostChangedGamemodeEvent.AddListener(HostChangedGameModeOption);
        EventManager.lobbyHostChangedAllowSpectatorEvent.AddListener(HostChangedAllowSpectatorOption);
        EventManager.lobbyHostChangedAllRandomOptionEvent.AddListener(HostChangedAllRandomOption);
        EventManager.lobbyHostChangedInsanityOptionEvent.AddListener(HostChangedInsanityOpton);

        players.Callback += OnPlayerChangedCharacter;

    }

    [Command(ignoreAuthority = true)]
    private void CmdSetUpNewClient(string playerName, NetworkConnectionToClient connection = null)
    {
        NetworkIdentity clientIdentity = connection.identity;
        players.Add(new LobbyPlayer { lobbyPlayerName = playerName, choosenCharacter = Character.Random, identity = clientIdentity });
    }

    public override void OnStopClient()
    {
        if (isServer)
        {

        }

        else
        {

        }

    }

    [Command(ignoreAuthority = true)]
    public void CmdChangeCharacter(Character character, NetworkConnectionToClient connection = null)
    {
        uint netId = connection.identity.netId;

        for (var i = 0; i < players.Count; i++)
        {
            uint foundnetId = players[i].identity.netId;

            if (foundnetId == netId)
            {
                Debug.Log($"Found player netid {netId} that matches netid {foundnetId} at index {i} ");
                //NOTE: Have to do this or else we will get error CS1612.
                LobbyPlayer lobbyPlayer = players[i];
                lobbyPlayer.choosenCharacter = character;
                players[i] = lobbyPlayer;
                break;
            }
        }
    }

    [Command(ignoreAuthority = true)]
    public void CmdKickPlayer(NetworkIdentity identity, NetworkConnectionToClient connection = null)
    {
        //NOTE: Server side check.
        if (connection.identity.netId != 1)
        {
            return;
        }
    }

    [Command(ignoreAuthority = true)]
    public void CmdChangeInsanityOption(bool newValue, NetworkConnectionToClient connection = null)
    {
        //NOTE: Server side check.
        if (connection.identity.netId != 1)
        {
            return;
        }

        insanityOptionEnabled = newValue;
    }

    [Command(ignoreAuthority = true)]
    public void CmdChangeStageOption(int newStage, NetworkConnectionToClient connection = null)
    {
        //NOTE: Server side check.
        if (connection.identity.netId != 1)
        {
            return;
        }

        stageName = newStage;
    }

    [Command(ignoreAuthority = true)]
    public void CmdChangeAllRandomOption(bool newValue, NetworkConnectionToClient connection = null)
    {
        //NOTE: Server side check.
        if (connection.identity.netId != 1)
        {
            return;
        }

        allRandomEnabled = newValue;
    }


    [Command(ignoreAuthority = true)]
    public void CmdChangeAllowSpectatorOption(bool newValue, NetworkConnectionToClient connection = null)
    {
        //NOTE: Server side check.
        if (connection.identity.netId != 1)
        {
            return;
        }

        allowSpectatorEnabled = newValue;

    }

    [Command(ignoreAuthority = true)]
    public void CmdChangeGameModeOption(int newValue, NetworkConnectionToClient connection = null)
    {

        //NOTE: Server side check.
        if (connection.identity.netId != 1)
        {
            return;
        }

        gameMode = newValue;
    }

    #region CLIENT
    [Client]
    private void OnYouChangedCharacterEvent(Character character)
    {
        if (isLocalPlayer)
        {
            CmdChangeCharacter(character);
        }
    }

    #region HOST_CALLBACKS
    [Client]
    private void HostChangedStage(int newValue)
    {
        if (isLocalPlayer)
        {
            CmdChangeStageOption(newValue);
        }
    }

    [Client]
    private void HostChangedGameModeOption(int newValue)
    {
        if (isLocalPlayer)
        {
            CmdChangeGameModeOption(newValue);
        }
    }

    [Client]
    private void HostChangedAllRandomOption(bool newValue)
    {
        if (isLocalPlayer)
        {
            CmdChangeAllRandomOption(newValue);
        }
    }

    [Client]
    private void HostChangedAllowSpectatorOption(bool newValue)
    {
        if (isLocalPlayer)
        {
            CmdChangeAllowSpectatorOption(newValue);
        }
    }


    [Client]
    private void HostChangedInsanityOpton(bool newValue)
    {
        if (isLocalPlayer)
        {
            CmdChangeInsanityOption(newValue);
        }
    }

    #endregion


    [Client]
    private void OnHostChangedStage(int oldValue, int newValue)
    {
        EventManager.lobbyClientHostChangedStageEvent.Invoke(newValue);
    }

    [Client]
    private void OnHostChangedGameModeOption(int oldValue, int newValue)
    {
        EventManager.lobbyClientHostChangedGamemodeEvent.Invoke(newValue);
    }

    [Client]
    private void OnHostChangedAllRandomOption(bool oldValue, bool newValue)
    {
        EventManager.lobbyClientHostChangedAllRandomEvent.Invoke(newValue);
    }

    [Client]
    private void OnHostChangedInsanityOption(bool oldValue, bool newValue)
    {
        EventManager.lobbyClientHostChangedInsanityOptionEvent.Invoke(newValue);
    }

    [Client]
    private void OnHostChangedAllowSpectatorOption(bool oldValue, bool newValue)
    {
        EventManager.lobbyClientHostChangedAllowSpectatorEvent.Invoke(newValue);
    }

    [Client]
    private void OnPlayerChangedCharacter(SyncList<LobbyPlayer>.Operation op, int index, LobbyPlayer oldValue, LobbyPlayer newValue)
    {
        Debug.Log($"Detected change! index = {index}");

        switch (op)
        {
            case SyncList<LobbyPlayer>.Operation.OP_ADD:
                Debug.Log($"Player added to the list! Index = {index}");
                string playerJoinedName = newValue.lobbyPlayerName;
                EventManager.lobbyPlayerJoinedLobbyEvent.Invoke(index, playerJoinedName);
                break;
            case SyncList<LobbyPlayer>.Operation.OP_CLEAR:
                break;
            case SyncList<LobbyPlayer>.Operation.OP_INSERT:
                break;
            case SyncList<LobbyPlayer>.Operation.OP_REMOVEAT:
                string playerDisconnectedName = newValue.lobbyPlayerName;
                EventManager.lobbyPlayerLeftLobbyEvent.Invoke(index, playerDisconnectedName);
                break;
            case SyncList<LobbyPlayer>.Operation.OP_SET:
                string playerChangedCharacterName = newValue.lobbyPlayerName;
                Character character = newValue.choosenCharacter;
                EventManager.lobbyHostPlayerChangedCharacterEvent.Invoke(character, playerChangedCharacterName, index);
                break;
        }
    }
}
#endregion

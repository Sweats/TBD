﻿using UnityEngine;
using Mirror;

public class PlayerSpectator : NetworkBehaviour
{
    [SerializeField]
    private Camera spectatorCamera;

    private const string PROFILE_NAME_KEY_STRING = "profile_name";

    public override void OnStartLocalPlayer()
    {
        this.enabled = true;
        spectatorCamera.enabled = true;
        spectatorCamera.GetComponent<AudioListener>().enabled = true;
        Settings.PROFILE_NAME = PlayerPrefs.GetString(PROFILE_NAME_KEY_STRING, "player");
        NetworkClient.Send(new ServerPlayerJoinedMessage{clientName = Settings.PROFILE_NAME, clientIdentity = netIdentity});
    }
}

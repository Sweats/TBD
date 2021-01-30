using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerSpectator : NetworkBehaviour
{
    [SerializeField]
    private Camera spectatorCamera;

    private const string PROFILE_NAME_KEY_STRING = "profile_name";

    private void Start()
    {

    }

    public override void OnStartLocalPlayer()
    {
        Settings.PROFILE_NAME = PlayerPrefs.GetString(PROFILE_NAME_KEY_STRING, "player");
        spectatorCamera.enabled = true;
        spectatorCamera.GetComponent<AudioListener>().enabled = true;
        NetworkClient.Send(new ServerPlayerJoinedPlayerMessage{clientName = Settings.PROFILE_NAME, identity = netIdentity});
    }
}

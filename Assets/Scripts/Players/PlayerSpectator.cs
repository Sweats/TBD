using UnityEngine;
using UnityEngine.EventSystems;
using Mirror;

public class PlayerSpectator : NetworkBehaviour
{
    [SerializeField]
    private Camera spectatorCamera;

    [SerializeField]
    [SyncVar]
    private string name;

    private const string PROFILE_NAME_KEY_STRING = "profile_name";

    [SerializeField]
    private PickCharacterUI pickCharacterUI;

    [SerializeField]
    private StandaloneInputModule inputModule;

    [SerializeField]
    private EventSystem eventSystem;
    public override void OnStartLocalPlayer()
    {
        this.enabled = true;
        inputModule.enabled = true;
        eventSystem.enabled = true;
        spectatorCamera.enabled = true;
        spectatorCamera.GetComponent<AudioListener>().enabled = true;
        CmdSetName(Settings.PROFILE_NAME);
        pickCharacterUI.LocalPlayerStart();
        NetworkClient.Send(new ServerClientGamePlayerSpectatorJoinedMessage{});
    }

    [Command]
    private void CmdSetName(string name)
    {
        this.name = name;

    }

    [Server]
    public string ServerName()
    {
        return name;

    }

}

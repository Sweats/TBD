using Mirror;

public class LobbyPlayer : NetworkBehaviour
{
    // NOTE: We have to do this here because apparently the net identity is not set inside the NetworkManager OnServerConnect() function.


    [SyncVar]
    private bool hosting;

    [SyncVar]
    private Character character;

    [SyncVar]
    private string name;

    public override void OnStartLocalPlayer()
    {
        CmdSetProfileName(Settings.PROFILE_NAME);
    }

    [Command]
    private void CmdSetProfileName(string name)
    {
        this.name = name;
    }


    public string Name()
    {
        return name;
    }

    public void SetCharacter(Character character)
    {
        this.character = character;
    }

    public bool Hosting()
    {
        return hosting;
    }

    public void SetHosting(bool value)
    {
        this.hosting = value;
    }

    public Character SelectedCharacter() 
    {
        return character;
    }
}

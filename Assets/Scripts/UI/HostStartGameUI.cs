using UnityEngine;
using Mirror;

public class HostStartGameUI: MonoBehaviour
{
    [SerializeField]
    private Canvas hostStartGameCanvas;

    private HostStartGameUI(){}

    public void LocalPlayerStart()
    {
        this.enabled = true;
        Show();
        EventManager.clientServerGameHostStartedGameEvent.AddListener(OnHostStartedGame);
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.KeypadEnter))
        {
            NetworkClient.Send(new ServerClientGameHostRequestedToStartGameMessage{});
        }
    }

    private void OnHostStartedGame()
    {
        this.enabled = false;
        Hide();
    }


    public void Show()
    {
        hostStartGameCanvas.enabled = true;
    }

    public void Hide()
    {
        hostStartGameCanvas.enabled = false;
    }


}
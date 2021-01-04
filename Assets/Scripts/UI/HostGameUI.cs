using UnityEngine;
using UnityEngine;
using UnityEngine.UI;

public class HostGameUI : MonoBehaviour
{

    [SerializeField]
    private InputField lobbyNameField;

    [SerializeField]
    private InputField passwordField;

    [SerializeField]
    private Canvas hostLobbyCanvas;

    [SerializeField]
    private Button backButton;

    [SerializeField]
    private Text lobbyNameNotification;

    public string LobbyName()
    {
        return lobbyNameField.text;

    }

    public string LobbyPassword()
    {
        return passwordField.text;

    }

    public void Hide()
    {
        hostLobbyCanvas.enabled = false;

    }

    public void Show()
    {
        hostLobbyCanvas.enabled = true;
    }

    public void ShowLobbyNotification()
    {
        lobbyNameNotification.enabled = true;
    }

    public void HideLobbyNotification()
    {
        lobbyNameNotification.enabled = false;
    }

    public void OnLobbyNameChanged()
    {
        HideLobbyNotification();
    }
}

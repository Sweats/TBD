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

    [SerializeField]
    private MainMenuUI mainMenuUI;

    private void Start()
    {
        this.enabled = false;
    }


    private void Update()
    {
        if (Keybinds.GetKey(Action.GuiReturn))
        {
            mainMenuUI.Show();
            Hide();

        }
    }

    public string LobbyName()
    {
        return lobbyNameField.text;

    }

    public string LobbyPassword()
    {
        return passwordField.text;

    }

    private void Hide()
    {
        this.enabled = false;
        hostLobbyCanvas.enabled = false;

    }

    public void Show()
    {
        this.enabled = true;
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


    private void OnHostLobbyBackButtonClicked()
    {
        Hide();
        mainMenuUI.Show();
    }

    public void OnLobbyNameChanged()
    {
        HideLobbyNotification();
    }
}

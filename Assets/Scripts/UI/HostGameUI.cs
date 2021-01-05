using UnityEngine;
using UnityEngine.UI;
using System;

public class HostGameUI : MonoBehaviour
{

    [SerializeField]
    private InputField lobbyNameField;

    [SerializeField]
    private InputField passwordField;

    [SerializeField]
    private Canvas hostLobbyCanvas;

    [SerializeField]
    private Text lobbyNameNotification;

    [SerializeField]
    private MainMenuUI mainMenuUI;

    [SerializeField]
    private LobbyUI lobbyUI;

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

    public void OnHostLobbyBackButtonClicked()
    {
        Hide();
        mainMenuUI.Show();
    }


    public void OnCreateLobbyButtonClicked()
    {
        string lobbyText = lobbyNameField.text;

        if (lobbyText == String.Empty)
        {
            lobbyNameNotification.enabled = true;
            return;
        }

        Hide();
        lobbyUI.Show(true);
    }

    public void OnLobbyNameChanged()
    {
        lobbyNameNotification.enabled = false;
    }
}

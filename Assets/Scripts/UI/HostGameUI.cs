using UnityEngine;
using UnityEngine.UI;
using System;
using Mirror;
using kcp2k;

public class HostGameUI : MonoBehaviour
{

    [SerializeField]
    private InputField lobbyNameField;

    [SerializeField]
    private InputField passwordField;

    [SerializeField]
    private InputField portField;

    [SerializeField]
    private Canvas hostLobbyCanvas;

    [SerializeField]
    private MainMenuUI mainMenuUI;

    [SerializeField]
    private LobbyUI lobbyUI;

    private ushort port;

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

    public void OnHostLobbyBackButtonClicked()
    {
        Hide();
        mainMenuUI.Show();
    }


    public void OnCreateLobbyButtonClicked()
    {
        string lobbyText = lobbyNameField.text;
        string password = passwordField.text;

        if (portField.text == string.Empty)
        {
            HostLobby.LOBBY_PORT = 7777;
        }

        else
        {
            HostLobby.LOBBY_PORT = port;
        }

        DarnedNetworkManager.CLIENT_HOSTING_LOBBY = true;
        string lobbySceneName = Stages.Name(StageName.Lobby);
        HostLobby.LOBBY_NAME = lobbyText;
        HostLobby.LOBBY_PASSWORD = password;
        Stages.Load(StageName.Lobby);
        //Hide();
        //lobbyUI.Show(true);
    }


    public void OnPortFieldChanged(string text)
    {
        port = Convert.ToUInt16(text);
    }
}

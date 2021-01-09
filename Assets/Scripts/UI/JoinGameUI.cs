using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public struct Lobby
{
    public string lobbyName;
    public int players;
    public bool inLobby;
    public bool privateLobby;
    public int ping;
}


public class JoinGameUI : MonoBehaviour
{
    [SerializeField]
    private Canvas joinGameCanvas;

    [SerializeField]
    private Button refreshAllButton;

    [SerializeField]
    private Button connectButton;

    [SerializeField]
    private Button backButton;

    [SerializeField]
    private MainMenuUI mainMenuUI;

    [SerializeField]
    private LobbyUI lobbyUI;

    private List<Lobby> lobbies;

    private void Update()
    {
        if (Keybinds.GetKey(Action.GuiReturn))
        {
            Hide();
            mainMenuUI.Show();
        }
    }

    void Start()
    {
        this.enabled = false;
        lobbies = new List<Lobby>();
    }

    public void Show()
    {
        this.enabled = true;
        joinGameCanvas.enabled = true;

    }

    private void Hide()
    {
        this.enabled = false;
        joinGameCanvas.enabled = false;
    }

    public void OnRefreshButtonClicked()
    {
        Debug.Log("Refresh all button clicked!");
        lobbies.Clear();
        //lobbies = GetLobbies();

    }


    public void OnJoinGameBackButtonClicked()
    {
        Hide();
        mainMenuUI.Show();
    }

    public void OnConnectButtonClicked()
    {
        Debug.Log("Connect button clicked!");
        lobbyUI.Show(false);
        Hide();

    }


    private List<Lobby> GetLobbies()
    {
        //TODO: Connect to the server and get the list of lobbies here and return it.
        return null;

    }

    private void UpdateLobbiesView()
    {
        for (var i = 0; i < lobbies.Count; i++)
        {

        }
    }
}

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


    private List<Lobby> lobbies;

    void Start()
    {
        lobbies = new List<Lobby>();
    }

    public void Show()
    {
        joinGameCanvas.enabled = true;

    }

    public void Hide()
    {
        joinGameCanvas.enabled = false;
    }

    public void OnRefreshButtonClicked()
    {
        Debug.Log("Refresh all button clicked!");
        //lobbies = GetLobbies();

    }

    public void OnConnectButtonClicked()
    {
        Debug.Log("Connect button clicked!");

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

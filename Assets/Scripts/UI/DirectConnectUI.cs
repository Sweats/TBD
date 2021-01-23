using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class DirectConnectUI : MonoBehaviour
{
    private string hostname = "localhost";

    private int port = 7777;

    [SerializeField]
    private Canvas directConnectCanvas;

    [SerializeField]
    private NetworkManager networkManager;

    [SerializeField]
    private JoinGameUI joinGameUI;

    [SerializeField]
    private LobbyUI lobbyUI;

    [SerializeField]
    private InputField hostnameField;

    [SerializeField]
    private Button directConnectButton;

    [SerializeField]
    private Button backButton;

    [SerializeField]
    private Text hostnameText;

    [SerializeField]
    private Text portText;

    [SerializeField]
    private InputField portField;

    private void Start()
    {
        this.enabled = false;
        EventManager.lobbyClientFailedToConnectToHostEvent.AddListener(OnFailedToConnectToHost);
        EventManager.lobbyClientPlayerConnectedToLobbyEvent.AddListener(OnSuccesfullyConnectedToHost);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Keybinds.GetKey(Action.GuiReturn))
        {
            /*
            if (NetworkClient.active)
            {
                NetworkClient.Disconnect();
            }
            */

            Hide();
            joinGameUI.Show();
        }
        
    }

    private void Hide()
    {
        this.enabled = false;
        directConnectCanvas.enabled = false;
    }

    public void Show()
    {
        this.enabled = true;
        directConnectCanvas.enabled = true;
        //NOTE: In case if we timed out. Needs more testing.
        EnableControls();
    }

    public void OnConnectButtonClicked()
    {
        string uriString = $"{hostname}:{port}";
        Uri uri = new Uri(uriString);
        DisableControls();
        networkManager.StartClient(uri);
        //StartCoroutine(ConnectRoutine());
    }

    // NOTE: We want to show the lobby when we connect. We use this because there can be a delay to reaching the host.
    /*
    private IEnumerator ConnectRoutine()
    {
        while (true)
        {
            if (NetworkClient.active)
            {
                break;
            }

            yield return new WaitForSeconds(0.1f);
        }

        lobbyUI.Show(false);
        Hide();
    }
    */
    

    private void OnFailedToConnectToHost(int errorCode)
    {
        Debug.Log($"Failed to connect to the remote host! Error code is {errorCode}");
        EnableControls();
    }

    public void OnHostnameFieldChanged()
    {
        hostname = hostnameField.text;
        Debug.Log($"uri string is now {hostname}:{port}");

    }

    public void OnPortFieldChanged()
    {
        port = Int32.Parse(portField.text);
        Debug.Log($"uri string is now {hostname}:{port}");
    }

    public void OnBackButtonClicked()
    {
        Hide();
        joinGameUI.Show();
    }

    private void OnSuccesfullyConnectedToHost()
    {
        //NOTE: Have to put this here or else this code will be called. A host is also a client.
        if (lobbyUI.Hosting())
        {
            //Debug.Log("TEST");
            return;
        }

        Debug.Log("Successfully connected to the remote host!");
        Hide();
        lobbyUI.Show(false);
    }


    private void DisableControls()
    {
        portField.interactable = false;
        hostnameField.interactable = false;
        backButton.interactable = false;
        directConnectButton.interactable = false;
        directConnectButton.GetComponentInChildren<Text>().color = Color.gray;
        backButton.GetComponentInChildren<Text>().color = Color.gray;
        hostnameText.enabled = false;
        portText.enabled = false;
    }

    private void EnableControls()
    {
        portField.interactable = true;
        hostnameField.interactable = true;
        backButton.interactable = true;
        directConnectButton.interactable = true;
        directConnectButton.GetComponentInChildren<Text>().color = Color.white;
        backButton.GetComponentInChildren<Text>().color = Color.white;
        hostnameText.enabled = true;
        portText.enabled = true;
    }


}

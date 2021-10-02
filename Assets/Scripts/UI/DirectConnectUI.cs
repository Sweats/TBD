using System;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class DirectConnectUI : MonoBehaviour
{
    private string hostname = "localhost";

    private ushort port = 7777;

    [SerializeField]
    private Canvas directConnectCanvas;

    [SerializeField]
    private JoinGameUI joinGameUI;

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

    private void JoinDarnedServer()
    {
        GameObject darnedNetworkManager = (GameObject)Resources.Load("Darned Network Manager");
        GameObject spawnedObject = Instantiate(darnedNetworkManager);
        string port = portField.text;
        string hostname = hostnameField.text;
        string address = $"{hostname}:{port}";
        Uri uri = new Uri(address);
        NetworkManager.singleton.StartClient();
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
        if (NetworkManager.singleton != null)
        {
            Destroy(NetworkManager.singleton.gameObject);
        }
    }

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
        port = Convert.ToUInt16(portField.text);
        Debug.Log($"uri string is now {hostname}:{port}");
    }

    public void OnBackButtonClicked()
    {
        Hide();
        joinGameUI.Show();
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

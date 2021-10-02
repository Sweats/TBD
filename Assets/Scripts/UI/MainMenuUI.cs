using UnityEngine;
using UnityEngine.UI;
using Mirror;
using System.IO;
using System;
using kcp2k;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField]
    private Canvas mainMenuCanvas;

    [SerializeField]
    private OptionsUI optionsUI;

    [SerializeField]
    private HostGameUI hostGameUI;

    [SerializeField]
    private JoinGameUI joinGameUI;

    [SerializeField]
    private CreditsUI creditsUI;

    [SerializeField]
    private Color buttonTextColor;

    private bool dedicatedServerBuild = false;

    private bool masterServerBuild = false;

    private void Start()
    {
#if UNITY_SERVER && DEDICATED_SERVER_BUILD
        dedicatedServerBuild = true;
#elif UNITY_SERVER && MASTER_SERVER_BUILD
        masterServerBuild = true;
#elif UNITY_SERVER && MASTER_SERVER_BUILD && DEDICATED_SERVER_BUILD
        Application.Quit();
        return;
#endif

        if (dedicatedServerBuild)
        {
            EventManager.masterClientServerAddedDedicatedServerEvent.AddListener(OnMasterServerAddedDedicatedServer);
            AddDedicatedServerToMasterServerLobbyListing();
            return;
        }

        if (masterServerBuild)
        {
            CreateAndStartDarnedMasterServer();
        }

        if (!dedicatedServerBuild && !masterServerBuild)
        {
            Cursor.lockState = CursorLockMode.Confined;
        }

    }

    private void OnMasterServerAddedDedicatedServer(int id)
    {
        NetworkManager.singleton.StopClient();
        HostLobby.LOBBY_ID = id;
        Destroy(NetworkManager.singleton.gameObject);
        GameObject darnedNetworkManager = (GameObject)Resources.Load("Darned Network Manager");
        GameObject spawnedObject = Instantiate(darnedNetworkManager);
        NetworkManager.singleton.StartServer();
        string lobbySceneName = Stages.Name(StageName.Lobby);
        NetworkManager.singleton.ServerChangeScene(lobbySceneName);
    }

    private void CreateAndStartDarnedMasterServer()
    {
        GameObject darnedMasterServerManager = (GameObject)Resources.Load("Darned Master Server Manager");
        GameObject spawnedObject = Instantiate(darnedMasterServerManager);
        NetworkManager.singleton.StartServer();

    }

    private void AddDedicatedServerToMasterServerLobbyListing()
    {
        string configName = "Darned Dedicated Server Configuration.yml";
        bool exists = DedicatedServerConfiguration.Exists(configName);

        if (!exists)
        {
            string currentDirectoryName = Directory.GetCurrentDirectory();
            DedicatedServerConfiguration.GenerateConfig(currentDirectoryName);
            Debug.Log($"[Darned Server]: Generated configuration file {configName} in {currentDirectoryName}");
            Application.Quit();
            return;
        }

        DedicatedServerConfiguration config = DedicatedServerConfiguration.Load(configName);
        GameObject darnedMasterServerManager = (GameObject)Resources.Load("Darned Master Server Manager");
        GameObject spawnedObject = Instantiate(darnedMasterServerManager);
        string lobbyName = config.serverName;
        string password = config.lobbyPassword;
        bool isPrivate = password == string.Empty;
        //TODO: Put this in a config file somewhere.
        string masterServerAddress = "localhost:7777";
        Uri uri = new Uri(masterServerAddress);
        NetworkManager.singleton.StartClient(uri);
    }


    public void Show()
    {
        mainMenuCanvas.enabled = true;
    }

    private void Hide()
    {
        mainMenuCanvas.enabled = false;
    }

    public void OnQuitButtonClicked()
    {
        //if (Application.isEditor)
        //{
        //    UnityEditor.EditorApplication.isPlaying = false;
        //}

        Application.Quit();
    }

    public void OnOptionsButtonClicked()
    {
        optionsUI.Show();
        Hide();

    }

    public void OnHostGameButtonClicked()
    {
        hostGameUI.Show();
        Hide();
    }

    public void OnJoinGameButtonClicked()
    {
        joinGameUI.Show();
        Hide();
    }

    public void OnCreditsButtonClicked()
    {
        creditsUI.Show();
        Hide();
    }

    public void OnPointerEnterButton(Button button)
    {
        if (!button.enabled || !button.interactable)
        {
            return;
        }

        button.GetComponentInChildren<Text>().color = buttonTextColor;
    }

    public void OnPointerExitButton(Button button)
    {
        if (!button.enabled || !button.interactable)
        {
            return;
        }

        button.GetComponentInChildren<Text>().color = Color.white;
    }
}


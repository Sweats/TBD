using UnityEngine;
using UnityEngine.UI;
using Mirror;
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

    private void Start()
    {
#if UNITY_SERVER
        NetworkRoom.dedicatedServer = true;
#endif
        if (NetworkRoom.dedicatedServer && !NetworkManager.singleton.isNetworkActive)
        {
            string configName = "darned_lobby_server_configuration.yaml";

            if (!Configuration.Exists(configName))
            {
                Debug.Log("[Darned]: Exiting...");
                Application.Quit();
                return;
            }

            Configuration serverConfiguration = Configuration.Load(configName);
            string serverName = serverConfiguration.Name();
            ushort port = serverConfiguration.Port();
            string password = serverConfiguration.Password();
            bool isPrivate = password == string.Empty;
            Debug.Log($"[Darned]: Starting the server. Settings are:\n\nServer Name = {serverName}\nPort = {port}\nPrivate server = {isPrivate}");
            KcpTransport transport = (KcpTransport)Transport.activeTransport;
            transport.Port = port;
            NetworkRoom.dedicatedServer = true;
            NetworkManager.singleton.StartServer();
        }

        else
        {
            Cursor.lockState = CursorLockMode.Confined;

        }
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


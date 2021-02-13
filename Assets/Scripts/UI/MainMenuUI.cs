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
#if UNITY_SERVER && DEDICATED_SERVER_BUILD
        DarnedNetworkManager.DEDICATED_SERVER_HOSTING_LOBBY = true;
        DarnedNetworkManager.CLIENT_HOSTING_LOBBY = false;
#elif UNITY_SERVER && MASTER_SERVER_BUILD
        DarnedMasterServerManager.ENABLED = true;
#elif UNITY_SERVER && MASTER_SERVER_BUILD && DEDICATED_SERVER_BUILD
        DarnedNetworkManager.Log("You have built both the dedicated server and the master server. Please only define one of the two, not both. Exiting...");
        Application.Quit();
        return;
#endif
        if (DarnedNetworkManager.DEDICATED_SERVER_HOSTING_LOBBY)
        {
            DarnedNetworkManager.Log("SWITCHING TO THE LOBBY SCENE...");
            Stages.Load(StageName.Lobby);
            return;
        }

        else if (DarnedMasterServerManager.ENABLED)
        {
            Stages.Load(StageName.MasterServer);
            return;
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


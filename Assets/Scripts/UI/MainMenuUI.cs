using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{

    [SerializeField]
    private Color buttonTextColor;

    [SerializeField]
    private Canvas mainMenuCanvas;

    private const int ARRAY_SIZE = 12;

    [SerializeField]
    private SoundUI soundUI;

    [SerializeField]
    private OptionsUI optionsUI;

    [SerializeField]
    private ControlsUI controlsUI;

    [SerializeField]
    private GraphicsUI graphicsUI;

    [SerializeField]
    private ConnectionUI connectionUI;

    [SerializeField]
    private ProfileUI profileUI;

    [SerializeField]
    private MiscUI miscUI;


    [SerializeField]
    private HostGameUI hostGameUI;

    [SerializeField]
    private JoinGameUI joinGameUI;

    [SerializeField]
    private LobbyUI lobbyUI;

    [SerializeField]
    private CreditsUI creditsUI;

    public bool[] openedMenus;

    private bool hosting;

    public enum Menu
    {
        Main = 0,
        Options,
        HostGame,
        Credits,
        Connection,
        Graphics,
        Profile,
        Sound,
        Controls,
        Misc,
        JoinGame,
        Lobby
    }



    void Start()
    {
        openedMenus = new bool[ARRAY_SIZE];
        openedMenus[(int)Menu.Main] = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    void Update()
    {
        if (Keybinds.GetKey(Action.GUiReturn))
        {
            bool openedMenuFound = false;

            for (var i = 0; i < openedMenus.Length; i++)
            {
                if (!openedMenus[i])
                {
                    continue;
                }

                Menu openedMenu = (Menu)i;

                switch (openedMenu)
                {
                    case Menu.Connection:
                        connectionUI.Hide();
                        optionsUI.Show();
                        openedMenus[(int)Menu.Connection] = false;
                        openedMenus[(int)Menu.Options] = true;
                        openedMenuFound = true;
                        break;
                    case Menu.Profile:
                        profileUI.Hide();
                        optionsUI.Show();
                        openedMenus[(int)Menu.Profile] = false;
                        openedMenus[(int)Menu.Options] = true;
                        openedMenuFound = true;
                        break;
                    case Menu.Graphics:
                        graphicsUI.Hide();
                        optionsUI.Show();
                        openedMenus[(int)Menu.Graphics] = false;
                        openedMenus[(int)Menu.Options] = true;
                        openedMenuFound = true;
                        break;
                    case Menu.Options:
                        optionsUI.Hide();
                        openedMenus[(int)Menu.Options] = false;
                        openedMenus[(int)Menu.Main] = true;
                        Show();
                        openedMenuFound = true;
                        break;
                    case Menu.Controls:
                        controlsUI.Hide();
                        optionsUI.Show();
                        openedMenus[(int)Menu.Controls] = false;
                        openedMenus[(int)Menu.Options] = true;
                        openedMenuFound = true;
                        break;
                    case Menu.Misc:
                        miscUI.Hide();
                        optionsUI.Show();
                        openedMenus[(int)Menu.Misc] = false;
                        openedMenus[(int)Menu.Options] = true;
                        openedMenuFound = true;
                        break;

                    case Menu.Sound:
                        soundUI.Hide();
                        optionsUI.Show();
                        openedMenus[(int)Menu.Sound] = false;
                        openedMenus[(int)Menu.Options] = true;
                        openedMenuFound = true;
                        break;

                    case Menu.HostGame:
                        hostGameUI.Hide();
                        Show();
                        openedMenus[(int)Menu.HostGame] = true;
                        openedMenus[(int)Menu.Main] = false;
                        openedMenuFound = true;
                        break;

                    case Menu.Credits:
                        creditsUI.Hide();
                        Show();
                        openedMenus[(int)Menu.Credits] = true;
                        openedMenus[(int)Menu.Main] = false;
                        openedMenuFound = true;
                        break;

                    case Menu.JoinGame:
                        joinGameUI.Hide();
                        Show();
                        openedMenus[(int)Menu.Main] = true;
                        openedMenus[(int)Menu.JoinGame] = false;
                        openedMenuFound = true;
                        break;
                    case Menu.Lobby:

                        if (hosting)
                        {
                            hostGameUI.Show();
                            lobbyUI.Hide();
                            openedMenus[(int)Menu.Lobby] = false;
                            openedMenus[(int)Menu.HostGame] = true;
                            openedMenuFound = true;
                            hosting = false;
                        }

                        else
                        {
                            joinGameUI.Show();
                            lobbyUI.Hide();
                            openedMenus[(int)Menu.Lobby] = false;
                            openedMenus[(int)Menu.JoinGame] = true;
                            openedMenuFound = true;
                        }

                        break;
                }

                if (openedMenuFound)
                {
                    break;
                }
            }
        }
    }

    public void OnMouseOverButton(Button button)
    {
        if (!button.enabled)
        {
            return;
        }

        button.GetComponentInChildren<Text>().color = buttonTextColor;
    }

    public void OnMouseLeftButton(Button button)
    {
        if (!button.enabled)
        {
            return;
        }

        button.GetComponentInChildren<Text>().color = Color.white;
    }


    #region NON_BACK_BUTTONS

    public void OnQuitButtonClicked()
    {
        if (Application.isEditor)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }

        Application.Quit();
    }


    public void OnCreditsButtonClicked()
    {
        creditsUI.Show();
        Hide();
        openedMenus[(int)Menu.Credits] = true;
        openedMenus[(int)Menu.Main] = false;

    }


    public void OnHostGameButtonClicked()
    {
        hostGameUI.Show();
        Hide();
        openedMenus[(int)Menu.HostGame] = true;
        openedMenus[(int)Menu.Main] = false;
    }


    public void OnCreateLobbyButtonClicked()
    {
        if (hostGameUI.LobbyName() == string.Empty)
        {
            hostGameUI.ShowLobbyNotification();
            return;
        }

        lobbyUI.Show();
        hostGameUI.Hide();
        openedMenus[(int)Menu.HostGame] = false;
        openedMenus[(int)Menu.Lobby] = true;
        hosting = true;
    }

    public void OnPlayButtonClicked()
    {
        Stages.Load(StageName.Template);
    }

    public void OnOptionsButtonClicked()
    {
        optionsUI.Show();
        Hide();
        openedMenus[(int)Menu.Options] = true;
        openedMenus[(int)Menu.Main] = false;

    }


    public void OnConnectionButtonClicked()
    {
        connectionUI.Show();
        optionsUI.Hide();
        openedMenus[(int)Menu.Connection] = true;
        openedMenus[(int)Menu.Options] = false;

    }


    public void OnProfileButtonClicked()
    {
        profileUI.Show();
        optionsUI.Hide();
        openedMenus[(int)Menu.Profile] = true;
        openedMenus[(int)Menu.Options] = false;

    }


    public void OnGraphicsButtonClicked()
    {
        graphicsUI.Show();
        optionsUI.Hide();
        openedMenus[(int)Menu.Graphics] = true;
        openedMenus[(int)Menu.Options] = false;

    }


    public void OnSoundsButtonClicked()
    {
        soundUI.Show();
        optionsUI.Hide();
        openedMenus[(int)Menu.Sound] = true;
        openedMenus[(int)Menu.Options] = false;
    }


    public void OnMiscButtonClicked()
    {
        miscUI.Show();
        optionsUI.Hide();
        openedMenus[(int)Menu.Misc] = true;
        openedMenus[(int)Menu.Options] = false;

    }


    public void OnJoinGameButtonClicked()
    {
        joinGameUI.Show();
        Hide();
        openedMenus[(int)Menu.JoinGame] = true;
        openedMenus[(int)Menu.Main] = false;
    }


    public void OnControlsButtonClicked()
    {
        controlsUI.Show();
        optionsUI.Hide();
        openedMenus[(int)Menu.Controls] = true;
        openedMenus[(int)Menu.Options] = false;
    }

    #endregion


    #region BACK_BUTTONS

    public void OnSoundBackButtonClicked()
    {
        optionsUI.Show();
        soundUI.Hide();
        openedMenus[(int)Menu.Options] = true;
        openedMenus[(int)Menu.Sound] = false;

    }


    public void OnControlsBackButtonClicked()
    {
        controlsUI.Hide();
        optionsUI.Show();
        openedMenus[(int)Menu.Options] = true;
        openedMenus[(int)Menu.Controls] = false;

    }

    public void OnHostGameBackButtonClicked()
    {
        Show();
        hostGameUI.Hide();
        openedMenus[(int)Menu.Main] = true;
        openedMenus[(int)Menu.HostGame] = false;

    }


    public void OnCreditsBackButtonClicked()
    {
        optionsUI.Show();
        creditsUI.Hide();
        openedMenus[(int)Menu.Options] = true;
        openedMenus[(int)Menu.Credits] = false;

    }

    public void OnMiscBackButtonClicked()
    {
        miscUI.Hide();
        optionsUI.Show();
        openedMenus[(int)Menu.Options] = true;
        openedMenus[(int)Menu.Misc] = false;
    }


    public void OnOptionsBackButtonClicked()
    {
        optionsUI.Hide();
        Show();
        openedMenus[(int)Menu.Main] = true;
        openedMenus[(int)Menu.Options] = false;
    }

    public void OnJoinGameBackButtonClicked()
    {
        joinGameUI.Hide();
        Show();
        openedMenus[(int)Menu.Main] = true;
        openedMenus[(int)Menu.JoinGame] = false;
    }

    public void OnLeaveLobbyButtonClicked()
    {
        if (hosting)
        {
            hostGameUI.Show();
            lobbyUI.Hide();
            openedMenus[(int)Menu.HostGame] = true;
            openedMenus[(int)Menu.Lobby] = false;
            hosting = false;
        }

        else
        {
            joinGameUI.Show();
            lobbyUI.Hide();
            openedMenus[(int)Menu.JoinGame] = true;
            openedMenus[(int)Menu.Lobby] = false;
        }

    }

    #endregion

    public void Show()
    {
        mainMenuCanvas.enabled = true;

    }

    public void Hide()
    {

        mainMenuCanvas.enabled = false;
    }

}


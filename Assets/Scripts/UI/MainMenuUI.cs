﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{

    [SerializeField]
    private Color buttonTextColor;
    private const string HOTEL_SCENE = "hotel";

    [SerializeField]
    private Canvas mainMenuCanvas;

    private const int ARRAY_SIZE = 11;

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
    private CreditsUI creditsUI;

    public bool[] openedMenus;

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
        JoinGame
    }



    void Start()
    {
        openedMenus = new bool[ARRAY_SIZE];
        openedMenus[(int)Menu.Main] = true;
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
        button.GetComponentInChildren<Text>().color = buttonTextColor;

    }

    public void OnMouseLeftButton(Button button)
    {
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
    public void OnPlayButtonClicked()
    {
        SceneManager.LoadScene(HOTEL_SCENE);
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
        optionsUI.Show();
        hostGameUI.Hide();
        openedMenus[(int)Menu.Options] = true;
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

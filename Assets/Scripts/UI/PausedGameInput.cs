using UnityEngine;
using System;
using UnityEngine.UI;

public class PausedGameInput : MonoBehaviour
{
    public enum Menu
    {
        None = 0,
        Pause,
        Options,
        Controls,
        Graphics,
        Connection,
        Profile,
        Sound,
        Misc
    }

    private const int ARRAY_SIZE = 8;


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
    private PauseUI pauseUI;

    [SerializeField]
    private ProfileUI profileUI;

    [SerializeField]
    private MiscUI miscUI;

    [SerializeField]
    private bool[] openedMenus;

    public static bool GAME_PAUSED;

    [SerializeField]
    private Color buttonTextColor;

    // This class should fix the bug where if we hit escape we go back straight to the pause
    // menu instead of the previous menu.
    // This also makes it so all UI keyboard input is in one place.

    // There is probably a much better way to do this but I don't care. UI coding is always a pain.
    // If anyone wants to improve this, please do!
    // While this may not look the best, at least the in-game UI will only process the pause key once per frame.
    void Start()
    {
        openedMenus = new bool[ARRAY_SIZE];
    }

    // this update method does one job and that is to detect which menus are being opened and if they are opened
    // make it so it closes and goes back to the main options menu.
    void Update()
    {
        if (ConsoleUI.OPENED || Chat.OPENED)
        {
            return;
        }

        if (Keybinds.GetKey(Action.GUiReturn) && !GAME_PAUSED)
        {
            openedMenus[(int)Menu.Pause] = true;
            GAME_PAUSED = true;
            pauseUI.Show();
            Cursor.lockState = CursorLockMode.Confined;
        }

        // I tried to make this clean but oh well.
        else if (Keybinds.GetKey(Action.GUiReturn) && GAME_PAUSED)
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
                        openedMenus[(int)Menu.Connection] = false;
                        openedMenus[(int)Menu.Options] = true;
                        optionsUI.Show();
                        openedMenuFound = true;
                        break;
                    case Menu.Profile:
                        profileUI.Hide();
                        openedMenus[(int)Menu.Profile] = false;
                        openedMenus[(int)Menu.Options] = true;
                        openedMenuFound = true;
                        optionsUI.Show();
                        break;
                    case Menu.Graphics:
                        graphicsUI.Hide();
                        openedMenus[(int)Menu.Graphics] = false;
                        openedMenus[(int)Menu.Options] = true;
                        openedMenuFound = true;
                        optionsUI.Show();
                        break;
                    case Menu.Options:
                        optionsUI.Hide();
                        openedMenus[(int)Menu.Options] = false;
                        openedMenus[(int)Menu.Pause] = true;
                        pauseUI.Show();
                        openedMenuFound = true;
                        break;
                    case Menu.Controls:
                        controlsUI.Hide();
                        optionsUI.Show();
                        openedMenus[(int)Menu.Controls] = false;
                        openedMenus[(int)Menu.Options] = true;
                        openedMenuFound = true;
                        break;
                    case Menu.Pause:
                        pauseUI.Hide();
                        GAME_PAUSED = false;
                        openedMenuFound = true;
                        openedMenus[(int)Menu.Pause] = false;
                        Cursor.lockState = CursorLockMode.Locked;
                        break;
                    case Menu.Misc:
                        miscUI.Hide();
                        optionsUI.Show();
                        openedMenus[(int)Menu.Misc] = false;
                        openedMenus[(int)Menu.Options] = true;
                        break;

                    case Menu.Sound:
                        soundUI.Hide();
                        optionsUI.Show();
                        openedMenus[(int)Menu.Sound] = false;
                        openedMenus[(int)Menu.Options] = true;
                        break;
                }

                if (openedMenuFound)
                {
                    break;
                }
            }
        }
    }


    public void OnGraphicsButtonClicked()
    {
        openedMenus[(int)Menu.Graphics] = true;
        openedMenus[(int)Menu.Options] = false;
        graphicsUI.Show();
        optionsUI.Hide();
    }

    public void OnConnectionButtonClicked()
    {
        openedMenus[(int)Menu.Options] = false;
        openedMenus[(int)Menu.Connection] = true;
        connectionUI.Show();
        optionsUI.Hide();
    }

    public void OnControlsButtonClicked()
    {
        openedMenus[(int)Menu.Controls] = true;
        openedMenus[(int)Menu.Options] = false;
        controlsUI.Show();
        optionsUI.Hide();
    }

    public void OnProfileButtonClicked()
    {
        openedMenus[(int)Menu.Profile] = true;
        openedMenus[(int)Menu.Options] = false;
        profileUI.Show();
        optionsUI.Hide();
    }

    public void OnOptionsButtonClicked()
    {
        openedMenus[(int)Menu.Options] = true;
        openedMenus[(int)Menu.Pause] = false;
        optionsUI.Show();
        pauseUI.Hide();
    }

    public void OnSoundsButtonClicked()
    {
        openedMenus[(int)Menu.Sound] = true;
        openedMenus[(int)Menu.Options] = false;
        soundUI.Show();
        optionsUI.Hide();
    }

    public void OnReturnToGameButtonClicked()
    {
        openedMenus[(int)Menu.Pause] = false;
        GAME_PAUSED = false;
        pauseUI.Hide();
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnMiscButtonClicked()
    {
        openedMenus[(int)Menu.Misc] = true;
        openedMenus[(int)Menu.Options] = false;
        miscUI.Show();
        optionsUI.Hide();
    }

    public void OnSoundsBackButtonClicked()
    {
        openedMenus[(int)Menu.Sound] = false;
        openedMenus[(int)Menu.Options] = true;
        soundUI.Hide();
        optionsUI.Show();
    }


    public void OnGraphicsBackButtonClicked()
    {
        openedMenus[(int)Menu.Graphics] = false;
        openedMenus[(int)Menu.Options] = true;
        graphicsUI.Hide();
        optionsUI.Show();

    }

    public void OnProfileBackButtonClicked()
    {
        openedMenus[(int)Menu.Profile] = false;
        openedMenus[(int)Menu.Options] = true;
        profileUI.Hide();
        optionsUI.Show();

    }


    public void OnKeybindsBackButtonClicked()
    {
        openedMenus[(int)Menu.Controls] = false;
        openedMenus[(int)Menu.Options] = true;
        controlsUI.Hide();
        optionsUI.Show();
    }


    public void OnOptionsBackButtonClicked()
    {
        openedMenus[(int)Menu.Options] = false;
        openedMenus[(int)Menu.Pause] = true;
        optionsUI.Hide();
        pauseUI.Show();
    }

    public void OnMiscBackButtonClicked()
    {
        openedMenus[(int)Menu.Misc] = false;
        openedMenus[(int)Menu.Options] = true;
        miscUI.Hide();
        optionsUI.Show();
    }


    public void OnMouseHover(Button button)
    {
        button.GetComponentInChildren<Text>().color = buttonTextColor;
        
    }

    public void OnMouseLeft(Button button)
    {
        button.GetComponentInChildren<Text>().color = Color.white;
    }
}
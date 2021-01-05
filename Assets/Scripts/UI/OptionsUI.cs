using UnityEngine;

public class OptionsUI : MonoBehaviour
{
    [SerializeField]
    private Canvas optionsCanvas;

    [SerializeField]
    private PauseUI pauseUI;

    [SerializeField]
    private MainMenuUI mainMenuUI;

    [SerializeField]
    private SoundUI soundUI;

    [SerializeField]
    private ConnectionUI connectionUI;

    [SerializeField]
    private ProfileUI profileUI;

    [SerializeField]
    private ControlsUI controlsUI;

    [SerializeField]
    private MiscUI miscUI;

    [SerializeField]
    private GraphicsUI graphicsUI;

    private void Start()
    {
        this.enabled = false;
    }

    private void Update()
    {
        if (Keybinds.GetKey(Action.GuiReturn))
        {
            //NOTE:We must be calling this from the main menu
            //because I did not set it in the In-game UI prefab which is always in a 
            //game scene.

            if (pauseUI == null)
            {
                mainMenuUI.Show();
            }

            //NOTE:We must be calling this from the in-game menu
            //because I did not set it in the main menu scene.

            else if (mainMenuUI == null)
            {
                pauseUI.Show();

            }

            Hide();
        }
    }

    public void Show()
    {
        optionsCanvas.enabled = true;
        this.enabled = true;
    }

    private void Hide()
    {
        optionsCanvas.enabled = false;
        this.enabled = false;
    }

    public void OnConnectionButtonClicked()
    {
        connectionUI.Show();
        Hide();
    }

    public void OnControlsButtonClicked()
    {
        controlsUI.Show();
        Hide();

    }

    // NOTE: Used for the in-game instead of the main menu.
    public void OnOptionsBackButtonIngame()
    {
        pauseUI.Show();
        Hide();
    }

    // NOTE: Used for the main menu instead of in game.
    public void OnOptionsBackButtonMainMenu()
    {
        mainMenuUI.Show();
        Hide();
    }

    public void OnMiscButtonClicked()
    {
        miscUI.Show();
        Hide();

    }

    public void OnGraphicsButtonClicked()
    {
        graphicsUI.Show();
        Hide();
    }

    public void OnSoundsButtonClicked()
    {
        soundUI.Show();
        Hide();
    }

    public void OnProfileButtonClicked()
    {
        profileUI.Show();
        Hide();
    }


}


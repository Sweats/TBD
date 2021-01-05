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
            pauseUI.Show();
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


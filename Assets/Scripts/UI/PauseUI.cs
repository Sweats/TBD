using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    private const string MAIN_MENU_SCENE = "menu";

    [SerializeField]
    private Canvas pauseCanvas;

    [SerializeField]
    private ProfileUI profileUI;

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
    private MiscUI miscUI;

    [SerializeField]
    private Color buttonTextColor;

    [SerializeField]
    private Windows windows;

    private void Start()
    {
        this.enabled = false;
    }

    private void Update()
    {
        if (Keybinds.GetKey(Action.GuiReturn))
        {
            Hide();
        }
    }

    public void Show()
    {
        this.enabled = true;
        pauseCanvas.enabled = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void Hide()
    {
        this.enabled = false;
        pauseCanvas.enabled = false;
        windows.enabled = true;
        windows.MarkPauseWindowClosed();
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnExitGameButtonClicked()
    {
        Application.Quit();

        if (Application.isEditor)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }


    public void OnBackToTitleScreenButtonClicked()
    {
        Stages.Load(StageName.Menu);
    }

    public void OnGraphicsButtonClicked()
    {
        graphicsUI.Show();
        optionsUI.Hide();
    }

    private void OnClosePauseWindow()
    {
        Hide();
    }

    public void OnConnectionButtonClicked()
    {
        connectionUI.Show();
        optionsUI.Hide();
    }

    public void OnControlsButtonClicked()
    {
        controlsUI.Show();
        optionsUI.Hide();
    }

    public void OnProfileButtonClicked()
    {
        profileUI.Show();
        optionsUI.Hide();
    }

    public void OnOptionsButtonClicked()
    {
        optionsUI.Show();
        Hide();
    }

    public void OnSoundsButtonClicked()
    {
        soundUI.Show();
        optionsUI.Hide();
    }

    public void OnReturnToGameButtonClicked()
    {
        OnClosePauseWindow();
    }

    public void OnMiscButtonClicked()
    {
        miscUI.Show();
        optionsUI.Hide();
    }

    public void OnSoundsBackButtonClicked()
    {
        soundUI.Hide();
        optionsUI.Show();
    }


    public void OnGraphicsBackButtonClicked()
    {
        graphicsUI.Hide();
        optionsUI.Show();

    }

    public void OnProfileBackButtonClicked()
    {
        profileUI.Hide();
        optionsUI.Show();

    }


    public void OnControlsBackButtonClicked()
    {
        controlsUI.Hide();
        optionsUI.Show();
    }


    public void OnOptionsBackButtonClicked()
    {
        optionsUI.Hide();
        Show();
    }

    public void OnMiscBackButtonClicked()
    {
        miscUI.Hide();
        optionsUI.Show();
    }

    public void OnPointerEnterButton(Button button)
    {
        if (!button.enabled)
        {
            return;
        }

        button.GetComponentInChildren<Text>().color = buttonTextColor;
    }


    public void OnPointerExitButton(Button button)
    {
        if (!button.enabled)
        {
            return;
        }

        button.GetComponentInChildren<Text>().color = Color.white;

    }
}

using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    private const string MAIN_MENU_SCENE = "menu";

    [SerializeField]
    private Canvas pauseCanvas;

    [SerializeField]
    private OptionsUI optionsUI;

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
            windows.enabled = true;
            windows.MarkPauseWindowClosed();
            Cursor.lockState = CursorLockMode.Locked;
            Hide();
        }
    }

    public void Show()
    {
        this.enabled = true;
        pauseCanvas.enabled = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Hide()
    {
        this.enabled = false;
        pauseCanvas.enabled = false;
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

    public void OnOptionsButtonClicked()
    {
        optionsUI.Show();
        Hide();
    }


    public void OnReturnToGameButtonClicked()
    {
        windows.enabled = true;
        windows.MarkPauseWindowClosed();
        Hide();
        Cursor.lockState = CursorLockMode.Locked;
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

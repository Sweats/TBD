using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField]
    private OptionsUI optionsMenu;

    [SerializeField]
    private Color buttonTextColor;

    [SerializeField]
    private Canvas pauseCanvas;

    public bool gamePaused { get; private set; }

    // Update is called once per frame
    void Update()
    {
        if (Keybinds.GetKey(Action.GUiReturn))
        {
            pauseCanvas.enabled = !pauseCanvas.enabled;

            if (pauseCanvas.enabled)
            {
                Cursor.lockState = CursorLockMode.Confined;
                gamePaused = true;
            }

            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                gamePaused = false;
            }
        }
    }


    public void Show()
    {
        pauseCanvas.enabled = true;

    }
    public void Hide()
    {
        pauseCanvas.enabled = false;
    }


    public void OnResumeGameButtonClicked()
    {
        Hide();
        gamePaused = false;
    }


    public void OnHoverPauseMenuButton(Button button)
    {

        Text text = button.GetComponentInChildren<Text>();
        text.color = buttonTextColor;
    }

    public void OnMouseUnhoverPauseMenuButton(Button button)
    {

        Text text = button.GetComponentInChildren<Text>();
        text.color = Color.white;
    }


    public void OnOptionsButtonClicked()
    {
        optionsMenu.Show();
        Hide();

    }


    public void OnTitleScreenButtonClicked()
    {

    }

    public void OnExitGameButtonClicked()
    {
        Application.Quit();

        if (Application.isEditor)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }

}

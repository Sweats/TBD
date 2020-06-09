using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField]
    private Color buttonTextColor;

    [SerializeField]
    private Canvas pauseCanvas;

    [SerializeField]
    private OptionsUI optionsUI;
    public void Show()
    {
        pauseCanvas.enabled = true;

    }
    public void Hide()
    {
        pauseCanvas.enabled = false;
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
        optionsUI.Show();
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

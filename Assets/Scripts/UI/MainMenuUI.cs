using UnityEngine;
using UnityEngine.UI;

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
        Cursor.lockState = CursorLockMode.Confined;
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
        if (Application.isEditor)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }

        Application.Quit();
    }

    public void OnPlayButtonClicked()
    {
        Stages.Load(StageName.Template);
    }

    public void OnOptionsButtonClicked()
    {
        optionsUI.Show();
        Hide();

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


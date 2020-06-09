using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    [SerializeField]
    private Canvas optionsCanvas;

    [SerializeField]
    private GraphicsUI graphicsMenu;

    [SerializeField]
    private ControlsUI keybindingsUI;

    [SerializeField]
    private ProfileUI profileUI;

    [SerializeField]
    private ConnectionUI connectionUI;

    [SerializeField]
    private PauseUI pauseMenu;

    [SerializeField]
    private Color buttonColor;

    [SerializeField]
    private SoundUI soundUI;

    public void OnGraphicsButtonClicked()
    {
        graphicsMenu.Show();
        Hide();
    }

    public void OnMouseEnterOptionsButton(Button button)
    {
        Text text = button.GetComponentInChildren<Text>();
        text.color = buttonColor;
    }


    public void OnMouseLeftOptionsButton(Button button)
    {
        Text text = button.GetComponentInChildren<Text>();
        text.color = Color.white;
    }


    public void OnControlsButtonClicked()
    {
        Hide();
        keybindingsUI.Show();
    }


    public void OnSoundButtonClicked()
    {
        Hide();
        soundUI.Show();
    }


    public void OnProfileButtonClicked()
    {
        Hide();
        profileUI.Show();

    }


    public void OnConnectionButtonClicked()
    {

    }

    public void OnBackButtonClicked()
    {
        pauseMenu.Show();
        Hide();
    }

    public void Show()
    {
        optionsCanvas.enabled = true;
    }


    public void Hide()
    {
        optionsCanvas.enabled = false;
    }

}


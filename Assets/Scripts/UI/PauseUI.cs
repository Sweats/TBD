using UnityEngine;
using UnityEngine.UI;
using Mirror;

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
        if (NetworkServer.active)
        {
            Debug.Log("The host has stopped the server!");
            NetworkManager.singleton.StopHost();
        }

        else
        {
            NetworkManager.singleton.StopClient();
        }

        Application.Quit();
    }

    public void OnBackToTitleScreenButtonClicked()
    {
        if (NetworkServer.active)
        {
            //Debug.Log("Server has left the game!");
            Debug.Log("The host has stopped the server!");
            NetworkManager.singleton.StopHost();
        }

        else
        {
            NetworkManager.singleton.StopClient();
        }

        //Stages.Load(StageName.Menu);
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

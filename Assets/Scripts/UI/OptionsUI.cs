using UnityEngine;

public class OptionsUI : MonoBehaviour
{
    [SerializeField]
    private Canvas optionsCanvas;

    [SerializeField]
    private PauseUI pauseUI;

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

    public void Hide()
    {
        optionsCanvas.enabled = false;
        this.enabled = false;
    }
    
}


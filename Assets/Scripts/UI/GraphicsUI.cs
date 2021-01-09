using UnityEngine;

public class GraphicsUI : MonoBehaviour
{

    [SerializeField]
    private Canvas graphicsMenu;

    [SerializeField]
    private OptionsUI optionsUI;

    private void Start()
    {
        this.enabled = false;

    }


    private void Update()
    {
        if (Keybinds.GetKey(Action.GuiReturn))
        {
            optionsUI.Show();
            Hide();
        }

    }

    public void Show()
    {
        this.enabled = true;
        graphicsMenu.enabled = true;

    }


    public void Hide()
    {
        this.enabled = false;
        graphicsMenu.enabled = false;
        optionsUI.Show();
        SaveGraphicsConfig();
    }

    private void SaveGraphicsConfig()
    {

    }
}


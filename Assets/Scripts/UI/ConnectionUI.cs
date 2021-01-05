using UnityEngine;
public class ConnectionUI : MonoBehaviour
{

    [SerializeField]
    private Canvas connectionCanvas;

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
            Hide();
            optionsUI.Show();
        }

    }

    private void Hide()
    {
        SaveConnectionConfig();
        connectionCanvas.enabled = false;
        this.enabled = false;
    }


    public void Show()
    {
        this.enabled = true;
        connectionCanvas.enabled = true;
    }


    private void SaveConnectionConfig()
    {

    }
    
}


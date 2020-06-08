using UnityEngine;

public class ConnectionUI : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField]
    private Canvas connectionCanvas;

    [SerializeField]
    private OptionsUI optionsUI;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Keybinds.GetKey(Action.GUiReturn) && connectionCanvas.enabled)
        {
            Hide();
            optionsUI.Show();
        }
        
    }


    public void Hide()
    {
        connectionCanvas.enabled = false;

    }


    public void Show()
    {
        connectionCanvas.enabled = true;
    }
}

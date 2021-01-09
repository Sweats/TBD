using UnityEngine;

public class CreditsUI : MonoBehaviour
{

    [SerializeField]
    private Canvas creditsCanvas;


    [SerializeField]
    private  MainMenuUI mainMenuUI;

    private void Start()
    {
        this.enabled = false;
        
    }

    private void Update()
    {
        if (Keybinds.GetKey(Action.GuiReturn))
        {
            mainMenuUI.Show();
            Hide();
        }
        
    }


    public void Show()
    {
        this.enabled = true;
        creditsCanvas.enabled = true;

    }

    private void Hide()
    {
        this.enabled = false;
        creditsCanvas.enabled = false;
    }
}

using UnityEngine;

public class ProfileUI : MonoBehaviour
{
    [SerializeField]
    private Canvas profileCanvas;

    [SerializeField]
    private PauseUI pauseUI;

    public static string profileName;

    private void Update()
    {
        if (Keybinds.GetKey(Action.GuiReturn))
        {
            pauseUI.Show();
            Hide();
        }
    }

    private void Start()
    {
        this.enabled = false;
    }

    public void Show()
    {
        this.enabled = true;
        profileCanvas.enabled = true;
    }


    public void Hide()
    {
        this.enabled = false;
        profileCanvas.enabled = false;
        SaveProfileConfig();
    }


    public void OnEditProfileName(string name)
    {
        profileName = name;

    }

    private void SaveProfileConfig()
    {

    }
}

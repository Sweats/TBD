using UnityEngine;

public class ProfileUI : MonoBehaviour
{
    [SerializeField]
    private Canvas profileCanvas;

    [SerializeField]
    private OptionsUI optionsUI;

    private void Update()
    {
        if (Keybinds.GetKey(Action.GuiReturn))
        {
            optionsUI.Show();
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


    private void Hide()
    {
        this.enabled = false;
        profileCanvas.enabled = false;
        SaveProfileConfig();
    }


    public void OnEditProfileName(string name)
    {
        Settings.PROFILE_NAME = name;
    }

    private void SaveProfileConfig()
    {

    }
}

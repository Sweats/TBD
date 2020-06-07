using UnityEditor.PackageManager;
using UnityEngine;

public class ProfileUI : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField]
    private Canvas profileCanvas;


    [SerializeField]
    private OptionsUI optionsUI;

    public static string profileName;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Keybinds.GetKey(Action.GUiReturn) && profileCanvas.enabled)
        {

            Hide();
            optionsUI.Show();

        }
    }

    public void Show()
    {
        profileCanvas.enabled = false;
    }


    public void Hide()
    {
        profileCanvas.enabled = true;
    }


    public void OnEditProfileName(string name)
    {
        profileName = name;

    }
}

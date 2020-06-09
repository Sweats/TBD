using UnityEditor.PackageManager;
using UnityEngine;

public class ProfileUI : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField]
    private Canvas profileCanvas;

    public static string profileName;

    public void Show()
    {
        profileCanvas.enabled = false;
    }


    public void Hide()
    {
        profileCanvas.enabled = true;
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

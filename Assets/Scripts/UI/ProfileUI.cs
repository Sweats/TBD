using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class ProfileUI : MonoBehaviour
{
    [SerializeField]
    private Canvas profileCanvas;

    [SerializeField]
    private OptionsUI optionsUI;

    private const string PROFILE_NAME_KEY_STRING = "profile_name";

    [SerializeField]
    private Text successText;

    [SerializeField]
    private InputField profileNameInputField;

    [SerializeField]
    private Text failedText;

    private bool failed;

    private bool nameChanged;

    private string oldName;

    private void Update()
    {
        if (failed)
        {
            return;
        }

        if (Keybinds.GetKey(Action.GuiReturn))
        {
            optionsUI.Show();
            Hide();
        }
    }

    private void Start()
    {
        this.enabled = false;
        LoadProfileConfig();
    }

    public void Show()
    {
        oldName = Settings.PROFILE_NAME;
        this.enabled = true;
        profileCanvas.enabled = true;
        successText.enabled = false;
        failedText.enabled = false;
        failed = false;
    }

    private void Hide()
    {
        this.enabled = false;
        profileCanvas.enabled = false;
        successText.enabled = false;
        failedText.enabled = false;
        SaveProfileConfig();
        failed = false;

        if (nameChanged)
        {
            //EventManager.playerClientChangedNameEvent.Invoke(oldName, Settings.PROFILE_NAME);

            if (NetworkClient.active)
            {
                //NetworkClient.Send(new ServerPlayerChangedProfileNameMessage { oldName = this.oldName, newName = Settings.PROFILE_NAME });
            }
        }
        nameChanged = false;
    }

    public void OnEditProfileName(string newName)
    {
        if (newName == string.Empty)
        {
            failed = true;
            failedText.enabled = true;
            return;
        }

        failed = false;
        successText.enabled = true;
        failedText.enabled = false;
        Settings.PROFILE_NAME = newName;
        nameChanged = true;
    }

    private void SaveProfileConfig()
    {
        PlayerPrefs.SetString(PROFILE_NAME_KEY_STRING, Settings.PROFILE_NAME);
        PlayerPrefs.Save();
    }

    private void LoadProfileConfig()
    {
        string nameFromConfig = PlayerPrefs.GetString(PROFILE_NAME_KEY_STRING, "player");
        profileNameInputField.text = nameFromConfig;
        Settings.PROFILE_NAME = nameFromConfig;

    }
}

using UnityEngine;

public class MiscUI : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private Canvas miscCanvas;

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
        miscCanvas.enabled = true;
        this.enabled = true;
    }

    private void OnMiscBackButtonClicked()
    {

    }


    private void Hide()
    {
        SaveMiscConfig();
        miscCanvas.enabled = false;
        this.enabled = false;
        pauseUI.Show();

    }

    private void SaveMiscConfig()
    {

    }
}

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

    public void Show()
    {
        miscCanvas.enabled = true;
        this.enabled = true;
    }


    public void Hide()
    {
        SaveMiscConfig();
        miscCanvas.enabled = false;
        this.enabled = false;

    }

    private void SaveMiscConfig()
    {

    }
}

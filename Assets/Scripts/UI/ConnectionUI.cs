using UnityEngine;
public class ConnectionUI : MonoBehaviour
{

    [SerializeField]
    private Canvas connectionCanvas;
    void Start()
    {

    }

    public void Hide()
    {
        SaveConnectionConfig();
        connectionCanvas.enabled = false;
    }


    public void Show()
    {
        connectionCanvas.enabled = true;
    }


    private void SaveConnectionConfig()
    {

    }
    
}


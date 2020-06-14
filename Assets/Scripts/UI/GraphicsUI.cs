using UnityEngine;

public class GraphicsUI : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField]
    private Canvas graphicsMenu;

    void Start()
    {

    }

    public void Show()
    {
        graphicsMenu.enabled = true;

    }


    public void Hide()
    {
        graphicsMenu.enabled = false;
        SaveGraphicsConfig();

    }

    private void SaveGraphicsConfig()
    {

    }
}


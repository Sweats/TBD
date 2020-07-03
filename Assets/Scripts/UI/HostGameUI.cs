using UnityEngine;
public class HostGameUI : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private Canvas hostgameCanvas;

    void Start()
    {
        
    }


    public void Show()
    {
        hostgameCanvas.enabled = true;

    }


    public void Hide()
    {
        hostgameCanvas.enabled = false;
    }

    
}

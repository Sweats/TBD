using UnityEngine;

public class CreditsUI : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private Canvas creditsCanvas;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Show()
    {
        creditsCanvas.enabled = true;

    }


    public void Hide()
    {
        creditsCanvas.enabled = false;

    }
}

using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    [SerializeField]
    private Canvas optionsCanvas;

    public void Show()
    {
        optionsCanvas.enabled = true;
    }


    public void Hide()
    {
        optionsCanvas.enabled = false;
    }

}


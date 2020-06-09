using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiscUI : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private Canvas miscCanvas;

    void Start()
    {

    }

    public void Show()
    {

        miscCanvas.enabled = true;
    }


    public void Hide()
    {
        SaveMiscConfig();
        miscCanvas.enabled = false;

    }

    private void SaveMiscConfig()
    {

    }
}

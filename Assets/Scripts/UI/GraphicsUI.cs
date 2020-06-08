using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GraphicsUI : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField]
    private Canvas graphicsMenu;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Keybinds.GetKey(Action.GUiReturn) && graphicsMenu.enabled)
        {
            Hide();

        }
        
    }
    

    public void Show()
    {
        graphicsMenu.enabled = true;

    }


    public void Hide()
    {
        graphicsMenu.enabled = false;

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinGameUI : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField]
    private Canvas joinGameCanvas;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Show()
    {
        joinGameCanvas.enabled = true;

    }

    public void Hide()
    {
        joinGameCanvas.enabled = false;
    }

    public void OnRefreshButtonClicked()
    {

    }

    public void OnConnectButtonClicked()
    {

    }
}

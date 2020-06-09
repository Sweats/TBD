using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField]
    private Canvas pauseCanvas;

    public void Show()
    {
        pauseCanvas.enabled = true;

    }
    public void Hide()
    {
        pauseCanvas.enabled = false;
    }

     public void OnExitGameButtonClicked()
     {
        Application.Quit();

        if (Application.isEditor)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
     }
}

using UnityEngine;
public class PauseUI : MonoBehaviour
{
    private const string MAIN_MENU_SCENE = "menu";

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


     public void OnBackToTitleScreenButtonClicked()
     {
         Stages.Load(StageName.Menu);
     }
}

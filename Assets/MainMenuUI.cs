using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{

    [SerializeField]
    private Color buttonTextColor;
    private const string HOTEL_SCENE = "hotel";
    
    public void OnMouseOverButton(Button button)
    {
        button.GetComponentInChildren<Text>().color = buttonTextColor;

    }


    public void OnMouseLeftButton(Button button)
    {
        button.GetComponentInChildren<Text>().color = Color.white;
    }


    public void OnQuitButtonClicked()
    {
        if (Application.isEditor)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }

        Application.Quit();
    }


    public void OnCreditsButtonClicked()
    {

    }


    public void OnHostGameButtonClicked()
    {

    }

    public void OnPlayButtonClicked()
    {
        SceneManager.LoadScene(HOTEL_SCENE);
    }


    public void OnOptionsButtonClicked()
    {

    }

}

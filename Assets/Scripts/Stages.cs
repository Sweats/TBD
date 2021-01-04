using System.Collections.Generic;
using UnityEngine.SceneManagement;

// NOTE: Make sure that this list matches the dictionary down a few lines below.
public enum StageName
{
    Template = 0,
    Template_Lurker,
    Template_Mary,
    Template_Phantom,
    Template_Fallen,
    Menu
}

public class Stages
{

    private Stages() { }

    // NOTE: Make sure that the order of this matches the enum at the top of this file.
    private static Dictionary<StageName, string> stagesDict = new Dictionary<StageName, string>()
    {
        {StageName.Template, "stage-template"},
        {StageName.Template_Fallen, "stage-template-fallen"},
        {StageName.Template_Phantom, "stage-template-phantom"},
        {StageName.Template_Mary, "stage-template-mary"},
        {StageName.Template_Lurker, "stage-template-lurker"},
        {StageName.Menu, "menu"}
    };


    public static void Load(StageName stageName)
    {
        if (stagesDict.ContainsKey(stageName))
        {
            PausedGameInput.GAME_PAUSED = false;
            ConsoleUI.OPENED = false;
            Chat.OPENED = false;
            SceneManager.LoadScene(stagesDict[stageName]);
        }

        else
        {
            string stageNameFailedToLoad = stagesDict[stageName];
            EventManager.failedToLoadStageEvent.Invoke(stageNameFailedToLoad);
            //Debug.Log($"Failed to load the stage {stagesDict[stageName]}!");
        }
    }
}

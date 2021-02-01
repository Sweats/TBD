using System.Collections.Generic;
using UnityEngine.SceneManagement;

// NOTE: Make sure that this list matches the dictionary down a few lines below.
public enum StageName
{
    Template_Lurker,
    Menu
}

public class Stages
{

    private Stages() { }

    // NOTE: Make sure that the order of this matches the enum at the top of this file.
    private static Dictionary<StageName, string> stagesDict = new Dictionary<StageName, string>()
    {
        {StageName.Template_Lurker, "stage-template-lurker"},
        {StageName.Menu, "menu"}
    };

    public static void Load(StageName stageName)
    {
        if (stagesDict.ContainsKey(stageName))
        {
            SceneManager.LoadScene(stagesDict[stageName]);
        }

        else
        {
            string stageNameFailedToLoad = stagesDict[stageName];
            EventManager.failedToLoadStageEvent.Invoke(stageNameFailedToLoad);
            //Debug.Log($"Failed to load the stage {stagesDict[stageName]}!");
        }
    }

    public static string Name(StageName name)
    {
        return stagesDict[name];
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum StageName
{
    Hotel = 0,
    Menu
}

public class Stages
{
    private static Dictionary<StageName, string> stagesDict = new Dictionary<StageName, string>()
    {
        {StageName.Hotel, "hotel"},
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
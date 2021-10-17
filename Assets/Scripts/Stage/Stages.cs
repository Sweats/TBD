using System.Collections.Generic;
using UnityEngine.SceneManagement;

// NOTE: Make sure that this list matches the dictionary down a few lines below.
public enum StageName: byte
{
    Templace_Networking,
    Menu,
    Lobby,
    MasterServer,
}

public class Stages
{
    private Stages() { }

    // NOTE: Make sure that the order of this matches the enum at the top of this file.
    private static Dictionary<StageName, string> stagesDict = new Dictionary<StageName, string>()
    {
        {StageName.Templace_Networking, "stage-template-networking"},
        {StageName.Menu, "menu"},
        {StageName.Lobby, "lobby"},
        {StageName.MasterServer, "master-server"}
    };

    public static void Load(StageName stageName)
    {
        if (stagesDict.ContainsKey(stageName))
        {
            SceneManager.LoadScene(stagesDict[stageName]);
        }
    }

    public static string Name(StageName name)
    {
        return stagesDict[name];
    }
}

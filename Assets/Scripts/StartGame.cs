using UnityEngine;

public class StartGame : MonoBehaviour
{
    private void Start()
    {
        Stages.Load(StageName.Menu);
    }
}

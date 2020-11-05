using UnityEngine;

// NOTE: Not sure how this class will be used exactly. Maybe it will handle global events or something.
public class Stage : MonoBehaviour
{
    [SerializeField]
    private Survivor[] survivors;

    private bool matchOver;

    private void Start()
    {
        //EventManager.monsterSpawnedInStageEvent.AddListener(OnMonsterSpawnedOnStage);

    }

    public void OnSurviorDeath(Survivor who)
    {
        for (var i = 0; i < survivors.Length; i++)
        {
            if (survivors[i].survivorID == who.survivorID)
            {
                continue;
            }

        }
    }

    public void OnSurvivorStartSprintingEvent(Survivor who)
    {

    }

    public void OnSurvivorStopSprintingEvent(Survivor who)
    {

    }

    private void HandleKeyPaths()
    {
        GameObject[] paths = GameObject.FindGameObjectsWithTag(Tags.PATH);

        for (var i = 0; i < paths.Length; i++)
        {
            Path path = paths[i].GetComponent<Path>();
	    path.SpawnKeyObjectsInPath();
        }
    }

}

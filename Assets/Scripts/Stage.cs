using UnityEngine;

public class Stage : MonoBehaviour
{
    [SerializeField]
    private Survivor[] survivors;

    private Lurker lurker;

    private void Start()
    {

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


    public void OnSurvivorEnteredExitZone(Survivor who)
    {
        bool canEscape = true;

        for (var i = 0; i < survivors.Length; i++)
        {
            if (survivors[i].dead)
            {
                continue;
            }

            if (!survivors[i].isInEscapeRoom)
            {
                canEscape = false;
                break;

            }
        }

        if (canEscape)
        {
            for (var i = 0; i < survivors.Length; i++)
            {
                survivors[i].matchOver = true;
            }
        }
    }

    public void OnSurvivorLeftExitZone(Survivor who)
    {
        who.isInEscapeRoom = false;
    }
}

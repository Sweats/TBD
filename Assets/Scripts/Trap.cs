using UnityEngine;
using System.Collections;

public class Trap : MonoBehaviour
{
    [SerializeField]
    private AudioSource trapSound;

    [SerializeField]
    private float maxTimer;

    [SerializeField]
    private float minTimer;

    public float trapTimer;
    public float trapTimerRate;

    public bool armed;

    private void Start()
    {
        EventManager.survivorTriggeredTrapEvent.AddListener(OnSurvivorTriggeredTrap);
        StartCoroutine(TrapTimer());
    }

    public void Trigger()
    {	
        trapSound.Play();
        armed = false;
        trapTimer = maxTimer;
        StartCoroutine(TrapTimer());
    }


    private void OnSurvivorTriggeredTrap(Survivor survivor, Trap trap)
    {
        trap.Trigger();
    }

    private IEnumerator TrapTimer()
    {
        while (true)
        {
            trapTimer -= 1;

	    // TO DO: Make it so traps behave differently depending on the monster that is on the stage.

            if (trapTimer <= minTimer || armed)
            {
                armed = true;
                trapTimer = minTimer;
		yield break;
            }

            yield return new WaitForSeconds(1);
        }

    }
}


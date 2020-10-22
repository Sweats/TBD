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

		   if (trapTimer <= minTimer)
		   {
			   armed = true;
			   trapTimer = minTimer;
			   break;
		   }

		   yield return new WaitForSeconds(1);
	    }

	    StopCoroutine(TrapTimer());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class OnSurvivorTriggeredTrapEvent: UnityEvent<Survivor, Trap>{}

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

    void Update()
    {
        if (!armed)
        {
            trapTimer -= Time.deltaTime * trapTimerRate;

            if (trapTimer <= minTimer)
            {
                armed = true;
                trapTimer = minTimer;
            }
        }
        
    }

    public void Trigger()
    {
        trapSound.Play();
        Reset();
    }

    private void Reset()
    {
        armed = false;
        trapTimer = maxTimer;
    }


}

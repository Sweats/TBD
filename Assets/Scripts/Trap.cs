using UnityEngine;

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
    }

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
        armed = false;
        trapTimer = maxTimer;
    }

    private void OnSurvivorTriggeredTrap(Survivor survivor, Trap trap)
    {
        trap.Trigger();
    }
}

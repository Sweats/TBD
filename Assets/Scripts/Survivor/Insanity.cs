using UnityEngine;

public enum InsanityEffects
{
    None = 0,
    Gamma,
    Deaf,
    FlashlightFlicker,
    BlackAndWhite,
    FakeTrapSound,
    Jumpscare

}

public class Insanity : MonoBehaviour
{
    public float maxInsanity;
    public float insanityRate;
    public float insanityValue;
    public bool insanityEnabled;
    private bool maxed;

    [SerializeField]
    private float insanityHitTrapAmount;

    [SerializeField]
    private float insanitySurvivorDeathAmount;

    [SerializeField]
    private InsanityEffect[] insanityEffects;
    
    private void Start()
    {
        EventManager.survivorDeathEvent.AddListener(OnSurvivorDeath);
        EventManager.survivorTriggeredTrapEvent.AddListener(OnSurvivorTriggeredTrap);
    }

    void Update()
    {
        if (insanityEnabled && !maxed)
        {
            insanityValue += insanityRate * Time.deltaTime;

            if (insanityRate >= maxInsanity)
            {
                maxed = true;
                insanityRate = maxInsanity;
            }

            for (var i = 0; i < insanityEffects.Length; i++)
            {
                if (insanityEffects[i].insanityEnabled)
                {
                    insanityEffects[i].timer -= Time.deltaTime * insanityEffects[i].timerDischargeRate;
                }

                if (insanityValue >= insanityEffects[i].insanityNeededToStart)
                {
                    insanityEffects[i].insanityEnabled = true;
                }

                if (insanityEffects[i].timer <= 0)
                {
                    insanityEffects[i].insanitySoundEffect.Play();
                    insanityEffects[i].insanityEffectEvent.Invoke();
                    // TO DO. Make this a little bit random. Easy to do but I don't know what values I want to use yet.
                    insanityEffects[i].timer = 100;
                }
            }
        }

    }

    public void Reset()
    {
        for (var i = 0; i < insanityEffects.Length; i++)
        {
            insanityEffects[i].insanityEnabled = false;
        }

        insanityEnabled = false;
        insanityValue = 0;
        maxed = false;

    }

    private void OnSurvivorDeath(Survivor survivor)
    {
        survivor.insanity.insanityValue += insanitySurvivorDeathAmount;
    }


    private void OnSurvivorTriggeredTrap(Survivor survivor, Trap trap)
    {
        survivor.insanity.insanityValue += insanityHitTrapAmount;
    }

}

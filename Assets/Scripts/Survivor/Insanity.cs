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
    [SerializeField]
    private float maxInsanity;

    [SerializeField]
    private float insanityRate;

    [SerializeField]
    private float insanityValue;

    [SerializeField]
    private bool insanityEnabled;

    [SerializeField]
    private float insanitySurvivorDeathAmount;

    [SerializeField]
    private InsanityEffect[] insanityEffects;

    private bool maxed;

    private void Start()
    {
        EventManager.survivorDeathEvent.AddListener(OnSurvivorDeath);
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

    public void Increment(float amount)
    {
        this.insanityValue += amount;
    }
    
    private void OnSurvivorDeath(string playerName)
    {
        this.insanityValue += insanitySurvivorDeathAmount;
    }
}

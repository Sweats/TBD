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
    public float insanityHitTrapAmount;
    public float insanitySurvivorDeathAmount;

    [SerializeField]
    private InsanityEffect[] insanityEffects;

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
                if (insanityEffects[i].enabled)
                {
                    insanityEffects[i].timer -= Time.deltaTime * insanityEffects[i].timerDischargeRate;
                }

                if (insanityValue >= insanityEffects[i].insanityNeededToStart)
                {
                    insanityEffects[i].enabled = true;
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
            insanityEffects[i].enabled = false;
        }

        insanityEnabled = false;
        insanityValue = 0;
        maxed = false;
    }
}

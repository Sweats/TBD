using UnityEngine;

//public class InsranityEvent : UnityEvent<Insanity> { }

public class Insanity : MonoBehaviour
{
    public float maxInsanity;
    public float insanityRate;
    public float insanityValue;
    public bool insanityEnabled;

    [SerializeField]
    private bool maxed;

    [SerializeField]
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

    private const int GAMMA = 0;

    private const int DEAF = 1;

    private const int FLASHLIGHTFLICKER = 2;

    private const int BLACK_AND_WHITE = 3;

    private const int FAKE_TRAP_SOUND = 4;

    private const int JUMP_SCARE = 5;

    public float insanityHitTrapAmount;
    public float insanitySurvivorDeathAmount;

    [SerializeField]
    private InsanityEffect[] insanityEffects;

    // Start is called before the first frame update
    void Start()
    {

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
                if (insanityValue >= insanityEffects[i].insanityNeededToStart)
                {
                    insanityEffects[i].Enable();
                }
            }
        }
        
    }

    public void Reset()
    {
        for (var i = 0; i < insanityEffects.Length; i++)
        {
            insanityEffects[i].Disable();
        }

        insanityEnabled = false;
        insanityValue = 0;
        maxed = false;
    }
}

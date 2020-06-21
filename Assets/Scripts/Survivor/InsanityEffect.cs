using UnityEngine;
using UnityEngine.Events;

// will be used to show spectating players the insaniy effect that the player is currently seeing.
//public class InsanityEffectEvent : UnityEvent<InsanityEffect> { }

public class InsanityEffect : MonoBehaviour
{
    private bool enabled;
    public float insanityNeededToStart;

    [SerializeField]
    private float timerDischargeRate;

    [SerializeField]
    private float timer = 100;

    [SerializeField]
    private AudioSource insanitySoundEffect;

    public InsanityEffectEvent insanityEffectEvent;

    public bool Enabled()
    {
        return enabled;
    }

    void Update()
    {
        if (enabled)
        {
            timer -= timerDischargeRate * Time.deltaTime;

            if (timer <= 0)
            {
                Trigger();
                timer = 100;
            }
        }
    }


    public void Trigger()
    {
        insanitySoundEffect.Play();
        insanityEffectEvent.Invoke();
    }


    public void OnGammaInsanityEffectTriggered()
    {

    }


    public void OnDeafInsanityEffectTriggered()
    {

    }


    public void OnFlashlightFlickerInsanityEffectTriggered()
    {

    }


    public void OnBlackAndWhiteInsanityEffectTriggered()
    {

    }


    public void OnFakeTrapInsanityEffectTriggered()
    {

    }


    public void OnJumpScareInsanityEffectTriggered()
    {

    }


    public void Enable()
    {
        enabled = true;

    }

    public void Disable()
    {
        enabled = false;
    }

}
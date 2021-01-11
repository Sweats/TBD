using UnityEngine;
using System.Collections;
using Mirror;

// we will use this at some point to tell other clients that we have toggeled the flashlight.

public class Flashlight : MonoBehaviour
{
    [SerializeField]
    private float maxCharge;

    [SerializeField]
    private float minCharge;

    [SerializeField]
    private float dischargeRate;

    [SerializeField]
    private AudioSource flashlightToggleSound;

    [SerializeField]
    private float charge;

    private Light flashlightSource;

    private bool flashlightDead;

    private bool toggled;

    private IEnumerator flashlightRoutine;

    public Flashlight(float charge, float intensity, bool toggled, bool dead)
    {

    }

    public void ClientStart()
    {
        toggled = true;
        flashlightDead = false;
        flashlightSource = GetComponent<Light>();
        flashlightRoutine = FlashlightRoutine();
        StartCoroutine(flashlightRoutine);
    }


    private IEnumerator FlashlightRoutine()
    {
        while (true)
        {
            charge -= dischargeRate;
            flashlightSource.intensity = charge;

            if (charge <= minCharge)
            {
                charge = minCharge;
                flashlightSource.intensity = minCharge;
                flashlightDead = true;
                yield break;
            }

            yield return new WaitForSeconds(1);
        }
    }

    public void Toggle()
    {
        flashlightSource.enabled = !flashlightSource.enabled;
        toggled = !toggled;

        if (flashlightDead)
        {
            return;
        }

        if (toggled && charge > minCharge)
        {
            StartCoroutine(flashlightRoutine);
        }

        else if (!toggled && charge > minCharge)
        {
            StopCoroutine(flashlightRoutine);
        }

    }

    public bool Toggled()
    {
        return toggled;
    }

    public bool Dead()
    {
        return flashlightDead;
    }

    public void Recharge()
    {
        charge = maxCharge;
        flashlightSource.intensity = maxCharge;
        flashlightDead = false;
    }

    // For the Monsters
    public void Hide()
    {
        flashlightSource.enabled = false;
    }

    // For the Monsters
    public void Show()
    {
        if (toggled)
        {
            flashlightSource.enabled = true;

        }
    }

    public float Charge()
    {
        return charge;
    }

    public void PlayToggleSound()
    {
        flashlightToggleSound.Play();
    }
}

namespace FlashlightExtension
{
    public static class FlashlightCustomExtension
    {
        public static void WriteFlashlight(this NetworkWriter writer, Flashlight value)
        {
            writer.WriteSingle(value.Charge());
            writer.WriteSingle(value.Charge());
            writer.WriteBoolean(value.Toggled());
            writer.WriteBoolean(value.Dead());
        }

        public static Flashlight ReadFlashlight(this NetworkReader reader)
        {
            float charge = reader.ReadSingle();
            float intensity = reader.ReadSingle();
            bool toggled = reader.ReadBoolean();
            bool dead = reader.ReadBoolean();
            return new Flashlight(charge, intensity, toggled, dead);

        }

    }

}

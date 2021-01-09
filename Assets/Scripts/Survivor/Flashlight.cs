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

    private bool flashlightDead = false;

    // May seem like an unneeded variable but this is used for the Lurker. We don't want to accidentally show the flashlight to the Lurker if the survivor has their actual flashlight off.
    private bool flashlightEnabled = true;

    private IEnumerator flashlightRoutine;

    void Start()
    {
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
        flashlightEnabled = !flashlightEnabled;

        if (flashlightDead)
        {
            return;
        }

        if (flashlightEnabled && charge > minCharge)
        {
            StartCoroutine(flashlightRoutine);
        }

        else if (!flashlightEnabled && charge > minCharge)
        {
            StopCoroutine(flashlightRoutine);
        }

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
        if (flashlightEnabled)
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

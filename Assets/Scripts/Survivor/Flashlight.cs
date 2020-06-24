using UnityEngine;
using UnityEngine.Events;

// we will use this at some point to tell other clients that we have toggeled the flashlight.
public class FlashlightEvent: UnityEvent<Flashlight> { }

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

    public float charge;

    private Light flashlightSource;


    private bool flashlightDead = false;

    void Start()
    {
        flashlightSource = GetComponent<Light>();
    }

    void Update()
    {
        if (flashlightSource.enabled && charge > 0 && !flashlightDead)
        {
            charge -= Time.deltaTime * dischargeRate;
            flashlightSource.intensity = charge;

            if (charge < minCharge)
            {
                charge = minCharge;
                flashlightSource.intensity = minCharge;
                flashlightDead = true;
            }
        }
    }


    public void Toggle()
    {
        flashlightSource.enabled = !flashlightSource.enabled;
        flashlightToggleSound.Play();
    }

    public void Recharge()
    {
        charge = maxCharge;
        flashlightSource.intensity = 4;
        flashlightDead = false;
    }
}

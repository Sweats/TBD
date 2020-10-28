using UnityEngine;
using UnityEngine.Events;

// we will use this at some point to tell other clients that we have toggeled the flashlight.
public class FlashlightEvent : UnityEvent<Flashlight> { }

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

    // May seem like an unneeded variable but this is used for the Lurker. We don't want to accidentally show the flashlight to the Lurker if the survivor has their actual flashlight off.
    private bool flashlightEnabled = true;

    void Start()
    {
        flashlightSource = GetComponent<Light>();
        //EventManager.survivorPickedUpBatteryEvent.AddListener(OnSurvivorPickedUpBattery);
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
        flashlightEnabled = !flashlightEnabled;
        flashlightToggleSound.Play();
    }

    public void Recharge()
    {
        charge = maxCharge;
        flashlightSource.intensity = 4;
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

}

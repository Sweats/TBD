using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [SerializeField]
    private float maxCharge;

    [SerializeField]
    private float minCharge;

    [SerializeField]
    private float dischargeRate;

    public float chargeNeededToGrab;

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

            if (charge < flashlightSource.intensity)
            {
                flashlightSource.intensity = charge;
            }

            if (charge < minCharge)
            {
                charge = minCharge;
                flashlightSource.intensity = 0;
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

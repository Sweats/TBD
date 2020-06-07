using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public float maxCharge;
    public float minCharge;
    public float charge;
    public float dischargeRate;
    private bool flashlightDead = false;

    public Light flashlightSource;

    public AudioSource flashlightToggleSound;

    [SerializeField]
    private PauseUI pauseUI;

    private const int MOUSEBUTTON_RIGHT = 1;
    void Start()
    {
        flashlightSource = GetComponent<Light>();
    }

    void Update()
    {
        if (Keybinds.GetKey(Action.SwitchFlashlight) && !pauseUI.gamePaused)
        {
            flashlightSource.enabled = !flashlightSource.enabled;
            flashlightToggleSound.Play();
        }

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
                flashlightDead = true;
            }
        }
    }
}

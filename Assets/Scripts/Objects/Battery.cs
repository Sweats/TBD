using UnityEngine;

public class Battery : MonoBehaviour
{
    [SerializeField]
    private string name = "battery";

    [SerializeField]
    private Color originalMeshColor = Color.yellow;

    [SerializeField]
    private Color outlineGlowColor = Color.red;

    [SerializeField]
    private float timeBetweenGlows = 45;

    [SerializeField]
    private float rechargeAmount = 100;

    [SerializeField]
    private float distanceToPickUp = 20;

    [SerializeField]
    private float glowTimerLength = 115;

    [SerializeField]
    private Color glowColor = Color.white;

    public float chargeNeededToGrab;

    [SerializeField]
    private float maxTimerForGlow = 100;
    [SerializeField]
    private float minTimer = 0;
    [SerializeField]
    private float maxTimer;

    private float glowTimer = 1.0f;
    private float noGlowTimer = 1.0f;

    private Renderer batteryRenderer;

    private float defaultSpecular;

    private bool glowing = false;

    void Start()
    {
        noGlowTimer = maxTimerForGlow;
        batteryRenderer = GetComponent<Renderer>();
        //defaultSpecular = batteryRenderer.material.GetFloat("_Shininess");
        //batteryRenderer.material.SetColor("_Color", originalMeshColor);


        EventManager.survivorPickedUpBatteryEvent.AddListener(OnSurvivorPickedUpBattery);

    }

    // Update is called once per frame
    void Update()
    {
        noGlowTimer -= Time.deltaTime * timeBetweenGlows;

        if (noGlowTimer <= minTimer && !glowing)
        {
            glowing = true;
        }

        if (glowing)
        {
            glowTimer -= Time.deltaTime * glowTimerLength;
            noGlowTimer = 0;
            Glow();

            if (glowTimer <= minTimer)
            {
                glowing = false;
                UnGlow();
                noGlowTimer = maxTimerForGlow;
                glowTimer = maxTimerForGlow;
            }
        }
    }


    void OnMouseEnter()
    {
        float distanceFromPlayer = 0f;

        if (distanceFromPlayer <= distanceToPickUp)
        {
            batteryRenderer.material.SetColor("_Color", outlineGlowColor);
        }
    }

    void OnMouseExit()
    {
        // Set the outline color to Color.clear.
    }

    private void Glow()
    {
        batteryRenderer.material.SetColor("_Color", outlineGlowColor);
        //batteryRenderer.material.SetFloat("_Shininess", 1.0f);

    }

    private void UnGlow()
    {
        batteryRenderer.material.SetColor("_Color", originalMeshColor);
        //batteryRenderer.material.SetFloat("_Shininess", defaultSpecular);

    }

    public void Pickup()
    {
        Destroy(this.gameObject);
    }


    private void OnSurvivorPickedUpBattery(Survivor survivor, Battery battery)
    {
        battery.Pickup();
    }
}

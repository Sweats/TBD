using UnityEngine;
using Mirror;

public class Battery : NetworkBehaviour
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

    [SerializeField]
    private float chargeNeededToGrab;

    [SerializeField]
    private float maxTimerForGlow = 100;
    [SerializeField]
    private float minTimer = 0;
    [SerializeField]
    private float maxTimer;

    private float glowTimer = 1.0f;
    private float noGlowTimer = 1.0f;

    [SerializeField]
    private Renderer batteryRenderer;

    [SerializeField]
    private CapsuleCollider batteryCollider;

    private float defaultSpecular;

    private bool glowing = false;

    private int batteryID;

    [Client]
    private void Start()
    {
        batteryID = Random.Range(0, 10000);
        noGlowTimer = maxTimerForGlow;
        //defaultSpecular = batteryRenderer.material.GetFloat("_Shininess");
        //batteryRenderer.material.SetColor("_Color", originalMeshColor);

    }

    [Client]
    private void Update()
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

    [Client]
    private void Glow()
    {
        batteryRenderer.material.SetColor("_Color", outlineGlowColor);
    }

    [Client]
    private void UnGlow()
    {
        batteryRenderer.material.SetColor("_Color", originalMeshColor);
        //batteryRenderer.material.SetFloat("_Shininess", defaultSpecular);

    }

    public float ChargeNeededToGrab()
    {
        return chargeNeededToGrab;
    }

    // For the Monsters.
    [Client]
    public void Hide()
    {
        batteryCollider.enabled = false;
        batteryRenderer.enabled = false;

    }

    // For the Monsters.
    [Client]
    public void Show()
    {
        batteryCollider.enabled = true;
        batteryRenderer.enabled = true;
    }
}

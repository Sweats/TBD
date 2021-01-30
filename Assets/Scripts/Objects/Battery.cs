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

    public float chargeNeededToGrab;

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


    void OnMouseExit()
    {
    }

    private void Glow()
    {
        batteryRenderer.material.SetColor("_Color", outlineGlowColor);
    }

    private void UnGlow()
    {
        batteryRenderer.material.SetColor("_Color", originalMeshColor);
        //batteryRenderer.material.SetFloat("_Shininess", defaultSpecular);

    }

    // For the Monsters.
    public void Hide()
    {
        batteryCollider.enabled = false;
        batteryRenderer.enabled = false;

    }

    // For the Monsters.
    public void Show()
    {
        batteryCollider.enabled = true;
        batteryRenderer.enabled = true;
    }

    public int BatteryID()
    {
        return batteryID;
    }

    [Command(ignoreAuthority=true)]
    public void CmdPlayerClickedOnBattery(NetworkConnectionToClient sender = null)
    {
        Survivor survivor = sender.identity.GetComponent<Survivor>();

        if (survivor.FlashlightCharge() <= chargeNeededToGrab)
        {
            survivor.ServerRechargeFlashlight();
            NetworkServer.Destroy(this.gameObject);
        }

        else
        {
            TargetFailedToPickUpBattery();
        }
    }

    [TargetRpc]
    private void TargetFailedToPickUpBattery()
    {
        EventManager.survivorFailedToPickUpBatteryEvent.Invoke();
    }

    [ClientRpc]
    private void RpcPlayerPickedUpBattery()
    {
        Debug.Log("A survivor picked up a battery!");
    }
}

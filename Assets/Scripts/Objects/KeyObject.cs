using UnityEngine;
using Mirror;

public class KeyObject : NetworkBehaviour
{
    [SerializeField]
    private string keyName;

    [SerializeField]
    private int mask;

    [SerializeField]
    private KeyType type;

    [SerializeField]
    private int pathID;

    private bool pickedUp = false;

    [SerializeField]
    private Color groupColor;

    [SerializeField]
    private float timeBetweenGlows = 45;
    [SerializeField]
    private float glowTimerLength = 115;

    [SerializeField]
    private float maxTimerForGlow = 100;

    [SerializeField]
    private float minTimer = 0;

    [SerializeField]
    private float noGlowTimer;

    [SerializeField]
    private float glowTimer;


    [SerializeField]
    private AudioSource pickupSound;

    [SerializeField]
    private Texture keyIcon;

    [SerializeField]
    // this is the color that will appear around the key mesh to let the player know they can pick it up
    private Color glowOutlineColor = Color.red;

    [SerializeField]
    private Color glowColor = Color.white;

    [SerializeField]
    private Renderer keyRenderer;

    [SerializeField]
    private MeshCollider keyCollider;


    [Client]
    private void Start()
    {
        noGlowTimer = maxTimerForGlow;
        keyRenderer.material.SetColor("_Color", glowOutlineColor);
    }

    [Client]
    private void Update()
    {
        noGlowTimer -= Time.deltaTime * timeBetweenGlows;

        if (noGlowTimer <= minTimer)
        {
            Glow();

            glowTimer -= Time.deltaTime * glowTimerLength;
            noGlowTimer = 0;

            if (glowTimer <= minTimer)
            {
                UnGlow();
                noGlowTimer = maxTimerForGlow;
                glowTimer = maxTimerForGlow;
            }
        }

    }


    private void Glow()
    {
        keyRenderer.material.SetColor("_Color", glowColor);

    }

    private void UnGlow()
    {
        keyRenderer.material.SetColor("_Color", glowOutlineColor);

    }

    public void Hide()
    {
        keyRenderer.enabled = false;
        this.enabled = false;
        keyCollider.enabled = false;
    }

    public void Show()
    {
        if (!pickedUp)
        {
            keyRenderer.enabled = true;
            keyCollider.enabled = true;
        }
    }

    [Client]
    public void ClientPlayPickupSound()
    {
        pickupSound.Play();
    }

    public void SetName(string name)
    {
        this.keyName = name;

    }

    public void SetMask(int mask)
    {
        this.mask = mask;
    }

    public void SetType(KeyType type)
    {
        this.type = type;
    }

    public int Mask()
    {
        return mask;
    }

    public int PathID()
    {
        return pathID;
    }

    public string Name()
    {
        return keyName;
    }

    public KeyType Type()
    {
        return type;
    }
}


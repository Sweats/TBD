using UnityEngine;

// this class stores the stuff we need to update the mesh itself. The actual key information will be stored in the class key.
public class KeyObject : MonoBehaviour
{
    public Key key;

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
    // this is the color that will appear around the key mesh to let the player know they can pick it up
    private Color glowOutlineColor = Color.red;

    [SerializeField]
    private Color glowColor = Color.white;

    private Renderer keyRenderer;

    void Start()
    {
        noGlowTimer = maxTimerForGlow;
        keyRenderer = GetComponent<Renderer>();
        keyRenderer.material.SetColor("_Color", glowOutlineColor);
        EventManager.survivorPickedUpKeyEvent.AddListener(OnSurvivorPickedUpKey);
    }

    void Update()
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

    public void Pickup()
    {
        Destroy(this.gameObject);
    }

    private void OnSurvivorPickedUpKey(Survivor survivor, Key key)
    {
        key.PlayPickupSound();
    }

}


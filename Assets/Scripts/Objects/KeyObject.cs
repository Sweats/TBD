using UnityEngine;

// this class stores the stuff we need to update the mesh itself. The actual key information will be stored in the class key.
public class KeyObject : MonoBehaviour
{
    [SerializeField]
    private Key _key;

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

    private bool pickedUp = false;

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

    void Start()
    {
        noGlowTimer = maxTimerForGlow;
        keyRenderer.material.SetColor("_Color", glowOutlineColor);
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

    public Key Key()
    {
        return _key;

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
        pickedUp = true;
	pickupSound.Play();
        Hide();
    }

    public void Hide()
    {
	    keyRenderer.enabled = false;
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


    public void SetKey(Key key)
    {
	    _key =  key;
    }


    public Texture Texture()
    {
	    return keyIcon;

    }
}


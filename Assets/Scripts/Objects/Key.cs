using UnityEngine;
using UnityEngine.UI;

public class Key : MonoBehaviour
{
    public string keyName = "Rusty Key";

    public int keyMask = -1;

    public int group = 0;

    [SerializeField]
    private Color groupColor;

    [SerializeField]
    private AudioSource pickupSound;

    public Texture iconTexture;

    [SerializeField]
    private float distanceToPickUp = 20f;

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


    void OnMouseEnter()
    {
        float distance = 0f;

        if (distance <= distanceToPickUp)
        {
            keyRenderer.material.SetColor("_Color", glowOutlineColor);
        }
    }


    void OnMouseExit()
    {
        float distance = 0f;

        if (distance <= distanceToPickUp)
        {
            keyRenderer.material.SetColor("_Color", Color.clear);

        }

    }


    void OnMouseDown()
    {
        float distance = 0f;

        if (distance <= distanceToPickUp)
        {
            pickupSound.Play();
            Grab();
        }
    }


    public void Grab()
    {

    }
}


using UnityEngine;



public class Key : MonoBehaviour
{

    public string keyName;
    public float unlockMask;

    private Renderer keyRenderer;

    public float timeBetweenGlows;
    public float glowTimerLength;

    private float maxTimerForGlow = 100;
    private float minTimer = 0;
    public float noGlowTimer;

    public float glowTimer;

    private Color originalMeshColor = Color.red;


    void Start()
    {
        noGlowTimer = maxTimerForGlow;
        keyRenderer = GetComponent<Renderer>();
        keyRenderer.material.SetColor("_Color", originalMeshColor);
        
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
        keyRenderer.material.SetColor("_Color", Color.white);

    }

    private void UnGlow()
    {
        keyRenderer.material.color = originalMeshColor;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{
    public float timeBetweenGlows;
    public float rechargeAmount;


    public float glowTimerLength;

    private float maxTimerForGlow = 100;
    private float minTimer = 0;
    public float noGlowTimer;

    public float glowTimer;


    private Renderer batteryRenderer;

    private Color originalMeshColor = Color.yellow;

    // Used for when we pick it up so we can print it to the top left to the screen as well as to other players.
    private string batteryName = "Battery";

    // Start is called before the first frame update
    void Start()
    {
        noGlowTimer = maxTimerForGlow;
        batteryRenderer = GetComponent<Renderer>();
        batteryRenderer.material.SetColor("_Color", originalMeshColor);

    }

    // Update is called once per frame
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
        batteryRenderer.material.SetColor("_Color", Color.white);

    }

    private void UnGlow()
    {
        batteryRenderer.material.color = originalMeshColor;

    }

}

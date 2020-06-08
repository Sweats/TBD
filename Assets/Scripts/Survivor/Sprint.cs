using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprint : MonoBehaviour
{
    [SerializeField]
    private float sprintSpeed;

    public bool sprinting;

    [SerializeField]
    private float maxEnergy;

    [SerializeField]
    private float minEnergy;

    [SerializeField]
    private float dischargeRate;

    [SerializeField]
    private float rechargeRate;

    [SerializeField]
    private float energyNeededToSprint;

    [SerializeField]
    private AudioSource outOfBreath;

    private float energy;

    private const int DEFAULT_SPEED = 3;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { 
        if (sprinting)
        {
            energy -= dischargeRate * Time.deltaTime;

            if (energy <= minEnergy)
            {
                outOfBreath.Play();
                sprintSpeed = DEFAULT_SPEED;
                sprinting = false;
            }
        }

        else
        {
            energy += rechargeRate * Time.deltaTime;

            if (energy >= maxEnergy)
            {
                energy = maxEnergy;
            }
        }
    }
}

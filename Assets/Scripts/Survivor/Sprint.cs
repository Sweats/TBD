using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Sprint : MonoBehaviour
{
    public float sprintSpeed;

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

    // Update is called once per frame
    void Update()
    {
        if (Keybinds.Get(Action.Sprint))
        {
            if (energy >= energyNeededToSprint)
            {
                sprinting = true;
            }
        }

        else if (Keybinds.GetKey(Action.Sprint, true))
        {
            sprinting = false;
        }

        if (sprinting)
        {
            energy -= dischargeRate * Time.deltaTime;

            if (energy <= minEnergy)
            {
                outOfBreath.Play();
                sprinting = false;
            }
        }

        else
        {
            energy += rechargeRate * Time.deltaTime;

            if (energy > maxEnergy)
            {
                energy = maxEnergy;
            }
        }
    }
}

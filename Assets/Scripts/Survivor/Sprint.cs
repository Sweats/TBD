﻿using System.Collections;
using UnityEngine;
public class Sprint : MonoBehaviour
{
    private bool isSprinting;

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
    private float tickRate;

    [SerializeField]
    private AudioSource outOfBreath;

    //This points back to Chad in the Editor, is this shit even right? i just need fucking shit from the fucking Survivor fucking class fuck also fuck debugger doesn't work fuck
    //on second though, it's probably right, since chad /would/ be the only local player. handling stamina for other dummies isn't necessary and gasping sound will be handled by network events anyway
    [SerializeField]
    private Survivor localSurvivor;

    private float energy;
    
    private void Start()
    {
        //Survs start with Full Energy
        energy = maxEnergy;
        StartCoroutine(calcStamina());
        //InvokeRepeating("calcSprint", 0, 1);
    }
    
private IEnumerator calcStamina()
    {
        while (true)
        {
            //don't need to do any calcs if Match is over or Char is dead, so check first.
            if (localSurvivor.GetMatchOver() || localSurvivor.GetDead())
            {
                break;
            }

            if (isSprinting)
            {
                if (energy > minEnergy)
                {
                    energy -= dischargeRate;
                }
                else
                {
                    outOfBreath.Play();
                    isSprinting = false;
                }
            }
            else
            {
                if (energy < maxEnergy)
                {
                    energy += rechargeRate;
                }
            }

            yield return new WaitForSeconds(tickRate);

        }

        StopCoroutine(calcStamina());
    }
    public float GetEnergy()
    {
        return energy;
    }
    public float GetEnergyNeededToSprint()
    {
        return energyNeededToSprint;
    }
    public void SetEnergy(float input_energy)
    {
        energy += input_energy;
    }
    public bool GetSprinting()
    {
        return isSprinting;
    }
    public void SetSprinting(bool input_sprinting)
    {
        isSprinting = input_sprinting;
    }
    public float GetTickRate()
    {
        return tickRate;
    }
}

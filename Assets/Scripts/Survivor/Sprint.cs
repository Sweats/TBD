using UnityEngine;
public class Sprint : MonoBehaviour
{
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

    private void Start()
    {
        //Survs start with Full Energy
        energy = maxEnergy;
        InvokeRepeating("calcSprint", 0, 1);
    }

    // Update is called once per frame
    /*
    void Update()
    {
        
    }
    */
    //move Sprint Calc to custom function, attempting 9heads-like way for Stamina
    void calcSprint()
    {
        Debug.Log("Energy: " + energy);

        if (sprinting)
        {
            if (energy > minEnergy)
            {
                energy -= dischargeRate;
                //* Time.deltaTime;
            }
            else
            {
                outOfBreath.Play();
                sprinting = false;
            }
        }
        else
        {
            if (energy < maxEnergy)
            {
                energy += rechargeRate;
                //* Time.deltaTime;
            }
        }
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
}

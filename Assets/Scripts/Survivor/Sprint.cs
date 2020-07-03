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
    pr
    kddk

    // Update is called once per frame
    void Update()
    {
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

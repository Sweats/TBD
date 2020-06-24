using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lurker : MonoBehaviour
{
    private CharacterController lurkerController;

    public float physicalFormSpeed;

    public float ghostFormSpeed;

    public float maxEnergy = 100;

    public float minEnergy = 0;

    public float energyDischargeRate;

    public float energyRechargeRate;

    public float attackDistance;

    public float attackSpeed;

    public float energy;

    private float speed;


    private bool ghostForm = true;

    [SerializeField]
    private AudioSource lurkerChangeFormSound;

    [SerializeField]
    private AudioSource lurkerGhostFormMusic;

    [SerializeField]
    private AudioSource lurkerPhysicalFormMusic;

    [SerializeField]
    private AudioSource footstepSound;

    #region EVENTS

    public LurkerChangedFormEvent lurkerChangedFormEvent;

    #endregion

    void Start()
    {
        lurkerController = GetComponent<CharacterController>();
    }

    void Update()
    {
        UpdateEnergy();

        if (Keybinds.GetKey(Action.Transform))
        {
            ghostForm = !ghostForm;

            if (ghostForm)
            {
                lurkerChangedFormEvent.Invoke(ghostForm);
            }
        }

        else if (Keybinds.GetKey(Action.Attack))
        {
            HandleAttack();
        }
    }


    private void HandleAttack()
    {
        RaycastHit hit;

        //if (Physics.SphereCast(transform.position, 1.))
    }


    private void UpdateEnergy()
    {
        if (ghostForm)
        {
            energy += energyRechargeRate * Time.deltaTime;

            if (energy >= maxEnergy)
            {
                energy = maxEnergy;
            }
        }

        else
        {
            energy -= energyDischargeRate * Time.deltaTime;

            if (energy <= minEnergy)
            {
                energy = minEnergy;
            }
        }

    }


    public void OnLurkerFormChanged(bool ghostForm)
    {
        lurkerChangeFormSound.Play();

        if (ghostForm)
        {
            speed = ghostFormSpeed;
            lurkerGhostFormMusic.Play();
        }

        else
        {
            speed = physicalFormSpeed;
            lurkerGhostFormMusic.Stop();
            lurkerPhysicalFormMusic.Play();
        }
    }
}

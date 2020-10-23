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

    public float trapArmDistance;

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

    [SerializeField]
    private AudioSource trapArmSound;

    [SerializeField]
    private AudioSource attackSound;

    private Camera lurkerCamera;

    #region EVENTS

    public LurkerChangedFormEvent lurkerChangedFormEvent;

    #endregion

    void Start()
    {
        lurkerController = GetComponent<CharacterController>();
        lurkerCamera = GetComponent<Camera>();
        //LurkerChangedFormEvent.AddListener(OnLurkerFormChanged());
    }

    void Update()
    {
        UpdateEnergy();

        if (Keybinds.GetKey(Action.Transform))
        {
            if (ghostForm)
            {
                if (energy >= maxEnergy)
                {
                    OnLurkerFormChanged();

                }
            }

            else
            {
                if (energy <= minEnergy)
                {
                    OnLurkerFormChanged();
                }
            }

        }

        else if (Keybinds.GetKey(Action.Attack))
        {
            OnAttack();
        }

        else if (Keybinds.GetKey(Action.MoveForward))
        {

        }

        else if (Keybinds.GetKey(Action.MoveLeft))
        {

        }

        else if (Keybinds.GetKey(Action.MoveRight))
        {

        }

        else if (Keybinds.GetKey(Action.MoveBack))
        {

        }

        else if (Keybinds.GetKey(Action.Grab))
        {
            HandleGrab();

        }
    }


    private void UpdateEnergy()
    {
        if (ghostForm)
        {
            energy += energyRechargeRate * Time.deltaTime;

            if (energy >= maxEnergy)
            {
                energy = maxEnergy;
                EventManager.lurkerReadyToGoIntoPhysicalFormEvent.Invoke();

            }
        }

        else
        {
            energy -= energyDischargeRate * Time.deltaTime;

            if (energy <= minEnergy)
            {
                energy = minEnergy;
                OnLurkerFormChanged();
            }
        }

    }


    private void OnLurkerFormChanged()
    {
        lurkerChangeFormSound.Play();
        ghostForm = !ghostForm;
        EventManager.lurkerChangedFormEvent.Invoke(ghostForm);

        if (ghostForm)
        {
            speed = ghostFormSpeed;

            if (lurkerPhysicalFormMusic.isPlaying)
            {
                lurkerPhysicalFormMusic.Stop();
            }

            lurkerGhostFormMusic.Play();
        }

        else
        {
            speed = physicalFormSpeed;

            if (lurkerGhostFormMusic.isPlaying)
            {
                lurkerGhostFormMusic.Stop();
            }

            lurkerPhysicalFormMusic.Play();
        }
    }


    private void OnAttack()
    {
        Ray ray = lurkerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // TO DO: Play an attack sound or animation here somewhere
        if (!ghostForm)
        {
            if (Physics.Raycast(ray, out hit, attackDistance))
            {
                GameObject hitGameObject = hit.collider.gameObject;
                string tag = hitGameObject.tag;

                if (tag == "Survivor")
                {
                    Survivor survivor = hitGameObject.GetComponent<Survivor>();
                    survivor.Die();
                }
            }
        }

        else
        {
            if (Physics.Raycast(ray, out hit, trapArmDistance))
            {
                GameObject hitGameObject = hit.collider.gameObject;
                string tag = hitGameObject.tag;

                if (tag == "Trap")
                {
                    Trap trap = hitGameObject.GetComponent<Trap>();
                    trap.armed = true;
                    trapArmSound.Play();
                }
            }
        }
    }

    private void HandleGrab()
    {

    }

    private void UpdateScreen()
    {
        // TO DO: Change what the color of the screen looks like. Not sure how to do this in Unity yet.
    }
}


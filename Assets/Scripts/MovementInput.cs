using UnityEngine;

public class MovementInput : MonoBehaviour
{

    [SerializeField]
    private CharacterController playerController;

    public float defaultSpeed;
    public float sprintSpeed;
    public float maxSprintEnergy;
    public float minSprintEnergyNeeded;
    public float crouchAndSneakingSpeed;
    public float sprintDischargeRate;
    public float sprintRechargeRate;
    public bool sprinting;
    public float sprintEnergy;
    public float gravity;

    public LayerMask groundMask;
    public Vector3 velocity;

    public AudioSource sprintingSound;
    public AudioSource walkingSound;
    public AudioSource maleGaspingSound;
    [SerializeField]
    private PauseUI pauseUI;

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        float speed;
        velocity.y -= gravity * Time.deltaTime;
        playerController.Move(velocity * Time.deltaTime);

        if (pauseUI.gamePaused)
        {
            return;
        }

        else if (playerController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }


        if (Keybinds.Get(Action.MoveForward))
        {
            if (Keybinds.GetKey(Action.Sprint))
            {
                if (sprintEnergy >= minSprintEnergyNeeded)
                {
                    sprinting = true;
                }
            }

            if (Keybinds.GetKey(Action.Sprint, true))
            {
                sprinting = false;
            }

            if (sprinting)
            {
                if (walkingSound.isPlaying)
                {
                    walkingSound.Stop();
                }

                sprintingSound.Play();
                speed = sprintSpeed;
                sprintEnergy -= sprintDischargeRate * Time.deltaTime;
                sprintEnergy = Mathf.Clamp(sprintEnergy, 0.0f, maxSprintEnergy);

                if (sprintEnergy <= 0)
                {
                    sprinting = false;
                    maleGaspingSound.Play();
                }
            }

            else
            {
                if (sprintingSound.isPlaying)
                {
                    sprintingSound.Stop();
                }

                walkingSound.Play();
                speed = defaultSpeed;
                sprintEnergy += sprintRechargeRate * Time.deltaTime;
                sprintEnergy = Mathf.Clamp(sprintEnergy, 0f, maxSprintEnergy);
            }

            if (Keybinds.GetKey(Action.Crouch) || Keybinds.GetKey(Action.Walk))
            {
                speed = crouchAndSneakingSpeed;
            }

            Vector3 move = transform.right * x + transform.forward * z;
            playerController.Move(move * speed * Time.deltaTime);
        }
    }
             
    private bool IsPressingMovementKey()
    {
        return Keybinds.Get(Action.MoveForward) || Keybinds.Get(Action.MoveLeft) || Keybinds.Get(Action.MoveRight) || Keybinds.Get(Action.MoveBack);
    }
}

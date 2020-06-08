using UnityEngine;

public class MovementInput : MonoBehaviour
{

    [SerializeField]
    private CharacterController playerController;

    [SerializeField]
    private float crouchAndSneakingSpeed;
    [SerializeField]
    private float gravity;
    [SerializeField]
    private float speed;
    public LayerMask groundMask;
    public Vector3 velocity;

    public AudioSource sprintingSound;
    public AudioSource walkingSound;

    [SerializeField]
    private PauseUI pauseUI;

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
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

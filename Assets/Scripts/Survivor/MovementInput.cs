using UnityEngine;
public class MovementInput : MonoBehaviour
{
    [SerializeField]
    private float crouchAndSneakingSpeed;
    [SerializeField]
    private float gravity;
    [SerializeField]
    private float speed;
    public LayerMask groundMask;
    public Vector3 velocity;

    public void Handle(CharacterController controller, bool gamePaused)
    {
        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (gamePaused)
        {
            return;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (Keybinds.GetKey(Action.Crouch) || Keybinds.GetKey(Action.Walk))
        {
            speed = crouchAndSneakingSpeed;
        }

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);
    }

    private bool IsPressingMovementKey()
    {
        return Keybinds.Get(Action.MoveForward) || Keybinds.Get(Action.MoveLeft) || Keybinds.Get(Action.MoveRight) || Keybinds.Get(Action.MoveBack);
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }
}

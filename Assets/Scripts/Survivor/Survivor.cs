using Darned;
using System.Runtime.Serialization;
using UnityEngine;

public class Survivor : MonoBehaviour
{
    [SerializeField]
    private string name = "player";
    [SerializeField]
    private Insanity insanity;
    [SerializeField]
    private Inventory inventory;
    [SerializeField]
    private Flashlight flashlight;
    [SerializeField]
    private Sprint sprint;

    [SerializeField]
    private PausedGameInput pausedGameInput;

    [SerializeField]
    private Texture crosshair;

    private Transform survivorBody;

    [SerializeField]
    private Camera survivorCamera;

    private CharacterController controller;

    [SerializeField]
    private float defaultSpeed;

    [SerializeField]
    private float sprintSpeed;

    [SerializeField]
    private float walkSpeed;

    [SerializeField]
    private float crouchSpeed;

    [SerializeField]
    private GameMessages survivorChat;


    private bool crouched;
    private bool walking;

    private Rect rect;

    [SerializeField]
    private float minimumX;
    [SerializeField]
    private float maximumX;

    public static int invertX;
    public static int invertY;
    public static float mouseSensitivity;


    [SerializeField]
    private float gravity;

    private Vector3 velocity;

    [SerializeField]
    private float grabDistance;

    private float xRotation;

    void Start()
    {
        survivorBody = GetComponent<Transform>();
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        rect = new Rect(Screen.width / 2, Screen.height / 2, 2, 2);

    }

    // We will try to handle as much input as possible here. If not that is okay I think.


    void LateUpdate()
    {
        if (pausedGameInput.gamePaused)
        {
            return;
        }

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        if (invertX == 1)
        {
            mouseX *= -1;
        }

        if (invertY == 1)
        {
            mouseY *= -1;
        }


        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minimumX, maximumX);
        transform.Rotate(Vector3.up * mouseX);
        survivorCamera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        flashlight.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        //survivorBody.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    void Update()
    {
        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (pausedGameInput.gamePaused)
        {
            return;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        float speed;

        if (sprint.sprinting)
        {
            speed = sprintSpeed;

        }

        else
        {
            speed = defaultSpeed;
        }
  
        if (Keybinds.GetKey(Action.SwitchFlashlight))
        {
            flashlight.Toggle();
        }


        else if (Keybinds.GetKey(Action.Crouch))
        {
            speed = crouchSpeed;
            crouched = true;

        }

        else if (Keybinds.GetKey(Action.Crouch, true))
        {
            speed = defaultSpeed;
            crouched = false;
        }

        else if (Keybinds.GetKey(Action.Walk))
        {
            speed = walkSpeed;
            walking = true;

        }

        else if (Keybinds.GetKey(Action.Walk, true))
        {
            speed = defaultSpeed;
            walking = false;
        }

        else if (Keybinds.GetKey(Action.Grab))
        {
            AttemptToGrabObject();

        }

        Vector3 secondmove = transform.right * x + transform.forward * z;
        controller.Move(secondmove * speed * Time.deltaTime);

    }


    private void AttemptToGrabObject()
    {
        RaycastHit hit;

        Ray ray = survivorCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, grabDistance))
        {
            var gameObject = hit.collider.gameObject;
            var name = gameObject.name;

            if (name.Contains("Key"))
            {
                Key key = gameObject.GetComponent<Key>();

                if (!inventory.HasKey(key))
                {
                    inventory.Add(key);
                    key.Grab();
                }
            }

            else if (name.Contains("Door"))
            {

                Door door = gameObject.GetComponent<Door>();

                if (door.Unlockable(inventory))
                {
                    door.Unlock();
                }
            }

            else if (name.Contains("Battery"))
            {
                Battery battery = gameObject.GetComponent<Battery>();

                if (flashlight.charge < battery.chargeNeededToGrab)
                {
                    battery.Grab();
                    flashlight.Recharge();
                }
                
                else
                {
                    Debug.Log("Your flashlight is already charged!");
                }

            }
        }
    }



    void OnGUI()
    {
        if (pausedGameInput.gamePaused)
        {
            return;
        }

        GUI.DrawTexture(rect, crosshair);
        inventory.Draw();

        if (walking)
        {

            // TO DO. Find a crouching walking icon and draw it here.
        }

        if (crouched)
        {

            // TO DO. Find a crouching icon and draw it here.
        }
    }

}

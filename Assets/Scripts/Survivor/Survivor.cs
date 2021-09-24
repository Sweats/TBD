using UnityEngine;
using Mirror;
using System.Collections.Generic;

public class Survivor : NetworkBehaviour
{
    public Insanity insanity;

    [SerializeField]
    private AudioSource deathSound;

    private CharacterController controller;

    private string name;

    private Inventory inventory;

    //NOTE: Server will send us the key type when we pick it up. Client side OnGUI will loop through this and draw depending on the key type
    private List<KeyType> keyTypes;

    private Animator animator;

    [Header("Speed Settigs.")]
    [SerializeField]
    private float defaultSpeed;

    [SerializeField]
    private float sprintSpeed;

    [SerializeField]
    private float walkSpeed;

    [SerializeField]
    private float crouchSpeed;

    private float currentSpeed;


    private bool crouched;

    private bool walking;

    private Rect crouchingAndWalkingIconPosition;

    [Header("Camera Settings")]
    [SerializeField]
    private float minimumX;

    [SerializeField]
    private float maximumX;

    [SerializeField]
    private Camera survivorCamera;

    private float xRotation;

    [Header("Physics Settings")]
    [SerializeField]
    private float gravity;

    private Vector3 velocity;

    [Header("Distance Settings")]
    [SerializeField]
    private float grabDistance;

    [SerializeField]
    private float trapDistance;

    [Header("Flashlight Settings")]
    [SerializeField]
    private float maxCharge;

    [SerializeField]
    private float minCharge;

    [SerializeField]
    private float dischargeRate;

    [SerializeField]
    private AudioSource flashlightToggleSound;

    [SerializeField]
    private Light flashlight;

    [SerializeField]
    [SyncVar]
    private float charge;

    [SerializeField]
    private bool toggled;

    [SyncVar]
    [SerializeField]
    private bool flashlightDead;

    [Header("Sprint Settings")]
    [SerializeField]
    private bool sprinting;

    [SerializeField]
    private float sprintMaxEnergy;

    [SerializeField]
    private float sprintMinEnergy;

    [SerializeField]
    private float sprintConsumptionRate;

    [SerializeField]
    private float sprintRegenerationRate;

    [SerializeField]
    private float energyNeededToSprint;

    [SerializeField]
    private float sprintEnergy;

    [SerializeField]
    private AudioSource outOfBreath;

    [Header("Misc Settings")]
    private bool isDead = false;

    private bool isMatchOver = false;

    [SerializeField]
    private bool grabbingAnObject;

    [SerializeField]
    private DarnedObject grabbedObject;

    private bool matchOver;

    // NOTE: Did someone join and then disconnect and die?
    private bool disconnected;

    [SerializeField]
    private Renderer survivorRenderer;

    [SerializeField]
    private Windows windows;

    [SerializeField]
    private GameObject hand;

    private bool escaped;

    private Vector3 mPrevPos = Vector3.zero;

    private Vector3 mPosDelta = Vector3.zero;

    private Character character;

    //NOTE: For drawing the keys in the inventory
    private Rect currentPosition;

    public override void OnStartLocalPlayer()
    {
        controller = GetComponent<CharacterController>();
        this.enabled = true;
        windows.enabled = true;
        survivorCamera.enabled = true;
        survivorCamera.GetComponent<AudioListener>().enabled = true;
        controller.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;

        currentPosition = new Rect
        {
            height = 30,
            width = 30
        };
    }

    [Client]
    private void LateUpdate()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (matchOver)
        {
            return;
        }

        if (windows.IsWindowOpen())
        {
            return;
        }

        float mouseX = Input.GetAxis("Mouse X") * Settings.MOUSE_SENSITIVITY;
        float mouseY = Input.GetAxis("Mouse Y") * Settings.MOUSE_SENSITIVITY;

        if (Settings.INVERT_X == 1)
        {
            mouseX *= -1;
        }

        if (Settings.INVERT_Y == 1)
        {
            mouseY *= -1;
        }

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minimumX, maximumX);
        transform.Rotate(Vector3.up * mouseX);
        survivorCamera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        flashlight.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    [Client]
    private void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        windows.Tick();

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (matchOver)
        {
            return;
        }

        if (windows.IsWindowOpen())
        {
            return;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 secondMove = transform.right * x + transform.forward * z;
        bool isMoving = transform.hasChanged;

        if (sprinting)
        {
            if (!transform.hasChanged)
            {
                sprinting = true;
                currentSpeed = defaultSpeed;
            }

            currentSpeed = sprintSpeed;
        }

        else
        {
            currentSpeed = defaultSpeed;
        }

        if (Keybinds.GetKey(Action.SwitchFlashlight))
        {
            NetworkClient.Send(new ServerClientGameToggledFlashlightMessage{toggled = !this.toggled});
        }

        else if (Keybinds.GetKey(Action.Crouch))
        {
            currentSpeed = crouchSpeed;
            crouched = true;
        }

        else if (Keybinds.GetKey(Action.Crouch, true))
        {
            currentSpeed = defaultSpeed;
            crouched = false;
        }

        else if (Keybinds.GetKey(Action.Sprint) && isMoving)
        {

        }

        else if (Keybinds.GetKey(Action.Sprint, true) && !isMoving)
        {

        }


        else if (Keybinds.GetKey(Action.Walk))
        {
            currentSpeed = walkSpeed;
            walking = true;

        }

        else if (Keybinds.GetKey(Action.Walk, true))
        {
            currentSpeed = defaultSpeed;
            walking = false;
        }

        else if (Keybinds.GetKey(Action.Grab))
        {
            Grab();
        }

        else if (Keybinds.GetKey(Action.Grab, true))
        {
            if (grabbingAnObject)
            {
                grabbedObject.CmdDrop();
            }
        }

        controller.Move(secondMove * currentSpeed * Time.deltaTime);
    }

    public GameObject Hand()
    {
        return hand;
    }

    public Character PlayerCharacter()
    {
        return character;
    }

    public bool Dead()
    {
        return isDead;
    }

    public void SetDead(bool value)
    {
        isDead = value;
    }

    [Server]
    public Insanity SurvivorInsanity()
    {
        return insanity;

    }

    public float TrapDistance()
    {
        return trapDistance;
    }

    [Server]
    public float FlashlightCharge()
    {
        return charge;
    }

    [Server]
    public void RechargeFlashlight()
    {
        charge = maxCharge;
    }

    [Server]
    public Inventory Items()
    {
        return inventory;
    }

    [Server]
    public void SetEscaped(bool value)
    {
        escaped = value;
    }

    [Server]
    public bool Escaped()
    {
        return escaped;
    }

    public string Name()
    {
        return name;
    }

    [Server]
    public float GrabDistance()
    {
        return grabDistance;
    }

    [Client]
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        var gameObject = hit.gameObject;

        if (gameObject.CompareTag(Tags.DOOR))
        {
            Door door = gameObject.GetComponent<Door>();
            Vector3 moveDirection = hit.moveDirection;
            door.CmdPlayerHitDoor(moveDirection);
        }
    }

    //[Client]
    //public void OnGui()
    //{
    //    if (windows.IsWindowOpen())
    //    {
    //        return;
    //    }

    //    int currentX = Screen.width - 20;
    //    int currentY = 20;

    //    // TODO: Client only needs to know the key type to draw it. Everything else is server sided.
    //    for (var i = 0; i < keyTypes.Count; i++)
    //    {
    //        if (i % 8 == 0)
    //        {
    //            // Go back to the top where we were when we started and then go left 50 because images will be 50 in pixels
    //            currentX -= 50;
    //            currentY = 20;
    //        }

    //        currentPosition.x = currentX;
    //        currentPosition.y = currentY;
    //        Texture texture = null;

    //        switch (keyTypes[i])
    //        {

    //        }
    //        GUI.DrawTexture(currentPosition, itemIcon);
    //        currentY += 35;

    //    }
    //}

    #region SERVER

    [Client]
    private void Grab()
    {
        RaycastHit hit;

        Ray ray = survivorCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, grabDistance))
        {
            var gameObject = hit.collider.gameObject;

            if (gameObject.CompareTag(Tags.KEY))
            {
                uint clickedkeyId = gameObject.GetComponent<KeyObject>().netIdentity.netId;
                NetworkClient.Send(new ServerClientGameClickedOnKeyMessage{requestedKeyId = clickedkeyId});
            }

            else if (gameObject.CompareTag(Tags.DOOR))
            {
                uint clickedDoorId = gameObject.GetComponent<Door>().netIdentity.netId;
                NetworkClient.Send(new ServerClientGameClickedOnDoorMessage{requestedDoorID = clickedDoorId});
            
            }

            else if (gameObject.CompareTag(Tags.BATTERY))
            {
                uint clickedBatteryId = gameObject.GetComponent<Battery>().netIdentity.netId;
                NetworkClient.Send(new ServerClientGameClickedOnBatteryMessage{requestedBatteryId = clickedBatteryId});
            }

            else if (gameObject.CompareTag(Tags.DARNED_OBJECT))
            {
                grabbedObject = gameObject.GetComponent<DarnedObject>();
                grabbedObject.CmdGrab();
                grabbingAnObject = true;
            }
        }
    }
}
#endregion

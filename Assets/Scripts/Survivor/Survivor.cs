using UnityEngine;
using System.Collections;

public class Survivor : MonoBehaviour
{

    public string survivorName = "player";
    public Insanity insanity;
    public Inventory inventory;
    public Flashlight flashlight;
    public int survivorID;

    [SerializeField]
    private AudioSource deathSound;

    [SerializeField]
    private Sprint sprint;

    [SerializeField]
    private Texture crosshair;

    [SerializeField]
    private Camera survivorCamera;

    //[SerializeField]
    //private PausedGameInput pausedGameInput;

    //[SerializeField]
    //private ConsoleUI consoleUI;

    private CharacterController controller;

    private Animator animator;

    [SerializeField]
    private float defaultSpeed;

    [SerializeField]
    private float sprintSpeed;

    [SerializeField]
    private float walkSpeed;

    [SerializeField]
    private float crouchSpeed;

    private bool crouched;
    private bool walking;
    private Rect crouchingAndWalkingIconPosition;


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

    [SerializeField]
    private float trapDistance;

    private float xRotation;
    private bool isPlayerStatsOpened;

    public bool matchOver;

    public bool isInEscapeRoom;

    public bool dead;

    private Vector3 moving;


    void Start()
    {
        //survivorBody = GetComponent<Transform>();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        moving = new Vector3();
        //EventManager.survivorClosedChatEvent.AddListener(OnSurvivorClosedChatEvent);
        //EventManager.survivorOpenedChatEvent.AddListener(OnSurvivorOpenedChatEvent);
        EventManager.survivorClosedPlayerStats.AddListener(OnSurvivorClosedPlayerStats);
        EventManager.survivorOpenedPlayerStats.AddListener(OnSurvivorOpenedPlayerStats);
        EventManager.survivorsEscapedStageEvent.AddListener(OnSurvivorsEscapedStageEvent);
        StartCoroutine(CheckForTraps());
    }

    void LateUpdate()
    {
        if (IsAnotherWindowOpen() || matchOver)
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
    }

    void Update()
    {
        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (IsAnotherWindowOpen() || matchOver)
        {
            return;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 secondmove = transform.right * x + transform.forward * z;
        bool isMoving = moving != secondmove;
        float speed;

        if (sprint.sprinting)
        {
            if (!isMoving)
            {
                sprint.sprinting = false;
                speed = defaultSpeed;
            }

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


        else if (Keybinds.GetKey(Action.Sprint))
        {
            if (isMoving)
            {
                sprint.sprinting = true;
            }

        }

        else if (Keybinds.GetKey(Action.Sprint, true))
        {
            sprint.sprinting = false;
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
            OnActionGrab();
        }

        else if (Keybinds.Get(Action.PlayerStats))
        {
            EventManager.survivorOpenedPlayerStats.Invoke();
        }

        controller.Move(secondmove * speed * Time.deltaTime);

    }

    private IEnumerator CheckForTraps()
    {
        while (true)
        {

            RaycastHit[] objectsHit = Physics.SphereCastAll(transform.position, trapDistance, transform.forward, trapDistance);

            for (var i = 0; i < objectsHit.Length; i++)
            {
                GameObject hitObject = objectsHit[i].collider.gameObject;
                string tagName = hitObject.tag;

                if (tagName == "Trap")
                {
                    Trap trap = hitObject.GetComponent<Trap>();

                    if (trap.armed)
                    {
                        EventManager.survivorTriggeredTrapEvent.Invoke(this, trap);
                    }
                }
            }

	    if (matchOver || dead)
	    {
		    break;
	    }

            yield return new WaitForSeconds(0.5f);

        }

	StopCoroutine(CheckForTraps());
    }


    private void OnActionGrab()
    {
        RaycastHit hit;

        Ray ray = survivorCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, grabDistance))
        {
            var gameObject = hit.collider.gameObject;
            var tagName = gameObject.tag;

            if (tagName == "Key")
            {
                KeyObject keyObject = gameObject.GetComponent<KeyObject>();
                OnClickedOnKey(keyObject);
            }

            else if (tagName == "Door")
            {
                Door door = gameObject.GetComponent<Door>();
                OnClickedOnDoor(door);
            }

            else if (tagName == "Battery")
            {
                Battery battery = gameObject.GetComponent<Battery>();
                OnSurvivorClickedOnBattery(battery);
            }

            else
            {
                // TO DO: add in non important objects in here.
            }
        }
    }

    void OnGUI()
    {
        if (PausedGameInput.GAME_PAUSED)
        {
            return;
        }

        if (!isPlayerStatsOpened && !ConsoleUI.OPENED)
        {
            inventory.Draw();
        }

        // TO DO: Optimize this!
        GUI.DrawTexture(new Rect(Screen.width / 2, Screen.height / 2, 2, 2), crosshair);
    }


    private void OnClickedOnKey(KeyObject foundKey)
    {
        Key[] keysInInventory = inventory.Keys();
        bool found = false;

        for (var i = 0; i < keysInInventory.Length; i++)
        {
            Key key = keysInInventory[i];
            int foundKeyMask = foundKey.key.mask;
            int currentInventoryKeyMask = key.mask;

            if (foundKeyMask == currentInventoryKeyMask)
            {
                found = true;
                EventManager.SurvivorAlreadyHaveKeyEvent.Invoke();

            }
        }

        if (!found)
        {
            Key key = foundKey.key;
            foundKey.Pickup();
            EventManager.survivorPickedUpKeyEvent.Invoke(this, key);
        }
    }

    private void OnClickedOnDoor(Door door)
    {
        Key[] keys = inventory.Keys();
        bool found = false;

        for (var i = 0; i < keys.Length; i++)
        {
            int unlockMask = keys[i].mask;

            if (door.unlockMask == unlockMask)
            {
                Key key = keys[i];
                found = true;
                EventManager.survivorUnlockDoorEvent.Invoke(this, key, door);
                break;
            }
        }

        if (!found)
        {
            EventManager.survivorFailedToUnlockDoorEvent.Invoke(door);
        }
    }


    //    private void OnClickOnObject(DarnedObject object)
    //    {
    //
    //    }
    //
    private void OnSurvivorClickedOnBattery(Battery battery)
    {
        if (flashlight.charge <= battery.chargeNeededToGrab)
        {
            EventManager.survivorPickedUpBatteryEvent.Invoke(this, battery);
        }

        else
        {
            EventManager.survivorFailedToPickUpBatteryEvent.Invoke();
        }
    }

    public void Die()
    {
        dead = true;
        insanity.Reset();
        deathSound.Play();
        EventManager.survivorDeathEvent.Invoke(this);
        StopCoroutine(CheckForTraps());
    }

    private void OnSurvivorsEscapedStageEvent()
    {
        matchOver = true;
    }

    private void OnSurvivorOpenedPlayerStats()
    {
        isPlayerStatsOpened = true;
    }


    private void OnSurvivorClosedPlayerStats()
    {
        isPlayerStatsOpened = false;
    }


    private bool IsAnotherWindowOpen()
    {
        return (PausedGameInput.GAME_PAUSED) || (ConsoleUI.OPENED) || (Chat.OPENED);
    }
}

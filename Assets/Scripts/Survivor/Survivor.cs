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
    private Music music;

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

    [SerializeField]
    private Renderer survivorRenderer;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        moving = new Vector3();
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

    void Update()
    {
        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
	//controller.attachedRigidbody.

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

        if (sprint.GetSprinting())
        {
            if (!isMoving)
            {
                sprint.SetSprinting(false);
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
            //To make it more 9heads-like, needs to check if player attemps to sprint while moving diagonally
            //while sprinting, diagonal movement is impossible
            //needs to check if player is backpaddling
            if (isMoving && (sprint.GetEnergy()>=sprint.GetEnergyNeededToSprint()))
            {
                sprint.SetSprinting(true);
                //immediately consume energy
                sprint.SetEnergy(-3.0f/sprint.GetTickRate());
            }
        }
        /* Not needed since Sprint triggers on tapping Shift key and works as long as you are moving
        else if (Keybinds.GetKey(Action.Sprint, true))
        {
            sprint.sprinting = false;
        }
        */


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

                if (hitObject.CompareTag(Tags.TRAP))
                {
                    Trap trap = hitObject.GetComponent<Trap>();

                    if (trap.Armed())
                    {
			    trap.Trigger();
                    }
                }
            }

	    if (matchOver || dead)
	    {
		    yield break;
	    }

            yield return new WaitForSeconds(0.5f);

        }
    }

    private void OnActionGrab()
    {
        RaycastHit hit;

        Ray ray = survivorCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, grabDistance))
        {
            var gameObject = hit.collider.gameObject;

            if (gameObject.CompareTag(Tags.KEY))
            {
                KeyObject keyObject = gameObject.GetComponent<KeyObject>();
                OnClickedOnKey(keyObject);
            }

            else if (gameObject.CompareTag(Tags.DOOR))
            {
                Door door = gameObject.GetComponent<Door>();
                OnClickedOnDoor(door);
            }

            else if (gameObject.CompareTag(Tags.BATTERY))
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
            int foundKeyMask = foundKey.Key().Mask();
            int currentInventoryKeyMask = key.Mask();

            if (foundKeyMask == currentInventoryKeyMask)
            {
                found = true;
                EventManager.SurvivorAlreadyHaveKeyEvent.Invoke();
		break;

            }
        }

        if (!found)
        {
            Key key = foundKey.Key();
	    inventory.Add(key);
            foundKey.Pickup();
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
                door.Unlock();
                break;
            }
        }

        if (!found)
        {
		door.PlayLockedSound();
        }
    }

    private void OnSurvivorClickedOnBattery(Battery battery)
    {
        if (flashlight.charge <= battery.chargeNeededToGrab)
        {
		flashlight.Recharge();
        }

        else
        {
            EventManager.survivorFailedToPickUpBatteryEvent.Invoke();
        }
    }

    public void Die()
    {
        dead = true;
	sprint.SetDead(dead);
        insanity.Reset();
        deathSound.Play();
        EventManager.survivorDeathEvent.Invoke(this);
    }

    // Called if the player is the Lurker.
    public void Hide()
    {
	    survivorRenderer.enabled = false;
	    flashlight.Hide();
    }

    // Called if the player is the Lurker.
    public void Show()
    {
	    survivorRenderer.enabled = true;
	    flashlight.Show();
    }

    private void OnSurvivorsEscapedStageEvent()
    {
        matchOver = true;
	sprint.SetMatchOver(true);
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

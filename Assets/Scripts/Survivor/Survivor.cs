﻿using UnityEngine;
using UnityEngine.Events;
public class Survivor : MonoBehaviour
{
    public string survivorName = "player";
    public Insanity insanity;
    public Inventory inventory;
    public Flashlight flashlight;
    public int survivorID;

    [SerializeField]
    private Sprint sprint;

    [SerializeField]
    private PausedGameInput pausedGameInput;

    [SerializeField]
    private Texture crosshair;

    [SerializeField]
    private Texture walkingTexture;
    
    [SerializeField]
    private Texture crouchingTexture;

    //private Transform survivorBody;

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

    #region EVENTS
    public SurvivorDeathEvent survivorDeathEvent;

    public SurvivorFailedToUnlockDoorEvent survivorFailedToUnlockDoorEvent;

    public SurvivorPickedUpKeyEvent survivorPickedUpKeyEvent;

    public SurvivorStartSprintingEvent survivorStartSprintingEvent;

    public SurvivorStopSprintingEvent survivorStopSprintingEvent;

    public SurvivorUnlockDoorEvent survivorUnlockDoorEvent;
    public SurvivorTriggeredTrapEvent survivorTriggeredTrapEvent;

    public SurvivorPickedUpBatteryEvent survivorPickedUpBatteryEvent;

    public SurvivorFailedToPickUpbBatteryEvent survivorFailedToPickUpBatteryEvent;

    public SurvivorAlreadyHaveKeyEvent survivorAlreadyHaveKeyEvent;

    public SurvivorOpenedChatEvent survivorOpenedChatEvent;

    public SurvivorClosedChat survivorClosedChatEvent;

    public SurvivorSendChatMessage survivorSendChatEvent;


    public SurvivorOpenedPlayerStats survivorOpenedPlayerStats;

    public SurvivorClosedPlayerStats survivorClosedPlayerStats;

    private bool isChatOpened;

    private bool isPlayerStatsOpened;

    #endregion

    void Start()
    {
        //survivorBody = GetComponent<Transform>();
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        crouchingAndWalkingIconPosition = new Rect(100, Screen.height - 50, crouchingTexture.width, crouchingTexture.height);

    }

    // We will try to handle as much input as possible here. If not that is okay I think.


    void LateUpdate()
    {
        if (pausedGameInput.gamePaused || isChatOpened)
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

        CheckForTraps();

        if (pausedGameInput.gamePaused || isChatOpened)
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

        else if (Keybinds.GetKey(Action.GuiAccept))
        {
            if (!pausedGameInput.gamePaused)
            {
                isChatOpened = true;
                survivorOpenedChatEvent.Invoke();
            }

            else if (isChatOpened)
            {
                survivorSendChatEvent.Invoke();
            }

        }

        else if (Keybinds.GetKey(Action.GUiReturn))
        {
            if (!pausedGameInput.gamePaused && isChatOpened)
            {
                survivorClosedChatEvent.Invoke();
                isChatOpened = false;
            }
        }


        else if (Keybinds.Get(Action.PlayerStats))
        {
            survivorOpenedPlayerStats.Invoke();
            isPlayerStatsOpened = true;
        }

        else if (Keybinds.GetKey(Action.PlayerStats, true))
        {
            survivorClosedPlayerStats.Invoke();
            isPlayerStatsOpened = false;
        }
        
        Vector3 secondmove = transform.right * x + transform.forward * z;
        controller.Move(secondmove * speed * Time.deltaTime);

    }
    private void CheckForTraps()
    {
        RaycastHit[] objectsHit = Physics.SphereCastAll(transform.position, trapDistance, transform.forward, trapDistance);

        for (var i = 0; i < objectsHit.Length; i++)
        {
            GameObject hitObject = objectsHit[i].collider.gameObject;
            string objectName = hitObject.name;

            if (objectName.Contains("Trap"))
            {
                Trap trap = hitObject.GetComponent<Trap>();

                if (trap.armed)
                {
                    survivorTriggeredTrapEvent.Invoke(this, trap);
                }
            }
        }
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
                KeyObject key = gameObject.GetComponent<KeyObject>();

                if (!inventory.HasKey(key.key))
                {
                    survivorPickedUpKeyEvent.Invoke(this, key);
                }

                else
                {
                    survivorAlreadyHaveKeyEvent.Invoke();
                }
            }

            else if (name.Contains("Door"))
            {
                Door door = gameObject.GetComponent<Door>();
                Key key;

                // TO DO: There may be a better way to do this.
                if (door.Unlockable(inventory, out key))
                {
                    survivorUnlockDoorEvent.Invoke(this, key, door);

                }

                else
                {
                    survivorFailedToUnlockDoorEvent.Invoke(door);
                }

            }

            else if (name.Contains("Battery"))
            {
                Battery battery = gameObject.GetComponent<Battery>();

                if (flashlight.charge <= battery.chargeNeededToGrab)
                {
                    survivorPickedUpBatteryEvent.Invoke(this, battery);
                }
                
                else
                {
                    survivorFailedToPickUpBatteryEvent.Invoke();
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

        if (!isPlayerStatsOpened)
        {
            inventory.Draw();
        }

        // TO DO: Optimize this!
        GUI.DrawTexture(new Rect(Screen.width / 2, Screen.height / 2, 2, 2), crosshair);

        if (walking)
        {
            GUI.DrawTexture(crouchingAndWalkingIconPosition, walkingTexture);
        }

        if (crouched)
        {

            GUI.DrawTexture(crouchingAndWalkingIconPosition, crouchingTexture);
        }
    }
}

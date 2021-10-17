using UnityEngine;
using Mirror;
using System.Collections.Generic;

public class Survivor : NetworkBehaviour
{
    public Insanity insanity;
    
    private Character survivorCharacter;

    [SerializeField]
    private AudioSource deathSound;

    private CharacterController controller;

    [SyncVar]
    private string survivorName;

    [SerializeField]
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
    [SyncVar(hook = nameof(OnClientServerGameUpdatedFlashlight))]
    private float charge;

    [SerializeField]
    [SyncVar(hook = nameof(OnClientServerGameToggledFlashlight))]
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
    private GameObject chadModel;

    [SerializeField]
    private Windows windows;

    [SerializeField]
    private GameObject hand;

    [SerializeField]
    private MeshCollider meshCollider;

    private bool escaped;

    private bool detectedByPhantom;

    private Character character;

    //NOTE: For drawing the keys in the inventory
    private Rect currentPosition;

    public override void OnStartServer()
    {
        base.OnStartServer();
        inventory.ServerInit();
    }

    public override void OnStartLocalPlayer()
    {
        controller = GetComponent<CharacterController>();
        this.enabled = true;
        survivorCamera.enabled = true;
        survivorCamera.GetComponent<AudioListener>().enabled = true;
        controller.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;

        currentPosition = new Rect
        {
            height = 30,
            width = 30
        };

        EventManager.clientServerGameSurvivorsDeadEvent.AddListener(OnAllSurvivorsDead);
        EventManager.clientServerGameSurvivorsEscapedEvent.AddListener(OnAllSurvivorsEscaped);
        windows.LocalPlayerStart();
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
            NetworkClient.Send(new ServerClientGameToggledFlashlightMessage { });
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

    [Server]
    public Character SurvivorCharacter()
    {
        return survivorCharacter;

    }

    [Server]
    public void SetCharacter(Character character)
    {
        survivorCharacter = character;
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
    public void ServerKill()
    {
        isDead = true;

    }

    [Server]
    public Insanity SurvivorInsanity()
    {
        return insanity;

    }

    [Server]
    public void ServerSetName(string name)
    {
        this.survivorName = name;
    }

    [Server]
    public bool ServerDetectedByPhantom()
    {
        return detectedByPhantom;

    }

    [Server]
    public void ServerSetDetectedByPhantom(bool value)
    {
        this.detectedByPhantom = value;
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
    public float ServerFlashlightDischargeRate()
    {
        return dischargeRate;
    }

    [Server]
    public void ServerToggleFlashlight()
    {
        toggled = !toggled;
    }

    public bool ServerFlashlightToggled()
    {
        return toggled;
    }

    [Server]
    public void ServerSetFlashlightCharge(float value)
    {
        this.charge = value;
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
    public void ServerSetEscaped(bool value)
    {
        escaped = value;
    }

    [Server]
    public bool ServerEscaped()
    {
        return escaped;
    }

    public string Name()
    {
        return survivorName;
    }

    [Client]
    public void ClientHide()
    {
        survivorRenderer.enabled = false;
        meshCollider.enabled = false;
        flashlight.enabled = false;
        chadModel.SetActive(false);
    }

    [Server]
    public float GrabDistance()
    {
        return grabDistance;
    }

    [Client]
    public void OnClientServerGameToggledFlashlight(bool oldValue, bool newValue)
    {
        flashlight.enabled = newValue;
        flashlightToggleSound.Play();
    }

    [Client]
    public void ClientPlayDeathSound()
    {
        deathSound.Play();
    }

    [Client]
    public void OnClientServerGameUpdatedFlashlight(float oldValue, float newValue)
    {
        flashlight.intensity = newValue;
    }


    [Client]
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        var hitGameObject = hit.gameObject;

        if (hitGameObject.CompareTag(Tags.DOOR))
        {
            Door door = hitGameObject.GetComponent<Door>();
            uint id = door.netIdentity.netId;
            Vector3 hitMoveDirection = hit.moveDirection;
            NetworkClient.Send(new ServerClientGameDoorBumpedIntoMessage { requestedDoorID = id, moveDirection = hitMoveDirection });
        }
    }

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
                NetworkClient.Send(new ServerClientGameClickedOnKeyMessage { requestedKeyId = clickedkeyId });
            }

            else if (gameObject.CompareTag(Tags.DOOR))
            {
                uint clickedDoorId = gameObject.GetComponent<Door>().netIdentity.netId;
                NetworkClient.Send(new ServerClientGameClickedOnDoorMessage { requestedDoorID = clickedDoorId });

            }

            else if (gameObject.CompareTag(Tags.BATTERY))
            {
                uint clickedBatteryId = gameObject.GetComponent<Battery>().netIdentity.netId;
                NetworkClient.Send(new ServerClientGameClickedOnBatteryMessage { requestedBatteryId = clickedBatteryId });
            }

            else if (gameObject.CompareTag(Tags.DARNED_OBJECT))
            {
                grabbedObject = gameObject.GetComponent<DarnedObject>();
                grabbedObject.CmdGrab();
                grabbingAnObject = true;
            }
        }
    }

    [Client]
    private void OnAllSurvivorsDead()
    {
        matchOver = true;

    }

    [Client]
    private void OnAllSurvivorsEscaped()
    {
        matchOver = true;
    }

    [Client]
    public void ClientShow()
    {
        this.survivorRenderer.enabled = true;
        this.meshCollider.enabled = true;
        // NOTE: Don't want to show it here just in case if the survivors flashlight is actually off. We use the toggle syncvar instead.
        this.flashlight.enabled = toggled;
        this.chadModel.SetActive(true);
    }
}

#endregion

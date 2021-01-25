using UnityEngine;
using System.Collections;
using Mirror;

public class Survivor : NetworkBehaviour
{
    [SerializeField]
    [SyncVar]
    private string playerName = "player";

    [SyncVar]
    public bool matchOver;

    [SerializeField]
    [SyncVar]
    private bool isInEscapeRoom;

    [SyncVar]
    [SerializeField]
    readonly private SyncList<Key> keys = new SyncList<Key>();

    [SyncVar]
    public bool dead;

    public Insanity insanity;

    [SerializeField]
    private AudioSource deathSound;

    [SerializeField]
    private Sprint sprint;

    private CharacterController controller;

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

    [SerializeField]
    private float doorPushStrength;

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
    [SyncVar(hook = nameof(SurvivorFlashlightChargeChanged))]
    private float charge;

    [SyncVar(hook = nameof(SurvivorToggledFlashlight))]
    [SerializeField]
    private bool toggled;

    [SyncVar]
    [SerializeField]
    private bool flashlightDead;

    private Light flashlightSource;

    private IEnumerator flashlightRoutine;

    private Vector3 moving;

    [SerializeField]
    private bool grabbingAnObject;

    [SerializeField]
    private DarnedObject grabbedObject;

    // NOTE: Did someone join and then disconnect and die?
    private bool disconnected;

    [SerializeField]
    private Renderer survivorRenderer;

    [SerializeField]
    private Windows windows;

    [SerializeField]
    private GameObject hand;

    private Vector3 mPrevPos = Vector3.zero;

    private Vector3 mPosDelta = Vector3.zero;

    private void Start()
    {
        controller = GetComponent<CharacterController>();

        if (!isLocalPlayer)
        {
            survivorCamera.enabled = false;
            survivorCamera.GetComponent<AudioListener>().enabled = false;
            controller.enabled = false;
            windows.enabled = false;
            this.insanity.enabled = false;
            return;
        }
    }

    public override void OnStartClient()
    {
        if (!isLocalPlayer)
        {
            EventManager.playerConnectedEvent.Invoke(playerName);
        }

    }

    public override void OnStartLocalPlayer()
    {
        EventManager.playerSentChatMessageEvent.AddListener(CmdOnPlayerSentMessage);
        EventManager.playerClientChangedNameEvent.AddListener(CmdOnPlayerChangedProfileNameEvent);
        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine(TrapDetectionRoutine());
        flashlightRoutine = FlashlightRoutine();
        StartCoroutine(flashlightRoutine);
        playerName = Settings.PROFILE_NAME;
        CmdSetName(playerName);
    }

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
        //hand.transform.RotateAround(survivorCamera.transform.position, Vector3.up, 20 * Time.deltaTime);
    }

    private void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

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
        Vector3 secondmove = transform.right * x + transform.forward * z;
        bool isMoving = moving != secondmove;

        if (sprint.GetSprinting())
        {
            if (!isMoving)
            {
                sprint.SetSprinting(false);
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
            CmdToggleFlashlight();
        }

        else if (Keybinds.GetKey(Action.Crouch))
        {
            currentSpeed = crouchSpeed;
            crouched = true;
        }


        else if (Keybinds.GetKey(Action.Sprint))
        {
            //To make it more 9heads-like, needs to check if player attemps to sprint while moving diagonally
            //while sprinting, diagonal movement is impossible
            //needs to check if player is backpaddling
            if (isMoving && (sprint.GetEnergy() >= sprint.GetEnergyNeededToSprint()))
            {
                sprint.SetSprinting(true);
                //immediately consume energy
                sprint.SetEnergy(-3.0f / sprint.GetTickRate());
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
            currentSpeed = defaultSpeed;
            crouched = false;
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
            OnActionGrab();
        }

        else if (Keybinds.GetKey(Action.Grab, true))
        {
            if (grabbingAnObject)
            {
                grabbedObject.CmdDrop();
            }
        }


        controller.Move(secondmove * currentSpeed * Time.deltaTime);

    }

    [Client]
    private IEnumerator TrapDetectionRoutine()
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
                        trap.CmdTriggerTrap();
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

    [Client]
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
                keyObject.CmdPlayerClickedOnKey();
            }

            else if (gameObject.CompareTag(Tags.DOOR))
            {
                Door door = gameObject.GetComponent<Door>();

                if (!door.Unlocked())
                {
                    door.CmdPlayerClickedOnLockedDoor();
                    return;
                }
            }

            else if (gameObject.CompareTag(Tags.BATTERY))
            {
                Battery battery = gameObject.GetComponent<Battery>();
                battery.CmdPlayerClickedOnBattery();
            }

            else if (gameObject.CompareTag(Tags.DARNED_OBJECT))
            {
                grabbedObject = gameObject.GetComponent<DarnedObject>();
                grabbedObject.CmdGrab();
                grabbingAnObject = true;
            }
        }
    }


    [Command]
    public void Die()
    {
        RpcDie();
    }

    [ClientRpc]
    private void RpcDie()
    {
        dead = true;
        sprint.SetDead(dead);
        string playerName = this.playerName;
        EventManager.survivorDeathEvent.Invoke(playerName);

        if (!isLocalPlayer)
        {
            deathSound.Play();
        }
    }

    [Command]
    private void CmdOnPlayerSentMessage(string playerName, string message)
    {
        RpcPlayerRecievedChatMessage(playerName, message);
    }

    [Command]
    private void CmdOnPlayerChangedProfileNameEvent(string oldName, string newName)
    {
        playerName = newName;
        RpcPlayerChangedProfileName(oldName, newName);
    }

    [Command]
    private void CmdSetName(string newPlayerName)
    {
        playerName = newPlayerName;
    }

    [ClientRpc]
    private void RpcPlayerRecievedChatMessage(string playerName, string message)
    {
        EventManager.playerRecievedChatMessageEvent.Invoke(playerName, message);
    }

    [ClientRpc]
    private void RpcPlayerChangedProfileName(string oldName, string newName)
    {
        EventManager.playerChangedNameEvent.Invoke(oldName, newName);
    }


    // TODO: DO we need the command and targets for these functions?
    // NOTE: Called if the player is the Lurker.
    [Command]
    public void Hide()
    {
        TargetHide();
    }

    [TargetRpc]
    private void TargetHide()
    {
        survivorRenderer.enabled = false;
    }

    // NOTE: Called if the player is the Lurker.
    [Command]
    public void Show()
    {
        TargetShow();
    }

    [TargetRpc]
    private void TargetShow()
    {
        survivorRenderer.enabled = true;
    }

    public string Name()
    {
        return playerName;
    }

    public bool Dead()
    {
        return dead;

    }

    public bool Disconnected()
    {
        return disconnected;

    }

    public bool Escaped()
    {
        return isInEscapeRoom;
    }

    public void SetEscaped(bool escaped)
    {
        isInEscapeRoom = escaped;
    }

    public SyncList<Key> Items()
    {
        return keys;
    }

    [Client]
    private IEnumerator FlashlightRoutine()
    {
        while (true)
        {
            if (toggled && !flashlightDead)
            {
                CmdUpdateFlashlight();
            }

            else if (matchOver || dead)
            {
                yield break;
            }

            yield return new WaitForSeconds(1);
        }
    }

    [Command]
    private void CmdUpdateFlashlight()
    {
        charge -= dischargeRate;

        if (charge <= minCharge)
        {
            charge = minCharge;
            flashlightDead = true;
        }
    }

    [Command]
    public void CmdToggleFlashlight()
    {
        toggled = !toggled;
    }

    [Client]
    private void SurvivorToggledFlashlight(bool oldToggle, bool newToggle)
    {
        flashlightToggleSound.Play();
        flashlight.enabled = newToggle;
    }

    [Client]
    private void SurvivorFlashlightChargeChanged(float oldValue, float newValue)
    {
        flashlight.intensity = newValue;
    }

    public float FlashlightCharge()
    {
        return charge;
    }

    public bool FlashlightToggled()
    {
        return toggled;
    }

    public bool FlashlightDead()
    {
        return flashlightDead;
    }

    public GameObject Hand()
    {
        return hand;
    }

    public float GrabDistance()
    {
        return grabDistance;
    }

    public float DoorPushStrength()
    {
        return doorPushStrength;
    }

    [Server]
    public void ServerRechargeFlashlight()
    {
        charge = maxCharge;
        flashlightSource.intensity = maxCharge;
        flashlightDead = false;
    }

    [Client]
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        var gameObject = hit.gameObject;

        if (gameObject.CompareTag(Tags.DOOR))
        {
            Door door = gameObject.GetComponent<Door>();
            Vector3 moveDirection = hit.moveDirection;
            door.CmdPlayerHitDoor(moveDirection, doorPushStrength);
        }
    }
}

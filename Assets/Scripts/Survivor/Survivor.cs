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

    private IEnumerator flashlightRoutine;

    [Header("Sprint Settings")]
    [SerializeField]
    [SyncVar]
    private bool sprinting;

    [SerializeField]
    [SyncVar]
    private float sprintMaxEnergy;

    [SerializeField]
    [SyncVar]
    private float sprintMinEnergy;

    [SerializeField]
    [SyncVar]
    private float sprintConsumptionRate;

    [SerializeField]
    [SyncVar]
    private float sprintRegenerationRate;

    [SerializeField]
    private float energyNeededToSprint;

    [SerializeField]
    [SyncVar]
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

    public override void OnStartClient()
    {
        if (!isLocalPlayer)
        {
            EventManager.playerConnectedEvent.Invoke(playerName);
        }

    }

    public override void OnStartLocalPlayer()
    {
        this.enabled = true;
        windows.enabled = true;
        controller = GetComponent<CharacterController>();
        survivorCamera.enabled = true;
        survivorCamera.GetComponent<AudioListener>().enabled = true;
        controller.enabled = true;
        this.insanity.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        playerName = Settings.PROFILE_NAME;
        CmdSetName(playerName);
    }

    public override void OnStartServer()
    {
        flashlightRoutine = FlashlightRoutine();
        StartCoroutine(flashlightRoutine);
        StartCoroutine(TrapDetectionRoutine());
        StartCoroutine(ServerCalcStamina());
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
    }

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
            CmdToggleFlashlight();
        }

        else if (Keybinds.GetKey(Action.Crouch))
        {
            currentSpeed = crouchSpeed;
            crouched = true;
        }


        else if (Keybinds.GetKey(Action.Sprint) && isMoving)
        {
            CmdStartSprinting(isMoving);
        }

        else if (Keybinds.GetKey(Action.Sprint, true) && !isMoving)
        {
            CmdStopSprinting();
        }

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
            CmdActionGrab();
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



    [ClientRpc]
    private void RpcDie(string playerName)
    {
        EventManager.survivorDeathEvent.Invoke(playerName);

        if (!isLocalPlayer)
        {
            deathSound.Play();
        }
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

    [TargetRpc]
    private void TargetHide()
    {
        survivorRenderer.enabled = false;
    }

    // NOTE: Called if the player is the Lurker.

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

    public float GetEnergyNeededToSprint()
    {
        return energyNeededToSprint;
    }

    public GameObject Hand()
    {
        return hand;
    }

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

    #region SERVER

    [Server]
    private IEnumerator FlashlightRoutine()
    {
        while (true)
        {
            if (toggled && !flashlightDead)
            {
                ServerUpdateFlashlight();
            }

            else if (matchOver || dead)
            {
                Debug.Log("Flashlight routine stopped.");
                yield break;
            }

            yield return new WaitForSeconds(1);
        }
    }

    [Server]
    private void ServerUpdateFlashlight()
    {
        charge -= dischargeRate;

        if (charge <= minCharge)
        {
            charge = minCharge;
            flashlightDead = true;
        }
    }

    [Server]
    private IEnumerator ServerCalcStamina()
    {
        while (true)
        {
            //don't need to do any calcs if Match is over or Char is dead, so check first.
            if (isMatchOver || isDead)
            {
                yield break;
            }

            if (sprinting)
            {
                sprintEnergy -= sprintConsumptionRate;

                if (sprintEnergy > sprintMinEnergy)
                {
                    sprintEnergy = sprintMinEnergy;
                }

                else
                {
                    outOfBreath.Play();
                    sprinting = false;
                }

                yield return new WaitForSeconds(sprintConsumptionRate);
            }

            else
            {
                if (sprintEnergy < sprintMaxEnergy)
                {
                    sprintEnergy += sprintRegenerationRate;
                }

                else if (sprintEnergy >= sprintMaxEnergy)
                {
                    sprintEnergy = sprintMaxEnergy;
                }
            }

            yield return new WaitForSeconds(sprintRegenerationRate);
        }
    }

    [Server]
    public float GetEnergy()
    {
        return sprintEnergy;
    }

    [Server]
    public float FlashlightCharge()
    {
        return charge;
    }

    [Server]
    public bool FlashlightToggled()
    {
        return toggled;
    }

    [Server]
    public bool FlashlightDead()
    {
        return flashlightDead;
    }

    [Server]
    public void ServerRechargeFlashlight()
    {
        charge = maxCharge;
        flashlight.intensity = maxCharge;
        flashlightDead = false;
    }

    [Server]
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

    [Command]
    private void CmdActionGrab()
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
    public void CmdDie()
    {
        dead = true;
        sprinting = false;
        RpcDie(this.playerName);
    }

    [Command]
    private void CmdStopSprinting()
    {
        sprinting = false;
    }

    [Command]
    public void Hide()
    {
        TargetHide();
    }

    [Command]
    public void Show()
    {
        TargetShow();
    }

    [Command]
    private void CmdSetName(string newPlayerName)
    {
        playerName = newPlayerName;
    }

    [Command]
    private void CmdOnPlayerChangedProfileNameEvent(string oldName, string newName)
    {
        playerName = newName;
        RpcPlayerChangedProfileName(oldName, newName);
    }

    [Command]
    private void CmdOnPlayerSentMessage(string playerName, string message)
    {
        RpcPlayerRecievedChatMessage(playerName, message);
    }

    [Command]
    private void CmdStartSprinting(bool isMoving)
    {
        if (isMoving && (sprintEnergy >= energyNeededToSprint))
        {
            sprinting = true;
            sprintEnergy -= 3f;
            //immediately consume energy
            //sprintEnergy = -3.0f / sprintTickRate;
        }
    }
}
#endregion

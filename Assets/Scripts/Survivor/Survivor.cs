using UnityEngine;
using System.Collections;
using Mirror;

public class Survivor : NetworkBehaviour
{
    [SerializeField]
    private string playerName = "player";

    public Insanity insanity;

    public Inventory inventory;

    public Flashlight flashlight;

    [SerializeField]
    private AudioSource deathSound;

    [SerializeField]
    private Sprint sprint;

    [SerializeField]
    private Camera survivorCamera;

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

    public bool matchOver;

    public bool isInEscapeRoom;

    public bool dead;

    private Vector3 moving;

    // NOTE: Did someone join and then disconnect and die?
    private bool disconnected;

    [SerializeField]
    private Renderer survivorRenderer;

    [SerializeField]
    private Windows windows;


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
            this.inventory.enabled = false;
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
        EventManager.survivorsEscapedStageEvent.AddListener(OnSurvivorsEscapedStageEvent);
        EventManager.playerSentChatMessageEvent.AddListener(CmdOnPlayerSentMessage);
        EventManager.playerClientChangedNameEvent.AddListener(CmdOnPlayerChangedProfileNameEvent);
        StartCoroutine(TrapDetectionRoutine());
        Cursor.lockState = CursorLockMode.Locked;
        playerName = Settings.PROFILE_NAME;
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

    void Update()
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
            CmdFlashlightToggle();
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


        controller.Move(secondmove * speed * Time.deltaTime);

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

    // This routine is only started when a lurker has spawned in the stage. An event will be responsible for this.

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
                CmdPlayerClickedOnKey(keyObject.netIdentity);

            }

            else if (gameObject.CompareTag(Tags.DOOR))
            {
                Door door = gameObject.GetComponent<Door>();
                CmdPlayerClickedOnDoor(door.netIdentity);
            }

            else if (gameObject.CompareTag(Tags.BATTERY))
            {
                Battery battery = gameObject.GetComponent<Battery>();
                CmdPlayerClickedOnBattery(battery.netIdentity);
            }

            else
            {
                // TO DO: add in non important objects in here.
            }
        }
    }

    [Command]
    private void CmdFlashlightToggle()
    {
        RpcFlashlightToggle();
    }

    [Command]
    private void CmdPlayerClickedOnKey(NetworkIdentity keyObjectNetworkIdentity)
    {
        KeyObject foundKey = keyObjectNetworkIdentity.gameObject.GetComponent<KeyObject>();
        int foundKeyMask = foundKey.Key().Mask();
        Key[] keys = inventory.Keys();
        bool found = false;

        for (var i = 0; i < keys.Length; i++)
        {
            Key keyInInventory = keys[i];

            if (foundKeyMask == keyInInventory.Mask())
            {
                found = true;
                break;
            }
        }

        if (found)
        {
            TargetPlayerAlreadyHasKey();
            return;
        }

        string foundKeyName = foundKey.Key().Name();
        string playerwhoPickedUpKey = playerName;
        inventory.Add(foundKey.Key());
        foundKey.Pickup();

        if (isLocalPlayer)
        {
            playerwhoPickedUpKey = "You";
            foundKey.PlayPickupSound();
        }

        PlayerPickedUpKeyMessage message = new PlayerPickedUpKeyMessage
        {
            playerName = playerwhoPickedUpKey,
            keyName = foundKeyName
        };

        RpcPlayerPickedUpKey(message);

    }

    [TargetRpc]
    private void TargetPlayerAlreadyHasKey()
    {
        EventManager.survivorAlreadyHasKeyEvent.Invoke();
    }

    [ClientRpc]
    private void RpcPlayerPickedUpKey(PlayerPickedUpKeyMessage serverMessage)
    {
        string playerName = serverMessage.playerName;
        string keyName = serverMessage.keyName;
        EventManager.survivorPickedUpKeyEvent.Invoke(playerName, keyName);
    }

    [ClientRpc]
    private void RpcFlashlightToggle()
    {
        flashlight.Toggle();

        if (isLocalPlayer)
        {
            flashlight.PlayToggleSound();
        }
    }

    [Command]
    private void CmdPlayerClickedOnBattery(NetworkIdentity batteryNetworkIdentity)
    {
        Battery battery = batteryNetworkIdentity.gameObject.GetComponent<Battery>();

        if (flashlight.Charge() <= battery.chargeNeededToGrab)
        {
            RpcPlayerPickedUpBattery(batteryNetworkIdentity);
        }

        else
        {
            TargetFailedToPickUpBattery();
        }
    }


    [ClientRpc]
    private void RpcPlayerPickedUpBattery(NetworkIdentity batteryNetworkIdentity)
    {
        Battery battery = batteryNetworkIdentity.gameObject.GetComponent<Battery>();
        battery.Pickup();
        flashlight.Recharge();
        Debug.Log("A player has picked up a battery!");
    }


    [TargetRpc]
    private void TargetFailedToPickUpBattery()
    {
        EventManager.survivorFailedToPickUpBatteryEvent.Invoke();
        
    }

    [Command]
    private void CmdPlayerClickedOnDoor(NetworkIdentity doorNetworkIdentity)
    {
        Door door = doorNetworkIdentity.gameObject.GetComponent<Door>();
        Key[] keys = inventory.Keys();
        bool found = false;
        int foundKeyIndex = 0;

        for (var i = 0; i < keys.Length; i++)
        {
            int keyMask = keys[i].Mask();

            if (door.UnlockMask() == keyMask)
            {
                found = true;
                foundKeyIndex = i;
                break;
            }
        }

        if (found)
        {
            string foundKeyName = keys[foundKeyIndex].Name();

            PlayerUnlockedDoorMessage unlockDoorMessage = new PlayerUnlockedDoorMessage
            {
                playerName = this.playerName,
                doorName = door.Name(),
                keyName = foundKeyName
            };

            RpcUnlockDoorMessage(unlockDoorMessage);
            RpcUnlockDoor(doorNetworkIdentity);
            return;
        }

        RpcFailedToUnlockDoor(doorNetworkIdentity);
    }

    [ClientRpc]
    private void RpcUnlockDoorMessage(PlayerUnlockedDoorMessage serverMessage)
    {
        string playerName = serverMessage.playerName;
        string doorName = serverMessage.doorName;
        string keyName = serverMessage.keyName;

        if (isLocalPlayer)
        {
            playerName = "You";
        }

        EventManager.survivorUnlockDoorEvent.Invoke(playerName, doorName, keyName);
    }

    [ClientRpc]
    private void RpcUnlockDoor(NetworkIdentity doorNetworkIdentity)
    {
        Door door = doorNetworkIdentity.gameObject.GetComponent<Door>();
        door.Unlock();

        if (!isLocalPlayer)
        {
            return;
        }

        door.PlayUnlockedSound();
    }

    [ClientRpc]
    private void RpcFailedToUnlockDoor(NetworkIdentity doorNetworkIdentity)
    {
        if (!isLocalPlayer)
        {
            return;
        }

        Door door = doorNetworkIdentity.gameObject.GetComponent<Door>();
        door.PlayLockedSound();
    }

    public void Die()
    {
        RpcDie();
    }

    [ClientRpc]
    private void RpcHitTrap(NetworkIdentity trapIdentity)
    {
        Trap trap = trapIdentity.gameObject.GetComponent<Trap>();
        insanity.Increment(trap.HitAmount());
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
            return;
        }


        deathSound.Play();
    }

    [Command]
    private void CmdOnPlayerSentMessage(string playerName, string message)
    {
        RpcPlayerRecievedChatMessage(playerName, message);
    }

    [Command]
    private void CmdOnPlayerChangedProfileNameEvent(string oldName, string newName)
    {
        RpcPlayerChangedProfileName(oldName, newName);
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

    public bool Dead()
    {
        return dead;

    }

    public bool Disconnected()
    {
        return disconnected;

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

    public string Name()
    {
        return playerName;
    }



}

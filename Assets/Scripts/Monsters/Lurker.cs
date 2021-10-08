using UnityEngine;
using System.Collections;
using Mirror;

public class Lurker : NetworkBehaviour
{
    [SyncVar]
    private string name;
    private CharacterController lurkerController;

    [SerializeField]
    private float physicalFormSpeed;

    [SerializeField]
    private float ghostFormSpeed;

    [SerializeField]
    private float maxEnergy = 100;

    [SerializeField]
    private float minEnergy = 0;

    [SerializeField]
    private float energy;

    [SerializeField]
    private float energyRegenerationRate;

    [SerializeField]
    private float energyConsumptionRate;

    [SerializeField]
    private float attackDistance;

    [SerializeField]
    private float trapArmDistance;

    [SerializeField]
    private float speed;

    [SyncVar(hook = nameof(OnLurkerFormChanged))]
    [SerializeField]
    private bool ghostForm = true;

    private bool isReadyToGoIntoPhysicalForm;

    [SerializeField]
    private bool canAttack = false;

    [SerializeField]
    private int attackCoolDownInSeconds;

    private bool hostStartedGame;

    private bool matchOver = false;

    [SerializeField]
    private int secondsBetweenGlows;

    [SerializeField]
    private int trapGlowTime;

    [SerializeField]
    private AudioSource lurkerChangeFormSound;

    [SerializeField]
    private AudioSource lurkerGhostFormMusic;

    [SerializeField]
    private AudioSource lurkerPhysicalFormMusic;

    [SerializeField]
    private AudioSource footstepSound;

    [SerializeField]
    private AudioSource trapArmSound;

    [SerializeField]
    private AudioSource attackSound;

    [SerializeField]
    private AudioSource lurkerReadyToTransformSound;

    [SerializeField]
    private Camera lurkerCamera;

    [SerializeField]
    private Windows windows;


    // Get the list of objects on the stage and cache them for the match.
    private GameObject[] doors;

    private GameObject[] traps;

    private GameObject[] keys;

    private GameObject[] batteries;

    private float xRotation;

    [SerializeField]
    private float minimumX;

    [SerializeField]
    private float maximumX;

    private Vector3 velocity;

    [SerializeField]
    private float gravity;

    [SerializeField]
    private Texture crosshair;

    [SerializeField]
    private int trapPercentage;

    [SerializeField]
    private Renderer lurkerRenderer;

    [SerializeField]
    private MeshCollider meshCollider;

    public override void OnStartLocalPlayer()
    {
        this.speed = ghostFormSpeed;
        this.enabled = true;
        lurkerController = GetComponent<CharacterController>();
        lurkerController.enabled = true;
        lurkerCamera.enabled = true;
        lurkerCamera.GetComponent<AudioListener>().enabled = true;
        windows.enabled = true;
        windows.LocalPlayerStart();
        doors = GameObject.FindGameObjectsWithTag(Tags.DOOR);
        keys = GameObject.FindGameObjectsWithTag(Tags.KEY);
        batteries = GameObject.FindGameObjectsWithTag(Tags.BATTERY);
        traps = GameObject.FindGameObjectsWithTag(Tags.TRAP);
        HideImportantObjects();
        Cursor.lockState = CursorLockMode.Locked;
        EventManager.clientServerGameLurkerReadyToGoIntoPhysicalFormEvent.AddListener(OnReadyToGoIntoPhysicalFormEvent);
        EventManager.clientServerGameLurkerArmableTrapsEvent.AddListener(OnArmableTrapsEvent);
        EventManager.clientServerGameLurkerTrapArmedEvent.AddListener(OnTrapArmed);
        NetworkClient.Send(new ServerClientGameLurkerJoinedMessage { });
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
        lurkerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }

    [Client]
    private void Update()
    {
        velocity.y -= gravity * Time.deltaTime;
        lurkerController.Move(velocity * Time.deltaTime);

        if (lurkerController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        windows.Tick();

        if (windows.IsWindowOpen())
        {
            return;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 secondmove = transform.right * x + transform.forward * z;
        bool isMoving = transform.hasChanged;

        if (Keybinds.GetKey(Action.Transform))
        {
            NetworkClient.Send(new ServerClientGameLurkerRequestToChangeFormMessage());
        }

        else if (Keybinds.GetKey(Action.Attack))
        {
            AttackOrGrab();
        }

        lurkerController.Move(secondmove * speed * Time.deltaTime);
    }

    [Client]
    private void HideImportantObjects()
    {
        if (doors != null)
        {
            for (var i = 0; i < doors.Length; i++)
            {
                Door door = doors[i].GetComponent<Door>();
                door.Hide();
            }
        }

        if (keys != null)
        {
            for (var i = 0; i < keys.Length; i++)
            {
                KeyObject key = keys[i].GetComponent<KeyObject>();
                key.Hide();
            }
        }

        if (batteries != null)
        {
            for (var i = 0; i < batteries.Length; i++)
            {
                Battery battery = batteries[i].GetComponent<Battery>();
                battery.Hide();
            }
        }

        HideSurviors();
    }

    [Client]
    private void HideSurviors()
    {
        //TODO: Cache this. We don't cache it above because survivors can connect and leave the game as they please.
        
        GameObject[] survivors = GameObject.FindGameObjectsWithTag(Tags.SURVIVOR);

        for (var i = 0; i < survivors.Length; i++)
        {
            Survivor survivor = survivors[i].GetComponent<Survivor>();
            survivor.ClientHide();
        }
    }


    [Client]
    private void ShowSurvivors()
    {
        //TODO: Cache this. We don't cache it above because survivors can connect and leave the game as they please.
        GameObject[] survivors = GameObject.FindGameObjectsWithTag(Tags.SURVIVOR);

        for (var i = 0; i < survivors.Length; i++)
        {
            Survivor survivor = survivors[i].GetComponent<Survivor>();
            survivor.ClientShow();
        }

    }

    [Client]
    private void AttackOrGrab()
    {
        Ray ray = lurkerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (!ghostForm)
        {
            if (Physics.Raycast(ray, out hit, attackDistance))
            {
                GameObject hitGameObject = hit.collider.gameObject;

                if (hitGameObject.CompareTag(Tags.DOOR))
                {
                    uint doorId = hitGameObject.GetComponent<Door>().netIdentity.netId;
                    NetworkClient.Send(new ServerClientGameClickedOnDoorMessage { requestedDoorID = doorId });
                }

                else if (hitGameObject.CompareTag(Tags.SURVIVOR))
                {
                    uint survivorId = hitGameObject.GetComponent<Survivor>().netIdentity.netId;
                    NetworkClient.Send(new ServerClientGameLurkerSwingAttackMessage { requestedTargetId = survivorId });
                }
            }

            else
            {
                NetworkClient.Send(new ServerClientGameLurkerSwingAtNothingMessage { });
            }
        }

        else
        {
            if (Physics.Raycast(ray, out hit, trapArmDistance))
            {
                GameObject hitGameObject = hit.collider.gameObject;

                if (hitGameObject.CompareTag(Tags.TRAP))
                {
                    Trap trap = hitGameObject.GetComponent<Trap>();
                    uint trapId = trap.netIdentity.netId;
                    NetworkClient.Send(new ServerClientGameLurkerRequestToArmTrapMessage { requestedTrapId = trapId });
                }
            }
        }
    }

    [Client]
    private void ShowImportantObjects()
    {
        if (doors != null)
        {
            for (var i = 0; i < doors.Length; i++)
            {
                Door door = doors[i].GetComponent<Door>();
                door.Show();
            }
        }

        if (keys != null)
        {
            for (var i = 0; i < keys.Length; i++)
            {
                KeyObject key = keys[i].GetComponent<KeyObject>();
                key.Show();
            }
        }

        if (batteries != null)
        {
            for (var i = 0; i < batteries.Length; i++)
            {
                Battery battery = batteries[i].GetComponent<Battery>();
                battery.Show();
            }
        }

        ShowSurvivors();
    }


    [Client]
    private void OnGUI()
    {
        if (windows.IsWindowOpen())
        {
            return;
        }

        // TO DO: Optimize this!
        GUI.DrawTexture(new Rect(Screen.width / 2, Screen.height / 2, 2, 2), crosshair);
    }

    [Server]
    public float ServerMaxEnergy()
    {
        return maxEnergy;

    }

    [Server]
    public float ServerAttackDistance()
    {
        return attackDistance;

    }

    [Server]
    public void ServerSetEnergy(float value)
    {
        energy = value;
    }

    public float ServerEnergy()
    {
        return energy;

    }

    public bool IsGhostForm()
    {
        return ghostForm == true;

    }

    public bool IsPhysicalForm()
    {
        return ghostForm != true;

    }

    public void SetGhostForm()
    {
        ghostForm = true;

    }

    [Server]
    public void ServerSetPhysicalForm()
    {
        ghostForm = false;
    }


    [Server]
    public float ServerMinEnergy()
    {
        return minEnergy;

    }

    [Server]
    public float ServerTrapArmDistance()
    {
        return trapArmDistance;
    }

    [Server]
    public float ServerEnergyConsumptionRate()
    {
        return energyConsumptionRate;

    }

    [Server]
    public float ServerEnergyRegenerationRate()
    {
        return energyRegenerationRate;

    }

    [Server]
    public string Name()
    {
        return name;
    }

    [Server]
    public int TrapPercentage()
    {
        return trapPercentage;
    }

    [Server]
    public int AttackCooldownInSeconds()
    {
        return attackCoolDownInSeconds;
    }

    [Server]
    public void AllowToAttack(bool value)
    {
        canAttack = value;

    }

    [Server]
    public bool CanAttack()
    {
        return canAttack;
    }


    [Client]
    public void ClientPlayAttackSound()
    {
        attackSound.Play();
    }

    [Client]
    private void OnLurkerFormChanged(bool oldValue, bool newValue)
    {
        //NOTE: We are in ghost form here.
        if (newValue)
        {
            this.lurkerRenderer.enabled = false;
            this.meshCollider.enabled = false;
        }

        // NOTE: We are in physical form here.
        else if (!newValue)
        {
            this.lurkerRenderer.enabled = true;
            this.meshCollider.enabled = true;
        }

        lurkerChangeFormSound.Play();

        if (isLocalPlayer)
        {
            if (!newValue)
            {
                this.speed = physicalFormSpeed;
                ShowImportantObjects();

            }

            else
            {
                this.speed = ghostFormSpeed;
                HideImportantObjects();

            }

        }
    }
    private void OnReadyToGoIntoPhysicalFormEvent()
    {
        lurkerReadyToTransformSound.Play();

    }

    [Client]
    private void OnTrapArmed()
    {
        trapArmSound.Play();
    }


    [Client]
    private void OnArmableTrapsEvent(uint[] armableTraps)
    {
        for (var i = 0; i < armableTraps.Length; i++)
        {
            uint netIdOfTrap = armableTraps[i];

            for (var j = 0; j < traps.Length; j++)
            {
                Trap trap = traps[j].GetComponent<Trap>();
                uint trapId = trap.netIdentity.netId;

                if (trapId == netIdOfTrap)
                {
                    Debug.Log($"Trap {trapId} is armable!");
                }
            }
        }

    }
}

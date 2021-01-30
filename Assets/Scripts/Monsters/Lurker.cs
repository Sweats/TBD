using UnityEngine;
using System.Collections;
using Mirror;

public class Lurker : NetworkBehaviour
{
    private CharacterController lurkerController;

    [SyncVar]
    [SerializeField]
    private float physicalFormSpeed;

    [SyncVar]
    [SerializeField]
    private float ghostFormSpeed;

    [SerializeField]
    [SyncVar]
    private float maxEnergy = 100;

    [SyncVar]
    [SerializeField]
    private float minEnergy = 0;

    [SerializeField]
    [SyncVar]
    private float energy;

    [SerializeField]
    [SyncVar]
    private float energyRegenerationRate;

    [SerializeField]
    [SyncVar]
    private float energyConsumptionRate;

    [SerializeField]
    [SyncVar]
    private float attackDistance;

    [SerializeField]
    [SyncVar]
    private float trapArmDistance;

    [SerializeField]
    [SyncVar]
    private float speed;

    [SyncVar]
    private bool ghostForm = true;

    [SyncVar]
    private bool isReadyToGoIntoPhysicalForm;

    [SyncVar]
    private bool canAttack = false;

    [SerializeField]
    [SyncVar]
    private int attackCoolDownInSeconds;

    [SyncVar]
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
    private GameObject[] traps;

    private GameObject[] doors;

    private GameObject[] survivors;

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

    private Coroutine ghostFormRoutine;

    private Coroutine physicalFormRoutine;

    public override void OnStartServer()
    {
        //EventManager.hostStartedGameEvent.AddListener(OnHostStartedTheGame);
        speed = ghostFormSpeed;
        ghostFormRoutine = StartCoroutine(ServerGhostFormEnergyRoutine());
        base.OnStartServer();
    }

    public override void OnStartLocalPlayer()
    {
        this.enabled = true;
        lurkerController = GetComponent<CharacterController>();
        lurkerController.enabled = true;
        lurkerCamera.enabled = true;
        lurkerCamera.GetComponent<AudioListener>().enabled = true;
        windows.enabled = true;
        StartCoroutine(GlowRoutine());
        doors = GameObject.FindGameObjectsWithTag(Tags.DOOR);
        keys = GameObject.FindGameObjectsWithTag(Tags.KEY);
        batteries = GameObject.FindGameObjectsWithTag(Tags.BATTERY);
        HideImportantObjects();
        Cursor.lockState = CursorLockMode.Locked;

        EventManager.survivorsEscapedStageEvent.AddListener(ServerOnMatchOver);
        //EventManager.hostStartedGameEvent.AddListener(OnHostStartedTheGame);

        base.OnStartLocalPlayer();
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

        //        if (!hostStartedGame)
        //        {
        //            return;
        //        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 secondmove = transform.right * x + transform.forward * z;
        //bool isMoving = moving != secondmove;
        bool isMoving = transform.hasChanged;

        if (Keybinds.GetKey(Action.Transform))
        {
            if (ghostForm)
            {
                if (isReadyToGoIntoPhysicalForm)
                {
                    CmdOnLurkerFormChanged();
                }
            }

            else
            {
                CmdOnLurkerFormChanged();
            }

        }

        else if (Keybinds.GetKey(Action.Attack))
        {
            CmdAttack();
        }

        else if (Keybinds.GetKey(Action.Grab))
        {
            CmdHandleGrab();
        }

        lurkerController.Move(secondmove * speed * Time.deltaTime);
    }

    [TargetRpc]
    private void TargetPlayPhysicalFormMusic()
    {
        if (lurkerGhostFormMusic.isPlaying)
        {
            lurkerGhostFormMusic.Stop();
        }

        lurkerPhysicalFormMusic.Play();
    }

    [TargetRpc]
    private void TargetPlayGhostFormMusic()
    {
        if (lurkerPhysicalFormMusic.isPlaying)
        {
            lurkerPhysicalFormMusic.Stop();
        }

        lurkerGhostFormMusic.Play();
    }


    [TargetRpc]
    private void TargetPlayAttackSound()
    {
        attackSound.Play();
    }

    [TargetRpc]
    private void TargetPlayTransformSound()
    {
        lurkerChangeFormSound.Play();
    }

    [TargetRpc]
    private void TargetPlayArmedSound()
    {
        trapArmSound.Play();
    }

    [Client]
    private void UpdateScreen()
    {
        // TO DO: Change what the color of the screen looks like. Not sure how to do this in Unity yet.
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

        if (survivors != null)
        {
            for (var i = 0; i < survivors.Length; i++)
            {
                Survivor survivor = survivors[i].GetComponent<Survivor>();
                survivor.Hide();
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

        if (survivors != null)
        {
            for (var i = 0; i < survivors.Length; i++)
            {
                Survivor survivor = survivors[i].GetComponent<Survivor>();
                survivor.Show();
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
    }

    // TODO. Figure out how the game should pick which traps that the lurker can actually arm.
    [Client]
    private IEnumerator DoLurkerTraps()
    {
        while (true)
        {
            if (matchOver)
            {
                yield break;
            }

            yield return new WaitForSeconds(1);
        }
    }

    // TODO. Rewrite this probably.
    [Client]
    private IEnumerator GlowRoutine()
    {
        bool glowing = false;

        while (true)
        {
            if (matchOver)
            {
                yield break;
            }

            if (traps != null)
            {
                for (var i = 0; i < traps.Length; i++)
                {
                    Trap trap = traps[i].GetComponent<Trap>();

                    if (trap.Armed())
                    {
                        continue;
                    }

                    if (!glowing)
                    {
                        Debug.Log("Glowing trap...");
                        trap.Glow();
                    }

                    else
                    {
                        Debug.Log("Unglowing trap..."); trap.Unglow();
                    }
                }
            }

            if (glowing)
            {
                yield return new WaitForSeconds(trapGlowTime);
            }

            else
            {
                yield return new WaitForSeconds(secondsBetweenGlows);
            }
        }
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

    [TargetRpc]
    private void TargetReadyToGoIntoPhysicalForm()
    {
        lurkerReadyToTransformSound.Play();
        EventManager.lurkerReadyToGoIntoPhysicalFormEvent.Invoke();
    }

    [Server]
    private void OnHostStartedTheGame()
    {
        hostStartedGame = true;
        ghostFormRoutine = StartCoroutine(ServerGhostFormEnergyRoutine());
    }

    [Server]
    public bool ServerIsInPhysicalForm()
    {
        return ghostForm;
    }

    [Server]
    private void ServerOnMatchOver()
    {
        matchOver = true;
    }

    [Server]
    private IEnumerator ServerGhostFormEnergyRoutine()
    {
        while (energy < maxEnergy)
        {
            if (matchOver)
            {
                yield break;
            }

            //if (!hostStartedGame)
            //{
            //    continue;
            //}

            energy++;
            yield return new WaitForSeconds(energyRegenerationRate);
        }

        isReadyToGoIntoPhysicalForm = true;
        TargetReadyToGoIntoPhysicalForm();
    }


    [Server]
    private IEnumerator ServerPhysicalFormEnergyRoutine()
    {
        while (energy > minEnergy)
        {
            if (matchOver || ghostForm)
            {
                yield break;
            }

            energy--;

            yield return new WaitForSeconds(energyConsumptionRate);
        }


        CmdOnLurkerFormChanged();
        TargetPlayTransformSound();
    }

    [Server]
    private IEnumerator ServerAttackCoolDownRoutine()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCoolDownInSeconds);
        canAttack = true;
    }

    [Command]
    private void CmdHandleGrab()
    {

    }

    [Command]
    private void CmdOnLurkerFormChanged()
    {
        ghostForm = !ghostForm;

        if (!ghostForm)
        {
            StopCoroutine(ghostFormRoutine);
            physicalFormRoutine = StartCoroutine(ServerPhysicalFormEnergyRoutine());
            isReadyToGoIntoPhysicalForm = false;
            canAttack = true;
            speed = physicalFormSpeed;
            ShowImportantObjects();
            TargetPlayPhysicalFormMusic();
            Debug.Log("Now in physical form...");
        }

        else
        {
            StopCoroutine(physicalFormRoutine);
            ghostFormRoutine = StartCoroutine(ServerGhostFormEnergyRoutine());
            canAttack = false;
            speed = ghostFormSpeed;
            HideImportantObjects();
            TargetPlayGhostFormMusic();
            Debug.Log("Now in ghost form...");
        }
    }

    [Command]
    private void CmdAttack()
    {
        Ray ray = lurkerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (!ghostForm && canAttack)
        {
            StartCoroutine(ServerAttackCoolDownRoutine());
            TargetPlayAttackSound();

            if (Physics.Raycast(ray, out hit, attackDistance))
            {
                GameObject hitGameObject = hit.collider.gameObject;

                if (hitGameObject.CompareTag(Tags.SURVIVOR))
                {
                    Survivor survivor = hitGameObject.GetComponent<Survivor>();
                    // TODO: Play an animation here.
                    survivor.CmdDie();
                }

                else if (hitGameObject.CompareTag(Tags.TRAP))
                {
                    Trap trap = hitGameObject.GetComponent<Trap>();

                    if (trap.Armed())
                    {
                        return;
                    }

                    TargetPlayArmedSound();
                    trap.CmdArm();
                }

                else if (hitGameObject.CompareTag(Tags.DOOR))
                {
                    Door door = hitGameObject.GetComponent<Door>();
                    door.CmdPlayerClickedOnLockedDoor();
                }
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

                    if (trap.Armed())
                    {
                        return;
                    }

                    TargetPlayArmedSound();
                    trap.CmdArm();
                }
            }
        }
    }
}

using UnityEngine;
using System.Collections;

public class Lurker : MonoBehaviour
{
    private CharacterController lurkerController;


    public float physicalFormSpeed;


    public float ghostFormSpeed;

    public float maxEnergy = 100;

    public float minEnergy = 0;

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

    private bool ghostForm = true;

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
    private Windows playerWindows;

    private bool isReadyToGoIntoPhysicalForm;


    private bool canAttack = false;

    [SerializeField]
    private int attackCoolDownInSeconds;

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


    private void Start()
    {
        lurkerController = GetComponent<CharacterController>();
        speed = ghostFormSpeed;
        Cursor.lockState = CursorLockMode.Locked;
        doors = GameObject.FindGameObjectsWithTag(Tags.DOOR);
        keys = GameObject.FindGameObjectsWithTag(Tags.KEY);
        batteries = GameObject.FindGameObjectsWithTag(Tags.BATTERY);
        HideImportantObjects();
        EventManager.survivorsEscapedStageEvent.AddListener(OnMatchOver);
        StartCoroutine(GhostFormEnergyRoutine());
        StartCoroutine(GlowRoutine());
    }


    private void LateUpdate()
    {
        if (matchOver)
        {
            return;
        }

        if (playerWindows.IsWindowOpen())
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

    private void Update()
    {
        velocity.y -= gravity * Time.deltaTime;
        lurkerController.Move(velocity * Time.deltaTime);

        if (lurkerController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (playerWindows.IsWindowOpen())
        {
            return;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 secondmove = transform.right * x + transform.forward * z;
        //bool isMoving = moving != secondmove;

        if (Keybinds.GetKey(Action.Transform))
        {
            if (ghostForm)
            {
                if (isReadyToGoIntoPhysicalForm)
                {
                    OnLurkerFormChanged();

                }
            }

            else
            {
                OnLurkerFormChanged();
            }

        }

        else if (Keybinds.GetKey(Action.Attack))
        {
            OnAttack();
        }

        else if (Keybinds.GetKey(Action.Grab))
        {
            HandleGrab();

        }

        lurkerController.Move(secondmove * speed * Time.deltaTime);
    }


    private void OnLurkerFormChanged()
    {
        lurkerChangeFormSound.Play();
        ghostForm = !ghostForm;
        EventManager.lurkerChangedFormEvent.Invoke(ghostForm);

        if (ghostForm)
        {
            StartCoroutine(GhostFormEnergyRoutine());
            isReadyToGoIntoPhysicalForm = false;
            speed = ghostFormSpeed;
            Debug.Log("Now in ghost form...");

            if (lurkerPhysicalFormMusic.isPlaying)
            {
                lurkerPhysicalFormMusic.Stop();
            }

            lurkerGhostFormMusic.Play();
            HideImportantObjects();
        }

        else
        {
            canAttack = true;
            speed = physicalFormSpeed;
            StartCoroutine(PhysicalFormEnergyRoutine());
            Debug.Log("Now in physical form...");

            if (lurkerGhostFormMusic.isPlaying)
            {
                lurkerGhostFormMusic.Stop();
            }

            lurkerPhysicalFormMusic.Play();
            ShowImportantObjects();
        }
    }


    private void OnAttack()
    {
        Ray ray = lurkerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (!ghostForm && canAttack)
        {
            StartCoroutine(AttackCoolDown());
            attackSound.Play();

            if (Physics.Raycast(ray, out hit, attackDistance))
            {
                GameObject hitGameObject = hit.collider.gameObject;

                if (hitGameObject.CompareTag(Tags.SURVIVOR))
                {
                    Survivor survivor = hitGameObject.GetComponent<Survivor>();
                    // TODO: Play an animation here.
                    survivor.Die();
                }

                else if (hitGameObject.CompareTag(Tags.TRAP))
                {
                    Trap trap = hitGameObject.GetComponent<Trap>();

                    if (trap.Armed())
                    {
                        return;

                    }

                    trapArmSound.Play();
                    trap.Arm();
                }

                else if (hitGameObject.CompareTag(Tags.DOOR))
                {
                    Door door = hitGameObject.GetComponent<Door>();
                    door.PlayLockedSound();
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

                    trapArmSound.Play();
                    trap.Arm();
                }
            }
        }
    }


    private void HandleGrab()
    {

    }

    private void UpdateScreen()
    {
        // TO DO: Change what the color of the screen looks like. Not sure how to do this in Unity yet.
    }

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

    private IEnumerator AttackCoolDown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCoolDownInSeconds);
        canAttack = true;
    }

    // TODO. Rewrite this probably.
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
                        Debug.Log("Unglowing trap...");
                        trap.Unglow();
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

    private IEnumerator PhysicalFormEnergyRoutine()
    {
        for (var i = energy; i > minEnergy; i--)
        {
            if (matchOver || ghostForm)
            {
                yield break;
            }

            energy--;

            yield return new WaitForSeconds(energyConsumptionRate);
        }


        OnLurkerFormChanged();
    }

    private IEnumerator GhostFormEnergyRoutine()
    {
        for (var i = energy; i < maxEnergy; i++)
        {
            if (matchOver)
            {
                yield break;
            }

            energy++;
            yield return new WaitForSeconds(energyRegenerationRate);
        }

        isReadyToGoIntoPhysicalForm = true;
        EventManager.lurkerReadyToGoIntoPhysicalFormEvent.Invoke();
        lurkerReadyToTransformSound.Play();
    }

    private void OnMatchOver()
    {
        matchOver = true;
    }

    private void OnGUI()
    {
        if (playerWindows.IsWindowOpen())
        {
            return;
        }

        // TO DO: Optimize this!
        GUI.DrawTexture(new Rect(Screen.width / 2, Screen.height / 2, 2, 2), crosshair);
    }

    public bool IsInPhysicalForm()
    {


        return ghostForm;
    }

}

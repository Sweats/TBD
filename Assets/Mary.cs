using UnityEngine;
using System.Collections;

// NOTE: This class may need a lot more testing. I think I covered all of the bugs though. It's a tricky problem to solve lol.
public class Mary : MonoBehaviour
{
    [SerializeField]
    private float energy;

    [SerializeField]
    private float energyRechargeRate;

    [SerializeField]
    private float energyConsumptionRate;

    [SerializeField]
    private float maxEnergy;

    [SerializeField]
    private float minEnergy;

    [SerializeField]
    private float normalSpeed;

    [SerializeField]
    private float frenzySpeed;


    [SerializeField]
    private float attackDistance;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float gravity;

    [SerializeField]
    private float minEnergyNeededToTeleport;

    [SerializeField]
    private float minEnergyNeededToFrenzy;

    [SerializeField]
    private float teleportEnergyCost;

    [SerializeField]
    private int minTeleportTimerSeconds;

    [SerializeField]
    private int maxTeleportTimerSeconds;

    [SerializeField]
    private int maryTeleportTimer;

    [SerializeField]
    private AudioSource maryScreamSound;

    [SerializeField]
    private AudioSource maryCryingSound;

    [SerializeField]
    private AudioSource maryCalmMusic;

    [SerializeField]
    private AudioSource maryFrenzyMusic;

    [SerializeField]
    private AudioSource maryTeleportSound;

    private GameObject[] teleportLocations;

    private CharacterController maryController;

    private bool matchOver;

    private float xRotation;

    [SerializeField]
    private float minimumX;

    [SerializeField]
    private float maximumX;

    [SerializeField]
    private Camera maryCamera;

    private Vector3 velocity;

    private Transform maryTransform;


    [SerializeField]
    private bool canTeleport = false;

    [SerializeField]
    private bool readyToFrenzy = false;

    private Coroutine maryRandomTeleportionRoutine;

    private Coroutine maryEnergyGainRoutine;

    private Coroutine maryEnergyFrenzyRoutine;

    // TODO: Make it so we don't need this variable.
    private bool coroutineAlreadyStarted;

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
        maryCamera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }

    void Start()
    {
        maryController = GetComponent<CharacterController>();
        teleportLocations = GameObject.FindGameObjectsWithTag(Tags.MARY_TELEPORT_LOCATION);
        speed = normalSpeed;
        Cursor.lockState = CursorLockMode.Locked;
        maryTransform = GetComponent<Transform>();
        maryEnergyGainRoutine = StartCoroutine(MaryRechargeTimer());
        maryRandomTeleportionRoutine = StartCoroutine(MaryRandomTeleportationTimer());
    }


    void Update()
    {
        velocity.y -= gravity * Time.deltaTime;
        maryController.Move(velocity * Time.deltaTime);

        if (maryController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 secondmove = transform.right * x + transform.forward * z;

        if (Keybinds.GetKey(Action.Teleport))
        {
            if (canTeleport)
            {
                Teleport(true);
            }
        }

        else if (Keybinds.GetKey(Action.Transform))
        {
            if (readyToFrenzy)
            {
                OnFrenzy();
            }

        }

        else if (Keybinds.GetKey(Action.Attack))
        {
            OnAttack();
        }

        maryController.Move(secondmove * speed * Time.deltaTime);
    }


    private IEnumerator MaryRechargeTimer()
    {
        Debug.Log("MaryRechargeTimer coroutine started.");

        while (energy < maxEnergy)
        {
            energy++;

            if (energy == minEnergyNeededToTeleport)
            {
                EventManager.maryReadyToTeleportEvent.Invoke();
                canTeleport = true;
            }


            if (energy == minEnergyNeededToFrenzy)
            {
                readyToFrenzy = true;
                EventManager.maryReadyToFrenzyEvent.Invoke();
            }

            yield return new WaitForSeconds(energyRechargeRate);
        }

        Debug.Log("MaryRechargeTimer coroutine stopped.");
    }


    private IEnumerator MaryRandomTeleportationTimer()
    {
        maryTeleportTimer = Random.Range(minTeleportTimerSeconds, maxTeleportTimerSeconds);
        Debug.Log("MaryRandomTeleportationTimer routine started.");

        while (true)
        {
            if (matchOver)
            {
                yield break;
            }

            maryTeleportTimer--;

            if (maryTeleportTimer <= 0)
            {
                maryTeleportTimer = Random.Range(minTeleportTimerSeconds, maxTeleportTimerSeconds);
                Teleport(false);
            }

            yield return new WaitForSeconds(1);
        }
    }

    private IEnumerator MaryFrenzyTimer()
    {
        Debug.Log("MaryFrenzyTimer coroutine started");

        while (energy > minEnergy)
        {
            energy--;
            yield return new WaitForSeconds(energyConsumptionRate);
        }

        Debug.Log("MaryFrenzyTimer coroutine stopped.");
        OnFrenzyEnd();
    }


    private void OnFrenzy()
    {
        readyToFrenzy = false;
        canTeleport = false;
        speed = frenzySpeed;

        if (maryCryingSound.isPlaying)
        {
            maryCryingSound.Stop();
        }

        if (maryCalmMusic.isPlaying)
        {
            maryCalmMusic.Stop();
        }

        maryFrenzyMusic.Play();

        maryEnergyFrenzyRoutine = StartCoroutine(MaryFrenzyTimer());
        StopCoroutine(maryRandomTeleportionRoutine);
	// NOTE:
	// If the player does a teleport and a frenzy at the same time, they can have 2 corutines running that allows them to have infinte frenzy energy. 
	// We can either have this StopCoroutine function called below or change the value minEnergyNeededToFrenzy so this cannot happen.
	// We will see. For now I will do this.
	StopCoroutine(maryEnergyGainRoutine);
    }

    private void OnFrenzyEnd()
    {
        speed = normalSpeed;

        if (maryFrenzyMusic.isPlaying)
        {
            maryFrenzyMusic.Stop();
        }

        maryCalmMusic.Play();
        maryCryingSound.Play();
        maryEnergyGainRoutine = StartCoroutine(MaryRechargeTimer());
        maryRandomTeleportionRoutine = StartCoroutine(MaryRandomTeleportationTimer());
        Teleport(false);
    }


    private void OnAttack()
    {
        // TODO: Make it so we do a cone raycast or something instead of simply having to click on the player.
        Ray ray = maryCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, attackDistance))
        {
            GameObject hitGameObject = hit.collider.gameObject;

            if (hitGameObject.CompareTag(Tags.DOOR))
            {
                Door door = hitGameObject.GetComponent<Door>();
                door.PlayLockedSound();
            }

            // play attack somewhere in here.

            else if (hitGameObject.CompareTag(Tags.SURVIVOR))
            {
                Survivor survivor = hitGameObject.GetComponent<Survivor>();
                survivor.Die();
            }
        }
    }


    private void Teleport(bool manuallyTeleported)
    {
        // If there are not teleportLocations on the stage, then we will simply do nothing.
        // The mapper is responsible for putting in the teleportLocations prefab onto the stage.
        if (teleportLocations != null)
        {
            if (manuallyTeleported)
            {
                energy -= teleportEnergyCost;
                StopCoroutine(maryRandomTeleportionRoutine);
                maryRandomTeleportionRoutine = StartCoroutine(MaryRandomTeleportationTimer());

                if (energy < minEnergyNeededToTeleport)
                {
                    canTeleport = false;
                }

                if (readyToFrenzy)
                {
                    if (energy < minEnergyNeededToFrenzy)
                    {
                        readyToFrenzy = false;
                    }

                    if (!coroutineAlreadyStarted)
                    {
			StopCoroutine(maryEnergyGainRoutine);
                        maryEnergyGainRoutine = StartCoroutine(MaryRechargeTimer());
                    }
                }
            }

            int randomNumber = Random.Range(0, teleportLocations.Length);
            maryTransform.position = teleportLocations[randomNumber].transform.position;
            maryTeleportSound.Play();
        }

    }

    private bool IsAnotherWindowOpen()
    {
        return (PausedGameInput.GAME_PAUSED) || (ConsoleUI.OPENED) || (Chat.OPENED);
    }

}

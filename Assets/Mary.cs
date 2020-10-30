using UnityEngine;
using System.Collections;

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
        StartCoroutine(MaryRechargeTimer());
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

        maryController.Move(secondmove * speed * Time.deltaTime);
    }


    private IEnumerator MaryRechargeTimer()
    {
	// TODO: Remove this temp fix.

        if (coroutineAlreadyStarted)
        {
            Debug.Log("EXTRA COROUTINE CREATED. FIXME?");
            yield break;
        }

        Debug.Log("MaryRechargeTimer coroutine started.");

        coroutineAlreadyStarted = true;

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
        coroutineAlreadyStarted = false;
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

        StartCoroutine(MaryFrenzyTimer());
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
        StartCoroutine(MaryRechargeTimer());
        Teleport(false);
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
                        StartCoroutine(MaryRechargeTimer());
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

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mirror;

// NOTE: This class may need a lot more testing. I think I covered all of the bugs though. It's a tricky problem to solve lol.
public class Mary : NetworkBehaviour
{

    [SyncVar]
    private string name;

    [SyncVar]
    [SerializeField]
    private float energy;

    [SerializeField]
    [SyncVar]
    private float energyRechargeRate;

    [SyncVar]
    [SerializeField]
    private float energyConsumptionRate;

    [SyncVar]
    [SerializeField]
    private float maxEnergy;

    [SyncVar]
    [SerializeField]
    private float minEnergy;

    [SyncVar]
    [SerializeField]
    private float normalSpeed;

    [SyncVar]
    [SerializeField]
    private float frenzySpeed;


    [SyncVar]
    [SerializeField]
    private float attackDistance;


    [SyncVar]
    [SerializeField]
    private float maryArmTrapDistance;


    [SyncVar]
    [SerializeField]
    private int attackCoolDownInSeconds;

    [SyncVar]
    [SerializeField]
    private float speed;

    [SyncVar]
    [SerializeField]
    private float gravity;

    [SyncVar]
    [SerializeField]
    private float minEnergyNeededToTeleport;

    [SyncVar]
    [SerializeField]
    private float minEnergyNeededToFrenzy;

    [SyncVar]
    [SerializeField]
    private float teleportEnergyCost;

    [SyncVar]
    [SerializeField]
    private int minTeleportTimerSeconds;

    [SyncVar]
    [SerializeField]
    private int maxTeleportTimerSeconds;

    [SyncVar]
    [SerializeField]
    private int maryTeleportTimer;

    [SyncVar]
    [SerializeField]
    private bool canTeleport = false;

    [SyncVar]
    [SerializeField]
    private bool readyToFrenzy = false;

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

    [SerializeField]
    private AudioSource attackSound;

    [SerializeField]
    private Windows windows;

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

    [SerializeField]
    private Texture crosshair;

    private Vector3 velocity;

    private Transform maryTransform;

    [SerializeField]
    private bool frenzied = false;

    [SerializeField]
    private bool canAttack = false;

    private Coroutine maryRandomTeleportionRoutine;

    private Coroutine maryEnergyGainRoutine;

    private Coroutine maryEnergyFrenzyRoutine;

    // Used to disarm old traps when she teleports.
    private List<Trap> oldTraps;

    void LateUpdate()
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
        maryCamera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }

    public override void OnStartLocalPlayer()
    {
        this.enabled = true;
        windows.enabled = true;
        maryTransform = GetComponent<Transform>();
        maryController = GetComponent<CharacterController>();
        maryController.enabled = true;
        maryCamera.enabled = true;
        maryCamera.GetComponent<AudioListener>().enabled = true;
        Cursor.lockState = CursorLockMode.Locked;

    }

    [Server]
    public void ServerSetName(string name)
    {
        this.name = name;
    }

    [Server]
    public string Name()
    {
        return name;

    }

    public override void OnStartServer()
    {
        oldTraps = new List<Trap>();
        maryEnergyGainRoutine = StartCoroutine(ServerMaryRechargeTimerRoutine());
        maryRandomTeleportionRoutine = StartCoroutine(ServerMaryRandomTeleportationTimerRoutine());
        teleportLocations = GameObject.FindGameObjectsWithTag(Tags.MARY_TELEPORT_LOCATION);
        speed = normalSpeed;
    }

    [Client]
    private void Update()
    {
        velocity.y -= gravity * Time.deltaTime;
        maryController.Move(velocity * Time.deltaTime);

        if (!isLocalPlayer)
        {
            return;
        }

        if (maryController.isGrounded && velocity.y < 0)
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

        if (Keybinds.GetKey(Action.Teleport))
        {
            CmdTeleport();
        }

        else if (Keybinds.GetKey(Action.Transform))
        {
            CmdOnFrenzy();
        }

        else if (Keybinds.GetKey(Action.Attack))
        {
            CmdAttack();
        }

        maryController.Move(secondmove * speed * Time.deltaTime);
    }

    [TargetRpc]
    private void TargetReadyToTeleport()
    {
        EventManager.clientServerGameMaryReadyToTeleportEvent.Invoke();
    }

    [TargetRpc]
    private void TargetReadyToFrenzy()
    {
        EventManager.ClientServerGameMaryReadyToFrenzyEvent.Invoke();
    }


    [Server]
    private IEnumerator ServerMaryRechargeTimerRoutine()
    {
        Debug.Log("MaryRechargeTimer coroutine started.");

        while (energy < maxEnergy)
        {
            energy++;

            if (energy == minEnergyNeededToTeleport)
            {
                canTeleport = true;
                TargetReadyToTeleport();
            }


            if (energy == minEnergyNeededToFrenzy)
            {
                readyToFrenzy = true;
                TargetReadyToFrenzy();
            }

            yield return new WaitForSeconds(energyRechargeRate);
        }

        Debug.Log("MaryRechargeTimer coroutine stopped.");
    }



    [Server]
    private IEnumerator ServerMaryRandomTeleportationTimerRoutine()
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
                ServerTeleport();
            }

            yield return new WaitForSeconds(1);
        }
    }

    [Server]
    private void ServerTeleport()
    {
        if (teleportLocations != null)
        {
            StopCoroutine(maryRandomTeleportionRoutine);
            maryRandomTeleportionRoutine = StartCoroutine(ServerMaryRandomTeleportationTimerRoutine());
            int randomNumber = Random.Range(0, teleportLocations.Length);
            Vector3 position = teleportLocations[randomNumber].transform.position;
            TargetTeleportPlayer(position);
        }
    }

    [Server]
    private IEnumerator ServerMaryFrenzyTimer()
    {
        Debug.Log("MaryFrenzyTimer coroutine started");

        while (energy > minEnergy)
        {
            energy--;
            yield return new WaitForSeconds(energyConsumptionRate);
        }

        Debug.Log("MaryFrenzyTimer coroutine stopped.");
        ServerOnFrenzyEnd();
    }

    [Server]
    private IEnumerator ServerAttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCoolDownInSeconds);
        canAttack = true;
    }

    [TargetRpc]
    private void TargetPlayMaryScreamSound()
    {
        maryScreamSound.Play();

        if (maryCryingSound.isPlaying)
        {
            maryCryingSound.Stop();
        }

        if (maryCalmMusic.isPlaying)
        {
            maryCalmMusic.Stop();
        }
    }

    [TargetRpc]
    private void TargetPlayMaryFrenzyMusic()
    {
        maryFrenzyMusic.Play();
    }

    [TargetRpc]
    private void TargetPlayCryingSoundAndCalmMusic()
    {
        if (maryFrenzyMusic.isPlaying)
        {
            maryFrenzyMusic.Stop();
        }

        maryCalmMusic.Play();
        maryCryingSound.Play();
    }

    [TargetRpc]
    private void TargetPlayAttackSound()
    {
        attackSound.Play();
    }


    [Command]
    private void CmdOnFrenzy()
    {
        if (!readyToFrenzy)
        {
            return;
        }

        readyToFrenzy = false;
        canTeleport = false;
        frenzied = true;
        canAttack = true;
        speed = frenzySpeed;
        TargetPlayMaryFrenzyMusic();

        maryEnergyFrenzyRoutine = StartCoroutine(ServerMaryFrenzyTimer());
        StopCoroutine(maryRandomTeleportionRoutine);
        // NOTE:
        // If the player does a teleport and a frenzy at the same time, they can have 2 corutines running that allows them to have infinte frenzy energy. 
        // We can either have this StopCoroutine function called below or change the value minEnergyNeededToFrenzy so this cannot happen.
        // We will see. For now I will do this.
        StopCoroutine(maryEnergyGainRoutine);

    }

    [Server]
    private void ServerOnFrenzyEnd()
    {
        frenzied = false;
        canAttack = false;
        speed = normalSpeed;
        maryEnergyGainRoutine = StartCoroutine(ServerMaryRechargeTimerRoutine());
        maryRandomTeleportionRoutine = StartCoroutine(ServerMaryRandomTeleportationTimerRoutine());
        ServerTeleport();
    }


    [Command]
    private void CmdAttack()
    {
        // TODO: Make it so we do a cone raycast or something instead of simply having to click on the player.
        Ray ray = maryCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (!frenzied)
        {
            if (Physics.Raycast(ray, out hit, attackDistance))
            {
                GameObject hitGameObject = hit.collider.gameObject;

                if (hitGameObject.CompareTag(Tags.DOOR))
                {
                    Door door = hitGameObject.GetComponent<Door>();
                    //door.CmdPlayerClickedOnLockedDoor();
                }
            }
        }

        else if (frenzied && canAttack)
        {
            TargetPlayAttackSound();
            StartCoroutine(ServerAttackCooldown());

            if (Physics.Raycast(ray, out hit, attackDistance))
            {
                GameObject hitGameObject = hit.collider.gameObject;

                if (hitGameObject.CompareTag(Tags.SURVIVOR))
                {
                    Survivor survivor = hitGameObject.GetComponent<Survivor>();
                    //survivor.CmdDie();
                }

                else if (hitGameObject.CompareTag(Tags.DOOR))
                {
                    Door door = hitGameObject.GetComponent<Door>();
                    //door.CmdPlayerClickedOnLockedDoor(); ;
                }
            }
        }
    }


    [Command]
    private void CmdTeleport()
    {
        if (canTeleport)
        {
            // NOTE: If there are not teleportLocations on the stage, then we will simply do nothing.
            // The mapper is responsible for putting in the teleportLocations prefab onto the stage.
            if (teleportLocations != null)
            {
                energy -= teleportEnergyCost;
                StopCoroutine(maryRandomTeleportionRoutine);
                maryRandomTeleportionRoutine = StartCoroutine(ServerMaryRandomTeleportationTimerRoutine());

                if (energy < minEnergyNeededToTeleport)
                {
                    canTeleport = false;
                }

                if (energy < minEnergyNeededToFrenzy)
                {
                    readyToFrenzy = false;
                }

                StopCoroutine(maryEnergyGainRoutine);
                maryEnergyGainRoutine = StartCoroutine(ServerMaryRechargeTimerRoutine());

                int randomNumber = Random.Range(0, teleportLocations.Length);
                Vector3 position = teleportLocations[randomNumber].transform.position;
                TargetTeleportPlayer(position);
                ServerOnTeleport();
            }
        }
    }

    // TODO: Do movement on the server side.

    [TargetRpc]
    private void TargetTeleportPlayer(Vector3 newPosition)
    {
        maryTransform.position = newPosition;
        maryTeleportSound.Play();
    }



    //TODO: Test this. Should be good though.
    [Server]
    private void ServerOnTeleport()
    {
        for (var i = 0; i < oldTraps.Count; i++)
        {
            oldTraps[i].ServerDisarm();
        }

        oldTraps.Clear();

        RaycastHit[] objectsHit = Physics.SphereCastAll(transform.position, maryArmTrapDistance, transform.forward, maryArmTrapDistance);

        for (var i = 0; i < objectsHit.Length; i++)
        {
            GameObject trapObject = objectsHit[i].collider.gameObject;

            if (trapObject.CompareTag(Tags.TRAP))
            {
                Trap trap = trapObject.GetComponent<Trap>();
                //trap.CmdArm();
                oldTraps.Add(trap);
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

        // TODO: Optimize this!
        GUI.DrawTexture(new Rect(Screen.width / 2, Screen.height / 2, 2, 2), crosshair);
    }
}

using UnityEngine;
using System.Collections.Generic;
using Mirror;

// NOTE: This class may need a lot more testing. I think I covered all of the bugs though. It's a tricky problem to solve lol.
public class Mary : NetworkBehaviour
{

    [SyncVar]
    private string name;

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
    private float maryArmTrapDistance;


    [SerializeField]
    private int attackCoolDownInSeconds;

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

    [SerializeField]
    private AudioSource maryAttackSound;

    [SerializeField]
    private Windows windows;

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
        maryCamera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }

    public override void OnStartLocalPlayer()
    {
        this.enabled = true;
        windows.enabled = true;
        windows.LocalPlayerStart();
        maryTransform = GetComponent<Transform>();
        maryController = GetComponent<CharacterController>();
        maryController.enabled = true;
        maryCamera.enabled = true;
        maryCamera.GetComponent<AudioListener>().enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        EventManager.clientServerGameMaryServerTeleportedYouEvent.AddListener(OnServerTeleportedYou);
        EventManager.clientServerGameMaryFrenziedEvent.AddListener(OnServerLetYouFrenzy);
        EventManager.clientServerGameMaryFrenzyOverEvent.AddListener(OnFrenzyOver);

        NetworkClient.Send(new ServerClientGameMaryJoinedMessage{});
    }

    [Client]
    private void OnServerTeleportedYou(float x, float y, float z)
    {
        this.transform.position = new Vector3(x, y, z);
        maryTeleportSound.Play();
    }

    [Client]
    private void OnFrenzyOver()
    {
        speed = normalSpeed;

    }

    [Client]
    private void OnServerLetYouFrenzy()
    {
        if (maryCryingSound.isPlaying)
        {
            maryCryingSound.Stop();
        }

        maryFrenzyMusic.Play();
        speed = frenzySpeed;
    }

    [Server]
    public void ServerSetName(string name)
    {
        this.name = name;
    }

    public float ServerTeleportationEnergyCost()
    {
        return teleportEnergyCost;
    }

    [Server]
    public string Name()
    {
        return name;

    }

    public int ServerMinTeleportationTimerInSeconds()
    {
        return minTeleportTimerSeconds;

    }

    public int ServerMaxTeleportationTimerInSeconds()
    {
        return maxTeleportTimerSeconds;
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
            NetworkClient.Send(new ServerClientGameMaryTeleportRequest{});
        }

        else if (Keybinds.GetKey(Action.Transform))
        {
            NetworkClient.Send(new ServerClientGameMaryFrenzyRequest{});
        }

        else if (Keybinds.GetKey(Action.Attack))
        {
            ClientAttack();
        }

        maryController.Move(secondmove * speed * Time.deltaTime);
    }



    [Server]
    public void ServerSetAttack(bool value)
    {
        canAttack = value;
    }

    [Server]
    public int ServerAttackCooldown()
    {
        return attackCoolDownInSeconds;

    }

    [Server]
    public bool CanAttack()
    {
        return canAttack;
    }

    [Server]
    public float ServerEnergy()
    {
        return energy;

    }

    [Server]
    public float ServerMaxEnergy()
    {
        return maxEnergy;

    }

    [Server]
    public float ServerMinEnergy()
    {
        return minEnergy;

    }

    [Server]
    public float ServerEnergyNeededToTeleport()
    {
        return minEnergyNeededToTeleport;

    }

    [Server]
    public float ServerEnergyNeededToFrenzy()
    {
        return minEnergyNeededToFrenzy;
    }

    [Server]
    public float ServerEnergyConsumptionRate()
    {
        return energyConsumptionRate;

    }

    [Server]
    public float ServerEnergyRechargeRate()
    {
        return energyRechargeRate;
    }


    [Server]
    public float ServerAttackDistance()
    {
        return attackDistance;
    }

    [Server]
    public void ServerSetFrenzied(bool value)
    {
        frenzied = value;
    }

    [Server]
    public bool ServerFrenzied()
    {
        return frenzied;
    }

    [Client]
    private void ClientAttack()
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
                uint netId = door.netIdentity.netId;
                NetworkClient.Send(new ServerClientGameClickedOnDoorMessage{requestedDoorID = netId});
            }

            else if (hitGameObject.CompareTag(Tags.SURVIVOR))
            {
                Survivor survivor = hitGameObject.GetComponent<Survivor>();
                uint survivorId = survivor.netIdentity.netId;
                NetworkClient.Send(new ServerClientGameMaryAttackedSurvivorMessage{requestedSurvivorId = survivorId});

            }

            else
            {
                NetworkClient.Send(new ServerClientGameMaryAttackedNothingMessage{});
            }
        }
    }

    [Client]
    public void ClientPlayFrenzySound()
    {
        maryScreamSound.Play();
    }

    [Client]
    public void ClientStopCryingSound()
    {
        if (maryCryingSound.isPlaying)
        {
            maryCryingSound.Stop();
        }
    }

    [Client]
    public void ClientPlayMaryCryingSound()
    {
        maryCryingSound.Play();

    }

    [Client]
    public void ClientPlayTeleportationSound()
    {
        maryTeleportSound.Play();

    }

    [Client]
    public void ClientPlayAttackSound()
    {
        maryAttackSound.Play();
    }


    [Server]
    public float ServerArmTrapDistance()
    {
        return maryArmTrapDistance;
    }

    [Server]
    public void ServerSetEnergy(float energy)
    {
        this.energy = energy;
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

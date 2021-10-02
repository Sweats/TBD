using UnityEngine;
using System.Collections;
using Mirror;

public class Phantom : NetworkBehaviour
{
    [SyncVar]
    private string name;
    [SerializeField]
    [SyncVar]
    private float speed;

    [SerializeField]
    [SyncVar]
    private float attackDistance;

    [SerializeField]
    [SyncVar]
    private int attackCoolDownInSeconds;

    [SyncVar]
    [SerializeField]
    private bool canAttack = true;

    [SerializeField]
    private AudioSource ambientMusic;

    [SerializeField]
    private AudioSource attackSound;

    [SerializeField]
    private Camera phantomCamera;

    [SerializeField]
    private Windows windows;

    private Vector3 velocity;

    private CharacterController phantomController;

    [SerializeField]
    private float gravity;

    private float xRotation;

    [SerializeField]
    private float minimumX;

    [SerializeField]
    private float maximumX;

    [SerializeField]
    private Texture crosshair;

    private bool matchOver;

    public override void OnStartLocalPlayer()
    {
        this.enabled = true;
        phantomController = GetComponent<CharacterController>();
        windows.enabled = true;
        phantomCamera.enabled = true;
        phantomCamera.GetComponent<AudioListener>().enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        phantomController.enabled = true;
        HideImportantObjects();
        base.OnStartLocalPlayer();
    }

    public override void OnStartServer()
    {

    }

    [Server]
    public string Name()
    {
        return name;
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
        phantomCamera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }

    [Client]
    private void Update()
    {
        velocity.y -= gravity * Time.deltaTime;
        phantomController.Move(velocity * Time.deltaTime);

        if (!isLocalPlayer)
        {
            return;
        }

        if (phantomController.isGrounded && velocity.y < 0)
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

        if (Keybinds.GetKey(Action.Attack))
        {
            CmdAttack();
        }

        phantomController.Move(secondmove * speed * Time.deltaTime);
    }

    [Client]
    private void HideImportantObjects()
    {
        GameObject[] doors = GameObject.FindGameObjectsWithTag(Tags.DOOR);

        if (doors != null)
        {
            for (var i = 0; i < doors.Length; i++)
            {
                Door door = doors[i].GetComponent<Door>();
                door.Hide();
            }
        }

        GameObject[] keys = GameObject.FindGameObjectsWithTag(Tags.KEY);

        if (keys != null)
        {
            for (var i = 0; i < keys.Length; i++)
            {
                KeyObject key = keys[i].GetComponent<KeyObject>();
                key.Hide();
            }
        }

        GameObject[] batteries = GameObject.FindGameObjectsWithTag(Tags.BATTERY);

        if (batteries != null)
        {
            for (var i = 0; i < batteries.Length; i++)
            {
                Battery battery = batteries[i].GetComponent<Battery>();
                battery.Hide();
            }
        }
    }

    [TargetRpc]
    private void TargetAttackSoundPlay()
    {
        attackSound.Play();
    }

    [Command]
    private void CmdAttack()
    {
        if (canAttack)
        {
            StartCoroutine(AttackCoolDown());
            RaycastHit hit;
            Ray ray = phantomCamera.ScreenPointToRay(Input.mousePosition);
            TargetAttackSoundPlay();

            if (Physics.Raycast(ray, out hit, attackDistance))
            {
                GameObject hitGameObject = hit.collider.gameObject;

                if (hitGameObject.CompareTag(Tags.SURVIVOR))
                {
                    Survivor survivor = hitGameObject.GetComponent<Survivor>();
                }
            }

        }
    }


    [Server]
    private IEnumerator AttackCoolDown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCoolDownInSeconds);
        canAttack = true;
    }


    // TODO: How do we want to handle the traps for the Phantom?
    [Server]
    private IEnumerator HandleTraps()
    {
        yield return null;
    }

    // TODO: How do we want to handle the survivor detection for the Phantom?
    [Server]
    private IEnumerator HandleSurvivorDetection()
    {
        yield return null;
    }

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

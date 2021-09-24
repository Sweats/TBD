using UnityEngine;
using Mirror;

public enum DoorType
{
    Normal = 0
}


// This cl
public class Door : NetworkBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private int unlockMask = -1;

    [SerializeField]
    private string doorName = "door";

    [SerializeField]
    [SyncVar]
    private bool unlocked;

    [SerializeField]
    private bool grabbed;

    [SerializeField]
    private float doorPushStrength;

    [SerializeField]
    private AudioSource unlockedSound;

    [SerializeField]
    private AudioSource lockedSound;

    [SerializeField]
    private Color unlockableColor = Color.green;

    [SerializeField]
    private Color lockedColor = Color.red;

    [SerializeField]
    private Color outlineColor = Color.yellow;

    [SerializeField]
    private Renderer doorRenderer;

    [SerializeField]
    private MeshCollider doorCollider;

    private Rigidbody doorRigidBody;

    private HingeJoint doorJoint;

    private GameObject playerGrabDoorObject;

    [ServerCallback]
    private void Start()
    {
        doorRigidBody = GetComponent<Rigidbody>();
        doorJoint = GetComponent<HingeJoint>();

        if (unlocked)
        {
            doorJoint.enableCollision = true;
            doorRigidBody.detectCollisions = true;
        }

        else
        {
            doorJoint.enableCollision = false;
            //doorRigidBody.detectCollisions = false;
        }
    }

    /*
    [Server]
    private void Update()
    {
        if (!grabbed)
        {
            return;
        }

        Transform position = playerGrabDoorObject.transform.position;
        Transform doorPosition = this.transform.position;

    }
    */


    [Command(requiresAuthority = false)]
    public void CmdPlayerHitDoor(Vector3 moveDirection)
    {
        if (unlocked)
        {
            Vector3 newMoveDirection = new Vector3(moveDirection.x, 0, moveDirection.z);
            Vector3 velocity = newMoveDirection * doorPushStrength;
            doorRigidBody.AddForce(velocity);
        }
    }

    [Client]
    public void Hide()
    {
        doorRenderer.enabled = false;
        doorCollider.enabled = false;
    }

    //NOTE: Lurker
    [Client]
    public void Show()
    {
        doorRenderer.enabled = true;
        doorCollider.enabled = true;
    }

    public bool Unlocked()
    {
        return unlocked;
    }

    [Server]
    public void Unlock()
    {
        unlocked = true;
    }

    public int UnlockMask()
    {
        return unlockMask;
    }

    public bool Grabbed()
    {
        return grabbed;
    }

    public string Name()
    {
        return doorName;
    }
}

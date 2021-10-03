using UnityEngine;
using Mirror;

public class Door : NetworkBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private int unlockMask = -1;

    [SerializeField]
    private string doorName = "door";

    [SerializeField]
    [SyncVar(hook = nameof(OnServerUnlockedDoor))]
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


    [Client]
    private void Start()
    {
        doorJoint = GetComponent<HingeJoint>();
        doorRigidBody = GetComponent<Rigidbody>();
    }

    [Client]
    private void OnServerUnlockedDoor(bool oldvalue, bool newValue)
    {
        if (newValue)
        {
            doorRigidBody.isKinematic = false;
        }

        else
        {
            doorRigidBody.isKinematic = true;
        }

    }

    [Client]
    public void Hide()
    {
        doorRenderer.enabled = false;
        doorCollider.enabled = false;
    }

    [Client]
    public void DisableCollision()
    {
        doorCollider.enabled = false;
    }

    //NOTE: Lurker
    [Client]
    public void Show()
    {
        doorRenderer.enabled = true;
        doorCollider.enabled = true;
    }

    [Server]
    public bool ServerUnlocked()
    {
        return unlocked;
    }

    [Server]
    public void AddForce(Vector3 moveDirection)
    {
        doorRigidBody.AddForce(moveDirection);
    }

    [Server]
    public void ServerUnlock()
    {
        unlocked = true;
    }

    [Server]
    public float PushStrength()
    {
        return doorPushStrength;

    }

    public int UnlockMask()
    {
        return unlockMask;
    }

    public bool Grabbed()
    {
        return grabbed;
    }

    [Client]
    public void ClientPlayLockedSound()
    {
        lockedSound.Play();

    }

    [Client]
    public void ClientPlayUnlockedSound()
    {
        unlockedSound.Play();

    }

    public string Name()
    {
        return doorName;
    }
}

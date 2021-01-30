using UnityEngine;
using Mirror;

public enum DoorType
{
    Normal = 0
}


public class Door : NetworkBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    [SyncVar]
    private int unlockMask = -1;

    [SerializeField]
    [SyncVar]
    private string doorName = "door";

    [SyncVar(hook = nameof(HookSurvivorUnlockedDoor))]
    [SerializeField]
    private bool unlocked;

    [SyncVar(hook = nameof(HookSurvivorGrabbedDoor))]
    [SerializeField]
    private bool grabbed;

    [SyncVar]
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
    private BoxCollider doorCollider;

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

    [Command(ignoreAuthority=true)]
    public void CmdPlayerClickedOnLockedDoor(NetworkConnectionToClient sender = null)
    {
        Survivor survivor = sender.identity.GetComponent<Survivor>();

        //NOTE: If null, it must be a monster
        if (survivor == null)
        {
            RpcFailedToUnlockDoor();
            return;
        }

        var keys = survivor.Items();
        bool found = false;
        int foundKeyIndex = 0;

        for (var i = 0; i < keys.Count; i++)
        {
            int keyMask = keys[i].Mask();

            if (this.unlockMask == keyMask)
            {
                found = true;
                foundKeyIndex = i;
                break;
            }
        }

        if (found)
        {
            string foundKeyName = keys[foundKeyIndex].Name();

            PlayerUnlockedDoorMessage unlockDoorMessage = new PlayerUnlockedDoorMessage
            {
                playerName = survivor.Name(),
                doorName = this.doorName,
                keyName = foundKeyName
            };

            RpcUnlockDoorMessage(unlockDoorMessage);
            unlocked = true;
            return;
        }

        RpcFailedToUnlockDoor();
    }

    [Command(ignoreAuthority = true)]
    public void CmdPlayerHitDoor(Vector3 moveDirection)
    {
        if (unlocked)
        {
            Vector3 newMoveDirection = new Vector3(moveDirection.x, 0, moveDirection.z);
            Vector3 velocity = newMoveDirection * doorPushStrength;
            doorRigidBody.AddForce(velocity);
        }
    }

    [ClientRpc]
    private void RpcUnlockDoorMessage(PlayerUnlockedDoorMessage serverMessage)
    {
        string playerName = serverMessage.playerName;
        string doorName = serverMessage.doorName;
        string keyName = serverMessage.keyName;
        EventManager.survivorUnlockDoorEvent.Invoke(playerName, doorName, keyName);
    }

    [ClientRpc]
    private void RpcFailedToUnlockDoor()
    {
        lockedSound.Play();
    }

    [Client]
    private void HookSurvivorUnlockedDoor(bool oldValue, bool newValue)
    {
        unlockedSound.Play();
        doorJoint.enableCollision = newValue;
        doorRigidBody.detectCollisions = newValue;
    }


    [Client]
    private void HookSurvivorGrabbedDoor(bool oldValue, bool newValue)
    {
        doorJoint.enableCollision = newValue;
        doorRigidBody.detectCollisions = newValue;
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

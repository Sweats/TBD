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

    [SyncVar]
    [SerializeField]
    private bool unlocked;

    [SyncVar]
    [SerializeField]
    private bool grabbed;

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

    private void Start()
    {
        doorRigidBody = GetComponent<Rigidbody>();
        doorJoint = GetComponent<HingeJoint>();

        if (unlocked)
        {
            doorJoint.enableCollision = true;
        }

        else
        {
            doorJoint.enableCollision = false;
        }
    }

    // for now we will play the locked sound in here. We may want to move it out at some point.
    public void PlayLockedSound()
    {
        lockedSound.Play();
    }

    public void PlayUnlockedSound()
    {
        unlockedSound.Play();
    }

    // Hide from the Lurker and the Phantom.
    public void Hide()
    {
        doorRenderer.enabled = false;
        doorCollider.enabled = false;
    }


    // Called for the Lurker monster. Later we will want to change it so doors behave like they do in the other game.
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

    [Command(ignoreAuthority = true)]
    public void CmdPlayerClickedOnLockedDoor(NetworkConnectionToClient sender = null)
    {
        Survivor survivor = sender.identity.GetComponent<Survivor>();
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

    [ClientRpc]
    private void RpcFailedToUnlockDoor()
    {
        lockedSound.Play();
    }

    [ClientRpc]
    private void RpcUnlockDoorMessage(PlayerUnlockedDoorMessage serverMessage)
    {
        unlockedSound.Play();
        doorJoint.enableCollision = true;
        string playerName = serverMessage.playerName;
        string doorName = serverMessage.doorName;
        string keyName = serverMessage.keyName;
        EventManager.survivorUnlockDoorEvent.Invoke(playerName, doorName, keyName);
    }



    public string Name()
    {
        return doorName;
    }

    [Client]
    public void PlayerHitDoor(ControllerColliderHit hit, float pushStrength)
    {
        GameObject hitGameobject = hit.gameObject;
        Vector3 newDoorMoveDirection = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        Vector3 velocity = newDoorMoveDirection * pushStrength;
        CmdPlayerHitDoor(velocity);
    }

    [Command(ignoreAuthority=true)]
    private void CmdPlayerHitDoor(Vector3 velocity)
    {
        RpcPlayerHitDoor(velocity);
    }

    [ClientRpc]
    private void RpcPlayerHitDoor(Vector3 velocity)
    {
        doorRigidBody.AddForce(velocity);
    }
}

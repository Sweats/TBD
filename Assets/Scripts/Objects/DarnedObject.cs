using UnityEngine;
using Mirror;

public class DarnedObject : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnObjectGrabbed))]
    [SerializeField]
    private bool grabbed;

    private GameObject playerGrabbingObject;

    [SerializeField]
    private float grabStrength;

    private Rigidbody rigidBody;

    [ServerCallback]
    private void Update()
    {
        if (!grabbed)
        {
            return;
        }

        Vector3 velocity = (playerGrabbingObject.transform.position - rigidBody.position) * grabStrength;
        rigidBody.velocity = velocity;
    }

    [ServerCallback]
    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        this.enabled = false;
    }

    [Command(ignoreAuthority = true)]
    public void CmdGrab(NetworkConnectionToClient sender = null)
    {
        if (grabbed)
        {
            return;
        }

        Survivor survivor = sender.identity.GetComponent<Survivor>();
        GameObject hand = survivor.Hand();
        this.playerGrabbingObject = hand;

        //TODO: Test this with multiple people.
        //This should hit if someone else is already grabbing the object.
        //
        grabbed = true;
        rigidBody.useGravity = false;
        this.enabled = true;
    }

    [Command(ignoreAuthority = true)]
    public void CmdDrop()
    {
        grabbed = false;
        this.playerGrabbingObject = null;
        rigidBody.useGravity = true;
        this.enabled = false;
    }

    [Client]
    private void OnObjectGrabbed(bool oldValue, bool newValue)
    {
        this.enabled = newValue;
    }

}


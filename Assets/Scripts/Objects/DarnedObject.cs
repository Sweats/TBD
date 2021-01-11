using UnityEngine;
using Mirror;

public class DarnedObject : NetworkBehaviour
{

    [SerializeField]
    private bool physicsEnabled;

    [SerializeField]
    private float mass;

    private bool grabbed;

    private GameObject playerGrabbingObject;


    [SerializeField]
    private float grabStrength;

    private Quaternion lookRotation;

    private Rigidbody rigidBody;

    [SyncVar]
    private Vector3 velocity;

    private void Update()
    {
        if (!grabbed)
        {
            return;
        }

        velocity = (playerGrabbingObject.transform.position - rigidBody.position) * grabStrength;
        CmdVelocity(velocity);
    }

    [Command(ignoreAuthority=true)]
    private void CmdVelocity(Vector3 velocity)
    {
        rigidBody.velocity = velocity;

    }
    
    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    public void Grab(GameObject playerGrabbingObject)
    {
        this.playerGrabbingObject = playerGrabbingObject;
        grabbed = true;
        rigidBody.useGravity = false;
    }

    public void Drop()
    {
        grabbed = false;
        this.playerGrabbingObject = null;
        rigidBody.useGravity = true;
    }

    public bool Grabbed()
    {
        return grabbed;

    }
}


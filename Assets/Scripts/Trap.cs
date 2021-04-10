using UnityEngine;
using Mirror;

public class Trap : NetworkBehaviour
{
    [SerializeField]
    private AudioSource trapSound;

    [SerializeField]
    private float maxTimer;

    [SerializeField]
    private float minTimer;

    [SerializeField]
    private float trapHitAmount;

    [SyncVar]
    [SerializeField]
    private float trapTimer;

    public float trapTimerRate;

    [SerializeField]

    [SyncVar]
    private bool armed;

    [SerializeField]
    private MeshRenderer trapRenderer;

    [SerializeField]
    private CapsuleCollider capsuleCollider;

    private Color originalColor;

    private void Start()
    {
        originalColor = trapRenderer.material.color;
        armed = true;
    }

    [Command(requiresAuthority=false)]
    public void CmdTrigger()
    {
        armed = false;
    }

    [Command(requiresAuthority=false)]
    public void CmdArm()
    {
        armed = true;
    }

    public bool Armed()
    {
        return armed;
    }

    public void Disarm()
    {
        armed = false;
    }

    // For the Lurker monster.
    [Client]
    public void Glow()
    {
        Debug.Log("Glowing...");
        trapRenderer.material.SetColor("_Color", Color.white);

    }

    [Client]
    public void Unglow()
    {
        Debug.Log("Unglowing...");
        trapRenderer.material.SetColor("_Color", originalColor);
    }

    public float HitAmount()
    {
        return trapHitAmount;
    }

    [Command(requiresAuthority=false)]
    public void CmdTriggerTrap(NetworkConnectionToClient sender = null)
    {
        Survivor survivor = sender.identity.GetComponent<Survivor>();
        survivor.insanity.Increment(trapHitAmount);
        armed = false;
    }

    [ClientRpc]
    private void RpcTriggerTrap()
    {
        trapSound.Play();
    }

}

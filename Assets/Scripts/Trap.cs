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
    private float trapInsanityHitAmount;

    [SerializeField]
    private float trapTimer;

    public float trapTimerRate;

    [SerializeField]
    private bool armed;

    private bool armable;

    [SerializeField]
    private MeshRenderer trapRenderer;

    [SerializeField]
    private CapsuleCollider capsuleCollider;

    private Color originalColor;

    private void Start()
    {
        originalColor = trapRenderer.material.color;
        armable = false;
    }

    [Server]
    public bool ServerArmed()
    {
        return armed;
    }

    [Server]
    public void ServerArm()
    {
        armed = true;
    }

    [Server]
    public float InsanityHitAmount()
    {
        return trapInsanityHitAmount;

    }

    [Server]
    public void ServerDisarm()
    {
        armed = false;
    }

    [Server]
    public bool ServerArmable()
    {
        return armable;
    }

    [Server]
    public void ServerSetArmable(bool armable)
    {
        this.armable = armable;
    }

    [Client]
    public void ClientTrapTriggered()
    {
        trapSound.Play();
    }

    [Server]
    public float HitAmount()
    {
        return trapInsanityHitAmount;
    }
}

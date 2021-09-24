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
    [SyncVar(hook = nameof(OnTrapTriggered))]
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

    public bool Armed()
    {
        return armed;
    }

    public void Disarm()
    {
        armed = false;
    }

    [Server]
    public float InsanityHitAmount()
    {
        return trapInsanityHitAmount;

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

    [Client]
    private void OnTrapTriggered()
    {
        if (!armed)
        {
            //AudioSource.PlayClipAtPoint(trapSound, this.transform.position);

        }

    }

    public float HitAmount()
    {
        return trapInsanityHitAmount;
    }
}

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

    public float trapTimer;

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


    private void PhantomRoutine()
    {

    }

    public void Trigger()
    {
        armed = false;
    }


    public void PlaySound()
    {
        trapSound.Play();
    }

    public void Arm()
    {
        armed = true;
        Unglow();
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
    public void Glow()
    {
        Debug.Log("Glowing...");
        trapRenderer.material.SetColor("_Color", Color.white);

    }

    public void Unglow()
    {
        Debug.Log("Unglowing...");
        trapRenderer.material.SetColor("_Color", originalColor);

    }

    public float HitAmount()
    {
        return trapHitAmount;
    }

}

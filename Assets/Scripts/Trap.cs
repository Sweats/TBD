using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField]
    private AudioSource trapSound;

    [SerializeField]
    private float maxTimer;

    [SerializeField]
    private float minTimer;

    public float trapTimer;

    public float trapTimerRate;

    private bool armed;

    [SerializeField]
    private MeshRenderer trapRenderer;

    [SerializeField]
    private CapsuleCollider capsuleCollider;

    private Color originalColor;

    private void Start()
    {
        originalColor = trapRenderer.material.color;

        //EventManager.lurkerChangedFormEvent.AddListener(OnLurkerGoBackToGhostForm);
        //int monster = 0;

        // get the kind of monster that is in the game and then call the corrisponding trap logic for the monster
        /*
        switch (monster)
        {
            case 0:
                StartCoroutine(DoLurkerTraps());
                break;
            case 1:
                StartCoroutine(DoPhantomTraps());
                break;
            case 2:
                StartCoroutine(DoMaryTraps());
                break;
            case 3:
                StartCoroutine(DoFallenTraps());
                break;
            default:
                break;

        }
        */

    }

    public void Trigger()
    {
        trapSound.Play();
        armed = false;
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

    public void UnArm()
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

}

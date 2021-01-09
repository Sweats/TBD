using UnityEngine;

public class Door : MonoBehaviour
{
    // Start is called before the first frame update
    public int unlockMask = -1;

    public string doorName = "door";

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

    public bool locked;

    [SerializeField]
    private Renderer doorRenderer;

    [SerializeField]
    private BoxCollider doorCollider;

    void Start()
    {
        locked = true;
    }

    // for now we will play the locked sound in here. We may want to move it out at some point.
    public void PlayLockedSound()
    {
        lockedSound.Play();
    }


    public void Unlock()
    {
        unlockedSound.Play();
        locked = false;
        doorRenderer.enabled = false;
        doorCollider.enabled = false;
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
}



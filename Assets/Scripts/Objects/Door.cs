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
    private int unlockMask = -1;

    [SerializeField]
    private string doorName = "door";

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

    private bool locked;

    [SerializeField]
    private Renderer doorRenderer;

    [SerializeField]
    private BoxCollider doorCollider;

    private void Start()
    {
        locked = true;
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


    public void Unlock()
    {
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


    public int UnlockMask()
    {
        return unlockMask;
    }

    public string Name()
    {
        return doorName;
    }
}



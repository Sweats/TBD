using UnityEngine;
using UnityEngine.Events;

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

    private Renderer doorRenderer;

    private BoxCollider doorCollider;

    void Start()
    {
        locked = false;
        doorRenderer = GetComponent<Renderer>();
	doorCollider = GetComponent<BoxCollider>();
        EventManager.survivorFailedToUnlockDoorEvent.AddListener(OnSurvivorFailedToUnlockDoor);
        EventManager.survivorUnlockDoorEvent.AddListener(OnSurvivorUnlockedDoor);
    }

    // for now we will play the locked sound in here. We may want to move it out at some point.
    public void PlayLockedSound()
    {
        lockedSound.Play();
    }


    private void Unlock()
    {
        unlockedSound.Play();
        locked = false;
	doorRenderer.enabled = false;
	doorCollider.enabled = false;
    }

    private void OnSurvivorFailedToUnlockDoor(Door door)
    {
        door.PlayLockedSound();
    }


    private void OnSurvivorUnlockedDoor(Survivor survivor, Key key, Door door)
    {
        door.Unlock();
    }
}



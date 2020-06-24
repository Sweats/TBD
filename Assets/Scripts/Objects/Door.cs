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

    void Start()
    {
        locked = false;
        doorRenderer = GetComponent<Renderer>();
    }

    public void Unlock()
    {
        locked = false;
        unlockedSound.Play();
        Destroy(this.gameObject);

    }

    // for now we will play the locked sound in here. We may want to move it out at some point.
    public void PlayLockedSound()
    {
        lockedSound.Play();
    }


/*
    private bool Locked()
    {
        return locked;
    }

    public bool Unlockable(Inventory inventory, out Key correctKey)
    {
        Key[] keys = inventory.Keys();
        bool unlockable = false;
        correctKey = null;

        for (var i = 0; i < keys.Length; i++)
        {
            if (keys[i].mask == unlockMask)
            {
                correctKey = keys[i];
                unlockable = true;
                break;
            }
        }

        return unlockable;
    }
    */
}



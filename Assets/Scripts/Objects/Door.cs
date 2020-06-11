using UnityEngine;

public class Door : MonoBehaviour
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

    [SerializeField]
    private float distanceToUnlock = 20f;

    public bool locked;

    [SerializeField]
    private Key[] keys;

    private Renderer doorRenderer;

    void Start()
    {
        locked = false;
        doorRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnMouseEnter()
    {
        if (locked)
        {
            float distanceFromPlayer = 0f;

            if (distanceFromPlayer <= distanceToUnlock)
            {
                //outlineColor = 
            }
        }

    }


    public void Unlock()
    {
        locked = false;
        unlockedSound.Play();
        //Destroy(GetComponent<MeshRenderer>());
        Destroy(this.gameObject);

    }

    public void Lock()
    {
        locked = true;

    }

    public void PlayLockedSound()
    {
        lockedSound.Play();
    }

    private bool Locked()
    {
        return locked;
    }

    public bool Unlockable(Inventory inventory)
    {
        Key[] keys = inventory.Keys();
        bool unlockable = false;

        for (var i = 0; i < keys.Length; i++)
        {
            if (keys[i].keyMask == unlockMask)
            {
                unlockable = true;
                break;
            }
        }

        return unlockable;
    }

    void OnMouseExit()
    {
        if (locked)
        {
            float distanceFromPlayer = 0;

            if (distanceFromPlayer >= distanceToUnlock)
            {
                outlineColor = Color.clear;
            }
        }
    }
}
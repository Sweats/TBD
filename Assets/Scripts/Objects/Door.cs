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
    }

    public void Lock()
    {
        locked = true;
        
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


    void OnMouseDown()
    {
        bool found = false;

        for (var i = 0; i < keys.Length; i++)
        {
            int keyMask = keys[i].keyMask;

            if (keyMask == unlockMask)
            {
                unlockedSound.Play();
                found = true;
                Unlock();
                break;
            }
        }

        if (!found)
        {
            lockedSound.Play();
        }
    }
}

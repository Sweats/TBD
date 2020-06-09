using UnityEngine;

public class InsanityEffect : MonoBehaviour
{
    public enum Insanity
    {
        None = 0,
        Gamma,
        Deaf
    }


    [SerializeField]
    private Insanity effect;

    [SerializeField]
    private bool enabled;

    [SerializeField]
    private float insanityNeededToStart;

    [SerializeField]
    private float minTimer;

    [SerializeField]
    private float maxTimer;



    // Start is called before the first frame update
    void Start()
    {

    }

    public bool Enabled()
    {
        return enabled;
    }
}


    


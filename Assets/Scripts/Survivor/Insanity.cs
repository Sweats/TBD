using UnityEngine;
public class Insanity : MonoBehaviour
{
    [SerializeField]
    private float maxInsanity;

    [SerializeField]
    private float insanityValue;

    [SerializeField]
    private bool insanityEnabled;

    public float Value()
    {
        return insanityValue;
    }

    public float Max()
    {
        return maxInsanity;

    }

    public void Increment(float amount)
    {
        if (insanityEnabled)
        {
            this.insanityValue += amount;

        }
    }
}

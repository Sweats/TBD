using UnityEngine;

public class Keylist : MonoBehaviour
{
    [SerializeField]
    private Key[] keys;

    // Not sure if we will need this
    public Key[] GetKeysInStage()
    {
        return keys;

    }

}

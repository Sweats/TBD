using UnityEngine;

public class ObjectSpawnPoint : MonoBehaviour
{
    [SerializeField]
    private Key[] keys;

    void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;

    }

    public Key[] Keys()
    {
	    return keys;
    }

}

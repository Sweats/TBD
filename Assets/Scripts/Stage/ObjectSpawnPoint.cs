using UnityEngine;
using Mirror;

public class ObjectSpawnPoint : NetworkBehaviour
{
    [SerializeField]
    private Key[] keys;

    private void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;

    }

    public Key[] Keys()
    {
	      return keys;
    }

}

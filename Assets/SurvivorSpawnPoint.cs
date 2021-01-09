using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SurvivorSpawnPoint : NetworkBehaviour
{
    private bool spawnPointUsed;

    private void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }

    public bool Used()
    {
        return spawnPointUsed;
    }

}

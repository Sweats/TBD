using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivorSpawnPoint : MonoBehaviour
{
    private bool spawnPointUsed = false;

    private void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }

    public bool Used()
    {
        return spawnPointUsed;
    }

    public void SetUsed()
    {
        spawnPointUsed = true;
    }

}

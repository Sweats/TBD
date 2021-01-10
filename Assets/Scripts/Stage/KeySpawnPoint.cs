using UnityEngine;

[System.Serializable]
public struct KeysAtSpawnPoint
{
    public string keyName;
    public int mask;
    public int pathID;
    public KeyType type;
}

public class KeySpawnPoint : MonoBehaviour
{
    [SerializeField]
    private KeysAtSpawnPoint[] potentialKeySpawns;

    private void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }

    public KeysAtSpawnPoint[] SpawnableKeysAtPoint()
    {
        return potentialKeySpawns;

    }
}

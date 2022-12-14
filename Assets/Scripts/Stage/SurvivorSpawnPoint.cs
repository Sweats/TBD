using UnityEngine;

public class SurvivorSpawnPoint : MonoBehaviour
{
    private bool spawnPointUsed = false;

    [Header("Cube appears only in the editor.")]

    [SerializeField]
    private float x;

    [SerializeField]
    private float y;

    [SerializeField]
    private float z;

    [SerializeField]
    private Color color = Color.green;

    public bool Used()
    {
        return spawnPointUsed;
    }

    public void SetUsed()
    {
        spawnPointUsed = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawCube(transform.position, new Vector3(x, y, z));
    }

}

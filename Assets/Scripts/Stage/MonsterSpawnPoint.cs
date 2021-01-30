using UnityEngine;

public class MonsterSpawnPoint: MonoBehaviour
{
    [Header("Cube appears only in the editor.")]

    [SerializeField]
    private float x;

    [SerializeField]
    private float y;

    [SerializeField]
    private float z;

    [SerializeField]
    private Color color = Color.red;

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawCube(transform.position, new Vector3(x, y, z));
    }
}


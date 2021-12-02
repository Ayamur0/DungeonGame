using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public float Range;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, Range);
    }
}
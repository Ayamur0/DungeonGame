using System.Collections.Generic;
using UnityEngine;

public class RoomContent : MonoBehaviour
{
    public List<EnemySpawn> EnemySpawns = new List<EnemySpawn>();

    public List<Transform> GetRandomSpawns(int amount)
    {
        List<Transform> spawnPositions = new List<Transform>();
        if (this.EnemySpawns.Count > 0)
        {
            for (int i = 0; i < amount; i++)
            {
                var randomIndex = Random.Range(0, this.EnemySpawns.Count - 1);
                var maxRange = this.EnemySpawns[randomIndex].Range;
                var randomRange = Random.Range(0f, maxRange);

                float angle = (float)(2.0 * Mathf.PI * Random.Range(0f, 1f));
                var center = this.EnemySpawns[randomIndex].transform.position;
                var x = center.x + randomRange * Mathf.Cos(angle);
                var y = center.y + randomRange * Mathf.Sin(angle);

                spawnPositions.Add(this.EnemySpawns[randomIndex].transform);
            }
        }

        return spawnPositions;
    }
}
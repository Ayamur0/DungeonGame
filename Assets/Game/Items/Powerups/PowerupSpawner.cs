using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour {
    public GameObject[] powerups;
    public GameObject[] weapons;
    public GameObject coin;

    public List<GameObject> SpawnPowerups(List<Vector3> positions) {
        List<GameObject> spawnedPowerups = new List<GameObject>();
        List<int> usedIndices = new List<int>();
        foreach (Vector3 pos in positions) {
            int r = Random.Range(0, powerups.Length);
            while (usedIndices.Contains(r))
                r = Random.Range(0, powerups.Length);
            GameObject temp = Instantiate(powerups[r]);
            temp.transform.position = pos;
            spawnedPowerups.Add(temp);
            usedIndices.Add(r);
            if (usedIndices.Count >= powerups.Length / 2)
                usedIndices.Clear();
        }
        return spawnedPowerups;
    }

    public void SpawnCoins(List<Vector3> positions) {
        foreach (Vector3 pos in positions) {
            GameObject temp = Instantiate(coin);
            temp.transform.position = pos;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour {
    public static GameObject[] powerups;

    public static void spawnPowerups(List<Vector3> positions) {
        List<int> usedIndices = new List<int>();
        foreach (Vector3 pos in positions) {
            int r = Random.Range(0, powerups.Length);
            while (usedIndices.Contains(r))
                r = Random.Range(0, powerups.Length);
            GameObject temp = Instantiate(powerups[r]);
            temp.transform.position = pos;
            usedIndices.Add(r);
            if (usedIndices.Count >= powerups.Length / 2)
                usedIndices.Clear();
        }
    }
}

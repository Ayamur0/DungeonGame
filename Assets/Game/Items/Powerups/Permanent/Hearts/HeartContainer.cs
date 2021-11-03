using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartContainer : Pickupable {
    public int size = 1;

    override protected bool Effect(PlayerStats stats) {
        stats.AddHeartContainers(size);
        return true;
    }
}

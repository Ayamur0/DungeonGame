using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartContainer : Powerup {
    public int size = 1;

    override protected bool PickupEffect(PlayerStats stats) {
        stats.AddHeartContainers(size);
        return true;
    }
}

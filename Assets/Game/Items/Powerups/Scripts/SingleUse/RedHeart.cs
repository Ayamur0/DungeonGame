using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedHeart : Powerup {
    public float size = 1;

    override protected bool PickupEffect(PlayerStats stats) {
        stats.AddRedHearts(size);
        return true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHeart : Powerup {
    public float size = 1;

    override protected bool PickupEffect(PlayerStats stats) {
        stats.AddBlackHearts(size);
        return true;
    }
}

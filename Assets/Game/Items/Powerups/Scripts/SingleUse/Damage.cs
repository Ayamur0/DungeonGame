using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : Powerup {
    public float size = 1;

    override protected bool PickupEffect(PlayerStats stats) {
        stats.TakeDamage(size);
        return true;
    }
}

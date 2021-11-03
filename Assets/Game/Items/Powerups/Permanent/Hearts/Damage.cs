using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : Pickupable {
    public float size = 1;

    override protected bool Effect(PlayerStats stats) {
        stats.TakeDamage(size);
        return true;
    }
}

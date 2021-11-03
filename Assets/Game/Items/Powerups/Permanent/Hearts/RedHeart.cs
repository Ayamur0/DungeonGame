using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedHeart : Pickupable {
    public float size = 1;

    override protected bool Effect(PlayerStats stats) {
        stats.AddRedHearts(size);
        return true;
    }
}

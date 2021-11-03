using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHeart : Pickupable {
    public float size = 1;

    override protected bool Effect(PlayerStats stats) {
        stats.AddBlackHearts(size);
        return true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulHeart : Pickupable {
    public float size = 0.5f;

    override protected bool Effect(PlayerStats stats) {
        stats.AddSoulHearts(size);
        return true;
    }
}

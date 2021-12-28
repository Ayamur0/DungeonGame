using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulHeart : Powerup {
    public float size = 0.5f;

    override protected bool PickupEffect(PlayerStats stats) {
        stats.AddSoulHearts(size);
        return true;
    }

    public override string GetDescription() {
        return "";
    }
}

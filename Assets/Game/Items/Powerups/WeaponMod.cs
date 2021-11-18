using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponMod : Powerup {
    public abstract void modProjectiles(List<GameObject> projectiles);

    protected override bool PickupEffect(PlayerStats stats) {
        return inventory.addWeaponMod(this);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItem : Powerup {
    public float flatMoveSpeed = 0;
    public float percentMoveSpeed = 1;
    public float flatAttackSpeed = 0;
    public float percentAttackSpeed = 1;
    public float flatProjectileSpeed = 0;
    public float percentProjectileSpeed = 1;
    public int flatDamage = 0;
    public float percentDamage = 1;
    public float flatRange = 0;
    public int critChance = 0;
    public int percentCritDamage = 0;
    public int lifesteal = 0;
    public bool weaponPiercing = false;
    public bool overchargeActiveItems = false;


    override protected bool PickupEffect(PlayerStats stats) {
        return inventory.addPassiveItem(this);
    }
}

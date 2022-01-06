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
    public int luck = 0;
    public int percentCritDamage = 0;
    public bool overchargeActiveItems = false;


    override protected bool PickupEffect(PlayerStats stats) {
        return inventory.addPassiveItem(this);
    }

    public override string GetDescription() {
        string s = "";
        if (flatDamage != 0)
            s += GetStatString("Damage", flatDamage);
        if (percentDamage != 1)
            s += GetStatString("Damage", percentDamage, "x");
        if (flatMoveSpeed != 0)
            s += GetStatString("Move Speed", flatMoveSpeed);
        if (percentMoveSpeed != 1)
            s += GetStatString("Move Speed", percentMoveSpeed, "x");
        if (flatAttackSpeed != 0)
            s += GetStatString("Attack CDR", flatAttackSpeed, "", "%");
        if (percentAttackSpeed != 1)
            s += GetStatString("Attack CDR", percentAttackSpeed, "x");
        if (flatProjectileSpeed != 0)
            s += GetStatString("Projectile Speed", flatProjectileSpeed);
        if (percentProjectileSpeed != 1)
            s += GetStatString("Projectile Speed", percentProjectileSpeed, "x");
        if (flatRange != 0)
            s += GetStatString("Range", flatRange);
        if (luck != 0)
            s += GetStatString("Luck", luck, "", "%");
        if (percentCritDamage != 0)
            s += GetStatString("Crit Damage", percentCritDamage, "", "%");
        return s;
    }

    private string GetStatString(string stat, float value, string sign = "", string unit = "") {
        if (sign == "" && value > 0)
            sign = "+";
        return $"<align=left>{stat}:<line-height=0>\n<align=right>{sign}{value}{unit}<line-height=1em>\n";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : Powerup {
    public GameObject projectile;
    private float timeSinceLastAttack = .0f;
    private float cooldown = 0.5f;
    protected PlayerStats playerStats;

    override protected void Update() {
        if (!activated)
            return;
        timeSinceLastAttack += Time.deltaTime;
        if (Input.GetKey("up") || Input.GetKey("down") || Input.GetKey("left") || Input.GetKey("right")) {
            if (timeSinceLastAttack >= cooldown / playerStats.attackCooldownReduction) {
                timeSinceLastAttack = 0;
                Attack();
            }
        }
    }

    abstract public void Attack();

    override protected bool PickupEffect(PlayerStats stats) {
        playerStats = stats;
        return inventory.addWeapon(this);
    }
}

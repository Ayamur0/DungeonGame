using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveItem : Powerup {
    protected int initialCooldown = 2;
    public int cooldown = 0;
    protected PlayerStats playerStats;

    abstract public void ActivationEffect();

    public void reduceCooldown() {
        if (cooldown > 0)
            cooldown--;
    }

    override protected bool PickupEffect(PlayerStats stats) {
        playerStats = stats;
        return inventory.addActiveItem(this);
    }

    protected override void Update() {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Space) && cooldown == 0)
            ActivationEffect();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveItem : Powerup {
    abstract public void ActivationEffect();

    override protected bool PickupEffect(PlayerStats stats) {
        return true;
    }
}

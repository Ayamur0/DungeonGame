using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballWeapon : Weapon {
    private float missileSpeed = 10f;

    public override void Attack() {
        List<GameObject> fireballObjs = new List<GameObject>();
        fireballObjs.Add(Instantiate(projectile, playerStats.transform.position, playerStats.transform.rotation));
        if (inventory.WeaponMod != null)
            inventory.WeaponMod.modProjectiles(fireballObjs);
        foreach (GameObject fireballObj in fireballObjs) {
            FireballController fireballController = fireballObj.GetComponent<FireballController>();
            fireballController.Speed = missileSpeed * playerStats.projectileSpeedMultiplier;
            fireballController.Lifetime = playerStats.range / fireballController.Speed;
        }
    }
}

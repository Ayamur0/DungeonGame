using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombWeapon : Weapon {
    private float missileSpeed = 10f;

    public override void Attack() {
        List<GameObject> bombObjs = new List<GameObject>();
        bombObjs.Add(Instantiate(projectile, playerStats.transform.position, playerStats.transform.rotation));
        if (inventory.WeaponMod != null)
            inventory.WeaponMod.modProjectiles(bombObjs);
        foreach (GameObject bombObj in bombObjs) {
            BombController bombController = bombObj.GetComponent<BombController>();
            bombController.Speed = missileSpeed * playerStats.projectileSpeedMultiplier;
            bombController.Lifetime = playerStats.range / 10 / bombController.Speed;
        }
    }
}

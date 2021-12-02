using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EightShot : ActiveItem {
    public override void ActivationEffect() {
        GameObject projectile = inventory.Weapon.projectile;
        if (projectile == null)
            return;
        List<GameObject> projectiles = new List<GameObject>();
        for (int i = 0; i < 8; i++)
            projectiles.Add(Instantiate(projectile, inventory.transform.position, Quaternion.Euler(0, (inventory.transform.rotation.eulerAngles.y + 45 * i) % 360, 0)));
        cooldown = initialCooldown;
    }
}

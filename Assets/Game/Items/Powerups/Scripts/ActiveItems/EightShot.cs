using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EightShot : ActiveItem {
    public override void ActivationEffect() {
        if (inventory.Weapon == null)
            return;
        GameObject projectile = inventory.Weapon.projectile;
        if (projectile == null)
            return;
        List<GameObject> projectiles = new List<GameObject>();
        Vector3 pos = inventory.transform.position;
        pos.y++;
        for (int i = 0; i < 8; i++)
            projectiles.Add(Instantiate(projectile, pos, Quaternion.Euler(0, (inventory.transform.rotation.eulerAngles.y + 45 * i) % 360, 0)));
        foreach (GameObject proj in projectiles) {
            FireballController fireballController = proj.GetComponent<FireballController>();
            BombController bombController = proj.GetComponent<BombController>();
            if (fireballController != null) {
                fireballController.playerStats = playerStats;
                fireballController.Speed = 10 * playerStats.projectileSpeedMultiplier;
                fireballController.Lifetime = playerStats.range / fireballController.Speed;
            } else if (bombController != null) {
                bombController.playerStats = playerStats;
                bombController.Speed = 10 * playerStats.projectileSpeedMultiplier;
                bombController.Lifetime = playerStats.range / 2 / bombController.Speed;
            }
        }
        cooldown = initialCooldown;
    }

    public override string GetDescription() {
        return "Fires eight Projectiles, one in each Direction.\nCooldown: 2 Rooms";
    }
}

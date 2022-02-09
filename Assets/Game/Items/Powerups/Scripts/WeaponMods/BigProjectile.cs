using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigProjectile : WeaponMod {
    public override void modProjectiles(List<GameObject> projectiles) {
        foreach (GameObject p in projectiles) {
            p.GetComponent<Collider>().transform.localScale *= 1.25f;
            p.transform.localScale *= 1.25f;
        }
    }

    public override string GetDescription() {
        return "Increases Projectile size by 125%";
    }
}

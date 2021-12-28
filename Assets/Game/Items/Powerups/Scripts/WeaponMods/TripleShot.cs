using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleShot : WeaponMod {
    public override void modProjectiles(List<GameObject> projectiles) {
        projectiles.Add(Instantiate(projectiles[0], projectiles[0].transform.position, Quaternion.Euler(0, projectiles[0].transform.rotation.eulerAngles.y + 45, 0)));
        projectiles.Add(Instantiate(projectiles[0], projectiles[0].transform.position, Quaternion.Euler(0, projectiles[0].transform.rotation.eulerAngles.y - 45, 0)));
    }

    public override string GetDescription() {
        return "Makes your Weapon fire three Projectiles";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballWeapon : Weapon {
    public GameObject fireball;
    private float missileSpeed = .1f;

    public override void Attack() {
        ArrayList fireballObjs = new ArrayList();
        fireballObjs.Add(Instantiate(fireball, playerStats.transform.position, playerStats.transform.rotation));
        // if (stats.tripleShot) {
        //     fireballObjs.Add(Instantiate(fireball, transform.position, Quaternion.Euler(0, transform.rotation.eulerAngles.y + 45, 0)));
        //     fireballObjs.Add(Instantiate(fireball, transform.position, Quaternion.Euler(0, transform.rotation.eulerAngles.y - 45, 0)));
        // }
        foreach (GameObject fireballObj in fireballObjs) {
            FireballController fireballController = fireballObj.GetComponent<FireballController>();
            fireballController.Speed = missileSpeed * playerStats.missileSpeedMultiplier;
        }
    }
}

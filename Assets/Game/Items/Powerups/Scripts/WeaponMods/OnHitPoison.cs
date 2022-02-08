using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHitPoison : WeaponMod {
    public override void modProjectiles(List<GameObject> projectiles) {
        foreach (GameObject p in projectiles) {
            p.GetComponent<Projectile>().SetOnHitEffect(inventory.GetComponent<PlayerStats>().luck, 1, PoisonEffect);
        }
    }

    public void PoisonEffect(GameObject target) {
        IEnumerator coroutine = PoisonEffectRoutine(target);
        StartCoroutine(coroutine);
    }

    public IEnumerator PoisonEffectRoutine(GameObject target) {
        for (int i = 0; i < 5; i++) {
            yield return new WaitForSeconds(0.25f);
            target.GetComponent<EnemyHealth>().ReceivePoisonDamage(1.0f);
        }
    }

    public override string GetDescription() {
        return "Adds Poison Effect to your Weapon that is triggered by your luck.\n"
                + "Does 1 Damage every 0.25 Seconds for 1.25 Seconds";
    }
}

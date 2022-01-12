using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningWeapon : Weapon {
    private float timeSinceLastDamageTick = 0;
    private bool lightningActive;
    private GameObject currentTarget;
    private GameObject projectileInstance;
    private float damageModifier = 0.5f;
    private List<System.Type> disabledWeaponMods = new List<System.Type> { typeof(TripleShot), typeof(BigProjectile) };
    private List<GameObject> projectileList;

    void applyWeaponMod() {
        if (inventory.WeaponMod == null)
            return;
        bool weaponModAllowed = true;
        foreach (System.Type t in disabledWeaponMods) {
            if (inventory.WeaponMod.GetType() == t) {
                weaponModAllowed = false;
                break;
            }
        }
        if (weaponModAllowed) {
            inventory.WeaponMod.modProjectiles(projectileList);
        }
    }

    override protected void Update() {
        if (!activated)
            return;
        if (projectileInstance == null) {
            projectileInstance = Instantiate(projectile, playerStats.transform.position, playerStats.transform.rotation);
            projectileList = new List<GameObject> { projectileInstance };
            applyWeaponMod();
        }
        timeSinceLastDamageTick += Time.deltaTime;
        if (Input.GetKey("up") || Input.GetKey("down") || Input.GetKey("left") || Input.GetKey("right")) {
            lightningActive = true;
            if (timeSinceLastDamageTick >= cooldown / playerStats.attackCooldownReduction) {
                timeSinceLastDamageTick = 0;
                DealDamage();
            }
        } else {
            lightningActive = false;
        }
        UpdateLightningBolt();
    }

    private void DealDamage() {
        if (currentTarget != null) {
            if (projectileInstance.GetComponent<Projectile>().onHitEffect != null)
                projectileInstance.GetComponent<Projectile>().onHitEffect(currentTarget);
            // reaply weapon mod to reset onhit effect
            applyWeaponMod();
            currentTarget.GetComponent<EnemyHealth>().ReceiveDamage(playerStats.GetDamage() * damageModifier);
        }
    }

    private void UpdateLightningBolt() {
        projectileInstance.GetComponent<LineRenderer>().enabled = lightningActive;
        projectileInstance.GetComponent<DigitalRuby.LightningBolt.LightningBoltScript>().enabled = lightningActive;
        if (!lightningActive) {
            return;
        }
        projectileInstance.GetComponent<DigitalRuby.LightningBolt.LightningBoltScript>().StartObject = playerStats.gameObject;
        Collider nearestEnemy = GetNearestCollision(projectileInstance.transform.GetChild(0).GetComponent<SphereCollider>());
        if (nearestEnemy != null)
            currentTarget = nearestEnemy.gameObject;
        else
            currentTarget = null;
        if (nearestEnemy == null) {
            lightningActive = false;
            projectileInstance.GetComponent<LineRenderer>().enabled = false;
            projectileInstance.GetComponent<DigitalRuby.LightningBolt.LightningBoltScript>().enabled = false;
            return;
        }
        projectileInstance.GetComponent<DigitalRuby.LightningBolt.LightningBoltScript>().EndObject = nearestEnemy.gameObject;
    }

    private Collider GetNearestCollision(SphereCollider collider) {
        collider.center = playerStats.transform.position;
        Collider[] colliders = Physics.OverlapSphere(collider.center, playerStats.range / 2);
        Collider nearestCollider = null;
        float minSqrDistance = Mathf.Infinity;

        for (int i = 0; i < colliders.Length; i++) {
            if (colliders[i].tag != "Enemy")
                continue;
            float sqrDistanceToCenter = (collider.center - colliders[i].transform.position).sqrMagnitude;

            if (sqrDistanceToCenter < minSqrDistance) {
                minSqrDistance = sqrDistanceToCenter;
                nearestCollider = colliders[i];
            }
        }
        return nearestCollider;
    }

    public override void Attack() { }

    public override string GetDescription() {
        return "A short ranged Auto Aim Weapon";
    }
}
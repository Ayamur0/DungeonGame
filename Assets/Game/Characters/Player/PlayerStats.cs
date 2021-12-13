using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {
    private const float MIN_MOVESPEED = 0.02f;
    private const float MAX_MOVESPEED = 0.15f;
    private const float DEFAULT_MOVESPEED = 0.05f;
    private const float MIN_COOLDOWN = 0.1f;
    private const float DEFAULT_COOLDOWN = 1f;
    private const float MAX_COOLDOWN = 5f;
    private const float MIN_DAMAGE = 0.1f;
    private const float DEFAULT_DAMAGE = 1f;
    private const float MAX_DAMAGE = 5f;
    private const float MIN_PROJECTILE_SPEED = 0.25f;
    private const float DEFAULT_PROJECTILE_SPEED = 0.25f;
    private const float MAX_PROJECTILE_SPEED = 5f;
    private const float MIN_RANGE = 2;
    private const float DEFUALT_RANGE = 10;
    private const float MAX_RANGE = 100;
    private const int MAX_CRIT_DAMAGE = 500;
    private const int DEFAULT_CRIT_DAMAGE = 50;
    private const int DEFAULT_LUCK = 10;


    public float movespeed = .05f;
    public float attackCooldownReduction = 1f;
    public float attackDamage = 1;
    public float projectileSpeedMultiplier = 1f;
    public float range = 10;
    public int luck = 10;
    public int critDamage = 50;
    public bool overchargeActiveItems = false;

    public int startHearts = 3;
    public int maxHearts = 15;
    public float blackHearts = 0;
    public float soulHearts = 0;
    public float redHearts;
    private float totalHearts;
    public int heartContainers;
    [HideInInspector]
    public int healthPerHeart = 2;
    // Start is called before the first frame update
    void Start() {
        heartContainers = startHearts;
        redHearts = startHearts;
        totalHearts = startHearts;
    }

    // Update is called once per frame
    void Update() {
        totalHearts = redHearts + blackHearts + soulHearts;
        // if (Input.GetKeyDown(KeyCode.E))
        //     TakeDamage(1);
        // if (Input.GetKeyDown(KeyCode.R))
        //     AddHeartContainers(1);
        // if (Input.GetKeyDown(KeyCode.T))
        //     AddRedHearts(1);
        // if (Input.GetKeyDown(KeyCode.Z))
        //     AddBlackHearts(1);
        // if (Input.GetKeyDown(KeyCode.U))
        //     AddSoulHearts(1);
    }

    public void TakeDamage(float amount) {
        while (amount > 0) {
            if (soulHearts > 0) {
                soulHearts -= 0.5f;
                if (soulHearts % 1 == 0)
                    heartContainers--;
            } else if (blackHearts > 0) {
                blackHearts -= 0.5f;
                if (blackHearts % 1 == 0) {
                    Collider[] colliders = Physics.OverlapSphere(transform.position, 8);
                    for (int i = 0; i < colliders.Length; i++) {
                        if (colliders[i].tag == "Enemy") {
                            // deal damage
                        }
                    }
                }
            } else if (redHearts > 0) {
                redHearts -= 0.5f;
            } else {
                // die
            }
            amount -= 0.5f;
        }
    }

    public void AddRedHearts(float amount) {
        if (totalHearts + amount <= heartContainers) {
            totalHearts += amount;
            redHearts += amount;
        } else {
            totalHearts = heartContainers;
            redHearts += heartContainers - totalHearts;
        }
    }

    public void AddBlackHearts(float amount) {
        if (totalHearts + amount <= heartContainers) {
            totalHearts += amount;
            blackHearts += amount;
        } else {
            totalHearts = heartContainers;
            float difference = amount - (heartContainers - totalHearts);
            if (redHearts >= difference) {
                redHearts -= difference;
                blackHearts += amount;
            } else {
                blackHearts += heartContainers - totalHearts + redHearts;
                redHearts = 0;
            }
        }
    }

    public void AddSoulHearts(float amount) {
        int heartContainerAmount;
        if (amount % 1 == 0)
            heartContainerAmount = (int)amount;
        else
            heartContainerAmount = soulHearts % 1 == 0 ? (int)(amount + 0.5) : (int)amount;
        int heartsAdded = AddHeartContainers(heartContainerAmount, false);
        float freeSpace = heartsAdded + soulHearts % 1;
        if (freeSpace >= amount)
            soulHearts += amount;
        else
            soulHearts += freeSpace;
    }

    public void ApplySoulHearts() {
        redHearts += soulHearts;
        soulHearts = 0;
    }

    public int AddHeartContainers(int amount, bool heal = true) {
        if (heartContainers + amount > maxHearts)
            amount = maxHearts - heartContainers;
        if (heal)
            redHearts += amount;
        heartContainers += amount;
        return amount;
    }

    public float GetDamage() {
        float damage = attackDamage;
        int r = Random.Range(0, 100);
        if (r <= luck) {
            damage *= critDamage / 100 + 1;
        }
        return damage;
    }

    public void updateStats(PassiveItem[] items) {
        movespeed = DEFAULT_MOVESPEED;
        attackCooldownReduction = DEFAULT_COOLDOWN;
        projectileSpeedMultiplier = DEFAULT_PROJECTILE_SPEED;
        attackDamage = DEFAULT_DAMAGE;
        critDamage = DEFAULT_CRIT_DAMAGE;
        luck = DEFAULT_LUCK;
        range = DEFUALT_RANGE;
        float moveSpeedModifier = 1;
        float attackSpeedModifier = 1;
        float projectileSpeedModifier = 1;
        float damageModifier = 1;
        foreach (PassiveItem i in items) {
            if (i == null)
                continue;
            movespeed += i.flatMoveSpeed;
            moveSpeedModifier += i.percentMoveSpeed;
            attackCooldownReduction += i.flatAttackSpeed;
            attackSpeedModifier += i.percentAttackSpeed;
            projectileSpeedMultiplier += i.flatProjectileSpeed;
            projectileSpeedModifier += i.percentProjectileSpeed;
            attackDamage += i.flatDamage;
            damageModifier += i.percentDamage;
            range += i.flatRange;
            luck += i.luck;
            critDamage += i.percentCritDamage;
            overchargeActiveItems = overchargeActiveItems || i.overchargeActiveItems;
        }
        movespeed = Mathf.Clamp(movespeed * moveSpeedModifier, MIN_MOVESPEED, MAX_MOVESPEED);
        attackCooldownReduction = Mathf.Clamp(attackCooldownReduction * attackSpeedModifier, MIN_COOLDOWN, MAX_COOLDOWN);
        projectileSpeedMultiplier = Mathf.Clamp(projectileSpeedMultiplier * projectileSpeedModifier, MIN_PROJECTILE_SPEED, MAX_PROJECTILE_SPEED);
        attackDamage = Mathf.Clamp(attackDamage * damageModifier, MIN_DAMAGE, MAX_DAMAGE);
        range = Mathf.Clamp(range, MIN_RANGE, MAX_RANGE);
        luck = Mathf.Clamp(0, luck, 100);
        critDamage = Mathf.Clamp(critDamage, 0, MAX_CRIT_DAMAGE);
    }
}

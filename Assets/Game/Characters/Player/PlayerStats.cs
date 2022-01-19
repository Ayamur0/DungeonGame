using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour {
    private const float MIN_MOVESPEED = 15f;
    private const float MAX_MOVESPEED = 40f;
    private const float DEFAULT_MOVESPEED = 18f;
    private const float MIN_COOLDOWN = 0.1f;
    private const float DEFAULT_COOLDOWN = 1f;
    private const float MAX_COOLDOWN = 5f;
    private const float MIN_DAMAGE = 0.1f;
    private const float DEFAULT_DAMAGE = 1f;
    private const float MAX_DAMAGE = 5f;
    private const float MIN_PROJECTILE_SPEED = 0.25f;
    private const float DEFAULT_PROJECTILE_SPEED = 1f;
    private const float MAX_PROJECTILE_SPEED = 5f;
    private const float MIN_RANGE = 2;
    private const float DEFUALT_RANGE = 10;
    private const float MAX_RANGE = 100;
    private const int MAX_CRIT_DAMAGE = 500;
    private const int DEFAULT_CRIT_DAMAGE = 50;
    private const int DEFAULT_LUCK = 10;


    public float movespeed = 18f;
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

    public Image PlayerStatsUI;
    public Image ItemStatsUI;
    public Image WeaponStatsUI;
    public GameObject Minimap;

    // Start is called before the first frame update
    void Start() {
        heartContainers = startHearts;
        redHearts = startHearts;
        totalHearts = startHearts;
    }

    // Update is called once per frame
    void Update() {
        totalHearts = redHearts + blackHearts + soulHearts;
        if (Input.GetKey(KeyCode.Tab))
            ShowStatsUI();
        else {
            Minimap.SetActive(true);
            ItemStatsUI.gameObject.SetActive(false);
            PlayerStatsUI.gameObject.SetActive(false);
            WeaponStatsUI.gameObject.SetActive(false);
        }
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
                        if (colliders[i].GetComponent<EnemyController>() != null) {
                            colliders[i].GetComponent<EnemyHealth>().ReceiveDamage(1);
                        }
                    }
                }
            } else if (redHearts > 0) {
                redHearts -= 0.5f;
            } else {
                FindObjectOfType<DeathMenu>().Open();
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
            moveSpeedModifier *= i.percentMoveSpeed;
            attackCooldownReduction += i.flatAttackSpeed;
            attackSpeedModifier *= i.percentAttackSpeed;
            projectileSpeedMultiplier += i.flatProjectileSpeed;
            projectileSpeedModifier *= i.percentProjectileSpeed;
            attackDamage += i.flatDamage;
            damageModifier *= i.percentDamage;
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

    private void ShowStatsUI() {
        Minimap.SetActive(false);
        ItemStatsUI.gameObject.SetActive(true);
        PlayerStatsUI.gameObject.SetActive(true);
        WeaponStatsUI.gameObject.SetActive(true);
        PlayerStatsUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"<align=left>Attack Damage:<line-height=0>\n<align=right>{attackDamage}<line-height=1em>\n"
        + $"<align=left>Move Speed:<line-height=0>\n<align=right>{movespeed}<line-height=1em>\n"
        + $"<align=left>Attack CDR:<line-height=0>\n<align=right>{attackCooldownReduction * 100}%<line-height=1em>\n"
        + $"<align=left>Projectile Speed:<line-height=0>\n<align=right>{projectileSpeedMultiplier}<line-height=1em>\n"
        + $"<align=left>Range:<line-height=0>\n<align=right>{range}<line-height=1em>\n"
        + $"<align=left>Luck:<line-height=0>\n<align=right>{luck}%<line-height=1em>\n"
        + $"<align=left>Crit Damage:<line-height=0>\n<align=right>{critDamage}%<line-height=1em>\n";

        Inventory inventory = GetComponent<Inventory>();

        for (int i = 0; i < 8; i++) {
            Transform transform;
            if (i < 3)
                transform = WeaponStatsUI.transform.GetChild(i);
            else
                transform = ItemStatsUI.transform.GetChild(i - 3);
            Image image = transform.GetComponent<Image>();
            TextMeshProUGUI text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();

            if (i == 0) {
                if (inventory.Weapon != null) {
                    image.enabled = true;
                    text.enabled = true;
                    image.sprite = inventory.Weapon.sprite;
                    text.text = inventory.Weapon.GetDescription();
                } else {
                    text.enabled = false;
                    image.enabled = false;
                }
            } else if (i == 1) {
                if (inventory.WeaponMod != null) {
                    image.enabled = true;
                    text.enabled = true;
                    image.sprite = inventory.WeaponMod.sprite;
                    text.text = inventory.WeaponMod.GetDescription();
                } else {
                    text.enabled = false;
                    image.enabled = false;
                }
            } else if (i == 2) {
                if (inventory.ActiveItem != null) {
                    image.enabled = true;
                    text.enabled = true;
                    image.sprite = inventory.ActiveItem.sprite;
                    text.text = inventory.ActiveItem.GetDescription();
                } else {
                    text.enabled = false;
                    image.enabled = false;
                }
            } else {
                if (inventory.PassiveItems[i - 3] != null) {
                    image.enabled = true;
                    text.enabled = true;
                    image.sprite = inventory.PassiveItems[i - 3].sprite;
                    text.text = inventory.PassiveItems[i - 3].GetDescription();
                } else {
                    text.enabled = false;
                    image.enabled = false;
                }
            }
        }
    }
}

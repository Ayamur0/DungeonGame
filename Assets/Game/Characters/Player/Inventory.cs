using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
    [HideInInspector]
    public Weapon Weapon;
    [HideInInspector]
    public WeaponMod WeaponMod;
    [HideInInspector]
    public ActiveItem ActiveItem;
    [HideInInspector]
    public PassiveItem[] PassiveItems = new PassiveItem[5];

    public Image WeaponSlot;
    public Image WeaponModSlot;
    public Image ActiveSlot;
    public Image[] PassiveSlots;

    private int Money = 0;
    public Text MoneyDisplay;

    // Update is called once per frame
    void Update() {
        processInputs();
        if (Weapon == null) {
            WeaponSlot.enabled = false;
        } else {
            WeaponSlot.sprite = Weapon.sprite;
            WeaponSlot.enabled = true;
        }
        if (WeaponMod == null) {
            WeaponModSlot.enabled = false;
        } else {
            WeaponModSlot.sprite = WeaponMod.sprite;
            WeaponModSlot.enabled = true;
        }
        if (ActiveItem == null) {
            ActiveSlot.enabled = false;
        } else {
            ActiveSlot.sprite = ActiveItem.sprite;
            ActiveSlot.enabled = true;
        }
        for (int i = 0; i < PassiveItems.Length; i++) {
            if (PassiveItems[i] == null) {
                PassiveSlots[i].enabled = false;
            } else {
                PassiveSlots[i].sprite = PassiveItems[i].sprite;
                PassiveSlots[i].enabled = true;
            }
        }
    }

    public bool addWeapon(Weapon newWeapon) {
        if (Weapon != null)
            return false;
        Weapon = newWeapon;
        return true;
    }

    public void dropWeapon() {
        if (Weapon != null)
            Weapon.Drop();
        Weapon = null;
    }

    public bool addWeaponMod(WeaponMod newWeaponMod) {
        if (WeaponMod != null)
            return false;
        WeaponMod = newWeaponMod;
        return true;
    }

    public void dropWeaponMod() {
        if (WeaponMod != null)
            WeaponMod.Drop();
        WeaponMod = null;
    }

    public bool addActiveItem(ActiveItem item) {
        if (ActiveItem != null)
            return false;
        ActiveItem = item;
        return true;
    }

    public void dropActiveItem() {
        if (ActiveItem != null)
            ActiveItem.Drop();
        ActiveItem = null;
    }

    public bool addPassiveItem(PassiveItem item) {
        for (int i = 0; i < PassiveItems.Length; i++) {
            if (PassiveItems[i] != null)
                continue;
            PassiveItems[i] = item;
            GetComponent<PlayerStats>().updateStats(PassiveItems);
            return true;
        }
        return false;
    }

    public void dropPassiveItem(int index) {
        if (PassiveItems[index] != null)
            PassiveItems[index].Drop();
        PassiveItems[index] = null;
        GetComponent<PlayerStats>().updateStats(PassiveItems);
    }

    public void addMoney(int value) {
        Money += value;
        MoneyDisplay.text = "" + Money;
    }

    public int getMoney() {
        return Money;
    }

    private void processInputs() {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            dropWeapon();
        if (Input.GetKeyDown(KeyCode.Alpha2))
            dropWeaponMod();
        if (Input.GetKeyDown(KeyCode.Alpha3))
            dropActiveItem();
        if (Input.GetKeyDown(KeyCode.Alpha4))
            dropPassiveItem(0);
        if (Input.GetKeyDown(KeyCode.Alpha5))
            dropPassiveItem(1);
        if (Input.GetKeyDown(KeyCode.Alpha6))
            dropPassiveItem(2);
        if (Input.GetKeyDown(KeyCode.Alpha7))
            dropPassiveItem(3);
        if (Input.GetKeyDown(KeyCode.Alpha8))
            dropPassiveItem(4);
        if (Input.GetKeyDown(KeyCode.E)) {
            Powerup p = GetClosestPowerup();
            if (p != null)
            {
                // play sound
                GetComponent<AudioSource>().Play();
                p.Pickup(gameObject);
            }
        }
    }

    private Powerup GetClosestPowerup() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 5);
        Collider nearestCollider = null;
        float minSqrDistance = Mathf.Infinity;

        for (int i = 0; i < colliders.Length; i++) {
            if (colliders[i].tag != "Powerup")
                continue;
            float sqrDistanceToCenter = (transform.position - colliders[i].transform.position).sqrMagnitude;

            if (sqrDistanceToCenter < minSqrDistance) {
                minSqrDistance = sqrDistanceToCenter;
                nearestCollider = colliders[i];
            }
        }
        if (nearestCollider == null)
            return null;
        return nearestCollider.GetComponent<Powerup>();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
    public Weapon Weapon;
    public WeaponMod WeaponMod;
    public ActiveItem ActiveItem;
    private Powerup[] PassiveItems = new Powerup[5];

    public Image WeaponSlot;
    public Image WeaponModSlot;
    public Image ActiveSlot;
    public Image[] PassiveSlots;

    public Powerup closest;

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
        Debug.Log("Drop Weapon");
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

    public bool addPassiveItem(Powerup item) {
        for (int i = 0; i < PassiveItems.Length; i++) {
            if (PassiveItems[i] != null)
                continue;
            PassiveItems[i] = item;
            return true;
        }
        return false;
    }

    public void dropPassiveItem(int index) {
        if (PassiveItems[index] != null)
            PassiveItems[index].Drop();
        PassiveItems[index] = null;
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
        if (Input.GetKeyDown(KeyCode.E) && closest != null) {
            if (closest.Pickup(gameObject))
                closest = null;
        }
    }
}

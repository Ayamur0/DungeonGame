using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
    public Weapon Weapon;
    private Powerup WeaponMod;
    public ActiveItem ActiveItem;
    private Powerup[] PassiveItems = new Powerup[5];

    public Image WeaponSlot;
    public Image WeaponModSlot;
    public Image ActiveSlot;
    public Image[] PassiveSlots;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
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
        Weapon = null;
    }

    public bool addWeaponMod(Powerup newWeaponMod) {
        if (WeaponMod != null)
            return false;
        WeaponMod = newWeaponMod;
        return true;
    }

    public void removeWeaponMod() {
        WeaponMod = null;
    }

    public bool addActiveItem(ActiveItem item) {
        if (ActiveItem != null)
            return false;
        ActiveItem = item;
        return true;
    }

    public void removeActiveItem() {
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

    public void removePassiveItem(int index) {
        PassiveItems[index] = null;
    }
}

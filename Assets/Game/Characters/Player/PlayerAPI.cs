using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAPI : MonoBehaviour {
    public void TakeDamage(float amount) {
        GetComponent<PlayerStats>().TakeDamage(amount);
    }

    public void DecreaseActiveItemCooldown() {
        ActiveItem item = GetComponent<Inventory>().ActiveItem;
        if (item != null)
            item.reduceCooldown();
    }

    public bool Pay(int amount) {
        if (GetComponent<Inventory>().getMoney() >= amount) {
            GetComponent<Inventory>().addMoney(-amount);
            return true;
        }
        return false;
    }
}

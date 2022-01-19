using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Powerup {
    public int value = 25;

    override protected bool PickupEffect(PlayerStats stats) {
        inventory.addMoney(value);
        FindObjectOfType<GameManager>().CurrentScore.CollectedCoins += value;
        return true;
    }

    public override string GetDescription() {
        return "";
    }
}

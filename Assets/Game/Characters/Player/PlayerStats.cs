using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {
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
        if (Input.GetKeyDown(KeyCode.E))
            TakeDamage(1);
        if (Input.GetKeyDown(KeyCode.R))
            AddHeartContainers(1);
        if (Input.GetKeyDown(KeyCode.T))
            AddRedHearts(1);
        if (Input.GetKeyDown(KeyCode.Z))
            AddBlackHearts(1);
        if (Input.GetKeyDown(KeyCode.U))
            AddSoulHearts(1);
        if (Input.GetKeyDown(KeyCode.P))
            print();
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
                    // dealDamage
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

    private void print() {
        Debug.Log($"HeartContainers: {heartContainers} \nTotalHearts: {totalHearts}\nRedHearts: {redHearts}\nBlackHearts: {blackHearts}\nSoulHearts: {soulHearts}");
    }
}

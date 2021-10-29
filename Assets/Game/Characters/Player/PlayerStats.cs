using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {
    public int startHearts;
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

    }

    void AddHearts(float amount, int type) {
        if (totalHearts + amount <= heartContainers) {
            totalHearts += amount;
        } else {
            amount += heartContainers - totalHearts;
            totalHearts = heartContainers;
        }
        switch (type) {
            case 0:
                redHearts += amount;
                break;
            case 1:
                blackHearts += amount;
                break;
            case 2:
                soulHearts += amount;
                break;
        }
    }
}

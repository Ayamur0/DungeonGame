using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public Action<GameObject> onHitEffect;
    public PlayerStats playerStats;

    virtual protected void Start() { }

    virtual protected void Update() { }

    public void SetOnHitEffect(int luck, int rolls, Action<GameObject> func) {
        for (int i = 0; i < rolls; i++) {
            if (UnityEngine.Random.Range(0, 100) <= luck) {
                onHitEffect = func;
                return;
            }
        }
        onHitEffect = null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : MonoBehaviour {
    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player"))
            Pickup(other);
    }

    void Pickup(Collider player) {
        PlayerStats stats = player.GetComponent<PlayerStats>();
    }
}

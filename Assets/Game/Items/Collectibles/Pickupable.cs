using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickupable : MonoBehaviour {
    void OnTriggerEnter(Collider other) {
        Debug.Log("Collider enter");
        if (other.CompareTag("Player"))
            Pickup(other);
    }

    void OnCollisionEnter(Collision other) {
        Debug.Log("Collision enter");
        // if (other.gameObject.CompareTag("Player"))
        //     Pickup(other.gameObject);
    }

    void Pickup(Collider player) {
        PlayerStats stats = player.GetComponent<PlayerStats>();
        if (Effect(stats))
            Destroy(gameObject);
    }

    abstract protected bool Effect(PlayerStats stats);
}

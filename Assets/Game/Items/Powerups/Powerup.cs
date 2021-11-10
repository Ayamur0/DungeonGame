using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Powerup : MonoBehaviour {
    public Sprite sprite;
    protected bool activated = false;
    public Inventory inventory;

    virtual protected void Update() { }

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
        activated = true;
        PlayerStats stats = player.GetComponent<PlayerStats>();
        inventory = player.GetComponent<Inventory>();
        if (PickupEffect(stats)) {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;
        }
        //Destroy(gameObject);
    }

    abstract protected bool PickupEffect(PlayerStats stats);
}

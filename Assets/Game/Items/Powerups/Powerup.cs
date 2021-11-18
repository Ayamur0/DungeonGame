using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Powerup : MonoBehaviour {
    public Sprite sprite;
    protected bool activated = false;
    public Inventory inventory;

    virtual protected void Update() { }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player"))
            other.GetComponent<Inventory>().closest = this;
        //Pickup(other);
    }

    public bool Pickup(GameObject player) {
        activated = true;
        PlayerStats stats = player.GetComponent<PlayerStats>();
        inventory = player.GetComponent<Inventory>();
        bool success = PickupEffect(stats);
        if (success) {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;
        }
        return success;
    }

    public void Drop() {
        transform.position = inventory.transform.position;
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<BoxCollider>().enabled = true;
    }

    abstract protected bool PickupEffect(PlayerStats stats);
}

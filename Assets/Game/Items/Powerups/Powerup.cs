using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Powerup : MonoBehaviour {
    [HideInInspector]
    public Sprite sprite;
    protected bool activated = false;
    [HideInInspector]
    public Inventory inventory;

    void Start() {
        sprite = GetComponent<SpriteRenderer>().sprite;
    }

    virtual protected void Update() { }

    public bool Pickup(GameObject player) {
        PlayerStats stats = player.GetComponent<PlayerStats>();
        inventory = player.GetComponent<Inventory>();
        bool success = PickupEffect(stats);
        if (success) {
            activated = true;
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;
        }
        return success;
    }

    public void Drop() {
        activated = false;
        transform.position = inventory.transform.position;
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<BoxCollider>().enabled = true;
    }

    abstract protected bool PickupEffect(PlayerStats stats);

    abstract public string GetDescription();
}

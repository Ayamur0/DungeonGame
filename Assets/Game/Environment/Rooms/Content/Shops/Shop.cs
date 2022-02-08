using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour {
    public List<GameObject> ItemSlots = new List<GameObject>();
    public List<GameObject> Trigger = new List<GameObject>();

    private Text ErrorMessage;
    private GameObject player;
    private List<GameObject> items;
    private int currentTrigger = -1;
    private List<int> boughtItems = new List<int>();

    // Start is called before the first frame update
    private void Start() {
        ErrorMessage = GameObject.Find("ShopErrorMessage").GetComponent<Text>();
        player = FindObjectOfType<PlayerAPI>().gameObject;
        List<Vector3> positions = new List<Vector3>();
        foreach (var slot in ItemSlots) {
            positions.Add(slot.transform.position);
            // var rndIndex = Random.Range(0, AvailableItems.Count - 1);
            // var item = this.AvailableItems[rndIndex];
            // Instantiate(item, slot.transform);
        }
        items = FindObjectOfType<PowerupSpawner>().SpawnPowerups(positions);
        foreach (GameObject item in items) {
            item.GetComponent<Collider>().enabled = false;
        }

        int i = 0;
        foreach (GameObject t in Trigger) {
            t.GetComponent<Trigger>().id = i++;
            t.GetComponent<Trigger>().onTriggerEnter = SetCurrentTrigger;
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.E) && currentTrigger != -1 && !boughtItems.Contains(currentTrigger)) {
            BuyItem();
        }
    }

    void SetCurrentTrigger(int id) {
        Debug.Log("Trigger set to " + id);
        currentTrigger = id;
    }

    private void BuyItem() {
        if (!player.GetComponent<PlayerAPI>().Pay(100)) {
            StartCoroutine(DisplayErrorMessage("You need 100 coins to buy this"));
            return;
        }
        if (!items[currentTrigger].GetComponent<Powerup>().Pickup(player)) {
            StartCoroutine(DisplayErrorMessage("You don't have enough space in your inventory to buy this"));
            player.GetComponent<PlayerAPI>().Pay(-100);
            return;
        }
        boughtItems.Add(currentTrigger);
    }

    private IEnumerator DisplayErrorMessage(string message) {
        ErrorMessage.text = message;
        ErrorMessage.enabled = true;
        yield return new WaitForSeconds(2);
        ErrorMessage.enabled = false;
    }
}
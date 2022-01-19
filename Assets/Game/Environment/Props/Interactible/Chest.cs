using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour {
    public float ItemSpawnRadius = 1.5f;
    public GameObject OpenVFX;
    public int MaxSpawnItems = 3;
    public bool SpawnOnlyWeapons = false;

    private int spawnedItems;
    private bool isOpen = false;
    private bool canInteract = false;
    private Animator animator;
    private PowerupSpawner itemsSpawner;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start() {
        this.animator = GetComponent<Animator>();
        this.itemsSpawner = FindObjectOfType<PowerupSpawner>();
        this.audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        if (canInteract && Input.GetKeyDown(KeyCode.E)) {
            if (!isOpen) {
                OpenChest();
                if (this.OpenVFX)
                    this.OpenVFX.SetActive(true);
            }
        }
    }

    private void OpenChest() {
        this.animator.SetTrigger("OpenChest");
        this.audioSource.Play();
        this.isOpen = true;
    }

    public void SpawnItems() {
        for (int i = 0; i < 8; i++) {
            if (this.spawnedItems < this.MaxSpawnItems) {
                float angle = i * Mathf.PI * 2f / 8;
                Vector3 newPos = new Vector3(Mathf.Cos(angle) * this.ItemSpawnRadius, -0.75f, Mathf.Sin(angle) * this.ItemSpawnRadius);
                var spawnPos = gameObject.transform.position + newPos;

                GameObject item = null;
                if (SpawnOnlyWeapons) {
                    var rndItemIndex = Random.Range(0, this.itemsSpawner.weapons.Length);
                    item = Instantiate(this.itemsSpawner.weapons[rndItemIndex], spawnPos, Quaternion.identity);
                } else {
                    var rndItemIndex = Random.Range(0, this.itemsSpawner.powerups.Length);
                    item = Instantiate(this.itemsSpawner.powerups[rndItemIndex], spawnPos, Quaternion.identity);
                }


                item.transform.Rotate(90f, 0f, 0f);
                this.spawnedItems++;
            }
        }

        var light = GetComponentInChildren<Light>();
        if (light)
            light.enabled = false;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            this.canInteract = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            this.canInteract = false;
        }
    }
}

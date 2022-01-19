using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour {
    public float ItemSpawnRadius = 1.5f;
    public GameObject OpenVFX;
    public int MaxSpawnItems = 4;
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

    public void SpawnItems()
    {
        List<Vector3> spawnPositions = new List<Vector3>();

        for (int i = 0; i < 8; i++) {
            if (this.spawnedItems < Random.Range(1, this.MaxSpawnItems)) {
                float angle = i * Mathf.PI * 2f / 8;
                Vector3 newPos = new Vector3(Mathf.Cos(angle) * this.ItemSpawnRadius, -0.75f, Mathf.Sin(angle) * this.ItemSpawnRadius);
                var spawnPos = gameObject.transform.position + newPos;
                spawnPositions.Add(spawnPos);
                this.spawnedItems++;
            }
        }

        var spawnedItems = itemsSpawner.SpawnPowerups(spawnPositions);
        foreach (var item in spawnedItems)
        {
            Debug.Log(item.transform.rotation.x);
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

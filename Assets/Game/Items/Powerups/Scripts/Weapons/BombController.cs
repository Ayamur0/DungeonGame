using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour {
    public float Lifetime = 2f;
    public float Speed;
    private float direction = 0f;
    private Vector3 startPos;
    private Vector3 targetPos;
    private float arcHeight = 5;
    private float dist;

    // hide deprecated collider property
    new private Collider collider;
    private HashSet<Collider> enmiesInRange = new HashSet<Collider>();

    // Start is called before the first frame update
    void Start() {
        collider = GetComponent<Collider>();
        startPos = transform.position;
        direction = transform.rotation.eulerAngles.y;
        targetPos = transform.position + transform.forward * Speed * Lifetime;
        float dist = Vector3.Distance(startPos, targetPos);
        Explode();
        Destroy(gameObject, Lifetime);
    }

    // Update is called once per frame
    void Update() {
        float x0 = startPos.x;
        float z0 = startPos.z;
        float x1 = targetPos.x;
        float z1 = targetPos.z;
        transform.position += transform.forward * Speed * Time.deltaTime;
        float arc = arcHeight * (transform.position.x - x0 + transform.position.z - z0) * (transform.position.x - x1 + transform.position.z - z1) / (-0.25f * dist * dist);
        transform.position = new Vector3(transform.position.x, startPos.y + arc, transform.position.z);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Enemy")
            enmiesInRange.Add(other);
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Enemy")
            enmiesInRange.Remove(other);
    }

    void Explode() {
        foreach (Collider c in enmiesInRange) {
            // damage enemy
        }
    }
}

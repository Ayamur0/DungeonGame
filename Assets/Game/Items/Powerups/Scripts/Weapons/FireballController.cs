using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : MonoBehaviour {
    public float Lifetime = 2f;
    public float Speed;
    private float direction = 0f;
    // Start is called before the first frame update
    void Start() {
        direction = transform.rotation.eulerAngles.y;
        Destroy(gameObject, Lifetime);
    }

    // Update is called once per frame
    void Update() {
        transform.position += transform.forward * Speed * Time.deltaTime;
    }
}

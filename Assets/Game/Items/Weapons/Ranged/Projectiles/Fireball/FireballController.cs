using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : MonoBehaviour {
    private const int Up = 0, Right = 90, Down = 180, Left = 270;
    public float Lifetime = 2f;
    public float Speed = .2f;
    private float direction = 0f;
    // Start is called before the first frame update
    void Start() {
        direction = transform.rotation.eulerAngles.y;
        Destroy(gameObject, Lifetime);
    }

    // Update is called once per frame
    void Update() {
        float xVelocity = 0f;
        float zVelocity = 0f;

        switch (direction) {
            case Up:
                zVelocity = Speed;
                break;
            case Right:
                xVelocity = Speed;
                break;
            case Down:
                zVelocity = -Speed;
                break;
            case Left:
                xVelocity = -Speed;
                break;
        }
        Vector3 moveVelocity = new Vector3(xVelocity, .0f, zVelocity);
        transform.position += moveVelocity * Speed;
    }
}

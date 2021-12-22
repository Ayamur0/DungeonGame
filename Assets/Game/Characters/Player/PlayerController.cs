using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private PlayerStats stats;
    private Rigidbody body;

    void Start() {
        stats = this.GetComponent<PlayerStats>();
        this.body = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
        move();
        turn();
    }

    void move() {
        float xVelocity = Input.GetAxis("Horizontal");
        float zVelocity = Input.GetAxis("Vertical");

        Vector3 moveVelocity = new Vector3(xVelocity, .0f, zVelocity);

        //transform.position += moveVelocity * stats.movespeed;

        body.velocity = moveVelocity * stats.movespeed;
    }

    void turn() {
        float angle = transform.rotation.eulerAngles.y;
        if (Input.GetKey("up"))
            angle = 0;
        else if (Input.GetKey("right"))
            angle = 90;
        else if (Input.GetKey("down"))
            angle = 180;
        else if (Input.GetKey("left"))
            angle = 270;
        else if (Input.GetAxis("Vertical") > 0)
            angle = 0;
        else if (Input.GetAxis("Vertical") < 0)
            angle = 180;
        else if (Input.GetAxis("Horizontal") > 0)
            angle = 90;
        else if (Input.GetAxis("Horizontal") < 0)
            angle = 270;

        transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "PlayerProjectile") {
            Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
        }
    }
}

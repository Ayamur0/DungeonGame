using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public GameObject fireball;

    private PlayerStats stats;
    private float timeSinceLastAttack = .0f;

    void Start() {
        stats = this.GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update() {
        move();
        turn();
        shoot();
    }

    void move() {
        float xVelocity = Input.GetAxis("Horizontal");
        float zVelocity = Input.GetAxis("Vertical");

        Vector3 moveVelocity = new Vector3(xVelocity, .0f, zVelocity);

        transform.position += moveVelocity * stats.speed;
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

    void shoot() {
        timeSinceLastAttack += Time.deltaTime;
        if (Input.GetKey("up") || Input.GetKey("down") || Input.GetKey("left") || Input.GetKey("right")) {
            if (timeSinceLastAttack >= stats.attackCooldown) {
                timeSinceLastAttack = 0;
                summonFireball();
            }
        }
    }

    void summonFireball() {
        ArrayList fireballObjs = new ArrayList();
        fireballObjs.Add(Instantiate(fireball, transform.position, transform.rotation));
        if (stats.tripleShot) {
            fireballObjs.Add(Instantiate(fireball, transform.position, Quaternion.Euler(0, transform.rotation.eulerAngles.y + 45, 0)));
            fireballObjs.Add(Instantiate(fireball, transform.position, Quaternion.Euler(0, transform.rotation.eulerAngles.y - 45, 0)));
        }
        foreach (GameObject fireballObj in fireballObjs) {
            FireballController fireballController = fireballObj.GetComponent<FireballController>();
            fireballController.Speed = stats.missileSpeed;
        }
    }
}

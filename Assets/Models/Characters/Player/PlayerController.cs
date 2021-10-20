using UnityEngine;

public class PlayerController : MonoBehaviour {
    public GameObject fireball;
    public float speed = .1f;
    public float attackCooldown = .5f;
    private float timeSinceLastAttack = .0f;

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

        transform.position += moveVelocity * speed;
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
            if (timeSinceLastAttack >= attackCooldown) {
                timeSinceLastAttack = 0;
                summonFireball();
            }
        }
    }

    void summonFireball() {
        Instantiate(fireball, transform.position, transform.rotation);
    }
}

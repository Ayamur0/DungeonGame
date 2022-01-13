using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : Projectile {
    public float Lifetime = 2f;
    public float Speed;
    private float direction = 0f;
    private bool targetReached;
    // Start is called before the first frame update
    override protected void Start() {
        direction = transform.rotation.eulerAngles.y;
        ParticleSystem.MainModule particles = GetComponent<ParticleSystem>().main;
        Destroy(gameObject, Lifetime + particles.duration);
    }

    // Update is called once per frame
    override protected void Update() {
        if (targetReached)
            return;
        transform.position += transform.forward * Speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision col) {
        GameObject other = col.gameObject;
        EnemyController enemyController = other.GetComponent<EnemyController>();
        if (enemyController != null) {
            other.GetComponent<EnemyHealth>().ReceiveDamage(playerStats.GetDamage());
            if (onHitEffect != null)
                onHitEffect(other.gameObject);
            emitImpactParticles();
        } else if (other.tag == "Player") {
            Physics.IgnoreCollision(other.GetComponent<Collider>(), GetComponent<Collider>());
        } else {
            emitImpactParticles();
        }
    }

    private void emitImpactParticles() {
        ParticleSystem.MainModule particles = GetComponent<ParticleSystem>().main;
        Destroy(gameObject, particles.duration);
        targetReached = true;
        foreach (MeshRenderer r in transform.GetComponentsInChildren<MeshRenderer>())
            r.enabled = false;
        GetComponent<Collider>().enabled = false;
        GetComponent<ParticleSystem>().Play();
    }
}

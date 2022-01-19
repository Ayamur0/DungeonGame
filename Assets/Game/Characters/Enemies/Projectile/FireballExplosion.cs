using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballExplosion : MonoBehaviour
{
    private Vector3 Direction;
    private float Dmg;
    private float Speed;
    Rigidbody body;

    public void Setup(Vector3 direction, float speed, float dmg)
    {
        this.Direction = direction;
        this.Dmg = dmg;
        this.Speed = speed;
        Destroy(gameObject, 10f);

        body = GetComponent<Rigidbody>();
        body.AddForce(Direction * Speed, ForceMode.Impulse);
    }


    private void OnTriggerEnter(Collider other)
    {
        PlayerAPI player = other.GetComponent<PlayerAPI>();
        if (player != null)
        {
            player.TakeDamage(Dmg);
            startExplosionParticle();
            Debug.Log(other.gameObject);
        }
        else if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Room")
        {
        }
        else
        {
            Debug.Log(other.gameObject);
            startExplosionParticle();
        }
    }

    private void startExplosionParticle()
    {
        ParticleSystem particles = GetComponent<ParticleSystem>();
        Destroy(gameObject, particles.main.duration);
        foreach (MeshRenderer r in transform.GetComponentsInChildren<MeshRenderer>())
            r.enabled = false;
        GetComponent<Collider>().enabled = false;
        particles.Play();
    }
}

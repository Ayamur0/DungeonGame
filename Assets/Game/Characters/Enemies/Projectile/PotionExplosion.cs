using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionExplosion : MonoBehaviour
{
    public Vector3 Direction;
    private float Dmg;
    private float Speed;
    Rigidbody body;

    public void Setup(Vector3 direction, float speed, float dmg)
    {
        this.Direction = direction;
        this.Dmg = dmg;
        this.Speed = speed;
        Destroy(gameObject, 5f);

        body = GetComponent<Rigidbody>();
        body.AddForce(transform.forward * Speed, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerAPI player = other.GetComponent<PlayerAPI>();
        if (player != null)
        {
            player.TakeDamage(Dmg);
            startExplosionParticle();
        }
        else if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Room" || other.gameObject.name.Contains("Battle"))
        {
        }
        else
        {
            startExplosionParticle();
        }
    }

    private void startExplosionParticle()
    {
        gameObject.GetComponent<AudioSource>().Play();

        Destroy(gameObject);

        //TODO break bottle create visible damage area
    }

}

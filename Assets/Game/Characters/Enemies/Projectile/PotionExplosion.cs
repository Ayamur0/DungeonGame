using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionExplosion : MonoBehaviour
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
        Destroy(gameObject, 5f);

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
            Debug.Log("Enemy ignored");
        }
        else
        {
            startExplosionParticle();
            Debug.Log(other.gameObject);
        }
    }

    private void startExplosionParticle()
    {
        Destroy(gameObject, 2f);
    }
}

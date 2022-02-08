using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowEffect : MonoBehaviour
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
        Destroy(gameObject.transform.parent.gameObject, 10f);

        body = GetComponent<Rigidbody>();
        body.AddRelativeForce(-transform.forward * Speed, ForceMode.Impulse);
        
        gameObject.GetComponent<AudioSource>().Play();
    }



    private void OnTriggerEnter(Collider other)
    {
        PlayerAPI player = other.GetComponent<PlayerAPI>();
        if (player != null)
        {
            player.TakeDamage(Dmg);
            FreezeArrow(other);
        }
        else if (other.gameObject.tag != "Enemy" && other.gameObject.tag != "Room" && !(other.gameObject.name.Contains("Battle")) && other.gameObject.tag != "PlayerProjectile")
        {
            FreezeArrow(other);
        }
    }

    private void FreezeArrow(Collider other)
    {
        body.velocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;
        body.isKinematic = true;
        GetComponentInChildren<BoxCollider>().enabled = false;
        if (other.gameObject.tag == "Player")
        {
            gameObject.transform.parent.SetParent(other.transform);
        }

        Destroy(gameObject.transform.parent.gameObject, 7f);
        ParticleSystem particles = GetComponentInChildren<ParticleSystem>();
        particles.Stop();
    }
}
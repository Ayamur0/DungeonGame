using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionExplosion : MonoBehaviour
{
    public Vector3 playerPosition;
    public Vector3 Direction;
    public Vector3 DirectionWithSpread;
    public float Distance;
    private float Dmg;
    public float UpForce;
    public float ForwardForce;
    public float spread = 4.0f;
    public GameObject ExplosionPrefab;
    Rigidbody body;

    public void Setup(Vector3 PlayerPosition, float speed, float dmg)
    {
        this.playerPosition = PlayerPosition;
        this.Direction = PlayerPosition - transform.position;

        float x = UnityEngine.Random.Range(-spread, spread);
        float z = UnityEngine.Random.Range(-spread, spread);
        this.DirectionWithSpread = this.Direction + new Vector3(x, 0, z);
        this.Distance = Vector3.Distance(playerPosition,DirectionWithSpread);

        this.Dmg = dmg;
        this.UpForce = speed * 2;
        this.ForwardForce = Distance / UnityEngine.Random.Range(3.0f , 8.0f);

        body = GetComponent<Rigidbody>();
        body.AddForce((ForwardForce * DirectionWithSpread.normalized), ForceMode.Impulse);
        body.AddForce((UpForce * transform.up), ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerAPI player = other.GetComponent<PlayerAPI>();
        if (player != null)
        {
            player.TakeDamage(Dmg);
            startExplosionParticle();
        }
        else if (other.gameObject.tag != "Enemy" && other.gameObject.tag != "Room" && !(other.gameObject.name.Contains("Battle")))
        {
            startExplosionParticle();
        }
    }

    private void startExplosionParticle()
    {
        body.velocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;
        body.isKinematic = true;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<MeshCollider>().enabled = false;

        gameObject.GetComponent<AudioSource>().pitch += UnityEngine.Random.Range(-0.25f, 0.25f);
        gameObject.GetComponent<AudioSource>().Play();

        if (gameObject.transform.position.y > -0.4f)
        {
            transform.position = new Vector3(transform.position.x, 0.7f, transform.position.z);
        }

        GameObject ExplosionObj = Instantiate(ExplosionPrefab, gameObject.transform.position, Quaternion.identity * Quaternion.Euler(-90,0,0));
        ExplosionPrefab.GetComponentInChildren<ParticleCollison>().Setup(this.Dmg);

        Destroy(gameObject, 2.0f);
    }

}

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
            FreezeArrow(other);
            Debug.Log(other.gameObject);
        }
        else if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Room")
        {
            Debug.Log("Enemy ignored");
        }
        else
        {
            FreezeArrow(other);
            Debug.Log(other.gameObject);
        }
    }

    private void FreezeArrow(Collider other)
    {
        body.velocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;
        body.isKinematic = true;
        transform.SetParent(other.transform);
        Destroy(gameObject, 10f);
    }
}

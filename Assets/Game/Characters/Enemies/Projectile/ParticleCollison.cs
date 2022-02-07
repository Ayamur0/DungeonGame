using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollison : MonoBehaviour
{
    public float Dmg;
    private float DmgTimeout = 1.2f;
    private bool waitforTimeout = false;

    public void Setup(float dmg)
    {
        this.Dmg = dmg;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!waitforTimeout && other.gameObject.tag == "Player")
        {
            PlayerAPI player = other.GetComponent<PlayerAPI>();
            if (player != null)
            {
                waitforTimeout = true;
                player.TakeDamage(this.Dmg);
                Invoke(nameof(SetShootTimeout), DmgTimeout);
            }
        }
    }

    private void SetShootTimeout()
    {
        waitforTimeout = false;
    }
}

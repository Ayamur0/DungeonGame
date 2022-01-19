using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private float _health;
    public float Health => _health;

    private AudioSource audiosource;
   
    public AudioClip[] GetDamageSounds;

    public void Init(EnemyGenerator.EnemyDifficulty mode, EnemyGenerator.EnemyType type)
    {
        switch (type)
        {
            case EnemyGenerator.EnemyType.SkeletonBasic:
                this._health = 4;
                break;
            case EnemyGenerator.EnemyType.Archer:
                this._health = 5;
                break;
            case EnemyGenerator.EnemyType.BigSkeleton:
                this._health = 7;
                break;
            case EnemyGenerator.EnemyType.Mage:
                this._health = 6;
                break;
            case EnemyGenerator.EnemyType.Wiking:
                this._health = 8;
                break;
            case EnemyGenerator.EnemyType.Witch:
                this._health = 3;
                break;
        }

        switch (mode)
        {
            case EnemyGenerator.EnemyDifficulty.Easy:
                _health += Random.Range(0.0f, 1.0f);
                break;
            case EnemyGenerator.EnemyDifficulty.Medium:
                _health += Random.Range(1.0f, 2.0f);
                break;
            case EnemyGenerator.EnemyDifficulty.Hard:
                _health += Random.Range(2.0f, 3.0f);
                break;
        }

        audiosource = GetComponent<AudioSource>();
    }

    public void ReceiveDamage(float damage)
    {
        _health -= damage;

        //TODO: SHOW HEALTHBAR

        if (GetDamageSounds != null)
        {
            if (GetDamageSounds.Length > 0)
            {
                audiosource.clip = GetDamageSounds[Random.Range(0, GetDamageSounds.Length)];
                audiosource.Play();
            }
        }
    }
}

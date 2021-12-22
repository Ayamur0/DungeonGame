using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float _health;
    public float health => _health;

    public void Init(EnemyGenerator.EnemyDifficulty mode, EnemyGenerator.EnemyType type)
    {
        switch (type)
        {
            case EnemyGenerator.EnemyType.SkeletonBasic:
                this._health = 3;
                break;
            case EnemyGenerator.EnemyType.Archer:
                this._health = 3;
                break;
            case EnemyGenerator.EnemyType.BigSkeleton:
                this._health = 5;
                break;
            case EnemyGenerator.EnemyType.Magician:
                this._health = 4;
                break;
            case EnemyGenerator.EnemyType.Wiking:
                this._health = 6;
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
    }

    public void ReceiveDamage(float damage)
    {
        _health -= damage;

        Debug.Log(gameObject);
        Debug.Log("Received" + damage + "Damage.");
        Debug.Log(_health + "HP remains.");
    }
}

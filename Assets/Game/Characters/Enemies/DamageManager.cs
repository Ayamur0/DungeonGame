using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    private float damage;

    public void Init(EnemyGenerator.EnemyDifficulty mode, EnemyGenerator.EnemyType type)
    {
        switch (type)
        {
            case EnemyGenerator.EnemyType.SkeletonBasic:
                this.damage = 0.5f;
                break;
            case EnemyGenerator.EnemyType.Archer:
                this.damage = 0.5f;
                break;
            case EnemyGenerator.EnemyType.BigSkeleton:
                this.damage = 1f;
                break;
            case EnemyGenerator.EnemyType.Mage:
                this.damage = 0.5f;
                break;
            case EnemyGenerator.EnemyType.Wiking:
                this.damage = 1f;
                break;
            case EnemyGenerator.EnemyType.Witch:
                this.damage = 1f;
                break;
        }

        switch (mode)
        {
            case EnemyGenerator.EnemyDifficulty.Easy:
                damage += Random.Range(0.0f, 1.0f);
                break;
            case EnemyGenerator.EnemyDifficulty.Medium:
                damage += Random.Range(1.0f, 2.0f);
                break;
            case EnemyGenerator.EnemyDifficulty.Hard:
                damage += Random.Range(2.0f, 3.0f);
                break;
        }
    }
    public float GetDamage()
    {
        float extraDmg = (float) Random.Range(0 , 1);
        return damage + extraDmg;
    }
}

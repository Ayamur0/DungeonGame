using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour {
    private float damage;

    private LevelManager levelManager;

    void Start() {
        levelManager = FindObjectOfType<LevelManager>();
    }

    public void Init(EnemyGenerator.EnemyDifficulty mode, EnemyGenerator.EnemyType type) {
        switch (type) {
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
    }
    public float GetDamage() {
        if (levelManager == null)
            levelManager = FindObjectOfType<LevelManager>();
        return damage + (levelManager.CurrentStage / 2) * 0.5f;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {
    private float _health;
    public float Health => _health;

    private AudioSource audiosource;

    public AudioClip[] GetDamageSounds;

    private LevelManager levelManager;

    void Start() {
        levelManager = FindObjectOfType<LevelManager>();
    }

    public void Init(EnemyGenerator.EnemyDifficulty mode, EnemyGenerator.EnemyType type) {
        switch (type) {
            case EnemyGenerator.EnemyType.SkeletonBasic:
                this._health = 4;
                break;
            case EnemyGenerator.EnemyType.Archer:
                this._health = 2;
                break;
            case EnemyGenerator.EnemyType.BigSkeleton:
                this._health = 6;
                break;
            case EnemyGenerator.EnemyType.Mage:
                this._health = 3;
                break;
            case EnemyGenerator.EnemyType.Wiking:
                this._health = 5;
                break;
            case EnemyGenerator.EnemyType.Witch:
                this._health = 3;
                break;
        }

        if (levelManager == null)
            levelManager = FindObjectOfType<LevelManager>();
        this._health += levelManager.CurrentStage / 2;

        audiosource = GetComponent<AudioSource>();
    }

    public void ReceiveDamage(float damage) {
        _health -= damage;

        //TODO: SHOW HEALTHBAR

        if (GetDamageSounds != null) {
            if (GetDamageSounds.Length > 0) {
                audiosource.clip = GetDamageSounds[Random.Range(0, GetDamageSounds.Length)];
                audiosource.Play();
            }
        }
    }
}

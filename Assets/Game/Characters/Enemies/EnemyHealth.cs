using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {
    private float _health;
    public float Health => _health;
    float maxHealth;

    private AudioSource audiosource;

    public AudioClip[] GetDamageSounds;

    private bool attacked = false;

    private LevelManager levelManager;
    private EnemyController controller;

    EnemyUIHealthBar healthBar;
    private float lastMultiplier = 1;

    void Start() {
        levelManager = FindObjectOfType<LevelManager>();
        healthBar = GetComponentInChildren<EnemyUIHealthBar>();
        healthBar.gameObject.SetActive(false);
    }

    public void Init(EnemyController controller, EnemyGenerator.EnemyType type) {
        this.controller = controller;
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
        maxHealth = _health;

        audiosource = GetComponent<AudioSource>();
    }

    public void ReceiveDamage(float damage) {
        _health -= damage;

        if (_health <= 0)
        {
            healthBar.gameObject.SetActive(false);
            controller.Die();
        }
        else
        {
            controller.showHitEffect(new Color(185,0,0));
            healthBar.SetHealthBarPercentage(_health / maxHealth);
        }
            

        if (!attacked)
        {
            attacked = true;
            controller.ActivateChasing();
            healthBar.gameObject.SetActive(true);
        }

        if (GetDamageSounds != null) {
            if (GetDamageSounds.Length > 0) {
                audiosource.clip = GetDamageSounds[Random.Range(0, GetDamageSounds.Length)];
                audiosource.Play();
            }
        }
    }

    public void ReceivePoisonDamage(float damage)
    {
        _health -= damage;

        if (_health <= 0)
        {
            healthBar.gameObject.SetActive(false);
            controller.Die();
        }
        else
        {
            controller.showHitEffect(Color.green);
            healthBar.SetHealthBarPercentage(_health / maxHealth);
        }


        if (!attacked)
        {
            attacked = true;
            controller.ActivateChasing();
            healthBar.gameObject.SetActive(true);
        }

        if (GetDamageSounds != null)
        {
            if (GetDamageSounds.Length > 0)
            {
                audiosource.clip = GetDamageSounds[Random.Range(0, GetDamageSounds.Length)];
                audiosource.Play();
            }
        }
    }

    public void SlowEnemy(float multiplier)
    {
        controller.Agent.speed *= multiplier;
        lastMultiplier = multiplier;
    }

    public void ResetSlowness()
    {
        controller.Agent.speed *= 1 / lastMultiplier;
        lastMultiplier = 1;
    }
}

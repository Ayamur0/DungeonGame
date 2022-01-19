using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{ 
    public enum State
    {
        Patroling,
        Chasing,
        Attack,
    }

    private EnemyGenerator.EnemyType EnemyType;
    
    public State currentState;


    [Header("Scripts")]
    public EnemyHealth HealthStats;
    public DamageManager DamageManager;

    [Header("ScriptableObject")]
    public EnemyStats EnemyStats;

    [Header("Meta Data")]
    public NavMeshAgent Agent;
    public GameObject Player;
    public Animator m_Animator;
    public LayerMask WhatIsGround, WhatIsPlayer;
    public Vector3 NextMovingPoint;
    public GameObject projectile;

    private bool MovePointisValid;
    private float playerDistance;
    private bool waitForTimeout = false;

    private Room activeRoom;

    [Header("Effects")]
    public AudioClip DeathSound;
    private AudioSource audiosource;
    public GameObject DeathVFX;
    private bool vfxStarted = false;

    public void Init(EnemyGenerator.EnemyDifficulty mode, EnemyGenerator.EnemyType type, Room activeRoom)
    {
        this.EnemyType = type;
        Player = GameObject.FindGameObjectWithTag("Player");
        Agent = GetComponent<NavMeshAgent>();
        m_Animator = GetComponent<Animator>();
        currentState = State.Patroling;

        HealthStats = GetComponent<EnemyHealth>();
        DamageManager = GetComponent<DamageManager>();
        HealthStats.Init(mode, type);
        DamageManager.Init(mode, type);
        audiosource = GetComponent<AudioSource>();

        this.activeRoom = activeRoom;
    }

    // Start is called before the first frame update
    void Start()
    {
        NextMovingPoint = ChooseNextMovingPoint();
    }

    // Update is called once per frame
    void Update()
    {
        if (HealthStats.Health > 0.0f)
        {
            switch (currentState)
            {
                case State.Patroling:
                    if (MovePointisValid) //check if player is not alive
                    {
                        Agent.SetDestination(NextMovingPoint);
                        m_Animator.SetTrigger("walking");
                        playerDistance = Vector3.Distance(transform.position, NextMovingPoint);
                        if (playerDistance < 1f)
                        {
                            MovePointisValid = false;
                            NextMovingPoint = ChooseNextMovingPoint();
                        }
                    }
                    else
                    {
                        NextMovingPoint = ChooseNextMovingPoint();
                    }
                    DetectPlayer();
                    break;
                case State.Chasing:
                    ChasePlayer();
                    break;
                case State.Attack:
                    Attack();
                    break;
                default:
                    break;
            }
        }
        else
        {
            Agent.ResetPath();

            m_Animator.ResetTrigger("walking");
            m_Animator.ResetTrigger("attacking");
            m_Animator.SetTrigger("defeated");

            if (DeathVFX && !vfxStarted)
            {
                var deathvfx = Instantiate(DeathVFX);
                deathvfx.transform.position = gameObject.transform.position;
                deathvfx.transform.localScale = new Vector3(2, 2, 2);
                vfxStarted = true;

                FindObjectOfType<GameManager>().CurrentScore.KilledEnemies++;
                Destroy(gameObject, 1f);
            }
        }
    }


    private Vector3 ChooseNextMovingPoint()
    { 
        Vector3 randomDirRange = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        Vector3 walkingPoint = randomDirRange * Random.Range(EnemyStats.patrolRange, EnemyStats.patrolRange);

        if (this.activeRoom)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + walkingPoint, -transform.up, out hit, 2f, WhatIsGround))
            {
                RaycastHit sphereHit;
                if (Physics.SphereCast(transform.position + walkingPoint, 2f, transform.up, out sphereHit, 0.1f))
                {
                    if(sphereHit.transform.gameObject.tag == "Decoration")
                    {
                        return transform.position + walkingPoint;
                    } 
                }
                var roomID = this.activeRoom.gameObject.GetInstanceID();
                // path: NavMeshPlane/Ground/Colliders/Room
                if (hit.transform.parent.parent.parent.gameObject.GetInstanceID() == roomID)
                {
                    MovePointisValid = true;
                }
            }
        }

        return transform.position + walkingPoint;
    }

    private void DetectPlayer()
    {
        if (Vector3.Distance(transform.position, Player.transform.position) < EnemyStats.searchRange)
        {
            RaycastHit sphereHit;
            if (Physics.SphereCast(transform.position, EnemyStats.searchRange, transform.forward, out sphereHit, 0.1f))
            {
                if (sphereHit.transform.gameObject.tag != "Player")
                {
                    return;
                }
            }
            currentState = State.Chasing;
        }
    }

    private void ChasePlayer()
    {
        Agent.SetDestination(Player.transform.position);
        if (Vector3.Distance(transform.position, Player.transform.position) < EnemyStats.attackRange)
        {
            currentState = State.Attack;
        }
    }

    private void Attack()
    {
        Agent.SetDestination(transform.position);
        transform.LookAt(new Vector3(Player.transform.position.x, 0, Player.transform.position.z));

        if (!waitForTimeout)
        {
            waitForTimeout = true;
            m_Animator.ResetTrigger("walking");
            m_Animator.SetTrigger("attacking");
            Invoke(nameof(SetShootTimeout), EnemyStats.shootInterval);
        }
        if (Vector3.Distance(transform.position, Player.transform.position) > EnemyStats.attackRange)
        {
            currentState = State.Chasing;
            m_Animator.SetTrigger("walking");
            m_Animator.ResetTrigger("attacking");
        }

    }

    private void SetShootTimeout()
    {
        waitForTimeout = false;
    }

    private void SendDamage()
    {
        Player.GetComponent<PlayerAPI>().TakeDamage(DamageManager.GetDamage());
        Debug.Log(gameObject.name + "Attack!!");
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, EnemyStats.attackRange);
        Gizmos.DrawWireSphere(NextMovingPoint, 1);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, EnemyStats.searchRange);
    }

    private void ShootProjectile()
    {
        Vector3 spawnPosition = transform.position + transform.forward + new Vector3(0, 1.5f, 0);
        GameObject projectileObj = Instantiate(projectile, spawnPosition, gameObject.transform.rotation); //TODO: Set rotation for arrows;

        switch (EnemyType)
        {
            case EnemyGenerator.EnemyType.Archer:
                projectileObj.GetComponent<ArrowEffect>().Setup(Player.transform.position, 1f, DamageManager.GetDamage());
                break;
            case EnemyGenerator.EnemyType.Mage:
                projectileObj.GetComponent<FireballExplosion>().Setup(Player.transform.position, 1f, DamageManager.GetDamage());
                break;
            case EnemyGenerator.EnemyType.Witch:
                projectileObj.GetComponent<PotionExplosion>().Setup(Player.transform.position, 1f, DamageManager.GetDamage());
                break;

        }
    }
}

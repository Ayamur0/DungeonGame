using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{ 
    private enum State
    {
        Patroling,
        Chasing,
        Attack
    }

    private State currentState;
    private float health;
    private float minDamage;

    private EnemyHealth healthStats;

    public EnemyStats enemyStats;

    public NavMeshAgent agent;
    public Transform player;
    public Animator m_Animator;

    public LayerMask whatIsGround, whatIsPlayer;

    public Vector3 nextMovingPoint;

    private bool MovePointisValid;

    private float playerDistance;
    private bool waitForTimeout = false;

    public void Init(EnemyGenerator.EnemyDifficulty mode, EnemyGenerator.EnemyType type)
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        m_Animator = GetComponent<Animator>();
        currentState = State.Patroling;
        healthStats.Init(mode, type);
    }

    // Start is called before the first frame update
    void Start()
    {
        nextMovingPoint = ChooseNextMovingPoint();
    }

    // Update is called once per frame
    void Update()
    {
        if (health > 0.0f)
        {
            switch (currentState)
            {
                case State.Patroling:
                    if (MovePointisValid) //check if player is not alive
                    {
                        agent.SetDestination(nextMovingPoint);
                        m_Animator.SetTrigger("walking");
                        playerDistance = Vector3.Distance(transform.position, nextMovingPoint);
                        if (playerDistance < 1f)
                        {
                            MovePointisValid = false;
                            nextMovingPoint = ChooseNextMovingPoint();
                        }
                    }
                    else
                    {
                        nextMovingPoint = ChooseNextMovingPoint();
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
    }

    private Vector3 ChooseNextMovingPoint()
    { 
        Vector3 randomDirRange = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        Vector3 walkingPoint = randomDirRange * Random.Range(enemyStats.patrolRange, enemyStats.patrolRange);

        if (Physics.Raycast(transform.position + walkingPoint, -transform.up, 2f, whatIsGround))
        {
            MovePointisValid = true;
        }

        return transform.position + walkingPoint;
    }

    private void DetectPlayer()
    {
        if (Vector3.Distance(transform.position, player.position) < enemyStats.searchRange)
        {
            currentState = State.Chasing;
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        if(Vector3.Distance(transform.position, player.position) > enemyStats.searchRange + 5f)
        {
            currentState = State.Patroling;
        }
        if (Vector3.Distance(transform.position, player.position) < enemyStats.attackRange)
        {
            currentState = State.Attack;
        }
    }

    private void Attack()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(new Vector3(player.position.x, 0, player.position.z));

        if (!waitForTimeout)
        { 
            waitForTimeout = true;
            SendDamage();
            Invoke(nameof(SetShootTimeout), enemyStats.shootInterval);
        }
        if (Vector3.Distance(transform.position, player.position) > enemyStats.attackRange)
        {
            currentState = State.Chasing;
        }

    }

    private void SetShootTimeout()
    {
        waitForTimeout = false;
    }

    private void SendDamage()
    {
        m_Animator.ResetTrigger("walking");
        m_Animator.SetTrigger("attacking");

        //Attack

        Debug.Log("Attack!!");
    }

    public void ReceiveDamage(float damage) {
        health -= damage;

        Debug.Log(gameObject);
        Debug.Log("Received" + damage + "Damage.");
        Debug.Log(health + "HP remains.");

        if (health <= 0)
        {
            m_Animator.ResetTrigger("walking");
            m_Animator.ResetTrigger("attacking");
            m_Animator.SetTrigger("defeated");

            agent.ResetPath();

            Destroy(gameObject, 2f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyStats.attackRange);
        Gizmos.DrawWireSphere(nextMovingPoint, 1);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, enemyStats.searchRange);
    }


}

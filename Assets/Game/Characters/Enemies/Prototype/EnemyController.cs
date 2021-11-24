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

    public float health = 3f;

    public float searchRange = 10f;
    public float attackRange = 5f;

    public float patrolRange = 8f;

    public NavMeshAgent agent;
    public Transform player;
    public Animator m_Animator;

    public LayerMask whatIsGround, whatIsPlayer;

    public Vector3 nextMovingPoint;

    private bool MovePointisValid;

    public float shootInterval;
    private float playerDistance;
    private bool waitForTimeout = false;

    public EnemyController(float health, float searchRange, float attackRange, float patrolRange, float shootInterval)
    {
        this.health = health;
        this.searchRange = searchRange;
        this.attackRange = attackRange;
        this.patrolRange = patrolRange;
        this.shootInterval = shootInterval;
    }

    public void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        m_Animator = GetComponent<Animator>();
        currentState = State.Patroling;
    }

    // Start is called before the first frame update
    [System.Obsolete]
    void Start()
    {
        nextMovingPoint = ChooseNextMovingPoint();
    }

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
        if (health <= 0.0f)
        {
            m_Animator.ResetTrigger("walking");
            m_Animator.ResetTrigger("attacking");
            m_Animator.SetTrigger("defeated");
        }
        else
        {
            switch (currentState)
            {
                case State.Patroling:
                    if (MovePointisValid)
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

    [System.Obsolete]
    private Vector3 ChooseNextMovingPoint()
    { 
        Vector3 randomDirRange = new Vector3(Random.RandomRange(-1f, 1f), 0, Random.RandomRange(-1f, 1f));
        Vector3 walkingPoint = randomDirRange * Random.Range(patrolRange, patrolRange);

        if (Physics.Raycast(transform.position + walkingPoint, -transform.up, 2f, whatIsGround))
        {
            MovePointisValid = true;
        }

        return transform.position + walkingPoint;
    }

    private void DetectPlayer()
    {
        if (Vector3.Distance(transform.position, player.position) < searchRange)
        {
            currentState = State.Chasing;
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        if(Vector3.Distance(transform.position, player.position) > searchRange + 5f)
        {
            currentState = State.Patroling;
        }
        if (Vector3.Distance(transform.position, player.position) < attackRange)
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
            Invoke(nameof(SetShootTimeout), shootInterval);
        }
        if (Vector3.Distance(transform.position, player.position) > attackRange)
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
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.DrawWireSphere(nextMovingPoint, 1);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, searchRange);
    }


}

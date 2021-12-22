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
    private float minDamage;

    public EnemyHealth healthStats;

    public EnemyStats enemyStats;

    public NavMeshAgent agent;
    public GameObject player;
    public Animator m_Animator;

    public LayerMask whatIsGround, whatIsPlayer;

    public Vector3 nextMovingPoint;

    private bool MovePointisValid;

    private float playerDistance;
    private bool waitForTimeout = false;

    private Room activeRoom;

    public void Init(EnemyGenerator.EnemyDifficulty mode, EnemyGenerator.EnemyType type, Room activeRoom)
    {
        player = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>();
        m_Animator = GetComponent<Animator>();
        currentState = State.Patroling;
        healthStats = GetComponent<EnemyHealth>();
        healthStats.Init(mode, type);
        this.activeRoom = activeRoom;
    }

    // Start is called before the first frame update
    void Start()
    {
        nextMovingPoint = ChooseNextMovingPoint();
    }

    // Update is called once per frame
    void Update()
    {
        if (healthStats.health > 0.0f)
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
        else
        {
            m_Animator.ResetTrigger("walking");
            m_Animator.ResetTrigger("attacking");
            m_Animator.SetTrigger("defeated");

            agent.ResetPath();

            Destroy(gameObject, 2f);
        }
    }

    private Vector3 ChooseNextMovingPoint()
    { 
        Vector3 randomDirRange = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        Vector3 walkingPoint = randomDirRange * Random.Range(enemyStats.patrolRange, enemyStats.patrolRange);

        RaycastHit hit;
        if (Physics.Raycast(transform.position + walkingPoint, -transform.up, out hit, 2f, whatIsGround))
        {
            var roomID = this.activeRoom.gameObject.GetInstanceID();
            // path: NavMeshPlane/Ground/Colliders/Room
            if (hit.transform.parent.parent.parent.gameObject.GetInstanceID() == roomID)
            {
                MovePointisValid = true;
            }
        }

        return transform.position + walkingPoint;
    }

    private void DetectPlayer()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < enemyStats.searchRange)
        {
            currentState = State.Chasing;
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.transform.position);
        if(Vector3.Distance(transform.position, player.transform.position) > enemyStats.searchRange + 5f)
        {
            currentState = State.Patroling;
        }
        if (Vector3.Distance(transform.position, player.transform.position) < enemyStats.attackRange)
        {
            currentState = State.Attack;
        }
    }

    private void Attack()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(new Vector3(player.transform.position.x, 0, player.transform.position.z));

        if (!waitForTimeout)
        { 
            waitForTimeout = true;
            SendDamage();
            Invoke(nameof(SetShootTimeout), enemyStats.shootInterval);
        }
        if (Vector3.Distance(transform.position, player.transform.position) > enemyStats.attackRange)
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

        //TODO: Attack
        //TODO: Remove
        this.healthStats.ReceiveDamage(2f);

        Debug.Log("Attack!!");
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

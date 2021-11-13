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

    public float searchRange = 10f;
    public float attackRange = 5f;

    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public Vector3 nextMovingPoint;

    private bool MovePointisValid;

    public float shootTimeout;
    private float playerDistance;
    private bool waitForTimeout = false;

    public void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
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
        switch (currentState)
        {
            case State.Patroling:
                if (MovePointisValid)
                {
                    agent.SetDestination(nextMovingPoint);
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

    [System.Obsolete]
    private Vector3 ChooseNextMovingPoint()
    {

        Vector3 randomDirRange = new Vector3(Random.RandomRange(-1f, 1f), 0, Random.RandomRange(-1f, 1f));
        Vector3 walkingPoint = randomDirRange * Random.Range(15f, 15f);

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
        transform.LookAt(player.position);

        if (!waitForTimeout)
        {
            
            waitForTimeout = true;
            Invoke(nameof(setShootTimeout), shootTimeout);
        }
        if (Vector3.Distance(transform.position, player.position) < attackRange)
        {
            currentState = State.Chasing;
        }

    }

    private void setShootTimeout()
    {
        waitForTimeout = false;
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

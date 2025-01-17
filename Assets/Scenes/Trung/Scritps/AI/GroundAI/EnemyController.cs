using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour,INpc
{
    enum AIState
    {
        Idle,Patrolling,Chasing,Attacking
    }

    
    [Header("Patrol")]
    [SerializeField] private Transform wayPoints;
    [SerializeField] private float waitAtPoint = 2f;
    private float waitCounter;
    private int currentWaypoint;

    [Header("Components")]
    NavMeshAgent agent;

    [Header("AI States")]
    [SerializeField] private AIState currentState;

    [Header("Chasing")]
    [SerializeField] private float chaseRange;

    [Header("Suspicious")]
    [SerializeField] private float suspiciousTime;
    private float timeSinceLastSawPlayer;

    [Header("Attack")]
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackTime = 2f;
    private float timeToAttack;

    private GameObject player;

    private float distanceToPlayer;
    private Animator _anim;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindAnyObjectByType<CharacterCtrl>().gameObject;
        waitCounter = waitAtPoint;
        timeSinceLastSawPlayer = suspiciousTime;
        timeToAttack = attackTime;
    }
    private void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position,player.transform.position);
        switch (currentState)
        {
            case AIState.Idle:
                if(waitCounter > 0)
                {
                    waitCounter -=Time.deltaTime;
                }
                else
                {
                    currentState = AIState.Patrolling;
                    _anim.SetBool("isPatrolling", true);
                    agent.SetDestination(wayPoints.GetChild(currentWaypoint).position);
                }
                if(distanceToPlayer <= chaseRange)
                {
                    _anim.SetBool("isChasing", true);
                    currentState = AIState.Chasing;
                }
                break;
            case AIState.Patrolling:
                Patrol();
                break;
            case AIState.Chasing:
                Chase();
                break;
            case AIState.Attacking:
                Attack();
                break;
        }
    }
    public void Patrol()
    {
        if(agent.remainingDistance <= 0.2f)
        {
            currentWaypoint++;
            if(currentWaypoint >= wayPoints.childCount)
            {
                currentWaypoint = 0;
            }
            currentState = AIState.Idle;
            waitCounter = waitAtPoint;
            //agent.SetDestination(wayPoints.GetChild(currentWaypoint).position);
        }
        if (distanceToPlayer <= chaseRange)
        {
            _anim.SetBool("isChasing", true);
            currentState = AIState.Chasing;
        }
    }
    public void Chase()
    {
        agent.SetDestination(player.transform.position);
        if (distanceToPlayer > chaseRange)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            timeSinceLastSawPlayer -= Time.deltaTime;
            if (timeSinceLastSawPlayer <= 0)
            {
                currentState = AIState.Idle;
                timeSinceLastSawPlayer = suspiciousTime;
                agent.isStopped = false;
            }
        }
        if(distanceToPlayer <= attackRange)
        {
            currentState = AIState.Attacking;
            agent.velocity  = Vector3.zero;
            agent.isStopped = true;
        }
    }
    public void Attack()
    {
        transform.LookAt(player.transform.position,Vector3.up);
        timeToAttack -= Time.deltaTime;
        if(timeToAttack <= 0)
        {
            //thực hiện animation tấn công
            _anim.SetBool("isAttacking", true);
            timeToAttack = attackTime;
        }
        if (distanceToPlayer > attackRange)
        {
            _anim.SetBool("isAttacking", false);
            currentState = AIState.Chasing;
            agent.isStopped = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Utilities;
using UnityEditor.ShaderGraph.Drawing;
using UnityEngine;

public class Boss : EnemyAnimatorAbstract
{
    protected float distance;
    protected Transform player;
    //private Animator _myAnimator;

    [SerializeField] protected float chaseDistance;
    [SerializeField] protected float attackDistance;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected bool _canMove = true;

    protected enum BossState { Idle, Chase, AttackMlee,AttackMissile, Death }
    protected BossState currentState = BossState.Idle;
    protected override void Awake()
    {
        this.player = FindAnyObjectByType<CharacterMove>().transform;
        //_myAnimator = GetComponent<Animator>();
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
    }

    protected void Update()
    {
        this.FiniteStateMachine();
    }
    protected virtual void FiniteStateMachine()
    {
        this.LoadNavMeshAgentProperties();
        this.distance = Vector3.Distance(this.transform.position, this.player.position);

        if (currentState != BossState.Death)
        {
            if (distance <= this.attackDistance)
            {
                this.ChangeState(BossState.AttackMlee);
            }
            else if (distance <= this.chaseDistance)
            {
                this.AttackMissile();
            }
            else
            {
                this.ChangeState(BossState.Idle);
            }
        }

        this.HandleState();
    }

    protected virtual void ChangeState(BossState newState)
    {
        if (currentState == newState) return;
        currentState = newState;

        switch (newState)
        {
            case BossState.Idle:
                _enemyAnimator.SetBool("isChasing", false);
                break;
            case BossState.Chase:
                _enemyAnimator.SetBool("isChasing", true);
                break;
            case BossState.AttackMissile:
                _enemyAnimator.SetBool("isChasing", false);
                this.AttackMissile();
                break;
            case BossState.AttackMlee:
                _enemyAnimator.SetBool("isChasing", false);
                this.AttackMlee();
                break;
            case BossState.Death:
                _enemyAnimator.SetTrigger("Death");
                break;
        }
    }
    protected virtual void AttackMlee()
    {
        _enemyAnimator.SetInteger("AttackIndex", 0);
        _enemyAnimator.SetTrigger("Attack");
    }
    protected virtual void AttackMissile()
    {
        int rand = Random.Range(0, this.amountAttacksAnimation);
        if (rand == 0)
        {
            currentState = BossState.Chase;
            this.ChangeState(currentState);
            return;
        }
        _enemyAnimator.SetTrigger("Attack");
        _enemyAnimator.SetInteger("AttackIndex", rand);
    }

    protected virtual void HandleState()
    {
        if (!this._canMove) return;
        if (currentState == BossState.Chase)
        {
            //Vector3 dir = (player.position - transform.position).normalized;
            //transform.position += dir * moveSpeed * Time.deltaTime;
            //transform.LookAt(player);
            this._agent.SetDestination(player.position);
        }
    }

    protected virtual void LoadNavMeshAgentProperties()
    {
        this._agent.speed = this.moveSpeed;
        this._agent.stoppingDistance = this.attackDistance;
    }
    public virtual void Die()
    {
        ChangeState(BossState.Death);
        // Disable further behavior
        this.enabled = false;
    }
}

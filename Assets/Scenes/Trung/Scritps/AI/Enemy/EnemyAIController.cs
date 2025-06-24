using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIController : VuMonoBehaviour
{
    [SerializeField] protected Transform targetPosition;
    [SerializeField] protected EnemyAI enemyReferences;

    [SerializeField] protected FindNearestTargets findTargets;

    [SerializeField] protected bool isAttacking = false;


    [SerializeField] protected EnemyHealth enemyHealth;

    //protected float attackDistance;
    protected float pathUpdateDeadline;

    protected bool inRangeAttack = false;



    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadTargetPosition();
        this.LoadEnemyHealth();
        this.LoadEnemyReferences();
        this.LoadFindTargets();
    }
    protected override void Start()
    {
        base.Start();
        this.LoadAllProperties();
    }



    protected virtual void LoadTargetPosition()
    {
        if (this.targetPosition != null) return;
        this.targetPosition = FindAnyObjectByType<CharacterAnimHandle>().transform;
    }
    protected virtual void LoadEnemyReferences()
    {
        if (this.enemyReferences != null) return;
        this.enemyReferences = GetComponentInChildren<EnemyAI>();
    }
    protected virtual void LoadFindTargets()
    {
        if (this.findTargets != null) return;
        this.findTargets = GetComponentInChildren<FindNearestTargets>();
    }

    protected virtual void LoadAllProperties()
    {
        //this.attackDistance = this.enemyReferences.EnemySO.AttackRange;
        this.enemyReferences.NavMeshAgent.speed = this.enemyReferences.EnemySO.ChaseSpeed;
        this.enemyReferences.NavMeshAgent.stoppingDistance = this.enemyReferences.EnemySO.AttackRange;
    }
    protected virtual void LoadEnemyHealth()
    {
        if (this.enemyHealth != null) return;
        this.enemyHealth = GetComponent<EnemyHealth>();
    }


    protected virtual void Update()
    {
        if(this.targetPosition != null)
        {
            if (this.EnemyIsDead())
            {
                this.enemyReferences.NavMeshAgent.enabled = false;
                return;
            }
            //this.inRangeAttack = Vector3.Distance(transform.position,targetPosition.position) <= this.attackDistance;
            this.inRangeAttack = this.findTargets.TargetsNearest.Count > 0;
            if (this.inRangeAttack) this.Attack();
            this.enemyReferences.Animator.SetBool("attack", this.inRangeAttack);
            this.UpdatePath();
        }
        this.enemyReferences.Animator.SetFloat("Speed",this.enemyReferences.NavMeshAgent.desiredVelocity.sqrMagnitude);
    }

    protected virtual bool EnemyIsDead() => this.enemyHealth.Health <= 0;

    protected virtual void Attack()
    {
        this.isAttacking = true;
        this.enemyReferences.NavMeshAgent.enabled = false;
        this.LookAtTarGet();
    }
    protected virtual void LookAtTarGet()
    {
        Vector3 lookPos = targetPosition.position - transform.position;
        lookPos.y = 0f;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.2f);
        //if (!this.inRangeAttack) isAttacking = false;
        //if(!this.isAttacking) this.EndAttack();
    }

    protected virtual void UpdatePath()
    {
        if (Time.time >= pathUpdateDeadline)
        {
            Debug.Log("Updating path!");
            this.pathUpdateDeadline = Time.time + this.enemyReferences.PathUpdateDelay;
             if (this.isAttacking) return;
             if (!this.enemyReferences.NavMeshAgent.enabled) return;
            this.enemyReferences.NavMeshAgent.SetDestination(targetPosition.position);
        }
    }
    protected virtual void EndAttack()
    {
        this.isAttacking = false;
        this.enemyReferences.NavMeshAgent.enabled = true;
    }
}

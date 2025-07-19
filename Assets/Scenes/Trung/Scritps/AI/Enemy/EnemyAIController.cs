using System.Collections;
using System.Collections.Generic;
using Autodesk.Fbx;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIController : EnemyBase
{
    [Header("All custom components when need to ref!")]
    [SerializeField] protected EnemyAI enemyReferences;
    [SerializeField] protected EnemyHealth enemyHealth;
    [SerializeField] protected EnemyCtrlDespawn enemyCtrlDespawn;

    [Space]
    [Header("Default Components nessessary!")]
    [SerializeField] protected Transform targetPosition;


    [Space]
    [Header("Control state when active/ deactive!")]

    [SerializeField] protected bool isLookAtTarGet = false;
    [SerializeField] protected bool isAttacking = false;
    [SerializeField] protected bool isAttackNear = false;
    [SerializeField] protected bool isNearTarget = false;

    [SerializeField] protected bool isMoving = true;
    [SerializeField] protected bool stateMovingDefault = true;


    [Space]
    [Header("Properties")]
    [SerializeField] protected float distanceNearest = 1f;
    [SerializeField] protected bool isBoss = false;



    [Space]
    [Space]
    [Space]
    [Space]
    [Header("Control when boss is start fight!")]
    [SerializeField] protected bool isStartToFight = true;
    public bool IsStartToFight { set => this.isStartToFight = value; }

    public float DistanceNearest { get => distanceNearest; }


    protected float distanceToTarget;
    public float DistanceToTarget { get => this.distanceToTarget; }

    //public variable

    public bool IsLookAtTarget { get => isLookAtTarGet; set => isLookAtTarGet = value; }
    public bool IsMoving { get => isMoving; set => isMoving = value; }

    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }


    [Space]
    [Header("Control behavior of enemies base!")]

    [SerializeField] protected int amountAttackNear = 1;
    [SerializeField] protected int amountAttackFar = 1;


    //Hide in Hirachi variable


    //protected float attackDistance;
    protected float pathUpdateDeadline;
    protected bool inRangeAttack = false;
    protected bool isDead = false;



    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadTargetPosition();
        this.LoadEnemyHealth();
        this.LoadEnemyReferences();
        this.LoadEnemyCtrlDespawn();
    }
    protected override void Start()
    {
        base.Start();
        this.LoadAllProperties();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        this.SetEnemyWhenAppear();
    }

    //Load All Properties
    protected virtual void LoadAllProperties()
    {
        //this.attackDistance = this.enemyReferences.EnemySO.AttackRange;
        this.enemyReferences.NavMeshAgent.speed = this.enemyReferences.EnemySO.ChaseSpeed;
        this.enemyReferences.NavMeshAgent.stoppingDistance = this.enemyReferences.EnemySO.AttackRange;
        this.enemyReferences.NavMeshAgent.acceleration = this.enemyReferences.EnemySO.ChaseSpeed;
    }


    //Load Custom Components
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
    protected virtual void LoadEnemyCtrlDespawn()
    {
        if (this.enemyCtrlDespawn != null) return;
        this.enemyCtrlDespawn = GetComponentInChildren<EnemyCtrlDespawn>();
        if (this.enemyCtrlDespawn == null) return;
    }
    protected virtual void LoadEnemyHealth()
    {
        if (this.enemyHealth != null) return;
        this.enemyHealth = GetComponent<EnemyHealth>();
    }


    protected virtual void SetEnemyWhenAppear()
    {

        //SnapToNavMesh();
        this.enemyReferences.NavMeshAgent.enabled = true;
        this.isAttacking = false;
        this.isLookAtTarGet = false;
        this.isNearTarget = false;
        this.isMoving = this.stateMovingDefault;
        this.isDead = false;


        this.WaitingForEnemyDeath();



    }
    void SnapToNavMesh()
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 5f, NavMesh.AllAreas))
        {
            transform.position = hit.position;
            this.enemyReferences.NavMeshAgent.Warp(hit.position);
            Debug.Log("Snapped enemy to NavMesh at: " + hit.position);
        }
        else
        {
            Debug.LogWarning("No NavMesh found near enemy! Cannot place NavMeshAgent.");
        }
    }

    protected virtual void Update()
    {
        this.ControlBehviorEnemy();
    }

    protected virtual void ControlBehviorEnemy()
    {
        if (this.targetPosition != null)
        {
            if (this.enemyHealth != null)
            {
                if (this.EnemyIsDead())
                {
                    return;
                }
            }
            this.Chase();
            //this.inRangeAttack = Vector3.Distance(transform.position,targetPosition.position) <= this.attackDistance;
            this.Attack();
            this.LookAtTarGet();






        }


        this.enemyReferences.Animator.SetFloat("Speed", this.enemyReferences.NavMeshAgent.desiredVelocity.sqrMagnitude);
    }

    protected virtual void WaitingForEnemyDeath()
    {
        StartCoroutine(this.WaitingForEnemyDeathRoutine());
    }


    protected virtual bool EnemyIsDead() => this.enemyHealth.Health <= 0;

    protected int RandomAnimationBlend(int _amountAnimations) => Random.Range(0, _amountAnimations);

    protected bool HasState(string _state) => this.enemyReferences.Animator.HasState(0, Animator.StringToHash(_state));


    protected virtual void Rise()
    {
        this.enemyHealth.CanGetDamage = true;
        this.isStartToFight = true;
        this.isMoving = true;
    }
    protected override void Idle()
    {
        if (!HasState("Idle"))
        {
            Debug.LogWarning("Chưa có trạng thái Idle, vui lòng thêm!");
            return;
        }
        this.enemyReferences.Animator.SetFloat("IdleState", this.RandomAnimationBlend(this.enemyReferences.EnemySO.IdleAnimations));
        
        this.enemyReferences.Animator.SetBool("isMoving", false);
        this.isMoving = false;

        this.enemyReferences.Animator.SetBool("attack", false);
        this.isAttacking = false;
    }


    protected override void Chase()
    {
        if (this.isAttacking) return;
        if (!this.isMoving) return;
        //Check Player is in chase range!
        if (!HasState("Movement"))
        {
            Debug.LogWarning("Chưa có trạng thái Movement, vui lòng thêm!");
            return;
        }

        if(this.targetPosition != null)
        {
            this.distanceToTarget = Vector3.Distance(this.transform.position, this.targetPosition.position);
            this.isNearTarget = this.distanceToTarget <= this.distanceNearest;
            bool canChasePlayer = Vector3.Distance(transform.position, targetPosition.position) <= this.enemyReferences.EnemySO.ChaseRange;
            if (canChasePlayer)
            {
                this.UpdateEnemyPath();
            }
        }
    }

    protected override void Attack()
    {
        this.enemyReferences.Animator.SetFloat("distance",this.distanceToTarget);
        this.AttackWhenNearPlayer();
    }



    //Death
    protected IEnumerator WaitingForEnemyDeathRoutine()
    {
        yield return new WaitUntil(() => this.enemyHealth.CanGetDamage);
        yield return new WaitUntil(() => this.enemyHealth.Health <= 0 && !this.enemyHealth.CanGetDamage);
        this.Death();
    }

    protected override void Death()
    {
        this.enemyReferences.NavMeshAgent.enabled = false;
        this.isDead = true;
        if (!HasState("Death")) return;
        this.enemyReferences.Animator.SetTrigger("Death");
        this.enemyReferences.Animator.SetFloat("DeathState", this.RandomAnimationBlend(this.enemyReferences.EnemySO.DeathAnimations)); 
    }
    protected virtual void DeleteEnemy()
    {
        //if(this.enemyCtrlDespawn != null)
        //{
        //    this.enemyCtrlDespawn.DoDespawn();
        //    return;
        //}
        this.gameObject.SetActive(false);
    }
    protected virtual void DeleteEnemyWhileHpEqual0()
    {
        if (!GetComponent<Collider>().enabled && gameObject.activeInHierarchy)
        {
            this.DeleteEnemy();
            //RewardPlayerAfterEnemyDead();
        }
    }





    protected override void AttackWhenNearPlayer()
    {
        if(this.targetPosition == null) return;
        if (!HasState("AttackNear")) return;
        if (this.isBoss) return;
        if (this.distanceToTarget < this.distanceNearest)
        {
            this.isAttacking = true;
            this.isAttackNear = true;
            this.isMoving = false;
            this.enemyReferences.NavMeshAgent.enabled = false;
            this.isNearTarget = true;
            this.enemyReferences.Animator.SetFloat("AttackNearState", this.RandomAnimationBlend(this.enemyReferences.EnemySO.AttackAnimations));
        }
        else
        {
            this.isNearTarget = false;
            this.isAttackNear = false;
            this.isAttacking = false;
            this.isMoving = true;
            this.enemyReferences.NavMeshAgent.enabled = true;
        }
        this.enemyReferences.Animator.SetBool("attack", this.isAttacking);
    }

    protected override void AttackWhenFarPlayer()
    {
        if(this.targetPosition == null) return;
        if (!HasState("AttackFar")) return;
        if (this.isAttackNear) return;
    }





    protected override void LookAtTarGet()
    {
        if (!this.isLookAtTarGet) return;
        Vector3 lookPos = this.targetPosition.position - this.transform.position;
        lookPos.y = 0f;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        this.transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.2f);
    }

    protected virtual void UpdateEnemyPath()
    {
        if (Time.time >= this.pathUpdateDeadline)
        {
            //  Debug.Log("Updating path!");
            this.pathUpdateDeadline = Time.time + this.enemyReferences.PathUpdateDelay;
            //this.enemyReferences.NavMeshAgent.enabled = true;
            if (this.isMoving && this.enemyReferences.NavMeshAgent.isOnNavMesh)
            {
                this.enemyReferences.NavMeshAgent.SetDestination(this.targetPosition.position);
                //Debug.Log("SetDestination: " + this.targetPosition.position);
            }
            this.enemyReferences.Animator.SetBool("isMoving", true);
        }
    }
    public virtual void EndAttack()
    {
        this.isAttacking = false;
        this.isAttackNear = false;
        this.isMoving = true;
        this.enemyReferences.NavMeshAgent.enabled = true;
        this.isLookAtTarGet = false;
        //this.enemyReferences.Animator.SetBool("attack", this.isAttacking);
    }





    protected virtual void SwitchFarState()
    {

    }

    protected virtual void LookAtTartgetPlease()
    {
        this.isLookAtTarGet = true;
    }
    protected virtual void DontLookAtTarget()
    {
        this.isLookAtTarGet = false;
    }
}

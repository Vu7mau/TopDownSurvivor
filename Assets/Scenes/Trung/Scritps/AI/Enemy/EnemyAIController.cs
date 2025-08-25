using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIController : EnemyBase
{
    [Header("All custom components when need to ref!")]
    [SerializeField] protected EnemyAI enemyReferences;
    [SerializeField] protected EnemyHealth enemyHealth;

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
    protected float chaseRange;
    public float ChaseRange { get => this.chaseRange; set => this.chaseRange = value;}



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

    [Space]
    [Space]
    [Header("Roar")]
    [SerializeField] protected List<AudioClip> snd_roar;
    [SerializeField] protected List<AudioClip> snd_StepFoot;
    [SerializeField] protected float timeRoar = 10f;
    protected float roarEclapse = 0f;
    protected bool isRoar = false;
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
        if(this.chaseRange == 0) this.chaseRange = this.enemyReferences.EnemySO.ChaseRange;
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
    protected virtual void LoadEnemyHealth()
    {
        if (this.enemyHealth != null) return;
        this.enemyHealth = GetComponent<EnemyHealth>();
    }


    protected virtual void SetEnemyWhenAppear()
    {

        this.SnapToNavMesh();
        this.enemyReferences.NavMeshAgent.enabled = false;
        this.isAttacking = false;
        this.isLookAtTarGet = false;
        this.isNearTarget = false;
        this.isMoving = this.stateMovingDefault;
        this.isDead = false;


        this.WaitingForEnemyDeath();
        if(this.snd_roar.Count > 0) StartCoroutine(this.RoarRoutine());
        


    }
    public virtual void SnapToNavMesh()
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 1000f, NavMesh.AllAreas))
        {
            transform.position = hit.position;
            this.enemyReferences.NavMeshAgent.Warp(hit.position);
            //Debug.Log("Snapped enemy to NavMesh at: " + hit.position);
        }
    }

    protected virtual void Roar()
    {
        if (this.snd_roar.Count == 0) return;
        this.roarEclapse += Time.deltaTime;
        if (this.roarEclapse < this.timeRoar) return;
        this.isRoar = true;
        this.roarEclapse = 0;
    }

    protected IEnumerator RoarRoutine()
    {
        float rateRoar = 100f;
        while (true)
        {
            int isPlayingRoar = Random.Range(0, 100);
            yield return new WaitUntil(() => this.isRoar);
            if(isPlayingRoar <= rateRoar)
            {
                if (this.snd_roar.Count != 0)
                {
                    int random = Random.Range(0, this.snd_roar.Count);
                    SoundEnemyManager.Instance.PlayEnemySoundFXClipOnce(this.snd_roar[random], this.transform);
                    if(rateRoar > 30) rateRoar -= 10f;
                    //Debug.Log("Đã phát roar rồi nha!");
                }
            }
            else
            {
                //Debug.Log("Không phát roar đâu nha!");
            }
                this.isRoar = false;
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
            this.Roar();
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
            //Debug.LogWarning("Chưa có trạng thái Idle, vui lòng thêm!");
            return;
        }
        this.enemyReferences.Animator.SetFloat("IdleState", this.RandomAnimationBlend(this.enemyReferences.EnemySO.IdleAnimations));
        this.enemyReferences.NavMeshAgent.enabled = false;
        this.enemyReferences.Animator.SetBool("isMoving", false);
        this.isMoving = false;

        this.enemyReferences.Animator.SetBool("attack", false);
        this.isAttacking = false;
        if (this.distanceToTarget <= this.chaseRange) this.Chase();
    }


    protected override void Chase()
    {
        if (this.isAttacking) return;
        if (!this.isMoving) return;
        //Check Player is in chase range!
        if (!HasState("Movement"))
        {
            //Debug.LogWarning("Chưa có trạng thái Movement, vui lòng thêm!");
            return;
        }

        if(this.targetPosition != null)
        {
            this.distanceToTarget = Vector3.Distance(this.transform.position, this.targetPosition.position);
            this.isNearTarget = this.distanceToTarget <= this.distanceNearest;
            bool canChasePlayer = this.distanceToTarget <= this.chaseRange;
            if (canChasePlayer)
            {
                this.UpdateEnemyPath();
                this.enemyReferences.Animator.SetBool("isMoving", true);
            }

            if(this.enemyReferences.NavMeshAgent.isOnNavMesh) this.enemyReferences.NavMeshAgent.enabled = true;
            if (this.distanceToTarget > this.chaseRange) this.Idle();
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
            else
            {

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
        this.enemyReferences.Animator.SetBool("attack", this.isAttacking);
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



    //Sound FX
    [Space]
    [Space]
    [Space]
    [Space]
    [Space]
    [Header("Sound FX Attack General!")]
    [SerializeField] protected List<AudioClip> snd_attack_1;

    protected virtual void PlaySoundFXAttack1()
    {
        if (snd_attack_1.Count == 0) return;
        int random = Random.Range(0, snd_attack_1.Count);
        if (this.snd_attack_1[random] == null) return;
        SoundFXManager.Instance.PlaySoundFXClip(this.snd_attack_1[random], this.transform);
    }

    protected virtual void StepFootSFX()
    {
        if (snd_StepFoot.Count == 0) return;
        int random = Random.Range(0, snd_StepFoot.Count);
        if (this.snd_StepFoot[random] == null) return;
        SoundEnemyManager.Instance.PlayEnemySoundFXClipOnce(this.snd_StepFoot[random], this.transform);
    }

}

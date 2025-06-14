using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Utilities;
using UnityEditor.ShaderGraph.Drawing;
using UnityEngine;

public abstract class Boss : EnemyAnimatorAbstract
{
    protected enum BossState
    {
        Idle,
        Chase,
        Attack,
        Death
    }
    protected BossState currentState = BossState.Idle;

    protected float distanceToPlayer;

    [Header("Base Properties of Boss")]
    [SerializeField] protected float detectionBaseRange = 100000f;
    [SerializeField] protected float attackBaseRange = 5f;
    [SerializeField] protected bool isAttackPlayer = false;
    [SerializeField] protected float moveBaseSpeed = 3f;
    [SerializeField] protected Transform playerPosition;
    [SerializeField] protected int healthBase = 100;

    [Header("Time to return swtich")]
    [SerializeField] protected float timeToReturnSwitch = 3f;



    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadPlayerPosition();
    }
    protected override void Start()
    {
        base.Start();
        this.StateRoutine();
    }
    protected virtual void LoadPlayerPosition()
    {
        if (this.playerPosition != null) return;
        this.playerPosition = FindAnyObjectByType<CharacterAnimHandle>().transform;
    }
    protected void Update()
    {
        this.distanceToPlayer = Vector3.Distance(this.transform.position,this.playerPosition.position);
    }
    protected void RandomState()
    {
        int randomStateIndex = Random.Range(1, 4);
        if(randomStateIndex != 3)
        {
            currentState = BossState.Chase;
            return;
        }
        currentState = BossState.Attack;
    }
    protected void StateRoutine()
    {
        if (healthBase <= 0)
        {
            this.currentState = BossState.Death;
        }
        switch (currentState)
        {
            case BossState.Idle:
                this.CheckBossIsChasePlayer();
                break;

            case BossState.Chase:
                this.ChasePlayer();
                //this.CheckBossIsAttackPlayer();
                break;

            case BossState.Attack:
                this.Attack();
                break;

            case BossState.Death:
                this.Die();
                break;
        }
    }
    protected virtual void CheckBossIsChasePlayer()
    {
        if (this.isAttackPlayer) return;
        if (Vector3.Distance(transform.position, this.playerPosition.position) < this.detectionBaseRange)
        {
            this.currentState = BossState.Chase;
        }
    }
    protected virtual void CheckBossIsAttackPlayer()
    {
        if (Vector3.Distance(transform.position, this.playerPosition.position) <= this.attackBaseRange)
        {
            this.currentState = BossState.Attack;
            this.isAttackPlayer = true;
        }
        else if (Vector3.Distance(transform.position, this.playerPosition.position) > this.detectionBaseRange)
        {
            this.currentState = BossState.Idle;
            this.isAttackPlayer = false;
        }
    }
    protected virtual bool CheckDistanceFromPlayer(float minDistance,float maxDistance,float currentDistance)
    {
        return currentDistance >= minDistance && currentDistance <= maxDistance;
    }

    protected virtual void ChasePlayer()
    {
        if (this._agent == null) return;
        this._agent.enabled = true;
        this._agent.speed = this.moveBaseSpeed;
        this._agent.SetDestination(playerPosition.position);
    }

    protected virtual void Attack()
    {
        if (!this.isAttackPlayer) return;
        //this._enemyAnimator.SetBool("isAttacking",this.isAttackPlayer);
    }

    //protected virtual IEnumerator AttackRoutine()
    //{
    //    currentState = BossState.Idle; // Tạm dừng trước khi chuyển lại Chase

    //    int attackIndex = Random.Range(0, this.amountAttacksAnimation); // Giả sử có 3 đòn tấn công

    //    switch (attackIndex)
    //    {
    //        case 0:
    //            AttackTypeA();
    //            break;
    //        case 1:
    //            AttackTypeB();
    //            break;
    //        case 2:
    //            AttackTypeC();
    //            break;
    //    }

    //    yield return new WaitForSeconds(2f); // Thời gian giữa các đòn đánh

    //    if (Vector3.Distance(transform.position, playerPosition.position) > attackBaseRange)
    //        currentState = BossState.Chase;
    //    else
    //        currentState = BossState.Attack; // Tấn công tiếp nếu vẫn còn gần
    //}

    

    protected virtual void Die()
    {
        Debug.Log("Boss chết");
        // Trigger animation chết, disable collider, ...
    }
}

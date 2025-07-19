using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Utilities;
using UnityEditor.ShaderGraph.Drawing;
using UnityEngine;

public abstract class BossController : EnemyAIController
{
    [Header("Time to return swtich")]
    [SerializeField] protected float timeToReturnSwitch = 5f;

    [SerializeField] protected bool CanAttackTargetLongDistance = false;
    protected enum FarState { Attack, Chase }

    protected FarState farState = FarState.Chase;


    protected float eclapse = 0f;

    protected override void OnEnable()
    {
        base.OnEnable();
        this.CoolDownStartAttack();
        this.StartFight();
    }

    protected override void Start()
    {
        base.Start();
        this.AttackFromLongDistance();
    }

    protected virtual void AttackFromLongDistance()
    {
        StartCoroutine(SwitchStateAttack());
    }
    protected virtual void StartFight()
    {
        StartCoroutine(SwitchFarAttackStateRoutine());
    }
    protected virtual void CoolDownStartAttack()
    {
        StartCoroutine(CooldownAttackStartRoutine());
    }


    protected IEnumerator CooldownAttackStartRoutine()
    {
        yield return new WaitUntil(() => this.isStartToFight);
        while (eclapse < this.timeToReturnSwitch)
        {
            eclapse += Time.deltaTime;
            yield return null;
        }
        this.CanAttackTargetLongDistance = true;
        eclapse = 0f;
    }


    protected IEnumerator SwitchStateAttack()
    {
        while (true)
        {
            yield return new WaitUntil(() => this.farState == FarState.Attack && this.isStartToFight);
            this.AttackFar1();
            yield return new WaitUntil(() => this.farState == FarState.Chase);
        }
    }
    protected IEnumerator SwitchFarAttackStateRoutine()
    {
        while (true)
        {
            yield return new WaitUntil(() => this.isStartToFight);
            this.CoolDownAttack();
            yield return new WaitUntil(() => this.farState == FarState.Chase);
        }
    }
    protected virtual void AttackFar1()
    {
        this.enemyReferences.NavMeshAgent.enabled = false;
        this.isAttacking = true;
        this.isMoving = false;
        this.RandomFarAttack();
    }
    protected virtual void RandomFarAttack()
    {
        if (!HasState("AttackFar"))
        {
            this.EndAttack();
            return;
        }
        float attackIndex = Random.Range(0, this.amountAttackFar);

        this.enemyReferences.Animator.SetFloat("AttackFarState", attackIndex);
        this.enemyReferences.Animator.SetBool("attackFar", true);

        //if (Vector3.Distance(transform.position, playerPosition.position) > attackBaseRange)
        //{
        //    currentState = BossState.Chase;
        //    this.isAttackPlayer = false;
        //}
        //else
        //    currentState = BossState.Attack;
    }
    protected virtual void RandomNearAttack()
    {
        if (!HasState("AttackNear"))
        {
            this.EndAttack();
            return;
        }
        float attackIndex = Random.Range(0, this.amountAttackNear);

        this.enemyReferences.Animator.SetFloat("AttackNearState", attackIndex);
        this.enemyReferences.Animator.SetBool("attackNear", true);

        //if (Vector3.Distance(transform.position, playerPosition.position) > attackBaseRange)
        //{
        //    currentState = BossState.Chase;
        //    this.isAttackPlayer = false;
        //}
        //else
        //    currentState = BossState.Attack;
    }

    protected virtual void CoolDownAttack()
    {
        if (!this.CanAttackTargetLongDistance) return;
        if (this.farState == FarState.Attack) return;
        if (this.isNearTarget && eclapse > 0)
        {
            this.RandomNearAttack();
            this.eclapse = 0f;
            this.farState = FarState.Attack;
            return;
        }
        eclapse += Time.deltaTime;
        if (eclapse < this.timeToReturnSwitch) return;
        this.farState = FarState.Attack;
        Debug.Log("Attack State");
        eclapse = 0f;
    }
    public override void EndAttack()
    {
        base.EndAttack();
        this.enemyReferences.Animator.SetBool("attackFar", false);
        this.enemyReferences.Animator.SetBool("attackNear", false);
        this.farState = FarState.Chase;
    }
    protected override void UpdateEnemyPath()
    {
        if (this.farState != FarState.Chase) return;
        base.UpdateEnemyPath();
    }
}

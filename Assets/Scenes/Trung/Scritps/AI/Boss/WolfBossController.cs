using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfBossController : EnemyAIController
{
    [Header("Time to return swtich")]
    [SerializeField] protected float timeToReturnSwitch = 5f;

    protected enum FarState { Attack, Chase}

    protected FarState farState = FarState.Chase;

    [SerializeField] protected bool CanAttackTargetLongDistance = false;

    protected float eclapse = 0f;

    [SerializeField] protected AbyssProjectitle abyss;
    [SerializeField] protected AbyssProjectitleSpawner abyssSpawner;

    [SerializeField] protected AbyssFollowTarget abyss2;
    [SerializeField] protected AbyssFollowTargetSpawner abyssFollowTargetSpawner;

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

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadAbyssToxicSpawner();
        this.LoadToxicAbyss();
        this.LoadAbyssFollowTarget();
        this.LoadAbyssFollowTargetSpawner();
    }

    //Load Componenents

    protected virtual void LoadAbyssToxicSpawner()
    {
        if (this.abyssSpawner != null) return;
        this.abyssSpawner = FindAnyObjectByType<AbyssProjectitleSpawner>();
        Debug.Log(transform.name + ": LoadAbyssToxicSpawner");
    }
    protected virtual void LoadToxicAbyss()
    {
        if (this.abyss != null) return;
        List<AbyssProjectitle> allMyComponents = ComponentFinder.FindAllComponentsInScene<AbyssProjectitle>();
        this.abyss = allMyComponents[0];
        Debug.Log(transform.name + ": LoadToxicAbyss");
    }
    protected virtual void LoadAbyssFollowTargetSpawner()
    {
        if (this.abyssFollowTargetSpawner != null) return;
        this.abyssFollowTargetSpawner = FindAnyObjectByType<AbyssFollowTargetSpawner>();
        Debug.Log(transform.name + ": LoadAbyssToxicSpawner");
    }
    protected virtual void LoadAbyssFollowTarget()
    {
        if (this.abyss2 != null) return;
        List<AbyssFollowTarget> allMyComponents = ComponentFinder.FindAllComponentsInScene<AbyssFollowTarget>();
        this.abyss2 = allMyComponents[0];
        Debug.Log(transform.name + ": LoadToxicAbyss");
    }


    protected virtual void AttackFromLongDistance()
    {
        StartCoroutine(SwitchStateAttack());
    }
    protected virtual void StartFight()
    {
        StartCoroutine(SwitchFarAttackStateRoutine());
    }

    //protected void RandomState()
    //{
    //    int randomStateIndex = Random.Range(1, 3);
    //    if (randomStateIndex != 2)
    //    {
    //        currentState = BossState.Chase;
    //        return;
    //    }
    //    currentState = BossState.Attack;
    //}
    protected virtual void CoolDownStartAttack()
    {
        StartCoroutine(CooldownAttackStartRoutine());
    }

    protected virtual void CoolDownAttack()
    {
        if (!this.CanAttackTargetLongDistance) return;
        if (this.farState == FarState.Attack) return;
        if (this.isNearTarget)
        {
            this.farState = FarState.Chase;
        }
        eclapse += Time.deltaTime;
        if (eclapse < this.timeToReturnSwitch) return;
        this.farState = FarState.Attack;
        Debug.Log("Attack State");
        eclapse = 0f;
    }

    protected IEnumerator CooldownAttackStartRoutine()
    { 
        while(eclapse < this.timeToReturnSwitch)
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
            yield return new WaitUntil(() => this.farState == FarState.Attack);
            this.AttackFar1();
            yield return new WaitUntil(() => this.farState == FarState.Chase);
        }
    }
    protected IEnumerator SwitchFarAttackStateRoutine()
    {
        while (true)
        {
            yield return new WaitUntil(() => this.CanAttackTargetLongDistance && !this.isNearTarget);
            this.CoolDownAttack();
            yield return new WaitUntil(() => this.farState == FarState.Chase);
        }
    }
    protected override void EndAttack()
    {
        base.EndAttack();
        this.enemyReferences.Animator.SetBool("attackFar",false);
        this.enemyReferences.Animator.SetBool("attack", false);
        this.farState = FarState.Chase;
    }
    protected override void UpdatePath()
    {
        if (this.farState != FarState.Chase) return;
        base.UpdatePath();
    }
    protected virtual void AttackFar1()
    {
        base.Attack();
        this.RandomFarAttack();
    }
    protected virtual void RandomFarAttack()
    {

        int attackIndex = Random.Range(0, 2);

        switch (attackIndex)
        {
            case 0:
                this.Attack2();
                break;
            case 1:
                this.Attack3();
                break;
            case 2:
                // this.AttackTypeC();
                //this.Attack4();
                break;
        }

        //if (Vector3.Distance(transform.position, playerPosition.position) > attackBaseRange)
        //{
        //    currentState = BossState.Chase;
        //    this.isAttackPlayer = false;
        //}
        //else
        //    currentState = BossState.Attack;
    }
    protected virtual void Attack2()
    {
        Debug.Log("Attack2");
        this.enemyReferences.Animator.SetBool("attackFar", true);
        this.enemyReferences.Animator.SetInteger("index", 2);
    }

    protected virtual void ShootAbyssBullet()
    {
        StartCoroutine(ShootAbyssBulletRoutine());
    }
    private IEnumerator ShootAbyssBulletRoutine()
    {
        Vector3 targetPosition = this.targetPosition.position;
        AbyssProjectitle newToxicAbyss = this.abyssSpawner.Spawn(this.abyss, transform.position);
        if (newToxicAbyss == null) yield break;
        newToxicAbyss.GetComponent<AbyssProjectitle>().ShootAt(this.transform.position + new Vector3(0, 100f, 0));
        newToxicAbyss.GetComponent<AbyssProjectitle>().SetVelocity(30f);
        yield return new WaitForSeconds(1f);
        newToxicAbyss.GetComponent<AbyssProjectitle>().ShootAt(targetPosition);


    }
    protected virtual void Attack3()
    {
        Debug.Log("Attack3");
        this.enemyReferences.Animator.SetBool("attackFar", true);
        this.enemyReferences.Animator.SetInteger("index", 3);
        this.ShootAbyss2();
    }
    protected virtual void ShootAbyss2()
    {
        AbyssFollowTarget abyss2 = this.abyssFollowTargetSpawner.Spawn(this.abyss2, transform.position + new Vector3(0,3,0));
        if (abyss2 == null) return;
    }


    //[SerializeField] protected Transform posCapture;
    //protected virtual void Attack4()
    //{
    //    Debug.Log("Attack4");
    //    this.enemyReferences.Animator.SetBool("attackFar", true);
    //    this.enemyReferences.Animator.SetInteger("index", 4);
    //}
    //protected virtual void CapturedTarGet()
    //{
    //    this.targetPosition.position = posCapture.position;
    //}
}

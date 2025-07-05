using System.Collections;
using System.Collections.Generic;
using Autodesk.Fbx;
using Unity.VisualScripting;
using UnityEngine;
using static Cinemachine.CinemachineImpulseManager.ImpulseEvent;

[RequireComponent(typeof(EShooting))]
public class SciFiController : EnemyAIController
{
    [SerializeField] protected EShooting e_Shooting;

    [SerializeField] protected Transform playerPosition;

    [Header("Time to return swtich")]
    [SerializeField] protected float timeToReturnSwitch = 5f;

    protected enum FarState { Attack, Chase }

    protected FarState farState = FarState.Chase;

    [SerializeField] protected bool CanAttackTargetLongDistance = false;

    protected float eclapse = 0f;


    [Space]
    [Header("Sound FX")]
    [Space]

    [SerializeField] protected List<AudioClip> snd_steps;
    [SerializeField] protected List<AudioClip> snd_attack1s;
    [SerializeField] protected AudioClip snd_attack2;
    [SerializeField] protected AudioClip snd_attack3;
    [SerializeField] protected AudioClip snd_attack4;
    //[SerializeField] protected AudioClip snd_attack5;
    [SerializeField] protected AudioClip snd_attack6;
    [SerializeField] protected List<AudioClip> snd_deaths;



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
        this.LoadPlayerPosition();
        this.LoadEShooting();
    }
    protected virtual void LoadPlayerPosition()
    {
        if (this.playerPosition != null) return;
        this.playerPosition = FindAnyObjectByType<CharacterAnimHandle>().transform;
    }
    protected virtual void LoadEShooting()
    {
        if (this.e_Shooting != null) return;
        this.e_Shooting = GetComponentInChildren<EShooting>();
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


    protected IEnumerator CooldownAttackStartRoutine()
    {
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
        this.enemyReferences.Animator.SetBool("attack", false);
        this.enemyReferences.Animator.SetBool("attackFar", false);
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

        int attackIndex = Random.Range(0, this.amountAnimationAttackFar);

        this.enemyReferences.Animator.SetFloat("AttackFar", attackIndex);
        this.enemyReferences.Animator.SetBool("attackFar", true);

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

    [Header("Hit Splash")]
    [SerializeField] protected HitSplash hitSplash;
    [SerializeField] protected Transform hitSplashPosition;

    protected virtual void Attack2()
    {
        this.e_Shooting.Shooting(hitSplash, hitSplashPosition);
        Vector3 dir = this.targetPosition.position - new Vector3(this.hitSplashPosition.position.x, this.targetPosition.position.y, this.hitSplashPosition.position.z);
        if (this.e_Shooting.NewProjectitle == null) return;
        this.e_Shooting.NewProjectitle.GetComponent<Projectitle>().SetDirection(dir);
        this.e_Shooting.NewProjectitle.gameObject.transform.rotation = this.transform.rotation;
    }

    [Space]
    [Header("Shooting")]
    [SerializeField] protected MinigunBullet minigunBullet;
    [SerializeField] protected Transform minigunBulletSpawnPosition;

    [SerializeField] protected float shootingTime = 3; // Số lần bắn
    [SerializeField] protected float timeDelay = 0.2f;

    protected virtual void Attack6()
    {
        StartCoroutine(ShootingRoutine());
    }
    private IEnumerator ShootingRoutine()
    {
        for (int i = 0; i < this.shootingTime; i++)
        {
            Vector3 target = this.targetPosition.position;
            yield return new WaitForSeconds(this.timeDelay);
            this.e_Shooting.Shooting(this.minigunBullet, this.minigunBulletSpawnPosition);
            if (this.e_Shooting.NewProjectitle == null) yield break;
            this.PlayerSFXAttack6();
            this.e_Shooting.NewProjectitle.GetComponent<Projectitle>().ShootAt(target);
            yield return new WaitForSeconds(this.timeDelay);
        }
        this.EndAttack();
    }

    //Sound FX
    protected virtual void OnStepFootSoundFX()
    {
        if (this.snd_steps.Count == 0) return;
        this.PlaySoundFX(this.snd_steps);
    }
    protected virtual void PlayerSFXAttack1()
    {
        if (this.snd_attack1s.Count == 0) return;
        this.PlaySoundFX(this.snd_attack1s);
    }
    protected virtual void PlayerSFXEDeath()
    {
        if (this.snd_deaths.Count == 0) return;
        this.PlaySoundFX(this.snd_deaths);
    }

    protected virtual void PlayerSFXAttack6()
    {
        if (this.snd_attack6 == null) return;
        SoundFXManager.Instance.PlaySoundFXClip(snd_attack6, transform, 1);
    }
    protected virtual void PlayerSFXAttack3()
    {
        if(this.snd_attack3 == null) return;
        SoundFXManager.Instance.PlaySoundFXClip(snd_attack3, transform, 1);
    }

    protected virtual void PlayerSFXDeath()
    {
        if (this.snd_deaths.Count > 0) return;
        this.PlaySoundFX(snd_deaths);
    }

    [SerializeField] protected ExplosionSpawner explosionSpawner;
    [SerializeField] protected Explosion explosion;
    [SerializeField] protected List<Transform> explosionPoss;
    [SerializeField] protected Transform fire;
    protected virtual void DestroyBossExplosion()
    {
        if(this.explosionSpawner == null) return;
        if(explosion == null) return;
        StartCoroutine(DestroyExplosionRoutine());
    }
    IEnumerator DestroyExplosionRoutine()
    {
        if (this.explosionPoss.Count == 0) yield break;
        for (int i = 0; i < this.explosionPoss.Count; i++)
        {
            this.explosionSpawner.Spawn(this.explosion, this.explosionPoss[i].position);
            yield return new WaitForSeconds(1);
        }
        yield return new WaitForSeconds(1f);
        Explosion nbig = this.explosionSpawner.Spawn(this.explosion, transform.position);
        nbig.transform.localScale = new Vector3(3, 3, 3);
        fire.transform.gameObject.SetActive(true);
    }

    private void PlaySoundFX(List<AudioClip> sounds)
    {
        int random = Random.Range(0, sounds.Count);
        SoundFXManager.Instance.PlaySoundFXClip(sounds[random], transform, 1);
    }
}

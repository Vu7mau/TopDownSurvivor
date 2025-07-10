using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;



//public class RobotZombie : Boss
//{
//    [SerializeField] protected LightBulletSpawner lightBulletSpawner;
//    [SerializeField] protected MissileSpawner missileSpawner;


//    [Header("Shooting")]
//    [SerializeField] protected LightBullet lightBulletPrefab;
//    [SerializeField] protected Transform position;
//    [SerializeField] protected List<Transform> listPositionsLeft;
//    [SerializeField] protected List<Transform> listPositionsRight;
//    //[SerializeField] protected int lightBulletSpawnCount;
//    //[SerializeField] protected float lightBulletSpawnRadius = 1.5f;
//    [SerializeField] protected float timeBetweenEachShoot = 1f;

//    [Header("Shooting")]
//    [SerializeField] protected Missile missilePrefab;
//    [SerializeField] protected List<Transform> missilePositions;

//    //[Header("Jump Attack")]
//    //[SerializeField] protected Transform jumpAttackZone;
//    //[SerializeField] protected float distanceSafe;
//    //[SerializeField] protected bool canJumpToSave = true;
//    //[SerializeField] protected float timeCooldownToJumpSafe;

//    protected bool isLookPlayer = true;

//    [Header("Lazer")]
//    [SerializeField] protected List<AudioClip> listLazerSnds;

//    protected override void LoadComponents()
//    {
//        base.LoadComponents();
//        this.LoadMissileSpawn();
//        this.LoadLightBulletSpawn();
//        this.LoadLightBullet();
//        this.LoadMissile();
//        this.LoadExplosionSpawner();
//        this.LoadExplosion();
//    }
//    protected virtual void LoadMissileSpawn()
//    {
//        if (this.missileSpawner != null) return;
//        this.missileSpawner = FindAnyObjectByType<MissileSpawner>();
//    }
//    protected virtual void LoadLightBulletSpawn()
//    {
//        if (this.lightBulletSpawner != null) return;
//        this.lightBulletSpawner = FindAnyObjectByType<LightBulletSpawner>();
//    }
//    protected virtual void LoadLightBullet()
//    {
//        if (this.lightBulletPrefab != null) return;
//        List<LightBullet> allMyComponents = ComponentFinder.FindAllComponentsInScene<LightBullet>();
//        this.lightBulletPrefab = allMyComponents[0];
//    }
//    protected virtual void LoadMissile()
//    {
//        if (this.missilePrefab != null) return;
//        List<Missile> allMyComponents = ComponentFinder.FindAllComponentsInScene<Missile>();
//        this.missilePrefab = allMyComponents[0];
//    }


//    protected override void Attack()
//    {
//        base.Attack();
//        this.AttackRoutine();
//    }
//    protected override void OnDisable()
//    {
//        base.OnDisable();
//        SpawnEnemies.Instance.IsFinale = true;
//    }

//    protected override void Update()
//    {
//        base.Update();
//        //if (this.isLookPlayer) transform.LookAt(playerPosition.position);
//        //if (distanceToPlayer < distanceSafe && this.canJumpToSave)
//        //{
//        //    this._agent.enabled = false;
//        //    this._enemyAnimator.SetBool("CanJump", this.canJumpToSave);
//        //    this._enemyAnimator.SetBool("isAttacking", !this.canJumpToSave);
//        //    this._enemyAnimator.SetBool("Attack1", !this.canJumpToSave);
//        //    this._enemyAnimator.SetBool("Attack2", !this.canJumpToSave);
//        //    this._enemyAnimator.SetBool("Attack3", !this.canJumpToSave);
//        //    this.canJumpToSave = false;
//        //}
//    }
//    protected virtual void AttackRoutine()
//    {
//        currentState = BossState.Idle;

//        int attackIndex = Random.Range(0, this.amountAttacksAnimation);

//        switch (attackIndex)
//        {
//            case 0:
//                this.AttackTypeA();
//                break;
//            case 1:
//                this.AttackTypeB();
//                break;
//            case 2:
//                this.AttackTypeC();
//                break;
//        }

//        if (Vector3.Distance(transform.position, playerPosition.position) > attackBaseRange)
//        {
//            currentState = BossState.Chase;
//            this.isAttackPlayer = false;
//        }
//        else
//            currentState = BossState.Attack;
//    }
//    private void AttackTypeA()
//    {
//        Debug.Log("Đòn đánh A");
//        this._enemyAnimator.SetBool("isAttacking", true);
//        this._enemyAnimator.SetBool("Attack1", true);
//        //StartCoroutine(ShootRoutine());
//    }
//    //protected IEnumerator RadialShootRoutine()
//    //{
//    //    for (int i = 0; i < lightBulletSpawnCount; i++)
//    //    {
//    //        float angle = i * Mathf.PI * 2f / lightBulletSpawnCount;
//    //        Vector3 dir = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)).normalized;
//    //        Vector3 spawnPos = position.position + dir * lightBulletSpawnRadius;
//    //        LightBullet newLightBullet = this.lightBulletSpawner.Spawn(this.lightBulletPrefab,spawnPos);
//    //        if (newLightBullet == null) yield break;
//    //        newLightBullet.Direction = dir;
//    //        newLightBullet.SetDirection(dir);
//    //    }
//    //    yield return new WaitForSeconds(2f);
//    //    this._enemyAnimator.SetBool("isAttacking", false);
//    //    this._enemyAnimator.SetBool("Attack1", false);
//    //}
//    protected virtual void ShootMissileLeft()
//    {
//        if (currentState == BossState.Death) return;
//        Missile newMissile = this.missileSpawner.Spawn(this.missilePrefab, missilePositions[0].position);
//        if (newMissile == null) return;
//        //newMissile.gameObject.transform.rotation = missilePositions[0].rotation;
//        newMissile.gameObject.GetComponent<Missile>().ShootAt(this.playerPosition.position, 30f);
//    }
//    protected virtual void ShootMissileRight()
//    {
//        if (currentState == BossState.Death) return;
//        Missile newMissile = this.missileSpawner.Spawn(this.missilePrefab, missilePositions[1].position);
//        if (newMissile == null) return;
//        //newMissile.gameObject.transform.rotation = Quaternion.Euler(-90,0,0);
//        newMissile.gameObject.GetComponent<Missile>().ShootAt(this.playerPosition.position, 30f);
//        EndAttack1();
//    }
//    protected virtual void EndAttack1()
//    {
//        this._enemyAnimator.SetBool("isAttacking", false);
//        this._enemyAnimator.SetBool("Attack1", false);
//    }



//    //[SerializeField] protected Transform missilePosition;
//    protected virtual void AttackTypeB()
//    {
//        Debug.Log("Đòn đánh B");
//        this._enemyAnimator.SetBool("isAttacking", true);
//        this._enemyAnimator.SetBool("Attack2", true);
//        //tartCoroutine(ShootMissile());
//    }

//    protected virtual void ShootLightBullet()
//    {
//        StartCoroutine(ShootLightBulletLeft());
//    }

//    //protected virtual void DownFlyState()
//    //{
//    //    this._enemyAnimator.SetBool("CanJump", this.canJumpToSave);
//    //}

//    //private IEnumerator ShootMissile()
//    //{
//    //    for(int i = 0; i < 3; i++)
//    //    {
//    //        Missile newMissile = this.missileSpawner.Spawn(this.missilePrefab, missilePosition.position);
//    //        if (newMissile == null) yield break;
//    //        //newMissile.gameObject.transform.rotation = Quaternion.Euler(-90,0,0);
//    //        newMissile.gameObject.GetComponent<Missile>().ShootAt(this.playerPosition.position, 50f);
//    //        yield return new WaitForSeconds(1f);
//    //    }
//    //    this._enemyAnimator.SetBool("isAttacking", false);
//    //    this._enemyAnimator.SetBool("Attack2", false);
//    //}
//    private IEnumerator ShootLightBulletLeft()
//    {
//        if(this.listPositionsLeft.Count == 0) yield break;
//        for (int i = 0; i < listPositionsLeft.Count; i++)
//        {
//            Vector3 target = this.playerPosition.position;
//            yield return new WaitForSeconds(0.3f);
//            if (this.listPositionsLeft[i] == null) continue;
//            if (currentState == BossState.Death) yield break;
//            LightBullet newMissile = this.lightBulletSpawner.Spawn(this.lightBulletPrefab, listPositionsLeft[i].position);
//            if (newMissile == null) yield break;
//            //newMissile.gameObject.transform.rotation = Quaternion.Euler(-90,0,0);
//            newMissile.gameObject.GetComponent<LightBullet>().ShootAt(target, 100f);
//            yield return new WaitForSeconds(this.timeBetweenEachShoot);
//        }
//        this._enemyAnimator.SetBool("isAttacking", false);
//        this._enemyAnimator.SetBool("Attack2", false);
//    }



//    protected virtual void AttackTypeC()
//    {
//        Debug.Log("Đòn đánh C");
//        this.isLookPlayer = false;
//        this._enemyAnimator.SetBool("isAttacking", true);
//        this._enemyAnimator.SetBool("Attack3", true);
//        //this.JumpToPlayer();
        
//    }
//    protected virtual void EndAttackTypeC()
//    {
//        this._enemyAnimator.SetBool("isAttacking", false);
//        this._enemyAnimator.SetBool("Attack3", false);
//        this.isLookPlayer = true;
//    }


//    protected virtual void ShootLazerSound()
//    {
//        int indexSound = Random.Range(0,listLazerSnds.Count);
//        SoundFXManager.Instance.PlaySoundFXClip(listLazerSnds[indexSound],transform);
//    }
//    //protected virtual void JumpToPlayer()
//    //{
//    //    this._agent.enabled = false;
//    //    Vector3 targetPos = this.playerPosition.position;
//    //    targetPos.y = transform.position.y;
//    //    float jumpPower = this.heightJump;     
//    //    float duration = this.durationJump;

//    //    this.transform.DOJump(targetPos, jumpPower, this.jumpCount, duration).OnComplete(() => {
//    //        Debug.Log("Boss đã nhảy tới Player");
//    //        currentState = BossState.Attack;
//    //        this._agent.enabled = true;
//    //        jumpAttackZone.gameObject.SetActive(true);
//    //    });
//    //    jumpAttackZone.gameObject.SetActive(false);
//    //}
//    [SerializeField] protected ExplosionSpawner explosionSpawner;
//    [SerializeField] protected Explosion explosion;
//    protected virtual void LoadExplosionSpawner()
//    {
//        if (this.explosionSpawner != null) return;
//        this.explosionSpawner = FindAnyObjectByType<ExplosionSpawner>();
//    }
//    protected virtual void LoadExplosion()
//    {
//        if (this.explosion != null) return;
//        List<Explosion> allMyComponents = ComponentFinder.FindAllComponentsInScene<Explosion>();
//        this.explosion = allMyComponents[0];
//    }



//    protected override void Die()
//    {
//        StartCoroutine(DieExplosionRoutine());
//        // Trigger animation chết, disable collider, ...
//    }
//    private IEnumerator DieExplosionRoutine()
//    {
//        this.isAttackPlayer = false;
//        this.isLookPlayer = false;
//        this._agent.enabled = false;
//        Explosion newExplosion = this.explosionSpawner.Spawn(explosion, transform.position);
//        yield return new WaitForSeconds(1f);
//        Explosion newExplosion1 = this.explosionSpawner.Spawn(explosion, transform.position+ new Vector3(-1,0,0));
//        yield return new WaitForSeconds(1f);
//        Explosion newExplosion2 = this.explosionSpawner.Spawn(explosion, transform.position + new Vector3(1, 0, 0));
//        yield return new WaitForSeconds(1f);
//        Explosion newExplosion3 = this.explosionSpawner.Spawn(explosion, transform.position + new Vector3(-1, 0, 0));
//        yield return new WaitForSeconds(1f);
//        Explosion newExplosion4 = this.explosionSpawner.Spawn(explosion, transform.position + new Vector3(1, 0, 0));
//        yield return new WaitForSeconds(1f);
//        Explosion newExplosion5 = this.explosionSpawner.Spawn(explosion, transform.position);
//        this.gameObject.SetActive(false);
//    }
//}

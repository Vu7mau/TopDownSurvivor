using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;



public class RobotZombie : Boss
{
    [SerializeField] protected LightBulletSpawner lightBulletSpawner;
    [SerializeField] protected MissileSpawner missileSpawner;

    [Header("Shooting")]
    [SerializeField] protected LightBullet lightBulletPrefab;
    [SerializeField] protected Transform position;
    [SerializeField] protected int lightBulletSpawnCount;
    [SerializeField] protected float lightBulletSpawnRadius = 1.5f;

    [Header("Shooting")]
    [SerializeField] protected Missile missilePrefab;

    [Header("Jump Attack")]
    [SerializeField] protected float heightJump = 2f;
    [SerializeField] protected float durationJump = 1f;
    [SerializeField] protected int jumpCount = 1;
    [SerializeField] protected Transform jumpAttackZone;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadMissileSpawn();
        this.LoadLightBulletSpawn();
        this.LoadLightBullet();
        this.LoadMissile();
    }
    protected virtual void LoadMissileSpawn()
    {
        if (this.missileSpawner != null) return;
        this.missileSpawner = FindAnyObjectByType<MissileSpawner>();
    }
    protected virtual void LoadLightBulletSpawn()
    {
        if (this.lightBulletSpawner != null) return;
        this.lightBulletSpawner = FindAnyObjectByType<LightBulletSpawner>();
    }
    protected virtual void LoadLightBullet()
    {
        if (this.lightBulletPrefab != null) return;
        List<LightBullet> allMyComponents = ComponentFinder.FindAllComponentsInScene<LightBullet>();
        this.lightBulletPrefab = allMyComponents[0];
    }
    protected virtual void LoadMissile()
    {
        if (this.missilePrefab != null) return;
        List<Missile> allMyComponents = ComponentFinder.FindAllComponentsInScene<Missile>();
        this.missilePrefab = allMyComponents[0];
    }

    protected override void Attack()
    {
        StartCoroutine(AttackRoutine());
    }
    protected virtual IEnumerator AttackRoutine()
    {
        currentState = BossState.Idle; // Tạm dừng trước khi chuyển lại Chase

        int attackIndex = Random.Range(0, this.amountAttacksAnimation); // Giả sử có 3 đòn tấn công

        switch (attackIndex)
        {
            case 0:
                this.AttackTypeA();
                break;
            case 1:
                this.AttackTypeB();
                break;
            case 2:
                this.AttackTypeC();
                break;
        }

        if (Vector3.Distance(transform.position, playerPosition.position) > attackBaseRange)
        {
            currentState = BossState.Chase;
            this.isAttackPlayer = false;
        }
        else
            currentState = BossState.Attack;
        yield return null;
    }
    protected virtual void AttackTypeA()
    {
        Debug.Log("Đòn đánh A");
        StartCoroutine(this.RadialShootRoutine());
        //StartCoroutine(ShootRoutine());
    }
    protected IEnumerator RadialShootRoutine()
    {
        this._agent.enabled = false;
        for (int i = 0; i < lightBulletSpawnCount; i++)
        {
            float angle = i * Mathf.PI * 2f / lightBulletSpawnCount;
            Vector3 dir = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)).normalized;
            Vector3 spawnPos = position.position + dir * lightBulletSpawnRadius;
            LightBullet newLightBullet = this.lightBulletSpawner.Spawn(this.lightBulletPrefab,spawnPos);
            if (newLightBullet == null) yield break;
            newLightBullet.Direction = dir;
            newLightBullet.SetDirection(dir);
        }
        yield return new WaitForSeconds(2f);
    }
    //protected IEnumerator ShootRoutine()
    //{
    //    for(int i = 0; i < this.baseAmountAttack; i++)
    //    {
    //        Transform newObj = Instantiate(bulletPrefab, position.position, Quaternion.identity);
    //        newObj.gameObject.GetComponent<Projectitle>().ShootAt(this.playerPosition.position);
    //        yield return new WaitForSeconds(0.5f);
    //    }
    //}
    protected virtual void AttackTypeB()
    {
        Debug.Log("Đòn đánh B");
        StartCoroutine(this.MissileRobotZombieRoutine());
        
    }
    protected IEnumerator MissileRobotZombieRoutine()
    {
        this._agent.enabled = false;
        Missile newMissile = this.missileSpawner.Spawn(this.missilePrefab, transform.position);
        newMissile.transform.rotation = Quaternion.Euler(0, 0, 0);
        newMissile.gameObject.GetComponent<Projectitle>().ShootAt(newMissile.transform.position + new Vector3(0,10000,0));
        yield return new WaitForSeconds(1f);
        newMissile.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        newMissile.gameObject.SetActive(true);
        newMissile.transform.rotation = Quaternion.Euler(0, 180, 0);
        newMissile.gameObject.GetComponent<Projectitle>().ShootAt(this.playerPosition.position);
    }


    protected virtual void AttackTypeC()
    {
        Debug.Log("Đòn đánh C");
        this.JumpToPlayer();
    }
    protected virtual void JumpToPlayer()
    {
        this._agent.enabled = false;
        Vector3 targetPos = this.playerPosition.position;
        targetPos.y = transform.position.y;
        float jumpPower = this.heightJump;     
        float duration = this.durationJump;      

        this.transform.DOJump(targetPos, jumpPower, this.jumpCount, duration).OnComplete(() => {
            Debug.Log("Boss đã nhảy tới Player");
            currentState = BossState.Attack;
            this._agent.enabled = true;
            jumpAttackZone.gameObject.SetActive(true);
        });
        jumpAttackZone.gameObject.SetActive(false);
    }
}

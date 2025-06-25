using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : DamageReceiver,IEnemy
{
    [SerializeField] protected EnemyAI enemyAI;
    [SerializeField] protected bool _canTakeDamage = true;


    [Header("These components would be loaded when run the game!")]
    //[SerializeField] protected EnemyDamageReceiver enemyDamageReceiver;
    [SerializeField] protected HpBarObj healthBarObj;

    [SerializeField] protected SpawnEnemies _spawnEnemies;
    [SerializeField] protected HitDamageSpawner hitDamageSpawner;
    [SerializeField] protected EnemyCtrlDespawn enemyCtrlDespawn;

    [SerializeField] protected BloodSplash bloodSplash;


    protected float _amountIncrease = 0;
    public float Health
    {
        get { return _hp; }
    }
    public float MaxHealth
    {
        get { return _hpMax; }
    }



    protected override void OnEnable()
    {
        base.OnEnable();
        this.RebornEnemy();
    }
    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void LoadComponents()
    {
        ///Load All Components
        base.LoadComponents();
        this.LoadEnemyAI();
        this.LoadSpawnEnemies();
        this.LoadHealthBar();
        this.LoadEnemyCtrlDespawn();
        this.LoadHitDamageSpawner();
        this.LoadBloodSplash();
        //this.LoadEnemyDamageReceiver();

        ///Load Enemies
        this.RebornEnemy();

    }




    //Load Components

    protected virtual void LoadHealthBar()
    {
        if (this.healthBarObj != null) return;
        this.healthBarObj = GetComponentInChildren<HpBarObj>();
        this.healthBarObj.gameObject.SetActive(true);
    }
    protected virtual void LoadSpawnEnemies()
    {
        if (this._spawnEnemies != null) return;
        this._spawnEnemies = FindAnyObjectByType<SpawnEnemies>();
    }
    protected virtual void LoadEnemyAI()
    {
        if (this.enemyAI != null) return;
        this.enemyAI = GetComponentInChildren<EnemyAI>();
    }
    protected virtual void LoadEnemyCtrlDespawn()
    {
        if (this.enemyCtrlDespawn != null) return;
        this.enemyCtrlDespawn = GetComponentInChildren<EnemyCtrlDespawn>();
    }
    protected virtual void LoadHitDamageSpawner()
    {
        if (this.hitDamageSpawner != null) return;
        this.hitDamageSpawner = FindAnyObjectByType<HitDamageSpawner>();
    }
    protected virtual void LoadBloodSplash()
    {
        if (this.bloodSplash != null) return;
        List<BloodSplash> allMyComponents = ComponentFinder.FindAllComponentsInScene<BloodSplash>();
        this.bloodSplash = allMyComponents[0];
    }


    //Reset Components
    protected virtual void RebornEnemy()
    {
        this.ResetHealthGeneral();
        this.ResetStateCollision();
        base.Reborn();
    }
    protected virtual void ResetHealthGeneral()
    {
        this.healthBarObj.gameObject.SetActive(true);
        this._hpMax = (int)this.enemyAI.EnemySO.Health;
    }
    protected virtual void ResetStateCollision()
    {
        this.gameObject.GetComponent<Collider>().enabled = true;
    }
    protected virtual void ResetValues()
    {
        this._amountIncrease = 0;
    }


    //protected virtual void LoadEnemyDamageReceiver()
    //{
    //    if (this.enemyDamageReceiver != null) return;
    //    this.enemyDamageReceiver = GetComponentInChildren<EnemyDamageReceiver>();
    //    Debug.Log(transform.name + ":Load EnemyDamageReceiver!");
    //}



    public void CheckAmountIncreaseHealth(int _amountIncrease)
    {
        this._amountIncrease = (float)_amountIncrease / 100;
        Debug.Log("Lượng tăng là "+ this._amountIncrease);
        this.UpdateHealthEnemy(this._amountIncrease);
    }
    public void UpdateHealthEnemy(float _amountIncrease)
    {
        this._hpMax = this._hpMax + (int)(this._hpMax * _amountIncrease);
        Debug.Log("Máu của quái là: "+this._hpMax);
    }



    public void TakeDamage(int damage)
    {
        if (!_canTakeDamage) { return; }
        this.Deduct(damage);
    }
    protected override void OnDead()
    {
        if(this._spawnEnemies != null) this._spawnEnemies.EnemyDefeated(1);
        this.gameObject.GetComponent<Collider>().enabled = false;
        this._canTakeDamage = false;
        if (HasDeadState()) this.enemyAI.Animator.SetTrigger("die");
        if (!this._isDead) this._isDead = true;
    }
    public override void Deduct(int damage)
    {
        if(this._isDead) { OnDead(); return; }
        base.Deduct(damage);
        CharacterEvents.characterDamaged?.Invoke(this.gameObject, damage);
        Debug.Log("Máu quái còn " + this._hp);    
        //if (_animator != null)
        //{
        //    if (HasHurtState() && !this._isDead)
        //        _animator.SetTrigger("damage");

        //}
    }
    //public virtual bool HasHurtState() => this.enemyAI.Animator.HasState(0, Animator.StringToHash("getHit"));
    public virtual bool HasDeadState() => this.enemyAI.Animator.HasState(0, Animator.StringToHash("die"));
    protected override void HurtEffect()
    {
        if(beastHurtSFX != null)
            SoundFXManager.Instance.PlaySoundFXClip(beastHurtSFX, transform);
        this.HurtFXRoutine();
    }


    [Header("Hurt FX")]
    [SerializeField] protected AudioClip beastHurtSFX;
    
    [SerializeField] private Vector3 hurtScale;
    [SerializeField] private Vector3 hurtPositionOffset;
    private void HurtFXRoutine()
    {
        BloodSplash newBloodSplash = this.hitDamageSpawner.Spawn(bloodSplash, transform.position);
        if (newBloodSplash == null) return;
            newBloodSplash.transform.localScale = hurtScale;
        newBloodSplash.gameObject.SetActive(true);
    }



    //Others
    public void RewardPlayerAfterEnemyDead()
    {
        Rewards.Instance.RewardGemsPlayerWhenKillEnemy(this.enemyAI.EnemySO.amount_Gems, transform);
    }
    public void DeleteEnemyRoutine()
    {
        //if(this.enemyCtrlDespawn != null)
        //{
        //    this.enemyCtrlDespawn.DoDespawn();
        //    return;
        //}
        this.gameObject.SetActive(false);
    }
    public void Victory()
    {
       UIManager.Instance.DisplayPanelWhenPlayerKillBoss();
        //MissonTracker.Instance.BossKilled(this.gameObject);
    }
    public void DeleteEnemyWhileHpEqual0()
    {
        if (!GetComponent<Collider>().enabled && gameObject.activeInHierarchy)
        {
            DeleteEnemyRoutine();
            RewardPlayerAfterEnemyDead();
        }
    }
}

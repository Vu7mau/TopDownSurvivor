using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : DamageReceiver
{
    [SerializeField] protected bool _canGetDamage = true;
    public bool CanGetDamage { get => _canGetDamage; set => _canGetDamage = value;}


    [Header("These components would be loaded when run the game!")]
    [SerializeField] protected EnemyAI enemyAI;
    [SerializeField] protected HpBarObj healthBarObj;

    [SerializeField] protected HitDamageSpawner hitDamageSpawner;


    [SerializeField] protected BloodSplash bloodSplash;


    protected float _amountIncrease = 0;

    //public event Action OnDeathEnemy;


    public float Health
    {
        get => _hp;
    }
    public float MaxHealth
    {
        get => _hpMax;
    }

    protected virtual void OnValidate()
    {
        if(this._hp < 0) this._hp = 0;
        if(this._hpMax < 0) this._hpMax = 0;
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
        this.LoadHealthBar();
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
        if(this.healthBarObj == null) return;
        this.healthBarObj.gameObject.SetActive(true);
    }
    protected virtual void LoadEnemyAI()
    {
        if (this.enemyAI != null) return;
        this.enemyAI = GetComponentInChildren<EnemyAI>();
        if(this.enemyAI == null) return;
    }
    protected virtual void LoadHitDamageSpawner()
    {
        if (this.hitDamageSpawner != null) return;
        this.hitDamageSpawner = GetComponentInChildren<HitDamageSpawner>();
        if( this.hitDamageSpawner == null) return;
    }
    protected virtual void LoadBloodSplash()
    {
        if (this.bloodSplash != null) return;
        this.bloodSplash = GetComponentInChildren<BloodSplash>();
        if( this.bloodSplash == null) return;
    }


    //Reset Components
    protected virtual void RebornEnemy()
    {
        this.ResetStateCollision();
        this.ResetHealthGeneral();
        base.Reborn();
    }
    protected virtual void ResetHealthGeneral()
    {
        if(this.healthBarObj != null) this.healthBarObj.gameObject.SetActive(true);
        if(this.enemyAI != null) this._hpMax = (int)this.enemyAI.EnemySO.Health;
    }
    protected virtual void ResetStateCollision()
    {
        this._canGetDamage = true;
        //this.gameObject.GetComponent<Collider>().enabled = true;
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
        if (!this._canGetDamage) { return; }
        this.Deduct(damage);
    }
    protected override void OnDead()
    {

        //this.gameObject.GetComponent<Collider>().enabled = false;
        this._canGetDamage = false;
        if (!this._isDead)
        {
            this._isDead = true;
        }
    }
    public override void Deduct(int damage)
    {
        if(this._isDead) 
        {
            OnDead();
            return;
        }
        if (!this._canGetDamage) return;



        base.Deduct(damage);


        CharacterEvents.characterDamaged?.Invoke(this.gameObject, damage);

        //Debug.Log("Máu quái còn " + this._hp);
        //if (this.enemyAI.Animator != null)
        //{
        //    if (HasHurtState() && !this._isDead)
        //        this.enemyAI.Animator.SetTrigger("damage");

        //}
    }

    public override void GetDamageSource(DamageSender damageSender)
    {
        CharacterEvents.OnDamageSourceChanged?.Invoke(damageSender);
    }
    //public virtual bool HasHurtState() => this.enemyAI.Animator.HasState(0, Animator.StringToHash("getHit"));
    protected override void HurtEffect()
    {
        if(this.beastHurtSFXs.Count > 0)
        {
            int random = UnityEngine.Random.Range(0, this.beastHurtSFXs.Count);
            SoundFXManager.Instance.PlaySoundFXClip(this.beastHurtSFXs[random], this.transform);
            //Debug.Log("Hurt Scifi");
        }
        this.HurtFXRoutine();
    }


    [Header("Hurt FX")]
    [SerializeField] protected List<AudioClip> beastHurtSFXs;
    
    [SerializeField] protected Vector3 hurtScale = new Vector3(0.25f,0.25f,0.25f);
    //[SerializeField] protected Vector3 hurtPositionOffset;
    private void HurtFXRoutine()
    {
        if (this.hitDamageSpawner == null) return;
        if (this.bloodSplash == null) return;
        BloodSplash newBloodSplash = this.hitDamageSpawner.Spawn(this.bloodSplash, this.transform.position);
        if (newBloodSplash != null)
        {
            newBloodSplash.transform.localScale = this.hurtScale;
            newBloodSplash.gameObject.SetActive(true);
        }
    }



    ////Others
    //public void RewardPlayerAfterEnemyDead()
    //{
    //    Rewards.Instance.RewardGemsPlayerWhenKillEnemy(this.enemyAI.EnemySO.amount_Gems, transform);
    //}


    //public void Victory()
    //{
    //   UIManager.Instance.DisplayPanelWhenPlayerKillBoss();
    //    //MissonTracker.Instance.BossKilled(this.gameObject);
    //}
}

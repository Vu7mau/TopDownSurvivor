using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : DamageReceiver,IEnemy
{
    [SerializeField] private EnemySO enemySO;
    [SerializeField] private bool _canTakeDamage = true;
    [SerializeField] private AudioClip beastHurtSFX;
    //[SerializeField] protected EnemyDamageReceiver enemyDamageReceiver;


    private Rigidbody rb;
    private Animator _animator;
    private SpawnEnemies _spawnEnemies;


    private float _amountIncrease = 0;
    public float Health
    {
        get { return _hp; }
    }
    public float MaxHealth
    {
        get { return _hpMax; }
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        //this.LoadEnemyDamageReceiver();
    }

    protected override void Awake()
    {
        base.Awake();
        this.rb = GetComponent<Rigidbody>();
        this._animator = GetComponent<Animator>();
        this._spawnEnemies =FindAnyObjectByType<SpawnEnemies>();
    }
    protected override void Start()
    {
        base.Start();
        this.ResetStateEnemyDefault();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        this.ResetStateEnemyDefault();
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        this._amountIncrease = 0;
    }
    protected virtual void ResetStateEnemyDefault()
    {
        this._hpMax = (int)enemySO.Health;
        base.Reborn();
        this.rb.isKinematic = false;
        this.gameObject.GetComponent<Collider>().enabled = true;
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
        this.gameObject.GetComponent<Collider>().enabled = false;
        this.rb.isKinematic= true;
        this._canTakeDamage = false;
        if (HasDeadState())
            this._animator.SetTrigger("die");
        if (!this._isDead)
        {
            //_spawnEnemies.EnemyDefeated(1);
            this._isDead = true;
        }
    }
    public override void Deduct(int damage)
    {
        if(this._isDead) { OnDead(); return; }
        base.Deduct(damage);
        Debug.Log("Máu quái còn " + this._hp);    
        if (_animator != null)
        {
            if (HasHurtState() && !this._isDead)
                _animator.SetTrigger("damage");
            CharacterEvents.characterDamaged?.Invoke(this.gameObject, damage);
        }
    }
    public virtual bool HasHurtState() => _animator.HasState(0, Animator.StringToHash("getHit"));
    public virtual bool HasDeadState() => _animator.HasState(0, Animator.StringToHash("die"));
    protected override void HurtEffect()
    {
        if(beastHurtSFX != null)
            SoundFXManager.Instance.PlaySoundFXClip(beastHurtSFX, transform);
        StartCoroutine(HurtFXRoutine());
    }



    [SerializeField] private Vector3 hurtScale;
    [SerializeField] private Vector3 hurtPositionOffset;
    private IEnumerator HurtFXRoutine()
    {
        PoolingObjectList hitPoolingObj = GameObject.Find("HitPooling").GetComponent<PoolingObjectList>();
        Transform hurt = hitPoolingObj.GetPoolingObject().transform;
        if (hurt != null)
        {
            hurt.position = transform.position + hurtPositionOffset;
            hurt.transform.localScale = hurtScale;
            hurt.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.75f);
            hitPoolingObj.ReturnToPool(hurt);
        }
    }
    public void RewardPlayerAfterEnemyDead()
    {
        Rewards.Instance.RewardGemsPlayerWhenKillEnemy(enemySO.amount_Gems, transform);
    }
    public void DeleteEnemyRoutine()
    {
        gameObject.SetActive(false);
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

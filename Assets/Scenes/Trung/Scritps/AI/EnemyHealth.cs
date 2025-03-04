using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : EnemyDamageReceiver
{
    [SerializeField] private EnemySO enemySO;
    [SerializeField] private bool _canTakeDamage = true;
    private Rigidbody rb;
    private bool hasState;
    private float _health;
    private float _amountIncrease = 0;
    private Animator _animator;
    private SpawnEnemies _spawnEnemies;
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private BossHealthBar _bossHealthBar;
    [SerializeField] private bool EnemyIsDead = false;
    [SerializeField] private AudioClip beastHurtSFX;
    public float Health
    {
        get { return _health; }
        set { _health = value; }
    }
    protected override void Awake()
    {
        base.Awake();
        rb= GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _spawnEnemies =FindAnyObjectByType<SpawnEnemies>();
    }
    protected override void Start()
    {
        base.Start();
        base.Reborn();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        UpdateEnemy();
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        this._amountIncrease = 0;
    }
    public void UpdateEnemy()
    {
        EnemyIsDead = false;
        rb.isKinematic = false;
        this._hpMax = (int)enemySO.Health;
        this._hp = this._hpMax;
        _healthBar?.LoadMaxHealth(this._hpMax);
        _bossHealthBar?.SetUpBar(this._hpMax);
        _healthBar?.gameObject.SetActive(true);
        _bossHealthBar?.gameObject.SetActive(true);
        gameObject.GetComponent<Collider>().enabled = true;
    }
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
        _healthBar?.LoadMaxHealth(this._hpMax);
        _bossHealthBar?.SetUpBar(this._hpMax);
    }
    public void TakeDamage(int damage)
    {
        if (!_canTakeDamage) { return; }
        this.Deduct(damage);
    }
    protected override void OnDead()
    {
        base.OnDead();
        gameObject.GetComponent<Collider>().enabled = false;
        rb.isKinematic= true;
        _canTakeDamage = false;
        hasState = _animator.HasState(0, Animator.StringToHash("die"));
        if (hasState)
            _animator.SetTrigger("die");
        //_animator.SetTrigger("die");
        if (!EnemyIsDead)
        {
            _spawnEnemies.EnemyDefeated(1);
            EnemyIsDead=true;
        }
    }
    public override void Deduct(int damage)
    {
        if(this._isDead) { OnDead(); return; }
        base.Deduct(damage);
        if (beastHurtSFX != null)
            
        if (_animator != null)
        {
            hasState = _animator.HasState(0, Animator.StringToHash("getHit"));
            if (hasState && !this._isDead)
                _animator.SetTrigger("damage");
            CharacterEvents.characterDamaged?.Invoke(gameObject, damage);
        }
    }
    protected override void CheckIsDead()
    {
        _healthBar?.LoadHealthBar(this._hp);
        _bossHealthBar?.UpdateCurrentHealthBoss(this._hp);
        base.CheckIsDead();
    }
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

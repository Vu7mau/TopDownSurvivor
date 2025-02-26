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
    private Animator _animator;
    private SpawnEnemies _spawnEnemies;
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private BossHealthBar _bossHealthBar;
    [SerializeField] private bool EnemyIsDead = false;
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
        EnemyIsDead = false;
        rb.isKinematic = false;
        this._hpMax = (int)enemySO.Health;
        _healthBar?.LoadMaxHealth(this._hpMax);
        _bossHealthBar?.SetUpBar(this._hpMax);
        rb.isKinematic = false;
        _healthBar?.gameObject.SetActive(true);
        _bossHealthBar?.gameObject.SetActive(true);
        gameObject.GetComponent<Collider>().enabled = true;
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
        if (_animator != null)
        {
            hasState = _animator.HasState(0, Animator.StringToHash("getHit"));
            if (hasState && !this._isDead)
                _animator.SetTrigger("damage");
            CharacterEvents.characterDamaged?.Invoke(gameObject, damage);
            _healthBar?.LoadHealthBar(this._hp);
            _bossHealthBar?.UpdateCurrentHealthBoss(this._hp);
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

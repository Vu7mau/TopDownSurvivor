using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : EnemyDamageReceiver
{
    public delegate void Action();
    public event Action onDeath;


    [SerializeField] private EnemySO enemySO;
    private float _health;
    private Animator _animator;
    [SerializeField] private bool _canTakeDamage = true;
    private Rigidbody rb;
    private bool hasState;
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
    }
    protected override void Start()
    {
        base.Start();
        _hpMax = (int)enemySO.Health;
        base.Reborn();
        rb.isKinematic = false;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
    }
    public void TakeDamage(int damage)
    {
        if (!_canTakeDamage) { return; }
        this.Deduct(damage);
    }
    protected override void OnDead()
    {
        base.OnDead();
        gameObject.GetComponent<BoxCollider>().enabled = false;
        rb.isKinematic= true;
        _canTakeDamage = false;
        Debug.Log("Quái đã chết");
        _animator.SetTrigger("die");
    }
    public override void Deduct(int damage)
    {
        if(this._isDead) { OnDead(); return; }
        base.Deduct(damage);
        if (_animator != null)
        {
            hasState = _animator.HasState(0, Animator.StringToHash("getHit"));
            Debug.Log($"State tồn tại: {hasState}");
            if (hasState && !this._isDead)
            {
                _animator.SetTrigger("damage");
            }
            Debug.Log("Máu quái còn: " + this._hp);
            
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
}

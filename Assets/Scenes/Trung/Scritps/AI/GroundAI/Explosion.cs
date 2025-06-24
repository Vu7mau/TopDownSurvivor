using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : PoolObj
{
    [SerializeField] protected string _name;
    public string Name { get => _name; }

    public override string GetName() => _name;
    protected bool _canTakeDamage= false;
    [SerializeField] protected CreateHitEnemy hit;
    public CreateHitEnemy Hit => hit;
    protected override void OnDisable()
    {
        base.OnDisable();
        this.LoadStateDefault();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        this.LoadStateDefault();
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadCreateHitEnemy();
    }
    protected virtual void LoadStateDefault()
    {
        this._canTakeDamage = false;
        this.hit.gameObject.SetActive(true);
    }
    protected override void Start()
    {
        base.Start();
    }

    protected virtual void LoadCreateHitEnemy()
    {
        if (this.hit != null) return;
        this.hit = GetComponentInChildren<CreateHitEnemy>();
    }
    private void OnTriggerEnter(Collider other)
    {
        CharacterDamageReceiver player = other.GetComponent<CharacterDamageReceiver>();
        if (player != null && !_canTakeDamage)
        {
            Rigidbody rb = player.gameObject.GetComponentInChildren<Rigidbody>();
            _canTakeDamage = true;
            AddForceToTarget(rb);
            Debug.Log("Đã chạm Player!");
        }
    }
    private void AddForceToTarget(Rigidbody rb)
    {
        if (rb.CompareTag("Enemy")) return;
        rb.AddForce(new Vector3(0,15,0),ForceMode.Impulse);
    }
}

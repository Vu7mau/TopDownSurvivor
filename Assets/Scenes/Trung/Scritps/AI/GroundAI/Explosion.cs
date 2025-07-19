 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : EffectFX
{
    [SerializeField] protected CreateHitEnemy hit;

    [SerializeField] protected float force = 5f;

    public CreateHitEnemy Hit => hit;

    protected bool _canTakeDamage= false;
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
        if(this.Hit != null)
        {
            this.hit.gameObject.SetActive(true);
            this.hit.CanTakeDamage = this._canTakeDamage;
        }
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
        rb.AddForce(new Vector3(0, force, 0),ForceMode.Impulse);
    }
}

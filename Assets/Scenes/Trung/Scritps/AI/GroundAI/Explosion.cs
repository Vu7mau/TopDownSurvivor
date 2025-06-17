using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : PoolObj
{
    protected bool _canTakeDamage= false;
    public override string GetName() => "Explosion";
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
    protected virtual void LoadStateDefault()
    {
        this._canTakeDamage = false;
        this.transform.GetComponentInChildren<CreateHitEnemy>().transform.gameObject.SetActive(true);
    }
    protected override void Start()
    {
        base.Start();
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

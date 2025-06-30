using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateHitEnemy : DamageSender
{
    [SerializeField] protected bool canTakeDamage = false;
    public bool CanTakeDamage { get => canTakeDamage; set { canTakeDamage = value; } }
    [SerializeField] protected EnemySO enemySO;
    protected int dem;
    protected int _amountDamagePercent;
    protected override void OnEnable()
    {
        base.OnEnable();
        canTakeDamage = false;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        canTakeDamage = false;
    }
    protected override void Start()
    {
        base.Start();
        if (enemySO != null)
        {
            this._basedDamage = enemySO.Damage;
            UpdateDamage(this._amountDamagePercent);
        }
      //  this.SetDamage(_basedDamage);
    }
    public virtual void UpdateDamage(int _amount)
    {
        this._basedDamage = enemySO.Damage;
        //this._basedDamage = this._basedDamage + (int)((float)(this._basedDamage * _amount) / 100);
    }
    public virtual void IncreaseDamageAmount(int _amount)
    {
        this._amountDamagePercent = _amount;
    }
    protected void OnTriggerEnter(Collider other)
    {

        // Debug.Log("Va cham!");
        //Nếu va chạm với Player thì Player sẽ bị mất máu
        //CharacterCtrl player = other.GetComponent<CharacterCtrl>();
        CharacterDamageReceiver characterDamageReceiver = other.GetComponent<CharacterDamageReceiver>();
        if (characterDamageReceiver != null && !canTakeDamage)
        {
            this.Send(other.transform);
            dem++;
            Debug.Log($"Đã va chạm với CharacterDamageReceiver {dem} lần");
            canTakeDamage = true;
        }
    }

    public virtual void TriggerEnter(Collider other)
    {
        this.OnTriggerEnter(other);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateHitEnemy : DamageSender
{
    [SerializeField] protected bool CanTakeDamage = false;
    [SerializeField] protected EnemySO enemySO;
    protected int dem;
    protected int _amountDamagePercent;
    protected override void OnEnable()
    {
        base.OnEnable();
        CanTakeDamage = false;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        CanTakeDamage = false;
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
        if (characterDamageReceiver != null && !CanTakeDamage)
        {
            this.Send(other.transform);
            dem++;
            Debug.Log($"Đã va chạm với CharacterDamageReceiver {dem} lần");
            CanTakeDamage = true;
        }
    }
}

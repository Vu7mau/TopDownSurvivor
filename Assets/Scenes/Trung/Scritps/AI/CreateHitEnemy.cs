using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateHitEnemy : DamageSender
{
    [SerializeField] private bool CanTakeDamage = false;
    [SerializeField] private EnemySO enemySO;
     int dem;
    private int _amountDamagePercent;
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
        if(enemySO != null)
        {
            this._basedDamage = enemySO.Damage;
            UpdateDamage(this._amountDamagePercent);
        }
      //  this.SetDamage(_basedDamage);
    }
    public void UpdateDamage(int _amount)
    {
        this._basedDamage = enemySO.Damage;
        this._basedDamage = this._basedDamage + (int)((float)(this._basedDamage * _amount) / 100);
    }
    public void IncreaseDamageAmount(int _amount)
    {
        this._amountDamagePercent = _amount;
    }
    private void OnTriggerEnter(Collider other)
    {

        // Debug.Log("Va cham!");
        //Nếu va chạm với Player thì Player sẽ bị mất máu
        //CharacterCtrl player = other.GetComponent<CharacterCtrl>();
        CharacterDamageReceiver player = other.GetComponent<CharacterDamageReceiver>();
        if (player != null && !CanTakeDamage)
        {
            this.Send(other.transform);
            dem++;
            Debug.Log($"Đã va chạm với Player {dem} lần");
            CanTakeDamage = true;
        }
    }
}

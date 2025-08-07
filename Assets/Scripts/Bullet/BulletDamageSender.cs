using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamageSender : DamageSender
{
    [Space]
    [Header("BulletDamageSender")]
    [SerializeField] protected BulletCtrl _bulletCtrl;

    public int DamageBonus => this._bulletCtrl.CharacterCtrl.GetDamageFromStats();

    protected override void OnEnable()
    {
        base.OnEnable();
        //this.UpdateDamage();
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadBulletCtrl();  
       // this.UpdateDamage();
       
    }
    
    protected virtual void LoadBulletCtrl()
    {
        if (_bulletCtrl != null) return;


        this._bulletCtrl=transform.parent.GetComponent<BulletCtrl>();
        Debug.Log("LoadBulletCtrl success " + this._bulletCtrl.transform.name);
    }

    //protected virtual void UpdateDamage()
    //{
    //   int damage=this._basedDamage+this.DamageBonus;
    //    this.SetDamage(damage);
    //}
    protected override int SetDamage()
    {
        int damage = this._basedDamage + this.DamageBonus;
        return damage;
    }
}

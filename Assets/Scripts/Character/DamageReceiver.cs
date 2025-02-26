using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[RequireComponent(typeof(SphereCollider))]

public abstract class DamageReceiver : VuMonoBehaviour
{
    [Space]
    [Header("DamageReceiver")]
   // [SerializeField] protected SphereCollider _sphereCollider;
    [SerializeField] protected int _hp;
    [SerializeField] protected int _hpMax=10;
    [SerializeField] protected bool _isDead = false;
   
    protected override void OnEnable()
    {
        base.OnEnable();
        this.Reborn();
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
    }
    //protected virtual void LoadCollider()
    //{
    //    if (_sphereCollider != null) return;

    //    this._sphereCollider = GetComponent<SphereCollider>();
    //    this._sphereCollider.isTrigger = true;
    //  //  this._sphereCollider.radius = 0.686757f;
    //    Debug.Log("Load collider success"+ this._sphereCollider.name);
    //}

    protected virtual void Reborn()
    {
        this._hp=this._hpMax;
        this._isDead = false;
    }
   
    public virtual void Add(int add)
    {
        this._hp += add;
        if (this._hp > this._hpMax) this._hp = this._hpMax;

    }
    public virtual void GetMaxHealth(int health)
    {
        this._hpMax = health;
    }    

    public virtual void Deduct(int Deduct)
    {
        if(this._isDead) return;
        this.HurtEffect();
        this._hp -= Deduct;    
        if (this._hp <= 0) this._hp= 0;

        this.CheckIsDead();
    }
   
    public virtual bool IsDead()
    {     
        return this._hp <= 0;
    }
    protected abstract void OnDead();

    protected virtual void CheckIsDead()
    {
        if (!this.IsDead()) return;

        this._isDead = true;
        this.OnDead();
        CharacterCtrl.Instance.CharacterLeveUp.AddExp(2);
        SetCoin.Instance.setCoin(10);


    }
    protected virtual void HurtEffect()
    {
    }
}

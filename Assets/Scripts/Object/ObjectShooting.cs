using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectShooting : VuMonoBehaviour
{
    [Header("Object Shooting")]
    [SerializeField] protected bool _isShooting = false;
    [SerializeField] protected bool _isReloadAmmour = false;
//    public bool IsFire=> _isShooting;
    [SerializeField] protected float _shootDelay = .2f;
    [SerializeField] protected float _shootTimer = 0f;
    [SerializeField] protected int _MaxBulletCount = 30;
    [SerializeField] protected int _bulletsCount = 0;
    [SerializeField] protected float _reloadAmmoTime = 2f;
    [SerializeField] protected float _reloadAmmoTimer = 0f;

    protected override void OnEnable()
    {
        base.OnDisable();

        this._bulletsCount = this._MaxBulletCount;
    }

    protected virtual void Update()
    {
        this.IsShooting();
    }
    protected virtual void FixedUpdate()
    {
        this.Shooting();
    }
    protected virtual void Shooting()
    {
       
    
        this._shootTimer += Time.fixedDeltaTime;
        
        if (this.IsReloadingAmmo()) return;
        if (!this.IsShooting()) return;
        if (this._shootTimer < this._shootDelay) return;
        this._shootTimer = 0;
        // Shoot
        this.Shoot();
        this._bulletsCount --;

    }
    protected abstract void Shoot();

    protected virtual bool IsReloadingAmmo()
    {
        if (this._bulletsCount >=0) return false ;

      
        if (this._reloadAmmoTimer < this._reloadAmmoTime)
        {

            //CursorManager.Instance.StartReloadAnimation(this._reloadAmmoTime);
            this._reloadAmmoTimer += Time.fixedDeltaTime;
            _isReloadAmmour = true;
            return true;
        }

        this._reloadAmmoTimer = 0;
        this._bulletsCount =this._MaxBulletCount;
         _isReloadAmmour = false;
        return false;
    }
    protected abstract bool IsShooting();
}


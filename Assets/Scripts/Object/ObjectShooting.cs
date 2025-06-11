using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public abstract class ObjectShooting : VuMonoBehaviour
{
    [Header("Object Shooting")]
    [SerializeField] protected WeponSO weaponInfo;
    [SerializeField] protected bool _isShooting = false;
    [SerializeField] protected bool _isReloadAmmour = false;
    //[SerializeField] protected float _shootDelay = .2f;
    [SerializeField] protected float _shootTimer = 0f;
    //[SerializeField] protected int _MaxBulletCount = 30;
    [SerializeField] protected int _bulletsCount = 0;
    //[SerializeField] protected float _reloadAmmoTime = 2f;
    [SerializeField] protected float _reloadAmmoTimer = 0f;
    [SerializeField] protected FireMode fireMode;
    [SerializeField] protected bool _isBursting = false;


    public bool IsShooting => _isShooting;

    protected override void OnEnable()
    {
        base.OnDisable();

        this._bulletsCount = weaponInfo._MaxBulletCount;
    }

    protected virtual void Update()
    {
        this.IsFireInputPresse();
    }
    protected virtual void FixedUpdate()
    {
        if (this.IsReloadingAmmo()) return;
        if (this._isShooting)
            this.Shooting();
        else
           this.HoldFire();
    }

    protected virtual void Shooting()
    {      
        this._shootTimer += Time.fixedDeltaTime;
        if (this._shootTimer < weaponInfo._shootDelay)
        {
            _isBursting = true;
            return;
        }
        this._shootTimer = 0;
        this.Shoot();
        this._bulletsCount--;


    }
    protected abstract void Shoot();

    protected virtual bool IsReloadingAmmo()
    {
        if (this._bulletsCount > 0) return false;
        if (this._reloadAmmoTimer < weaponInfo._reloadAmmoTime)
        {
            this._reloadAmmoTimer += Time.fixedDeltaTime;
            _isReloadAmmour = true;
            return true;
        }
        this._reloadAmmoTimer = 0;
        this._bulletsCount = weaponInfo._MaxBulletCount;
        _isReloadAmmour = false;
        return false;
    }
    protected abstract bool IsFireInputPresse();


    private int FireModeCheck()
    {
        switch (fireMode)
        {
            case FireMode.Single:
                return 1;

            case FireMode.Burst:
                return 3;

            case FireMode.Auto:
                return weaponInfo._MaxBulletCount;

            default:
                return 0;
        }
    }

    protected virtual void HoldFire()
    {
       
    }

    public enum FireMode
    {
        Single,     // Bắn từng viên
        Burst,      // Bắn một loạt (ví dụ 3 viên/lần)
        Auto        // Bắn tự động khi giữ nút
    }
}


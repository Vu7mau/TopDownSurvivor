using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectShooting : VuMonoBehaviour
{
    [Header("Object Shooting")]
    [SerializeField] protected WeponSO weaponInfo;
    [SerializeField] protected bool _isShooting = false;
    [SerializeField] protected bool _isReloadAmmour = false;
    [SerializeField] protected float _shootTimer = 0f;
    [SerializeField] protected int _bulletsCount = 0;
    [SerializeField] protected float _reloadAmmoTimer = 0f;
    [SerializeField] protected FireMode fireMode;
    [SerializeField] protected bool _isBursting = false;

    private int _totalBulletTemp = 0;


    public bool IsShooting => _isShooting;

    protected override void OnEnable()
    {
        _totalBulletTemp = weaponInfo.totalAmmo;
        this._bulletsCount = weaponInfo.maxBulletCount;
    }

    protected virtual void Update()
    {
        if(Input.GetKeyUp(KeyCode.R)&&_bulletsCount<weaponInfo.maxBulletCount)
        {
            _isReloadAmmour=true;
        }    
        this.IsFireInputPresse();
    }
    protected virtual void FixedUpdate()
    {
        if (this.IsReloadingAmmo()) return;
        this._shootTimer += Time.fixedDeltaTime;
        if (this._isShooting)
            this.Shooting();
        else if(!this._isShooting)
            this.HoldFire();


    }

    protected virtual void Shooting()
    {
    
        if (this._shootTimer < weaponInfo.shootDelay)
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
        // Kiểm tra nếu còn đạn trong kho
        if (_totalBulletTemp < 1&& _isReloadAmmour==false) return true;
        // Nếu thời gian reload chưa hết, tiếp tục reload

        // Kiểm tra nếu còn đạn trong băng đạn
        if (this._bulletsCount > 0 && _isReloadAmmour == false) return false;



        if (this._reloadAmmoTimer < weaponInfo.reloadAmmoTime)
        {
            this._reloadAmmoTimer += Time.deltaTime;  // Sử dụng Time.deltaTime cho Update
            _isReloadAmmour = true;
            return true;
        }

        // Nếu thời gian reload hoàn tất, tiến hành reload
        this._reloadAmmoTimer = 0;

        // Kiểm tra và cập nhật totalAmmo không bị giảm dưới 0
        if (_totalBulletTemp > weaponInfo.maxBulletCount)
        {
            _totalBulletTemp -= weaponInfo.maxBulletCount;
            this._bulletsCount = weaponInfo.maxBulletCount;
        }
        else
        {
            this._bulletsCount = _totalBulletTemp;
            _totalBulletTemp = 0;
        }


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
                return weaponInfo.maxBulletCount;

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


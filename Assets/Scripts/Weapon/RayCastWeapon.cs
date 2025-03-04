using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Experimental.GlobalIllumination;

public class RayCastWeapon : ObjectShooting
{


    [SerializeField] protected bool _isFiring => CharacterCtrl.Instance.CharacterShooting.IsShooting();
    [Space]
    [Header("RayCastWeapon")]
    [SerializeField] protected ParticleSystem _muzzelFlash;
    [SerializeField] protected LineRenderer lineRenderer;
    [SerializeField] protected LayerMask _enemyLayer;
    [SerializeField] protected Transform _gunPoint;
    [SerializeField] protected Texture _gunTexture;
    [SerializeField] protected string _weaponName;
    [SerializeField] protected bool _isWeaponActivate = false;

    protected RaycastHit _targetEnemy;
  //  bool isReload => IsReloadingAmmo();

    public ActiveWeapon.WeaponSlot weaponSlot;
    public Transform GunPoint => _gunPoint;
    public RaycastHit TargetEnemy => _targetEnemy;
    public string WeaponName => _weaponName;
    public bool IsWeaponActivate => _isWeaponActivate;

    public float ReloadAmmorTime=>this._reloadAmmoTime;


    protected override void Awake()
    {
        base.Awake();
        this._muzzelFlash = GetComponentInChildren<ParticleSystem>();

    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
    }

    protected override void Update()
    {
        base.Update();
        CinemachineCtrl.Instance.CinemachineZoom.SetIsZoom(this.IsShooting());
    }
  
    protected virtual void ShooterEffect()
    {
        if (this._muzzelFlash == null) return;
        this._muzzelFlash.Play();
    }

    protected override void Shoot()
    {
        Transform newBullet = BulletSpawner.Instance.Spawn(this.SetBulletType(), this.GunPoint.position, Quaternion.LookRotation(this.GunPoint.forward));
        if (newBullet == null) return;
        newBullet.gameObject.SetActive(true);
       
        this.ShooterEffect();
    }

    protected override bool IsShooting()
    {
        if (this._isWeaponActivate)
            return _isFiring;
        else return false;
    }
    protected virtual void ShootLaser()
    {
        if (!_isWeaponActivate) return;

        RaycastHit hit;
        Vector3 endPosition;

        if (Physics.Raycast(_gunPoint.position, _gunPoint.forward, out hit, 20, this._enemyLayer))
        {
            endPosition = hit.point;
            //this.SetTarget(hit);
            this._targetEnemy = hit;
        }
        else
        {
            endPosition = _gunPoint.position + _gunPoint.forward * 100;
            this._targetEnemy = hit;
        }
        lineRenderer.SetPosition(0, _gunPoint.position);
        lineRenderer.SetPosition(1, endPosition);

    }
    //protected virtual void SetTarget(RaycastHit target)
    //{
    //    this._targetEnemy = target;
    //}
    public virtual void SetIsWeaponActivate(bool isWeaponActivate)
    {
        _isWeaponActivate = isWeaponActivate;
    }
    protected virtual string SetBulletType()
    {
        return null;
    }
    public virtual bool GetIsReloadingAmmo()
    {
        return _isReloadAmmour;
    }
    public virtual int GetCurrentAmmour()
    {
        return this._bulletsCount;
    }
    public virtual int GetMaxBullets()
    {
        return this._MaxBulletCount;
    }
    public virtual Texture GunTexture()
    { 
        if(_gunTexture == null) return null;
        return _gunTexture;
    }
}

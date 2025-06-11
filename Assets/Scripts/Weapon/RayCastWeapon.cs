using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Experimental.GlobalIllumination;

public class RayCastWeapon : ObjectShooting
{


    [SerializeField] protected bool _isFiring => CharacterCtrl.Instance.CharacterShooting.IsPressShooting();
    [Space]
    [Header("RayCastWeapon")]
    [SerializeField] protected ParticleSystem _muzzelFlash;
    [SerializeField] protected LineRenderer lineRenderer;
    [SerializeField] protected Transform _gunPoint;
    [SerializeField] protected Transform _shellSpawnPos;
    [SerializeField] protected bool _isWeaponActivate = false;


    protected RaycastHit _targetEnemy;


    public WeaponSlot weaponSlot => weaponInfo.weaponSlot;
    public Transform GunPoint => _gunPoint;
    public RaycastHit TargetEnemy => _targetEnemy;
    public string WeaponName => weaponInfo.weaponName;
    public bool IsWeaponActivate => _isWeaponActivate;

    public float ReloadAmmorTime => weaponInfo._reloadAmmoTime;


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
        this.ActivateZoom();

    }

    protected virtual void ShooterEffect()
    {
        if (this._muzzelFlash == null) return;
        this._muzzelFlash.Play();
    }

    protected override void Shoot()
    {
        if (string.IsNullOrEmpty(this.SetBulletType())) return;
        Transform newBullet = BulletSpawner.Instance.Spawn(this.SetBulletType(), this.GunPoint.position, Quaternion.LookRotation(this.GunPoint.forward));
        //  if (newBullet == null) return;
        //  newBullet.gameObject.SetActive(true);
        this.ShooterEffect();
        this.SpawnShell();
    }
    protected virtual void SpawnShell()
    {
        if (string.IsNullOrEmpty(this.SetShellType())) return;
        Transform newBullet = ShellSpawner.Instance.Spawn(this.SetShellType(), this._shellSpawnPos.position, Quaternion.LookRotation(this._shellSpawnPos.forward));
        SoundFXManager.Instance.PlaySoundFXClip(SoundFXManager.Instance.handlingGun, this.transform);
    }
    protected override bool IsFireInputPresse()
    {
        if (this._isWeaponActivate)
        {
            _isShooting = _isFiring;
            return _isShooting;
        }
        else
            return false;
    }
    protected virtual void ShootLaser()
    {
        if (!_isWeaponActivate) return;

        RaycastHit hit;
        Vector3 endPosition;
        lineRenderer.enabled = true;

        if (Physics.Raycast(_gunPoint.position, _gunPoint.forward, out hit, 10, weaponInfo._enemyLayer))
        {
            // endPosition = hit.point;
            float distance = Vector3.Distance(this._gunPoint.position, hit.point);
            endPosition = _gunPoint.position + _gunPoint.forward * distance;
            //this.SetTarget(hit);
            this._targetEnemy = hit;
        }
        else
        {
            endPosition = _gunPoint.position + _gunPoint.forward * 10;
            this._targetEnemy = hit;
        }
        lineRenderer.SetPosition(0, _gunPoint.position);
        lineRenderer.SetPosition(1, endPosition);

    }
    protected virtual void ActivateZoom()
    {
        CinemachineCtrl.Instance.CinemachineZoom.ToggleZoom(IsShooting, weaponInfo.zoomSpeed);
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
        return string.Empty;
    }
    protected virtual string SetShellType()
    {
        return string.Empty;
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
        return weaponInfo._MaxBulletCount;
    }
    public virtual bool GetBurstLocked()
    {
        return _isBursting;
    }
    public virtual Texture GunTexture()
    {
        if (weaponInfo._gunTexture == null) return null;
        return weaponInfo._gunTexture;
    }
  
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class RayCastWeapon : ObjectShooting
{


    [SerializeField] protected bool _isFiring => CharacterCtrl.Instance.CharacterShooting.IsPressShooting();
    [SerializeField] protected Vector3 _mousePoint => CharacterCtrl.Instance.CharacterAim.GetAim().position;
    [Space]
    [Header("RayCastWeapon")]
    [SerializeField] protected ParticleSystem _muzzelFlash;
    [SerializeField] protected LineRenderer lineRenderer;
    [SerializeField] protected Transform _gunPoint;
    [SerializeField] protected Transform _shellSpawnPos;
    [SerializeField] protected bool _isWeaponActivate = false;


    protected RaycastHit _targetEnemy;


    [SerializeField] public Transform model;
    public WeaponSlot weaponSlot => weaponInfo.weaponSlot;
    public Transform GunPoint => _gunPoint;
    public Vector3 MousePoint => _mousePoint;
    public RaycastHit TargetEnemy => _targetEnemy;
    public string WeaponName => weaponInfo.weaponName;
    public bool IsWeaponActivate => _isWeaponActivate;

    public float ReloadAmmorTime => weaponInfo.reloadAmmoTime;


    protected override void Awake()
    {
        base.Awake();
        this._muzzelFlash = GetComponentInChildren<ParticleSystem>();

    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadModel();
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
        CharacterUIManager.OnWeaponReload?.Invoke(_bulletsCount, weaponInfo.maxBulletCount);
        this.ShooterEffect();
        this.SpawnShell();
    }
    protected virtual void SpawnShell()
    {
        if (string.IsNullOrEmpty(this.SetShellType())) return;
        Transform newBullet = ShellSpawner.Instance.Spawn(this.SetShellType(), this._shellSpawnPos.position, Quaternion.LookRotation(this._shellSpawnPos.forward));
    }
    protected override bool IsFireInputPresse()
    {
        if (this._isWeaponActivate)
        {
            _isShooting = _isFiring;
            return _isShooting;
        }
        else
            return _isShooting = false;
    }
    protected virtual void ShootLaser()
    {
        if (!_isWeaponActivate) return;

        RaycastHit hit;
        Vector3 endPosition;
        lineRenderer.enabled = true;

        Vector3 shootDirection = (MousePoint - GunPoint.position).normalized;
        shootDirection.y = 0;
        float distance = Vector3.Distance(this._gunPoint.position, MousePoint);
        if (Physics.Raycast(_gunPoint.position, _gunPoint.forward, out hit, distance, weaponInfo.enemyLayer))
        {
            endPosition = _gunPoint.position + _gunPoint.forward * distance;
            this._targetEnemy = hit;
        }
        else
        {
            endPosition = _gunPoint.position + _gunPoint.forward * distance;
            this._targetEnemy = hit;
        }
        lineRenderer.SetPosition(0, _gunPoint.position);
        lineRenderer.SetPosition(1, endPosition);

    }
    protected virtual void ActivateZoom()
    {
        CinemachineCtrl.Instance.CinemachineZoom.ToggleZoom(IsShooting, weaponInfo.zoomSpeed);
    }

    private void LoadModel()
    {
        if (model != null) return;

        this.model = this.transform.Find("Model").GetComponent<Transform>();
        if (this.model != null) Debug.Log("Load model success");
    }
    public virtual void SetIsWeaponActivate(bool isWeaponActivate)
    {
        _isWeaponActivate = isWeaponActivate;
        //  this.model.gameObject.SetActive(isWeaponActivate);    
    }
    protected override void HoldFire()
    {
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
    public virtual int GetMaxAmmour()
    {
        return weaponInfo.maxBulletCount;
    }
    public virtual bool GetBurstLocked()
    {
        return _isBursting;
    }
    public virtual Sprite GunSprite()
    {
        if (weaponInfo.gunImage == null) return null;
        return weaponInfo.gunImage;
    }
    public virtual void UpdateTotalBullet(int bulletCount)
    {
        weaponInfo.totalAmmo += bulletCount;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class RayCastWeapon : ObjectShooting
{


    [SerializeField] protected bool _isFiring => CharacterCtrl.Instance.CharacterShooting.IsShooting();
    [Space]
    [Header("RayCastWeapon")]
    [SerializeField] protected ParticleSystem _muzzelFlash;
    [SerializeField] protected Transform _gunPoint;
    [SerializeField] protected string _weaponName;
    [SerializeField] protected bool _isWeaponActivate = false;


    public Transform GunPoint => _gunPoint;
    public string WeaponName => _weaponName;
    public bool IsWeaponActivate => _isWeaponActivate;


    public ActiveWeapon.WeaponSlot weaponSlot;

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
        CinemachineCtrl.Instance.CinemachineZoom.SetIsZoom(this.IsShooting());
    }
    protected virtual void ShooterEffect()
    {
        if (this._muzzelFlash == null) return;


        this._muzzelFlash.Play();
    }

    protected override void Shoot()
    {
        Transform newBullet = BulletSpawner.Instance.Spawn(BulletSpawner.bulletOne, this.GunPoint.position, Quaternion.LookRotation(this.GunPoint.forward));
        if (newBullet == null) return;
        newBullet.gameObject.SetActive(true);
        SoundFXManager.Instance.PlaySoundFXClip(SoundFXManager.Instance.rifleShoot, this.GunPoint);
        this.ShooterEffect();
    }

    protected override bool IsShooting()
    {
        if (this._isWeaponActivate)
            return _isFiring;
        else return false;
    }

    public virtual void SetIsWeaponActivate(bool isWeaponActivate)
    {
        _isWeaponActivate = isWeaponActivate;
    }
}

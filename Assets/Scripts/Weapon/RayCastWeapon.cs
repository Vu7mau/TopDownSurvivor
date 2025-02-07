using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastWeapon : ObjectShooting
{
    [SerializeField] protected bool _isFiring=>CharacterCtrl.Instance.CharacterShooting.IsShooting();
    [SerializeField] protected ParticleSystem _muzzelFlash;
    [SerializeField] protected Transform _rayCastOrigin;
    [SerializeField] public Transform gunPoint;

    Ray _ray;
    RaycastHit _hit;

    protected override void Awake()
    {
        base.Awake();
        this._muzzelFlash=GetComponentInChildren<ParticleSystem>();
      
    }

    public virtual void FireMuzzelFlash()
    {
        if (this._muzzelFlash == null) return;

        _ray.origin=_rayCastOrigin.position;
        _ray.direction=_rayCastOrigin.forward;
        this._muzzelFlash.Play();
      
    }

    protected override void Shoot()
    {
        Transform newBullet = BulletSpawner.Instance.Spawn(BulletSpawner.bulletOne, this.gunPoint.position, Quaternion.LookRotation(this.gunPoint.forward));
        if (newBullet == null) return;
        newBullet.gameObject.SetActive(true);
        this.FireMuzzelFlash();
       // Debug.Log("do here");
    }

    protected override bool IsShooting()
    {
     return _isFiring;
    }
}

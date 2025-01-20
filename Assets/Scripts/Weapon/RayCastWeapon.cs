using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastWeapon : VuMonoBehaviour
{
    [SerializeField] protected bool _isFiring=>CharacterCtrl.Instance.CharacterShooting.IsFire;
    [SerializeField] protected ParticleSystem _muzzelFlash;
    [SerializeField] protected Transform _rayCastOrigin;

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
}

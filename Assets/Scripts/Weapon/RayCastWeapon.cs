using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastWeapon : ObjectShooting
{


    [Space]
    [Header("RayCastWeapon")]
    [SerializeField] protected ParticleSystem _muzzelFlash;
   // [SerializeField] protected Transform _rayCastOrigin;
    [SerializeField] public Transform gunPoint;
    [SerializeField] protected bool _isFiring => CharacterCtrl.Instance.CharacterShooting.IsShooting();

    [SerializeField] public AnimationClip weaponAnimation;
    //[SerializeField] Cinemachine.CinemachineImpulseSource source;


  
    protected override void Awake()
    {
        base.Awake();
        this._muzzelFlash = GetComponentInChildren<ParticleSystem>();

    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        
    }
  
    protected virtual void ShooterEffect()
    {
        if (this._muzzelFlash == null) return;

        //_ray.origin = _rayCastOrigin.position;
        //_ray.direction = _rayCastOrigin.forward;
        this._muzzelFlash.Play();

    }

    protected override void Shoot()
    {
        Transform newBullet = BulletSpawner.Instance.Spawn(BulletSpawner.bulletOne, this.gunPoint.position, Quaternion.LookRotation(this.gunPoint.forward));
        if (newBullet == null) return;
        newBullet.gameObject.SetActive(true);
        SoundFXManager.Instance.PlaySoundFXClip(SoundFXManager.Instance.rifleShoot, this.gunPoint);
        this.ShooterEffect();    
        // Debug.Log("do here");
    }

    protected override bool IsShooting()
    {
        return _isFiring;
    }
}

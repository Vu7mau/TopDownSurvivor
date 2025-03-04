using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class RifleShooter : RayCastWeapon
{
    //[Space]
    //[Header("PistolShooter")]
    //[SerializeField] protected LineRenderer lineRenderer;
    //[SerializeField] protected LayerMask _enemyLayer;


    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadLineRenderer();
    }
    protected virtual void LoadLineRenderer()
    {
        if (lineRenderer != null) return;

        lineRenderer = GetComponent<LineRenderer>();
    }
    protected override void ShooterEffect()
    {
        base.ShooterEffect();
        CinemachineCtrl.Instance.CinemachineShake.ShakeCamera(2f, .1f);
        SoundFXManager.Instance.PlaySoundFXClip(SoundFXManager.Instance.rifleShoot, this.GunPoint);
    }
    protected override string SetBulletType()
    {
        return BulletSpawner.bulletOne;
    }
    protected override void Update()
    {
        base.Update();
        this.ShootLaser();
    }
    protected override void ShootLaser()
    {
        base.ShootLaser();
    }

}

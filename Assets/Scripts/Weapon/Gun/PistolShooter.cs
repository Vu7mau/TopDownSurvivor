using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PistolShooter : RayCastWeapon
{
   
    protected override void Update()
    {
        base.Update();
        if (_isFiring)
            this.ShootLaser();
        else
            lineRenderer.enabled = false;
    }
    protected override void ShooterEffect()
    {
        base.ShooterEffect();
        CinemachineCtrl.Instance.CinemachineShake.ShakeCamera(5f, .1f);
        SoundFXManager.Instance.PlaySoundFXClip(SoundFXManager.Instance.pistloShoot, this.GunPoint);
    }
    protected override string SetBulletType()
    {
        return BulletSpawner.PistolBullet;
    }
    protected override void ShootLaser()
    {
        base.ShootLaser();
    }
  
}

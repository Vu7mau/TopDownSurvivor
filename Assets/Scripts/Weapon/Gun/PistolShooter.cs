using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PistolShooter : RayCastWeapon
{
    //[Space]
    //[Header("PistolShooter")]

  
    protected override void Update()
    {
        base.Update();
        this.ShootLaser();
    }
    protected override void ShooterEffect()
    {
        base.ShooterEffect();

        CinemachineCtrl.Instance.CinemachineShake.ShakeCamera(5f, .1f);
    }
    protected override string SetBulletType()
    {
        return BulletSpawner.bulletTwo;
    }
    protected override void ShootLaser()
    {
        base.ShootLaser();
    }

   
}

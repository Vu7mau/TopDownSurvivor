using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerShooter : RayCastWeapon
{
    protected override string SetBulletType()
    {
        return BulletSpawner.Flamethrower_bullet;
    }
    protected override void Shoot()
    {
        base.Shoot();
       
        //    this.ShooterEffect();
    }
    protected override void ShooterEffect()
    {
        base.ShooterEffect();
        //SoundFXManager.Instance.PlaySoundFXClip(SoundFXManager.Instance.flamethrower, this.GunPoint,.3f,1.9f);
    }
}

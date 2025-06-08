using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketShooter : RayCastWeapon
{
    protected override string SetBulletType()
    {
        return BulletSpawner.RocketBullet;
    }
    protected override void ShooterEffect()
    {
        base.ShooterEffect();
        CinemachineCtrl.Instance.CinemachineShake.ShakeCamera(weaponInfo.recoilSize, weaponInfo.recoilDuration);
        SoundFXManager.Instance.PlaySoundFXClip(SoundFXManager.Instance.pistloShoot, this.GunPoint);
    }
    protected override void ActivateZoom()
    {
       base.ActivateZoom();
    }
}

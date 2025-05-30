using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunShooter : RayCastWeapon
{
    [Space]
    [Header("MachineGunShooter")]
    [SerializeField] private float reduceSpeed = 7;
    protected override string SetBulletType()
    {
        return BulletSpawner.MachineGunBullet;
    }
    protected override void ShooterEffect()
    {
        base.ShooterEffect();
        CinemachineCtrl.Instance.CinemachineShake.ShakeCamera(2f, .1f);
        SoundFXManager.Instance.PlaySoundFXClip(SoundFXManager.Instance.rifleShoot, this.GunPoint);
    }

}

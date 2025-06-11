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
    protected override string SetShellType()
    {
        return ShellSpawner.MachineShell;
    }
    protected override void ShooterEffect()
    {
        base.ShooterEffect();
        CinemachineCtrl.Instance.CinemachineShake.ShakeCamera(weaponInfo.recoilSize, weaponInfo.recoilDuration);
        SoundFXManager.Instance.PlaySoundFXClip(SoundFXManager.Instance.machineShoot, this.GunPoint);
    }


}

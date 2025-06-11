using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerShooter : RayCastWeapon
{

    [Space]
    [Header("LazerShooter")]
    [SerializeField] private TrailRenderer lazerTrailRenderer;


    protected override string SetBulletType()
    {
        return BulletSpawner.LazerGunBullet;
    }
    protected override void ShooterEffect()
    {
        base.ShooterEffect();
        SoundFXManager.Instance.PlaySoundFXClip(SoundFXManager.Instance.lazerShoot, this.GunPoint);
        if (!lazerTrailRenderer.enabled)
            lazerTrailRenderer.enabled = true;
    }
    protected override void HoldFire()
    {
        if (lazerTrailRenderer.enabled)
            lazerTrailRenderer.enabled = false;

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGunShooter : RayCastWeapon
{
    [Space]
    [Header("ShotGunShooter")]
    [SerializeField] private int bulletsPerShot = 0;
  
    
    protected override string SetBulletType()
    {
        return BulletSpawner.ShotGunBullet;
    }
    protected override void ShooterEffect()
    {
        base.ShooterEffect();
        CinemachineCtrl.Instance.CinemachineShake.ShakeCamera(7f, .1f);
        SoundFXManager.Instance.PlaySoundFXClip(SoundFXManager.Instance.rifleShoot, this.GunPoint);
    }
    protected override void Shoot()
    {
        List<Transform> list = new List<Transform>();
        for (int i = 0; i < bulletsPerShot; i++)
        {
            var newBullet = BulletSpawner.Instance.Spawn(this.SetBulletType(), this.GunPoint.position, Quaternion.LookRotation(this.GunPoint.forward));
            if (newBullet == null) continue;
            list.Add(newBullet);
        }
        if (list.Count < 1) return;
        foreach (Transform t in list)
        {

            t.gameObject.SetActive(true);

        }
        this.ShooterEffect();
      _isShooting = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGunBulletFly : BulletFly
{
    [SerializeField] private float spreadAngle = 10;

    protected override Vector3 BulletDirection()
    {
        Vector3 direction = (/*this.AimPos().position*/this.GetAimPos() - this.GunPos().position).normalized;

        if (_bulletCtrl.CharacterCtrl.CharacterAim.CanAimPrecisly() == false &&
          _bulletCtrl.CharacterCtrl.CharacterAim.GetTarget() == null)
        {
            direction.y = 0;
        }
        Quaternion randomRotation = Quaternion.AngleAxis(
   Random.Range(-spreadAngle, spreadAngle),
   Random.onUnitSphere);


        return (randomRotation * direction).normalized;
    }
}

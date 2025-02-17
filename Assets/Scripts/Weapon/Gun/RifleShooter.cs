using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleShooter : RayCastWeapon
{
    protected override void ShooterEffect()
    {
        base.ShooterEffect();
        CinemachineCtrl.Instance.CinemachineShake.ShakeCamera(2f, .1f);
    }
}

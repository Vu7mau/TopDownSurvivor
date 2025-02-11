using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleShooter : RayCastWeapon
{
    protected override void ShooterEffect()
    {
        base.ShooterEffect();
        CinemachineShake.Instance.ShakeCamera(1f, .1f);
    }
}

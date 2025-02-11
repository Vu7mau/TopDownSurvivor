using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolShooter : RayCastWeapon
{
    protected override void ShooterEffect()
    {
        base.ShooterEffect();
        CinemachineShake.Instance.ShakeCamera(5f, .1f);
    }
}

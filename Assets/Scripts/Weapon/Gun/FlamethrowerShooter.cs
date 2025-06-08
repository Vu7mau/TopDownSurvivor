using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerShooter : RayCastWeapon
{
    protected override void Shoot()
    {
        this.ShooterEffect();
    }
}

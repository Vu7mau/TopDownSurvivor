using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerShooter : RayCastWeapon
{

    [Space]
    [Header("LazerShooter")]
    [SerializeField] private TrailRenderer trailRenderer;

    private RaycastHit hit;
    private Vector3 endPosition;
    protected override string SetBulletType()
    {
        return BulletSpawner.LazerGunBullet;
    }
    protected override void Shoot()
    {
        this.ShootLaser();

    }
}

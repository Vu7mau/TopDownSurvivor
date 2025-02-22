using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class RifleShooter : RayCastWeapon
{
    //[Space]
    //[Header("PistolShooter")]
    //[SerializeField] protected LineRenderer lineRenderer;
    //[SerializeField] protected LayerMask _enemyLayer;


    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadLineRenderer();
    }
    protected virtual void LoadLineRenderer()
    {
        if (lineRenderer != null) return;

        lineRenderer = GetComponent<LineRenderer>();
    }
    protected override void ShooterEffect()
    {
        base.ShooterEffect();
        CinemachineCtrl.Instance.CinemachineShake.ShakeCamera(2f, .1f);
    }
    protected override string SetBulletType()
    {
        return BulletSpawner.bulletOne;
    }
    private void LateUpdate()
    {
        this.ShootLaser();
    }
    protected override void ShootLaser()
    {
        base.ShootLaser();
    }

}

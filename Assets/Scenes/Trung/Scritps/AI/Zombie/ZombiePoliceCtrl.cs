using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePoliceCtrl : EShooting
{
    [SerializeField] protected BulletInvisible bulletInvisible;

    [SerializeField] protected int shootTime = 2;
    [SerializeField] protected float timeDelay = 0.1f;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadBulletInvisible();
    }
    protected virtual void LoadBulletInvisible()
    {
        if (this.bulletInvisible != null) return;
        List<BulletInvisible> allMyComponents = ComponentFinder.FindAllComponentsInScene<BulletInvisible>();
        this.bulletInvisible = allMyComponents[0];
    }

    protected virtual void Shoot()
    {
        StartCoroutine(ShootingRoutine());
    }

    private IEnumerator ShootingRoutine()
    {
        for (int i = 0; i < shootTime; i++)
        {
            yield return new WaitForSeconds(this.timeDelay);
            this.Shooting(this.bulletInvisible, this.positionSpawn);
            if (this.newProjectitle == null) yield break;
            this.newProjectitle.GetComponent<BulletInvisible>().ShootAt(this.targetPosition.position);
        }
    }
}

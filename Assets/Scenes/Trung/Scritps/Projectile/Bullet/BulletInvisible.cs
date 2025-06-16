using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletInvisible : Projectitle
{
    public override string GetName() => "BulletInvisible";
    [SerializeField] protected BulletInvisbleDespawn bulletInvisbleDespawn;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadBulletInvisbleDespawn();
    }
    protected virtual void LoadBulletInvisbleDespawn()
    {
        if (this.bulletInvisbleDespawn != null) return;
        this.bulletInvisbleDespawn = GetComponentInChildren<BulletInvisbleDespawn>();
    }
    protected void OnTriggerEnter(Collider other)
    {
        this.bulletInvisbleDespawn.DoDespawn();
    }
}

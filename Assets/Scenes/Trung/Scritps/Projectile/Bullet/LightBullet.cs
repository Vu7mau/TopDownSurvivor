using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LightBullet : Projectitle
{
    public override string GetName() => "LightBullet";
    [SerializeField] protected LightBulletDespawn lightBulletDespawn;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadLightBulletDespawn();
    }
    protected virtual void LoadLightBulletDespawn()
    {
        if (this.lightBulletDespawn != null) return;
        this.lightBulletDespawn = GetComponentInChildren<LightBulletDespawn>();
    }
    protected void OnTriggerEnter(Collider other)
    {
        this.lightBulletDespawn.DoDespawn();
    }
}

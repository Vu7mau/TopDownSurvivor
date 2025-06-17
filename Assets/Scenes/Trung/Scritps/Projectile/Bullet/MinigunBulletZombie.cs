
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigunBulletZombie : Projectitle
{
    public override string GetName() => "MinigunBulletZombie";
    [SerializeField] protected MinigunBulletZombieDespawn minigunBulletZombieDespawn;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadMinigunBulletZombieDespawn();
    }
    protected virtual void LoadMinigunBulletZombieDespawn()
    {
        if (this.minigunBulletZombieDespawn != null) return;
        this.minigunBulletZombieDespawn = GetComponentInChildren<MinigunBulletZombieDespawn>();
    }
    protected void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy") || !other.GetComponentInChildren<DamageSender>()) this.minigunBulletZombieDespawn.DoDespawn();
    }
}

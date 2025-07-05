using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigunBullet : Projectitle
{
    [SerializeField] protected ExplosionSpawner explosionSpawner;
    [SerializeField] protected Explosion explosion;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadExplosionSpawner();
        this.LoadExplosion();
    }
    protected virtual void LoadExplosionSpawner()
    {
        if (this.explosionSpawner != null) return;
        this.explosionSpawner = GetComponentInChildren<ExplosionSpawner>();
        if (this.explosionSpawner == null) return;
    }
    protected virtual void LoadExplosion()
    {
        if (this.explosion != null) return;
        this.explosion = GetComponentInChildren<Explosion>();
        if (this.explosion == null) return;
    }

    protected void OnCollisionEnter(Collision other)
    {
        if (!other.transform.CompareTag("BulletEnemy") && !other.transform.CompareTag("Enemy") && !other.transform.CompareTag("bullet"))
        {
            if (this.explosion == null) return;
            if (this.explosionSpawner == null) return;
            this.explosionSpawner.Spawn(this.explosion, this.transform.position);
            this.Despawn.DoDespawn();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyProjectitle : VuMonoBehaviour
{
    [SerializeField] protected ExplosionSpawner explosionSpawner;
    [SerializeField] protected Explosion explosion;

    [SerializeField] protected Projectitle projectitle;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadExplosionSpawner();
        this.LoadExplosion();
        this.LoadProjectitle();
    }
    protected virtual void LoadExplosionSpawner()
    {
        if (this.explosionSpawner != null) return;
        this.explosionSpawner = GetComponentInChildren<ExplosionSpawner>();
    }
    protected virtual void LoadExplosion()
    {
        if (this.explosion != null) return;
        this.explosion = GetComponentInChildren<Explosion>();
    }
    protected virtual void LoadProjectitle()
    {
        if (this.projectitle != null) return;
        this.projectitle = GetComponentInParent<Projectitle>();
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("BulletEnemy") && !other.transform.CompareTag("Enemy") && !other.transform.CompareTag("bullet"))
        {
            this.explosionSpawner.Spawn(this.explosion, this.transform.parent.position);
            this.projectitle.Despawn.DoDespawn();
        }
    }
}

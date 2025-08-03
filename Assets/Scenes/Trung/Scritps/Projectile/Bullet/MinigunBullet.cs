using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigunBullet : Projectitle
{
    [SerializeField] protected EffectFXSpawner explosionSpawner;
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
        this.explosionSpawner = FindAnyObjectByType<EffectFXSpawner>();
        if (this.explosionSpawner == null) return;
    }
    protected virtual void LoadExplosion()
    {
        if (this.explosion != null) return;
        this.explosion = GetComponentInChildren<Explosion>();
        if (this.explosion == null) return;
    }


    protected override void DestroyProjectitle()
    {
        base.DestroyProjectitle();
        this.Explosion();
    }
    protected override void DestroyProjectitleByPlayer()
    {
        this.Explosion();
    }
    private void Explosion()
    {
        if (this.explosion == null) return;
        if (this.explosionSpawner == null) return;
        this.explosionSpawner.Spawn(this.explosion, this.transform.position);
    }
}

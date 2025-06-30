using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_FireFighterCtrl : ZombieCtrl
{
    [SerializeField] protected Explosion explosionPrefab;
    [SerializeField] protected ExplosionSpawner explosionSpawner;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadExplosion();
        this.LoadExplosionSpawner();
    }
    protected virtual void LoadExplosion()
    {
        if (this.explosionPrefab != null) return;
        List<Explosion> allMyComponents = ComponentFinder.FindAllComponentsInScene<Explosion>();
        this.explosionPrefab = allMyComponents[0];
    }
    protected virtual void LoadExplosionSpawner()
    {
        if (this.explosionSpawner != null) return;
        this.explosionSpawner = FindAnyObjectByType<ExplosionSpawner>();
        if(this.explosionSpawner == null) return;
    }
    protected override void Shooting()
    {
    }
    protected virtual void Explode()
    {
        if(this.explosionPrefab == null) return;
        if (this.explosionPrefab == null) return;
        Explosion newExplosion = this.explosionSpawner.Spawn(this.explosionPrefab, transform.position);
        if (newExplosion == null) return;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Projectitle
{
    public override string GetName() => "Missile";
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
        this.explosionSpawner = FindAnyObjectByType<ExplosionSpawner>();
    }
    protected virtual void LoadExplosion()
    {
        if (this.explosion != null) return;
        List<Explosion> allMyComponents = ComponentFinder.FindAllComponentsInScene<Explosion>();
        this.explosion = allMyComponents[0];
    }
    protected override void OnTriggerEnter(Collider collider)
    {
        Explosion newExplosion = this.explosionSpawner.Spawn(explosion, transform.position);
    }
}

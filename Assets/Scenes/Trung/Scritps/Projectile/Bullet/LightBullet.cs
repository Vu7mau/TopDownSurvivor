using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LightBullet : Projectitle
{
    public override string GetName() => "LightBullet";
    [SerializeField] protected LightBulletDespawn lightBulletDespawn;
    [SerializeField] protected ExplosionSpawner explosionSpawner;
    [SerializeField] protected Explosion explosion;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadLightBulletDespawn();

        this.LoadExplosionSpawner();
        this.LoadExplosion();
    }
    protected virtual void LoadLightBulletDespawn()
    {
        if (this.lightBulletDespawn != null) return;
        this.lightBulletDespawn = GetComponentInChildren<LightBulletDespawn>();
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
    private void OnTriggerEnter(Collider other)
    {
        if (!other.transform.CompareTag("Enemy") && !other.transform.CompareTag("bullet"))
        {
            Explosion newExplosde = this.explosionSpawner.Spawn(explosion, transform.position);
            newExplosde.GetComponentInChildren<CreateHitEnemy>().transform.gameObject.SetActive(false);
            this.lightBulletDespawn.DoDespawn();
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (!other.transform.CompareTag("Enemy") && !other.transform.CompareTag("bullet"))
        {
            Explosion newExplosde = this.explosionSpawner.Spawn(explosion, transform.position);
            newExplosde.GetComponentInChildren<CreateHitEnemy>().transform.gameObject.SetActive(false);
            this.lightBulletDespawn.DoDespawn();
        }
    }
}

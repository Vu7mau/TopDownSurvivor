using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbyssProjectitle : Projectitle
{
    [SerializeField] protected Explosion explosion;
    [SerializeField] protected EffectFXSpawner explosionSpawner;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadExplosionSpawner();
        this.LoadExplosion();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected virtual void LoadExplosion()
    {
        if (this.explosion != null) return;
        List<Explosion> allMyComponents = ComponentFinder.FindAllComponentsInScene<Explosion>();
        foreach (var myComponent in allMyComponents)
        {
            if (myComponent.GetComponent<Explosion>().GetName() == "Explode3")
            {
                this.explosion = myComponent;
                break;
            }
        }
    }
    protected virtual void LoadExplosionSpawner()
    {
        if (this.explosionSpawner != null) return;
        this.explosionSpawner = FindAnyObjectByType<EffectFXSpawner>();
    }

    


    private void OnTriggerEnter(Collider other)
    {
        if (!other.transform.CompareTag("BulletEnemy") && !other.transform.CompareTag("Enemy") && !other.transform.CompareTag("bullet"))
        {
            this.SpawnExplosion3();
            this.Despawn.DoDespawn();
        }

    }
    protected virtual void SpawnExplosion3()
    {
        if (this.explosionSpawner == null) return;
        EffectFX newExplosion = this.explosionSpawner.Spawn(this.explosion, transform.position);
        if (newExplosion == null) return;
        newExplosion.transform.GetComponentInChildren<CreateHitEnemy>().GetComponent<SphereCollider>().radius = 12.5f;
        newExplosion.transform.localScale = new Vector3(1f, 1f, 1f);
    }
    
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LightBullet : Projectitle
{
    public override string GetName() => "LightBullet";
    [SerializeField] protected LightBulletDespawn lightBulletDespawn;
    [SerializeField] protected EffectFXSpawner explosionSpawner;
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
        this.explosionSpawner = FindAnyObjectByType<EffectFXSpawner>();
    }
    protected virtual void LoadExplosion()
    {
        if (this.explosion != null) return;
        List<Explosion> allMyComponents = ComponentFinder.FindAllComponentsInScene<Explosion>();
        foreach(var myComponent in allMyComponents)
        {
            if(myComponent.GetComponent<Explosion>().GetName() == "Explode2")
            {
                this.explosion = myComponent;
                break;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("BulletEnemy") && !other.transform.CompareTag("Enemy") && !other.transform.CompareTag("bullet"))
        {
            this.lightBulletDespawn.DoDespawn();
            EffectFX newExplosde = this.explosionSpawner.Spawn(explosion, transform.position);
            newExplosde.GetComponent<Explosion>().Hit.gameObject.SetActive(false);
        }
    }
}

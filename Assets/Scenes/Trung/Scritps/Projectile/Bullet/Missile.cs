using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Projectitle
{
    public override string GetName() => "Missile";
    [SerializeField] protected EffectFXSpawner explosionSpawner;
    [SerializeField] protected Explosion explosion;
    [SerializeField] protected MissileDespawn missileDespawn;

    //[SerializeField] protected bool isTouchPlayer = false;

    protected override void OnEnable()
    {
        base.OnEnable();
        //this.ResetProjectile();
    }
    //protected virtual void ResetProjectile()
    //{
    //    this.isTouchPlayer = false;
    //}
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadExplosionSpawner();
        this.LoadExplosion();
        this.LoadMissileDespawn();
    }
    protected virtual void LoadExplosionSpawner()
    {
        if (this.explosionSpawner != null) return;
        this.explosionSpawner = FindAnyObjectByType<EffectFXSpawner>();
    }
    protected virtual void LoadMissileDespawn()
    {
        if (this.missileDespawn != null) return;
        this.missileDespawn = GetComponentInChildren<MissileDespawn>();
    }
    protected virtual void LoadExplosion()
    {
        if (this.explosion != null) return;
        List<Explosion> allMyComponents = ComponentFinder.FindAllComponentsInScene<Explosion>();
        foreach (var myComponent in allMyComponents)
        {
            if (myComponent.GetComponent<Explosion>().GetName() == "Explode1")
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
            EffectFX newExplosde =  this.explosionSpawner.Spawn(explosion, transform.position);
            newExplosde.GetComponent<Explosion>().Hit.gameObject.SetActive(true);
            this.missileDespawn.DoDespawn();
        }
    }
    //protected void OnTriggerStay(Collider other)
    //{
    //    if (other.GetComponentInChildren<EnemyCtrl>() != null && !this.isTouchPlayer)
    //    {
    //        this.isTouchPlayer = true;
    //    }
    //}
}

using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class SpiderTiny : EnemyAI
{
    [SerializeField] protected Explosion explosionPrefab;
    [SerializeField] protected EffectFXSpawner explosionSpawner;

    [SerializeField] protected EnemyHealth spiderHealth;
    [SerializeField] protected bool _spiderIsDead;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadExplosion();
        this.LoadExplosionSpawner();
        this.LoadEnemyHealth();
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
        this.explosionSpawner = FindAnyObjectByType<EffectFXSpawner>();
    }
    protected virtual void LoadEnemyHealth()
    {
        if(this.spiderHealth != null) return;
        this.spiderHealth = GetComponent<EnemyHealth>();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        this.ExplosionWhenSpiderIsDead();
    }
    protected virtual void ExplosionWhenSpiderIsDead()
    {
        StartCoroutine(ExplosionSpiderRoutine());
    }
    private void FixedUpdate()
    {
        this.CheckIsDead();
    }
    private void CheckIsDead()
    {
        if (this.spiderHealth.Health > 0 && !this._spiderIsDead) return;
        this._spiderIsDead = true;
    }
    private IEnumerator ExplosionSpiderRoutine()
    {
        yield return new WaitUntil(() =>  this._spiderIsDead);
        this.Explode();
    }
    protected virtual void Explode()
    {
        if(this.explosionSpawner == null) return;
        if(this.explosionPrefab ==  null) return;
        EffectFX newExplosion = this.explosionSpawner.Spawn(this.explosionPrefab, transform.position);
        if(newExplosion == null) return;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DespawnGeneral<T> : DespawnBase where T: PoolObj
{
    [SerializeField] protected T parent;
    [SerializeField] protected SpawnerGeneral<T> spawner;
    [SerializeField] protected float timeLife = 5f;
    [SerializeField] protected float currentTime;

    protected void FixedUpdate()
    {
        this.DespawnChecking();
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadParent();
        this.LoadSpawner();
        this.LoadCurrentTimeLife();
    }

    protected virtual void LoadParent()
    {
        if (this.parent != null) return;
        this.parent = transform.parent.GetComponent<T>();
        Debug.Log(transform.name + ":Load Parent");
    }
    protected virtual void LoadSpawner()
    {
        if (this.spawner != null) return;
        this.spawner = FindAnyObjectByType<SpawnerGeneral<T>>();
        Debug.Log(transform.name + ":Load Spawner");
    }

    protected virtual void LoadCurrentTimeLife()
    {
        this.currentTime = this.timeLife;
    }
    protected virtual void DespawnChecking()
    {
        this.currentTime -= Time.deltaTime;
        if (this.currentTime > 0) return;
        this.currentTime = this.timeLife;
        this.DoDespawn();
    }
    public override void DoDespawn()
    {
        this.spawner.Despawn(this.parent);
    }
}

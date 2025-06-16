using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicAbyss : Projectitle
{
    public override string GetName() => "ToxicAbyss";
    [SerializeField] protected ToxicAbyssDespawn toxicAbyssDespawn;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadToxicAbyssDespawn();
    }
    protected virtual void LoadToxicAbyssDespawn()
    {
        if (this.toxicAbyssDespawn != null) return;
        this.toxicAbyssDespawn = GetComponentInChildren<ToxicAbyssDespawn>();
    }
    protected void OnTriggerEnter(Collider other)
    {
        this.toxicAbyssDespawn.DoDespawn();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicDamageSender : CreateHitEnemy
{
    [SerializeField] protected ProjectitleCtrl projectitleCtrl;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadToxicAbyss();
    }

    protected virtual void LoadToxicAbyss()
    {
        if (this.projectitleCtrl != null) return;
        this.projectitleCtrl = GetComponentInParent<ProjectitleCtrl>();
        Debug.Log(transform.name + "Load ToxicAbyss", gameObject);
    }
    public override void Send(DamageReceiver damageReceiver)
    {
        base.Send(damageReceiver);
        this.projectitleCtrl.ToxicAbyss.Despawn.DoDespawn();
    }
}

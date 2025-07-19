using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyCtrl : PoolObj
{
    [SerializeField] protected string nameEnemy;


    [SerializeField] protected Localization _local;

    public override string GetName() => nameEnemy;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadNameEnemy();
    }
    protected virtual void LoadNameEnemy()
    {
        if(this.nameEnemy.Length != 0) return;
        this.nameEnemy = this.transform.gameObject.name;
    }
}

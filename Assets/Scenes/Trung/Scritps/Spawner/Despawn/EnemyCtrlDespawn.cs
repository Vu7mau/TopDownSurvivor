using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtrlDespawn : DespawnGeneral<EnemyCtrl>
{




    protected override void Reset()
    {
        base.Reset();
        this.ResetDefaultVariables();
    }

    protected virtual void ResetDefaultVariables()
    {
        this.timeLife = 1000000f;
        this.currentTime = this.timeLife;
    }




}

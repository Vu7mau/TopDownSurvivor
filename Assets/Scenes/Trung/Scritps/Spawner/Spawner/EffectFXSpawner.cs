using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectFXSpawner : SpawnerGeneral<EffectFX>
{

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadEffectFXHoldParent();
    }
    protected virtual void LoadEffectFXHoldParent()
    {
        this.holderParent = GameObject.Find("EffectFXHolder").transform;
        if (this.holderParent == null) return;
    }
}

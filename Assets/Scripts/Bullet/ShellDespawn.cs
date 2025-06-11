using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellDespawn : DespawnByTime
{
    public override void DespawnObject()
    {
        ShellSpawner.Instance.Despawn(this.transform.parent);
    }
}

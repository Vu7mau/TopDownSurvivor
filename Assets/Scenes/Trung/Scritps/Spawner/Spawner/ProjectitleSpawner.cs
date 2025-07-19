using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectitleSpawner : SpawnerGeneral<Projectitle>
{
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadProjectitleSpawnerHoldParent();
    }
    protected virtual void LoadProjectitleSpawnerHoldParent()
    {
        this.holderParent = GameObject.Find("ProjectitleHolderSpawner").transform;
    }
}

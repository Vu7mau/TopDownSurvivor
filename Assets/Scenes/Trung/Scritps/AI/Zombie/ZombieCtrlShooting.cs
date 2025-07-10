using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ZombieCtrlShooting : ObjectSpawnerBase
{
    [SerializeField] protected Transform endPoint;
    protected abstract void Shooting();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawner : SpawnerGeneral<EnemyCtrl>
{
    public List<EnemyCtrl> Enemies => this.inPoolObjs;
}

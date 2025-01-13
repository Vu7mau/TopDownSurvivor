using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : Spawner 
{
    public static BulletSpawner Instance {  get; private set; }
    public static string bulletOne = "Bullet_1";

    protected override void Awake()
    {
        base.Awake();
        if (BulletSpawner.Instance != null) Debug.LogError("Only 1 BulletSpawner allow to exist");
        BulletSpawner.Instance = this;
    }
}

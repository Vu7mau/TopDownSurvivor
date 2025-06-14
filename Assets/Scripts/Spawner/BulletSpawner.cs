using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : Spawner
{
    public static BulletSpawner Instance { get; private set; }
    public static string RifleBullet = "Rifle_Bullet";
    public static string PistolBullet = "Pistol_Bullet";
    public static string ShotGunBullet = "ShotGun_Bullet";
    public static string MachineGunBullet = "MachineGun_Bullet";
    public static string LazerGunBullet = "Lazer_Bullet ";
    public static string RocketBullet = "Rocket_Bullet";
    public static string Flamethrower_bullet = "Flamethrower_bullet";

    protected override void Awake()
    {

        base.Awake();
        if (Instance == null)
            Instance = this;
    }
}

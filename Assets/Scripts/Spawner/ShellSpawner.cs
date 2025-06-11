using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellSpawner : Spawner
{
    public static ShellSpawner Instance { get; private set; }
    public static string RifleShell = "Rifle_Shell";
    public static string ShotGunShell = "ShotGun_Shell";
    public static string MachineShell = "Machine_Shell";
    protected override void Awake()
    {

        base.Awake();
        if (Instance == null)
            Instance = this;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagerMisson : MonoBehaviour
{
    public static event Action<GameObject> OnBossKilled; 
    public static void EnemyDied(GameObject boss)
    {
        OnBossKilled?.Invoke(boss);
        Debug.Log($"Enemy {boss.name} bị tiêu diệt!");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="Enemy")]
public class EnemySO : ScriptableObject
{
    [Header("Idle")]
    public int IdleAnimations = 1;

    [Header("Chase")]
    public float ChaseRange;
    public float ChaseSpeed;
    [HideInInspector] public int RunAnimations = 1;


    [Header("Attack")]
    public float AttackRange;
    public int Damage;
    [HideInInspector] public int AttackAnimations = 1;

    [Header("Death")]
    [HideInInspector] public int DeathAnimations = 1;


    [Header("Health")]
    public float Health;

    [Header("Rewards Player Can Receive")]
    public int Gems;
    public float Exp;
    public int Score = 10;

    private void OnValidate()
    {
        if (this.IdleAnimations < 0) this.IdleAnimations = 1;
        if (this.ChaseRange < 0) this.ChaseRange = 100000000;
        if (this.Score < 0) this.Score = 0;
    }
}

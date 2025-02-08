using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="Enemy")]
public class EnemySO : ScriptableObject
{
    [Header("Movement")]
    public float Speed;

    [Header("Chase")]
    public float ChaseRange;
    public float ChaseSpeed;

    [Header("Attack")]
    public float AttackRange;
    public float Damage;


    [Header("Health")]
    public float Health;

    [Header("Rewards Player Can Receive")]
    public int amount_Gems;
    public float amount_Experiences;
}

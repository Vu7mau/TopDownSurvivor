using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform _player;
    [SerializeField] private EnemySO enemySO;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        _player = FindAnyObjectByType<CharacterCtrl>().transform;
    }
    private void Start()
    {
        agent.speed = enemySO.ChaseSpeed;
    }
    private void Update()
    {
        agent.SetDestination(_player.position);
    }
}

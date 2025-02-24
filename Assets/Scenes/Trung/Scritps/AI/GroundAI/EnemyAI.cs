using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected Transform _player;
    [SerializeField] protected EnemySO enemySO;
    [SerializeField] protected float _timeDelete;
    private BoxCollider _collider;
    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        _player = FindAnyObjectByType<CharacterCtrl>().transform;
        _collider = GetComponent<BoxCollider>();
    }
    private void OnEnable()
    {
        _collider.enabled = true;
    }
    protected virtual void ChasePlayer()
    {
        agent.speed = enemySO.ChaseSpeed;
        agent.SetDestination(_player.position);
    }
    public void OnDelete()
    {
        gameObject.SetActive(false);
    }
}

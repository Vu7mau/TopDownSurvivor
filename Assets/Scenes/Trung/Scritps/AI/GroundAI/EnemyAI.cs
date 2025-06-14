using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : VuMonoBehaviour
{
    protected NavMeshAgent agent;
    protected Transform _player;
    [SerializeField] protected EnemySO enemySO;
    [SerializeField] protected float _timeDelete;
    private BoxCollider _collider;
    protected override void Awake()
    {
        base.Awake();
        this.agent = GetComponent<NavMeshAgent>();
        this._player = FindAnyObjectByType<CharacterCtrl>().transform;
        this._collider = GetComponent<BoxCollider>();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        this._collider.enabled = true;
    }
    protected virtual void ChasePlayer()
    {
        this.agent.speed = enemySO.ChaseSpeed;
        this.agent.SetDestination(_player.position);
    }
    public void OnDelete()
    {
        this.gameObject.SetActive(false);
    }
}

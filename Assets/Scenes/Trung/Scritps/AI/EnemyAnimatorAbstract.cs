using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyAnimatorAbstract : EnemyGeneral
{
    [SerializeField] protected Animator _enemyAnimator;
    [SerializeField] protected NavMeshAgent _agent;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadEnemyAnimator();
        this.LoadNavMeshAgent();
    }
    protected virtual void LoadEnemyAnimator()
    {
        if (_enemyAnimator != null) return;
        this._enemyAnimator = GetComponentInChildren<Animator>();
        Debug.Log(transform.name + ": Load Animator",gameObject);
    }
    protected virtual void LoadNavMeshAgent()
    {
        if (this._agent != null) return;
        this._agent = GetComponent<NavMeshAgent>();
        Debug.Log(transform.name + ": Load NavMeshAgent!");
    }
}

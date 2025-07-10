using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyAI : VuMonoBehaviour
{
    [SerializeField] protected NavMeshAgent navMeshagent;
    public NavMeshAgent NavMeshAgent {  get =>  navMeshagent; }


    [SerializeField] protected Animator animator;
    public Animator Animator { get => animator; }

    [Header("Stats")]
    [SerializeField] protected float pathUpdateDelay = 0.2f;
    [SerializeField] protected EnemySO enemySO;
    public EnemySO EnemySO { get => enemySO; }
    public float PathUpdateDelay { get => pathUpdateDelay;}


    protected virtual void OnValidate()
    {
        if(this.pathUpdateDelay < 0) this.pathUpdateDelay = 0.2f;
    }



    protected override void OnEnable()
    {
        base.OnEnable();
        this.OnEnableNavMeshAgent();
    }


    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadNavMeshAgent();
        this.LoadAnimator();
    }




    protected virtual void LoadAnimator()
    {
        if (this.animator != null) return;
        this.animator = GetComponentInChildren<Animator>();
    }
    protected virtual void LoadNavMeshAgent()
    {
        if (this.navMeshagent != null) return;
        this.navMeshagent = GetComponentInChildren<NavMeshAgent>();
        if(!this.navMeshagent.enabled) this.navMeshagent.enabled = true;
    }
    protected virtual void OnEnableNavMeshAgent()
    {
        if (!this.navMeshagent.enabled) this.navMeshagent.enabled = true;
    }
}

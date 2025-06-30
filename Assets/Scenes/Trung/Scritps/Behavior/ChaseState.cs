using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : StateMachineBehaviour
{
    [SerializeField] private EnemySO enemySO;
    NavMeshAgent agent;
    Transform player;
    private float distance;

    public float Distance => distance;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        player =FindAnyObjectByType<CharacterAnimHandle>().transform;
        agent.speed = enemySO.ChaseSpeed;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        distance = Vector3.Distance(player.position, animator.transform.position);
        if (distance <= enemySO.AttackRange)
        {
            animator.SetBool("isAttacking", true);
            animator.SetBool("Attack", true);
            this.agent.enabled = false;
            return;
        }
        if (this.agent == null) return;
        if (!this.agent.enabled) return;
        this.agent.SetDestination(this.player.position);
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!this.agent.enabled) return;
        agent.SetDestination(animator.transform.position);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}

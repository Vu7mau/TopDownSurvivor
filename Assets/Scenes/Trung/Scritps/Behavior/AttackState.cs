using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackState : StateMachineBehaviour
{
    [SerializeField] private EnemySO enemySO;
    private Transform player;
    private NavMeshAgent navMeshAgent;
    private float distance;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.player = FindAnyObjectByType<CharacterCtrl>().transform;/*GameObject.FindGameObjectWithTag("Player")*/;
        this.navMeshAgent = animator.GetComponent<NavMeshAgent>();
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.LookAt(this.player);

        distance = Vector3.Distance(player.position, animator.transform.position);
        if (this.distance >= this.enemySO.AttackRange)
        {
            animator.SetBool("isAttacking", false);
            animator.SetBool("Attack", false);
            return;
        }
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //animator.SetBool("isAttacking", false);
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

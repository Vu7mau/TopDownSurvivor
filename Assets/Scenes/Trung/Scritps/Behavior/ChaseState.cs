using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : StateMachineBehaviour
{
    [SerializeField] private EnemySO enemySO;
    NavMeshAgent agent;
    Transform player;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        player = /*GameObject.FindGameObjectWithTag("Player")*/CharacterCtrl.Instance.transform;
        agent.speed = enemySO.ChaseSpeed;
        agent.stoppingDistance = enemySO.AttackRange;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        float distance = Vector3.Distance(player.position, animator.transform.position);
        //if (distance > 15)
        //    animator.SetBool("isChasing", false);
        if(distance < enemySO.AttackRange)
        {
            animator.SetBool("isAttacking", true);
            return;
        }
        agent.SetDestination(player.position);
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
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

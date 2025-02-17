using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : StateMachineBehaviour
{
    [SerializeField] private EnemySO enemySO;
    NavMeshAgent agent;
    Transform player;
    //public int randomIndexAttack;
    private float distance;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        player =CharacterCtrl.Instance.transform;
        agent.speed = enemySO.ChaseSpeed;
        agent.stoppingDistance = enemySO.AttackRange;
        //randomIndexAttack = 2;
        //randomIndexAttack = Random.Range(0, 5);
        //if (distance >= enemySO.AttackRange)
        //{
        //    randomIndexAttack = Random.Range(0, 5);
        //}
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        distance = Vector3.Distance(player.position, animator.transform.position);
        //if (distance >= enemySO.AttackRange2 && randomIndexAttack == 2)
        //{
        //    animator.SetBool("isAttacking", true);
        //    animator.SetBool("Attack2", true);
        //    animator.SetBool("Attack1", false);
        //    animator.SetBool("Attack3", false);
        //    return;
        //}
        //if (distance >= enemySO.AttackRange3 && randomIndexAttack == 3)
        //{
        //    animator.SetBool("isAttacking", true);
        //    animator.SetBool("Attack3", true);
        //    animator.SetBool("Attack1", false);
        //    animator.SetBool("Attack2", false);
        //    return;
        //}
        if (distance < enemySO.AttackRange)
        {
            animator.SetBool("isAttacking", true);
            //animator.SetBool("Attack1", true);
            //animator.SetBool("Attack2", false);
            //animator.SetBool("Attack3", false);
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

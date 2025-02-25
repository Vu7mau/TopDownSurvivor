using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class AttackStateRandom : StateMachineBehaviour
{
    [SerializeField] private EnemySO enemySO;
    [SerializeField] private int randomIndexAttack;
    [SerializeField] private int randomIndexAttackStart;
    [SerializeField] private int randomIndexAttackEnd;
    private float distance;
    private bool isAttackingNow= false;
    NavMeshAgent agent;
    Transform player;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = CharacterCtrl.Instance.transform;
        agent = animator.GetComponent<NavMeshAgent>();
        randomIndexAttack = randomIndexAttackStart;
    }
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bool canAttackFromFar = CheckNearDistanceMonsterAndPlayer(player.position, animator.transform.position, 8f) && IsEnemyInViewPort(animator.transform);
        
        if (canAttackFromFar && IsEnemyInViewPort(animator.transform))
        {
            if (!isAttackingNow)
            {
                randomIndexAttack = Random.Range(0, 5);
                isAttackingNow = true;
            }
            animator.SetBool("isAttacking", true);
            animator.SetBool("Attack2", randomIndexAttack == 2);
            animator.SetBool("Attack3", randomIndexAttack == 3);
            animator.SetBool("Attack", false);
            return;
        }
    }
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(animator.transform.position);
    }
    private bool CheckFarDistanceMonsterAndPlayer(Vector3 playerPosition,Vector3 monster, float attackRange)
    {
        return Vector3.Distance(playerPosition, monster) <= attackRange;
    }
    private bool CheckNearDistanceMonsterAndPlayer(Vector3 playerPosition, Vector3 monster, float attackRange)
    {
        return Vector3.Distance(playerPosition, monster) >= attackRange;
    }
    private bool IsEnemyInViewPort(Transform enemy)
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(enemy.position);
        return screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
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

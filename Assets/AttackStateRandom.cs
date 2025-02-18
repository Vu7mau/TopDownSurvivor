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
    NavMeshAgent agent;
    Transform player;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = CharacterCtrl.Instance.transform;
        agent = animator.GetComponent<NavMeshAgent>();
        player = CharacterCtrl.Instance.transform;
        randomIndexAttack = randomIndexAttackStart;
        if (CheckDistanceMonsterAndPlayer(player.position, animator.transform.position, 50f) && IsEnemyInViewPort(animator.transform))
        {
            randomIndexAttack = Random.Range(randomIndexAttackStart, randomIndexAttackEnd);
        }
    }
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (CheckRandomIndexTrue(randomIndexAttack,4,4))
        {
            animator.SetBool("isAttacking", true);
            animator.SetBool("Attack2", true);
            animator.SetBool("Attack", false);
            animator.SetBool("Attack3", false);
            return;
        }
        if (CheckRandomIndexTrue(randomIndexAttack, 1, 3))
        {
            animator.SetBool("isAttacking", true);
            animator.SetBool("Attack2", false);
            animator.SetBool("Attack", false);
            animator.SetBool("Attack3", true);
            return;
        }
        agent.SetDestination(player.position);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(animator.transform.position);
    }
    private bool CheckDistanceMonsterAndPlayer(Vector3 playerPosition,Vector3 monster, float attackRange)
    {
        return Vector3.Distance(playerPosition, monster) <= attackRange;
    }
    private bool IsEnemyInViewPort(Transform enemy)
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(enemy.position);
        return screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
    }
    private bool CheckRandomIndexTrue(int indexRandom,int startIndex,int endIndex)
    {
        for(int i = startIndex; i < endIndex + 1; i++)
        {
            if (indexRandom == i) return indexRandom == i;
            break;
        }
        return false;
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

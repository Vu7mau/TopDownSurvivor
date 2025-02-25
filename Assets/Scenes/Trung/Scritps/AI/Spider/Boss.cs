using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bosses : MonoBehaviour
{
    private int randomIndexAttack;
    private float distance;
    private NavMeshAgent agent;
    private Transform player;
    private Animator _myAnimator;
    private void Awake()
    {
        player = CharacterCtrl.Instance.transform;
        agent = GetComponent<NavMeshAgent>();
        _myAnimator = GetComponent<Animator>();
        randomIndexAttack = 0;
    }
    public void RandomAttack()
    {
        randomIndexAttack = Random.Range(0, 7);
        _myAnimator.SetBool("isAttacking", true);
        _myAnimator.SetBool("Attack2", randomIndexAttack == 2);
        _myAnimator.SetBool("Attack3", randomIndexAttack == 3);
        _myAnimator.SetBool("Attack", false);
    }
}

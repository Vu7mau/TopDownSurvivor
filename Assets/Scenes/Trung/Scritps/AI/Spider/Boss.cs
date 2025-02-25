using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bosses : MonoBehaviour
{
    private float distance;
    private Transform player;
    private Animator _myAnimator;
    private void Awake()
    {
        player = FindAnyObjectByType<CharacterCtrl>().transform;
        _myAnimator = GetComponent<Animator>();
    }
    public void RandomAttack()
    {
        int randomIndexAttack = Random.Range(0, 5);
        _myAnimator.SetBool("isAttacking", true);
        _myAnimator.SetBool("Attack2", randomIndexAttack == 2);
        _myAnimator.SetBool("Attack3", randomIndexAttack == 3);
        _myAnimator.SetBool("Attack", false);
    }
}

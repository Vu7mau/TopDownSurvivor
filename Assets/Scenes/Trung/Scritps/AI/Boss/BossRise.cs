using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRise : VuMonoBehaviour
{

    [SerializeField] protected EnemyAI enemyAI;
    [SerializeField] protected EnemyAIController enemyCtrl;
    [SerializeField] protected EnemyHealth enemyHealth;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadEnemyAI();
    }

    protected virtual void LoadEnemyAI()
    {
        if (this.enemyAI != null) return;
        this.enemyAI = GetComponentInParent<EnemyAI>();
        if (this.enemyCtrl != null) return;
        this.enemyCtrl = GetComponentInChildren<EnemyAIController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.GetComponentInChildren<CharacterAnimHandle>() != null)
        {
            this.enemyAI.Animator.SetBool("isStartFightBoss",true);
            this.transform.gameObject.SetActive(false);
        }
    }
}

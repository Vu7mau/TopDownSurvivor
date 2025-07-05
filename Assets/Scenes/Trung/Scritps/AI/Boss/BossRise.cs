using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRise : VuMonoBehaviour
{

    [SerializeField] protected EnemyAI enemyAI;
    [SerializeField] protected EnemyHealth enemyHealth;

    protected bool isStartFightBoss = false;
    public bool IsStartFightBoss { get => this.isStartFightBoss; }
    
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadEnemyAI();
    }

    protected virtual void LoadEnemyAI()
    {
        if (this.enemyAI != null) return;
        this.enemyAI = GetComponentInParent<EnemyAI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.GetComponentInChildren<CharacterAnimHandle>() != null)
        {
            this.enemyAI.Animator.SetBool("isStartFightBoss",true);
            this.enemyHealth.CanTakeDamage = true;
            this.isStartFightBoss=true;
            this.enemyAI.IsMoving = true;
            this.transform.gameObject.SetActive(false);
        }
    }
}

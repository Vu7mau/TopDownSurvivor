using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyResponse : VuMonoBehaviour
{
    [SerializeField] protected EnemyAI enemyAI;

    [SerializeField] protected EnemyHealth enemyHealth;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadEnemyHealth();
        this.LoadEnemyAI();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        this.OnPlayerKillEnemy();
    }



    protected virtual void LoadEnemyHealth()
    {
        if (this.enemyHealth != null) return;
        this.enemyHealth = GetComponent<EnemyHealth>();
    }
    protected virtual void LoadEnemyAI()
    {
        if (this.enemyAI != null) return;
        this.enemyAI = GetComponent<EnemyAI>();
    }



    //Add any rewards when player kill enemy
    protected virtual void OnPlayerKillEnemy()
    {
        StartCoroutine(this.RewardToPlayerWhenKillEnemy());
    }
    private IEnumerator RewardToPlayerWhenKillEnemy()
    {
        yield return new WaitUntil(() => this.enemyHealth.Health <= 0);
        PlayerScoreManager.Instance.AddScore(this.enemyAI.EnemySO.Score);
    }





}

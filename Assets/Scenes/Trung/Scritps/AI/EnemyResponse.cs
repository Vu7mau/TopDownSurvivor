using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyResponse : VuMonoBehaviour
{
    [SerializeField] protected EnemyAI enemyAI;

    [SerializeField] protected EnemyHealth enemyHealth;

    [Space]
    [Space]
    [Header("This component use for despawn enemy when player kill them!")]
    [Space]
    [Space]
    [Space]
    [Header("This component need ref!")]
    [SerializeField] protected EnemyCtrlDespawn enemyCtrlDespawn;

    [Header("This component can be null if you don't need despawn in wave!")]
    [SerializeField] protected EnemiesSpawner enemiesSpawner;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadEnemyHealth();
        this.LoadEnemyAI();
        this.LoadEnemyCtrlDespawn();
        this.LoadEnemiesSpawner();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        this.OnPlayerKillEnemy();
    }

    private void Update()
    {
        
    }
    protected virtual void OnEnemyDeath()
    {
        if (this.enemyCtrlDespawn != null && this.enemiesSpawner != null)
        {
            this.enemyCtrlDespawn.DoDespawn();
            return;
        }
        this.transform.gameObject.SetActive(false);
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
    protected virtual void LoadEnemyCtrlDespawn()
    {
        if (this.enemyCtrlDespawn != null) return;
        this.enemyCtrlDespawn = GetComponentInChildren<EnemyCtrlDespawn>();
    }
    protected virtual void LoadEnemiesSpawner()
    {
        if (this.enemiesSpawner != null) return;
        this.enemiesSpawner = FindAnyObjectByType<EnemiesSpawner>();
    }


    //Add any rewards when player kill enemy
    protected virtual void OnPlayerKillEnemy()
    {
        //StartCoroutine(this.RewardToPlayerWhenKillEnemy());
    }
    private IEnumerator RewardToPlayerWhenKillEnemy()
    {
        yield return new WaitUntil(() => this.enemyHealth.Health <= 0);
        PlayerScoreManager.Instance.AddScore(this.enemyAI.EnemySO.Score);
    }





}

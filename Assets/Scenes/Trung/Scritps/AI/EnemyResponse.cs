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
    [SerializeField] protected Transform textDisplayParentHolder;

    //[Header("This component can be null if you don't need despawn in wave!")]
    /*[SerializeField] */protected CharacterLeveUp playerLevelSystem;
    /*[SerializeField] */protected PickUpSpawner pickUpSpawner;

    //For Survival
    /*[SerializeField] */protected EnemiesSpawner enemiesSpawner;
    /*[SerializeField] */protected WaveSpawner waveSpawner;

    //[SerializeField] protected bool isCountLevel = false;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadEnemyHealth();
        this.LoadEnemyAI();
        this.LoadEnemyCtrlDespawn();
        this.LoadEnemiesSpawner();
        this.LoadCharacterLeveUp();
        this.LoadWaveSpawner();
        this.LoadPickUpSpawner();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        this.DespawnAllText();
        this.OnPlayerKillEnemy();
    }

    private void Update()
    {
        
    }
    protected virtual void OnEnemyDeath()
    {
        if (this.enemyAI.ItemDropSO != null && this.pickUpSpawner != null)
        {
            this.DropItem(this.transform, this.pickUpSpawner);
        }
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
    protected virtual void LoadCharacterLeveUp()
    {
        if (this.playerLevelSystem != null) return;
        this.playerLevelSystem = FindAnyObjectByType<CharacterLeveUp>();
    }

    protected virtual void LoadWaveSpawner()
    {
        if (this.waveSpawner != null) return;
        this.waveSpawner = FindAnyObjectByType<WaveSpawner>();
    }
    protected virtual void LoadPickUpSpawner()
    {
        if (this.pickUpSpawner != null) return;
        this.pickUpSpawner = FindAnyObjectByType<PickUpSpawner>();
    }

    //Add any rewards when player kill enemy
    protected virtual void OnPlayerKillEnemy()
    {
        StartCoroutine(this.RewardToPlayerWhenKillEnemy());
    }
    private IEnumerator RewardToPlayerWhenKillEnemy()
    {
        yield return new WaitUntil(() => this.enemyHealth.Health <= 0);

        //Rewards to Players
        if(this.playerLevelSystem != null) this.playerLevelSystem.AddExp(this.enemyAI.EnemySO.Exp);


        //Update UI (only apply to survivals)
        if(this.waveSpawner != null) this.waveSpawner.SubstractEnemyToUI();


        //PlayerScoreManager.Instance.AddScore(this.enemyAI.EnemySO.Score);
    }

    protected virtual void DespawnAllText()
    {
        if(this.textDisplayParentHolder != null)
        {
            if (this.textDisplayParentHolder.childCount > 0)
            {
                foreach (Transform child in textDisplayParentHolder.transform)
                {
                    child.gameObject.GetComponentInChildren<TextDisplayDespawn>().DoDespawn();
                }
            }
        }
    }
    protected virtual void DropItem(Transform position, PickUpSpawner spawner)
    {
        foreach (ItemDrop drop in this.enemyAI.ItemDropSO.ItemDrops)
        {
            float rollItem = Random.Range(0f, 100f);
            if (rollItem <= drop.dropChance)
            {
                int amount = Random.Range(1, drop.maxAmount);
                for(int i = 0; i < amount; i++)
                {
                    float positionX = Random.Range(position.position.x - 2f, position.position.x + 2f);
                    float positionY = position.position.y + 5f;
                    float positionZ = Random.Range(position.position.z - 2f, position.position.z + 2f);
                    Vector3 positionSpawnItem = new Vector3(positionX, positionY, positionZ);
                    spawner.Spawn(drop.itemPrefab, positionSpawnItem);
                    if (drop.itemPrefab != null)
                    {

                    }
                }
            }
        }
    }


}

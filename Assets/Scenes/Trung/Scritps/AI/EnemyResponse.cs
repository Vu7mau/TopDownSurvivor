using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyResponse : VuMonoBehaviour
{
    [SerializeField] protected EnemyAI enemyAI;

    [SerializeField] protected EnemyHealth enemyHealth;
    [SerializeField] protected Transform textDisplayParentHolder;

    [SerializeField] protected CharacterLeveUp characterLeveUp;
    [SerializeField] protected CharacterCurrencies characterCurrencies;
    [SerializeField] protected WaveSpawner waveSpawner;

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

    //For Survival
    ///*[SerializeField] */protected EnemiesSpawner enemiesSpawner;
    ///*[SerializeField] */protected WaveSpawner waveSpawner;

    protected Coroutine coroutine;

    [Header("Rewards")]
    [SerializeField] protected PickUpSpawner pickUpSpawner;
    [SerializeField] protected bool isReward = false;
    public bool IsReward { set => this.isReward = value; }

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
        this.LoadCharacterCurrencies();
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
        if (this.enemyAI.ItemDropSO != null && this.pickUpSpawner != null && this.isReward)
        {
            this.DropItem(this.transform, this.pickUpSpawner);
        }
        if (this.characterCurrencies != null)
        {
            this.characterCurrencies.AddKills(1);
            this.characterCurrencies.AddScore(1);
        }
        this.DespawnEnemy();
    }
    protected virtual void DespawnEnemy()
    {
        if (this.enemyCtrlDespawn != null && this.enemiesSpawner != null)
        {
            this.enemyCtrlDespawn.DoDespawn();
            //Update UI (only apply to survivals)
            if (this.waveSpawner != null) this.waveSpawner.SubstractEnemyToUI();
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
        if (this.characterLeveUp != null) return;
        this.characterLeveUp = FindAnyObjectByType<CharacterLeveUp>();
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
    protected virtual void LoadCharacterCurrencies()
    {
        if (this.characterCurrencies != null) return;
        this.characterCurrencies = FindAnyObjectByType<CharacterCurrencies>();
    }

    //Add any rewards when player kill enemy
    public virtual void OnPlayerKillEnemy()
    {
        if(this.coroutine == null)
        {
            StartCoroutine(this.RewardToPlayerWhenKillEnemy());
        }
        else
        {
            StopCoroutine(this.coroutine);
            StartCoroutine(this.RewardToPlayerWhenKillEnemy());
        }
    }
    private IEnumerator RewardToPlayerWhenKillEnemy()
    {
        yield return new WaitUntil(() => this.enemyHealth.IsDead());

        //Rewards to Players
        if (this.characterLeveUp != null) this.characterLeveUp.AddExp(this.enemyAI.EnemySO.Exp);


        //Update UI (only apply to survivals)
       //if (this.waveSpawner != null) this.waveSpawner.SubstractEnemyToUI();


        //PlayerScoreManager.Instance.AddScore(this.enemyAI.EnemySO.Score);

        this.coroutine = null;
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
    public virtual void DropItem(Transform position, PickUpSpawner spawner)
    {
        foreach (ItemDrop drop in this.enemyAI.ItemDropSO.ItemDrops)
        {
            float rollItem = Random.Range(0f, 100f);
            if (rollItem <= drop.dropChance)
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

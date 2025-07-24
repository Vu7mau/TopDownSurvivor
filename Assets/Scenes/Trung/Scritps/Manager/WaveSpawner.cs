using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;
using UnityEngine.AI;

public class WaveSpawner :VuMonoBehaviour
{
    [SerializeField] private WaveConfig waveConfig;
    [SerializeField] private EnemiesSpawner enemiesSpawner;
    [SerializeField] private Transform[] spawnPoints;

    [SerializeField] protected Transform waveHolder;

    [SerializeField] protected float timeDelayEachSpawn = 0.1f;

    private int currentWaveIndex = 0;
    private bool isSpawning = false;
    [SerializeField] private bool canSpawnContinue = false;

    protected override void Start()
    {
        base.Start();
        this.StartWaves();
    }



    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadWaveHolder();
    }

    protected virtual void LoadWaveHolder()
    {
        if (this.waveHolder != null) return;
        this.waveHolder = GameObject.Find("WaveHolder").transform;
    }





    public void StartWaves()
    {
        if (!isSpawning)
            StartCoroutine(HandleWaves());
    }

    private IEnumerator HandleWaves()
    {
        isSpawning = true;

        while (currentWaveIndex < waveConfig.waves.Count)
        {
            this.canSpawnContinue = true;
            yield return StartCoroutine(SpawnWave(waveConfig.waves[currentWaveIndex]));
            currentWaveIndex++;
        }

        isSpawning = false;
    }

    private IEnumerator SpawnWave(WaveData wave)
    {
        int waveCount = wave.timeSpawnEachWave;
        float spawnInterval = wave.waveDuration / waveCount;

        //Tạo dictionary để theo dõi mỗi loại quái đã spawn được bao nhiêu.
        Dictionary<GameObject, int> spawnedSoFar = new Dictionary<GameObject, int>();

        foreach (var enemy in wave.enemies)
            spawnedSoFar[enemy.enemyPrefab] = 0;

        for (int i = 0; i < waveCount; i++)
        {
            foreach (var enemy in wave.enemies)
            {
                int remaining = enemy.totalAmount - spawnedSoFar[enemy.enemyPrefab]; 

                int spawnThisRound = Mathf.CeilToInt(enemy.totalAmount / (float)waveCount); //Tính số lượng cần spawn trong đợt hiện tại

                if (spawnThisRound > remaining) spawnThisRound = remaining;// Đảm bảo không spawn quá tổng số

                for (int j = 0; j < spawnThisRound; j++)
                {
                    var point = spawnPoints[Random.Range(0, spawnPoints.Length)];
                    if(this.enemiesSpawner != null)
                    {
                        EnemyCtrl enemyPrefab = enemy.enemyPrefab.GetComponentInChildren<EnemyCtrl>();
                        EnemyCtrl newEnemy = this.enemiesSpawner.Spawn(enemyPrefab, point.position);
                        if (!this.canSpawnContinue) break;
                        if(newEnemy != null)
                        {

                        }
                    }
                    yield return new WaitForSeconds(this.timeDelayEachSpawn);
                }
                if (!this.canSpawnContinue) break;
                spawnedSoFar[enemy.enemyPrefab] += spawnThisRound;
            }
            if (!this.canSpawnContinue) break;
            yield return new WaitForSeconds(spawnInterval);
        }
        if (!this.canSpawnContinue) yield break;
    }
}

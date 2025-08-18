using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class WaveSpawner :VuMonoBehaviour
{
    [Header("Survival!")]
    [SerializeField] private WaveConfig waveConfig;
    [SerializeField] private EnemiesSpawner enemiesSpawner;
    [SerializeField] private Timer timer;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] protected Transform waveHolder;
    [SerializeField] protected float timeDelayEachSpawn = 0.1f;
    [SerializeField] private bool canSpawnContinue = false;

    [Space]
    [Header("Finish the battle Survival!")]
    [SerializeField] protected Transform f_winPanel;
    [SerializeField] protected Transform f_losePanel;
    [SerializeField] protected float timeWaitToEndGame;

    [SerializeField] protected Transform e_winPanel;
    [SerializeField] protected Transform e_losePanel;
    [SerializeField] protected Transform bg;




    private int currentWaveIndex = 0;
    private int enemyLefts = 0;


    private int currentWaveEnemiesAlive = 0;
    private bool waveClearedEarly = false;


    private bool isSpawning = false;
    private bool isFinish = false;


    private enum Mode { TimedWave, Adventure }
    private Mode mode = Mode.TimedWave;

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
        Time.timeScale = 1.0f;
        if (!isSpawning)
            StartCoroutine(HandleWaves());
    }

    private IEnumerator HandleWaves()
    {
        isSpawning = true;
        this.isFinish = false;
        while (currentWaveIndex < waveConfig.waves.Count)
        {
            this.canSpawnContinue = true;
            yield return StartCoroutine(SpawnWave(waveConfig.waves[currentWaveIndex]));
            currentWaveIndex++;
        }
        this.FinishBattle();


        isSpawning = false;
    }

    protected virtual void FinishBattle()
    {
        if (this.bg != null) bg.gameObject.SetActive(false);
        if (this.timer.TimeIsUp && this.enemyLefts > 0)
        {
            StartCoroutine(this.EndGamePlayRoutine(false));
            return;
        }
        else
        {
            StartCoroutine(this.EndGamePlayRoutine(true));
            return;
        }
    }
    protected IEnumerator EndGamePlayRoutine(bool isWin)
    {
        if (this.f_losePanel != null && this.f_winPanel != null)
        {
            Transform obj = isWin ? this.f_winPanel : this.f_losePanel;
            obj.gameObject.SetActive(true);
            yield return new WaitForSeconds(3f);
            obj.gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(this.timeWaitToEndGame);

        if (this.e_winPanel != null && this.e_losePanel != null)
        {
            Transform objEndGame = isWin ? this.e_winPanel : this.e_losePanel;
            if (objEndGame != null) objEndGame.gameObject.SetActive(true);
        }
    }

    private IEnumerator SpawnWave(WaveData wave)
    {
        int waveCount = wave.timeSpawnEachWave;
        float spawnInterval = (float) wave.waveDuration / waveCount;
        UIManager.Instance.UpdateTimeToNextWave(wave.waveDuration);

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
                            this.AddEnemyToUI();
                            newEnemy.GetComponentInChildren<EnemyResponse>().IsReward = true;
                            newEnemy.GetComponent<EnemyAIController>().ChaseRange = 100000f;
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

    public virtual void SubstractEnemyToUI()
    {
        --this.enemyLefts;
        //--this.currentWaveEnemiesAlive;
        if (this.currentWaveIndex < this.waveConfig.waves.Count) UIManager.Instance.UpdateWaveUI(this.currentWaveIndex + 1, this.enemyLefts);

        //// Kiểm tra nếu đã giết hết enemy
        //if (this.currentWaveEnemiesAlive <= 0 && !this.timer.TimeIsUp)
        //{
        //    StartCoroutine(OnWaveClearedEarly());
        //}
    }
    public virtual void AddEnemyToUI()
    {
        ++this.enemyLefts;
        //++this.currentWaveEnemiesAlive;
        if(this.currentWaveIndex < this.waveConfig.waves.Count) UIManager.Instance.UpdateWaveUI(this.currentWaveIndex + 1, this.enemyLefts);
    }

    private IEnumerator ShowWaveClearUIAndCountdown()
    {
        //UIManager.Instance.ShowMessage("Wave Cleared!", 3f); // hiển thị 3 giây
        Debug.Log("Toàn bộ quái vật đã bị tiêu diệt");

        yield return new WaitForSeconds(3f);

        int countdown = 5;
        while (countdown > 0)
        {
            //UIManager.Instance.ShowMessage($"Next wave in: {countdown}", 1f);
            Debug.Log("Chuẩn bị đợt mới: " + countdown);
            yield return new WaitForSeconds(1f);
            countdown--;
        }

        //UIManager.Instance.HideMessage(); // ẩn thông báo nếu muốn
    }

    private IEnumerator OnWaveClearedEarly()
    {
        if (waveClearedEarly) yield break; // tránh chạy nhiều lần
        waveClearedEarly = true;

        this.canSpawnContinue = false; // dừng các vòng spawn tiếp theo trong wave hiện tại

        //yield return StartCoroutine(ShowWaveClearUIAndCountdown());

        //currentWaveIndex++;
        //if (currentWaveIndex < waveConfig.waves.Count)
        //{
        //    yield return StartCoroutine(SpawnWave(waveConfig.waves[currentWaveIndex]));
        //}
        //else
        //{
        //    Debug.Log("All waves completed!");
        //}
    }
}

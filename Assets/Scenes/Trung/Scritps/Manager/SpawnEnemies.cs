using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class SpawnEnemies :MonoBehaviour
{
    [SerializeField] private GameObject obj;
    [SerializeField] private EnemiesManageSO _spawn;
    [SerializeField] private EnemiesManageSO _spawnBosses;
    [SerializeField] private SpawnEnemiesSO _waves;
    [SerializeField] private GameObject panelBossFight;

    [SerializeField] private List<Transform> spawnPositionWaveEnemiesList;


    private int amountEnemiesMixed;

    [Header("Wave")]
    [SerializeField] private int waveNumber = 1;

    [Header("Change the time each wave (calculator by minutes)")]
    private int enemiesLeft = 0;
    private int enemiesPerWave;

    private List<GameObject> listParentGameObject = new List<GameObject>();
    private List<GameObject> listParentBossesGameObject = new List<GameObject>();
    [SerializeField] private List<GameObject> selectedChildren;
    private void Start()
    {
        UIManager.Instance.UpdateWaveUI(waveNumber,enemiesLeft);
        CreateAllEnemiesFirst();
        StartCoroutine(SpawnWave());
    }
    IEnumerator SpawnWave()
    {
        while (waveNumber <= _waves.listWaves.Count)
        {
            if (_waves.listWaves[_waves.WaveElement(waveNumber)].waveMode == Wave.ModeWave.EachType)
            {
                SpawnEnemiesFight(waveNumber);
            }
            if (_waves.listWaves[_waves.WaveElement(waveNumber)].waveMode == Wave.ModeWave.Mixed)
            {
                amountEnemiesMixed = _waves.listWaves[_waves.WaveElement(waveNumber)].Amount;
                SpawnRandomEnemy(waveNumber, amountEnemiesMixed);
            }
            if (_waves.listWaves[_waves.WaveElement(waveNumber)].waveMode == Wave.ModeWave.BossFight)
            {
                amountEnemiesMixed = _waves.listWaves[_waves.WaveElement(waveNumber)].bossLists[0].Amount;
                StartCoroutine(SpawnBosses(waveNumber, amountEnemiesMixed));
            }
            UIManager.Instance.UpdateTimeToNextWave(_waves.listWaves[_waves.WaveElement(waveNumber)].timeForNextWave);
            yield return new WaitForSeconds(_waves.listWaves[_waves.WaveElement(waveNumber)].timeForNextWave);
            waveNumber++;
        }
    }
    public void SpawnEnemiesFight(int wave)
    {
        if(wave > 0 && wave <= _spawn.listEnemies.Count)
        {
            enemiesPerWave = _waves.listWaves[_waves.WaveElement(wave)].Amount;
            enemiesLeft = CalculatorEnemiesLeft(enemiesPerWave);
            UIManager.Instance.UpdateWaveUI(waveNumber, enemiesLeft);
            StartCoroutine(SpawnEnemy(wave,enemiesPerWave));
        }
    }

    public void EnemyDefeated(int amount)
    {
        enemiesLeft -= amount;
        if(enemiesLeft < 0) enemiesLeft = 0;
        UIManager.Instance.UpdateWaveUI(waveNumber, enemiesLeft);
    }
    private IEnumerator SpawnEnemy(int wave,int amountEachWave)
    {
        if (wave < 1) { yield return null; }
        for(int dem = 0; dem < amountEachWave; dem++)
        {
            int randomPositionSpawnWave = Random.Range(0, spawnPositionWaveEnemiesList.Count);
            Vector3 spawnPosition = spawnPositionWaveEnemiesList[randomPositionSpawnWave].position;
            GameObject enemy = transform.GetChild(_waves.listWaves[_waves.WaveElement(wave)].EnemyTypeIndex - 1).gameObject.transform.GetChild(dem).gameObject;
            if(enemy != null)
            {
                enemy.transform.position = spawnPosition;
                enemy.SetActive(true);
            }
        }
    }
    private static bool IsBossFight = false;
    public static void StartFightBossRightNow(bool _isFight)
    {
        IsBossFight= _isFight;
    }
    private IEnumerator SpawnBosses(int wave, int amountEachWave)
    {
        Transform obj = GameObject.Find("SpawnBosses").transform;
        if (wave < 1) { yield return null; }
        enemiesPerWave = amountEachWave;
        enemiesLeft = CalculatorEnemiesLeft(enemiesPerWave);
        UIManager.Instance.UpdateWaveUI(wave, enemiesLeft);
        panelBossFight.SetActive(true);
        yield return new WaitUntil(() => IsBossFight);
        for (int dem = 0; dem < amountEachWave; dem++)
        {
            int randomPositionSpawnWave = Random.Range(0, spawnPositionWaveEnemiesList.Count);
            Vector3 spawnPosition = spawnPositionWaveEnemiesList[randomPositionSpawnWave].position;
            GameObject enemy = obj.GetChild(_waves.listWaves[_waves.WaveElement(wave)].bossLists[0].BossType - 1).gameObject.transform.GetChild(dem).gameObject;
            if (enemy != null)
            {
                enemy.transform.position = spawnPosition;
                enemy.SetActive(true);
            }
        }
        IsBossFight = false;
    }
    private void SpawnRandomEnemy(int wave, int amountEachWave)
    {
        SpawnRandomEnemies(amountEachWave);
        enemiesPerWave = amountEachWave;
        enemiesLeft = CalculatorEnemiesLeft(enemiesPerWave);
        UIManager.Instance.UpdateWaveUI(wave, enemiesLeft);
        StartCoroutine(SpawnRandomEnemiesRoutine());
        //GameObject spawnMixed = GameObject.Find("SpawnEnemiesMixed");
        
        
        Debug.Log("Đây là đợt quái trộn!");
    }
    private IEnumerator SpawnRandomEnemiesRoutine()
    {
        yield return new WaitForSeconds(2f);
        listEnemiesRandom = listEnemiesRandom.Where(e => e != null).ToList();
        int dem = 0;
        while (dem < listEnemiesRandom.Count)
        {
            GameObject enemy = listEnemiesRandom[dem];
            if (enemy != null)
            {
                int randomPositionSpawnWave = Random.Range(0, spawnPositionWaveEnemiesList.Count);
                Vector3 spawnPosition = spawnPositionWaveEnemiesList[randomPositionSpawnWave].position;
                enemy.transform.position = spawnPosition;
                enemy.SetActive(true);
            }
            dem++;
            listEnemiesRandom = listEnemiesRandom.Where(e => e != null).ToList();
        }
    }

    private int CalculatorEnemiesLeft(int _amount)
    {
        return enemiesLeft + _amount;
    }
    private void CreateEnemiesEachType()
    {
        for(int j= 0;j< _spawn.listEnemies.Count;j++)
        {
            for (int i = 0; i < _waves.MaxAmountEachEnemyType(j + 1); i++)
            {
                GameObject enemy = Instantiate(_spawn.listEnemies[j]);
                enemy.SetActive(false);
                enemy.transform.parent = gameObject.transform.GetChild(j);
            }
        }
    }
    private void CreateBossesEachType()
    {
        Transform obj = GameObject.Find("SpawnBosses").transform;
        for (int j = 0; j < _spawnBosses.listEnemies.Count; j++)
        {
            for (int i = 0; i < 20; i++)
            {
                GameObject enemy = Instantiate(_spawnBosses.listEnemies[j]);
                enemy.SetActive(false);
                enemy.transform.parent = obj.GetChild(j);
            }
        }
    }
    //Trộn quái
    private List<GameObject> listEnemiesRandom = new List<GameObject>();
    private void SpawnRandomEnemies(int _amountEnemiesMixed)
    {
        listEnemiesRandom.Clear();
        selectedChildren.Clear();
        selectedChildren = SelectRandomChildren(listParentGameObject, _amountEnemiesMixed);
        listEnemiesRandom = selectedChildren;
        //GameObject spawnMixed = GameObject.Find("SpawnEnemiesMixed");
    }
    private List<GameObject> SelectRandomChildren(List<GameObject> parentObjects, int totalToSelect)
    {
        List<GameObject> allChildren = new List<GameObject>();

        // Lấy tất cả gameobject con của từng cha
        foreach (GameObject parent in parentObjects)
        {
            if (parent.transform.childCount > 0)
            {
                List<GameObject> children = new List<GameObject>();
                foreach (Transform child in parent.transform)
                {
                    children.Add(child.gameObject);
                }
                allChildren.AddRange(children);
            }
        }
        totalToSelect = Mathf.Min(totalToSelect, allChildren.Count);

        List<GameObject> selectedChildren = new List<GameObject>();
        while (selectedChildren.Count < totalToSelect)
        {
            GameObject randomChild = allChildren[Random.Range(0, allChildren.Count)];
            if (!selectedChildren.Contains(randomChild))
            {
                selectedChildren.Add(randomChild);
            }
        }

        return selectedChildren;
    }


    //Tạo các GameObject cha lưu trữ các gameobject con (quái) theo từng loại
    private void CreateManageEnemiesParent()
    {
        for (int i = 0; i < _spawn.listEnemies.Count; i++)
        {
            GameObject e = Instantiate(obj);
            e.transform.parent = transform;
            e.gameObject.name = _spawn.listEnemies[i].name;
            listParentGameObject.Add(e);
        }
    }
    //Tạo các GameObject cha lưu trữ các gameobject con (quái) theo từng loại
    private void CreateManageBossesParent()
    {
        Transform bossesManagementObj = GameObject.FindGameObjectWithTag("BossesManager").transform;
        for (int i = 0; i < _spawnBosses.listEnemies.Count; i++)
        {
            GameObject e = Instantiate(obj);
            e.transform.parent = bossesManagementObj;
            e.gameObject.name = _spawnBosses.listEnemies[i].name;
            listParentBossesGameObject.Add(e);
        }
    }

    private float CalculatorSeconds(float _minutes)
    {
        return _minutes * 60;
    }
    private void CreateAllEnemiesFirst()
    {
        CreateManageEnemiesParent();
        CreateEnemiesEachType();
        CreateManageBossesParent();
        CreateBossesEachType();
    }
}

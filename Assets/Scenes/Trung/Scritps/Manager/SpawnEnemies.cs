using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class SpawnEnemies : Singleton<SpawnEnemies>
{
    [SerializeField] private GameObject obj;
    [SerializeField] private EnemiesManageSO _spawn;
    [SerializeField] private Transform position;
    [SerializeField] private SpawnEnemiesSO _waves;

    [SerializeField] private List<Transform> spawnPositionWaveEnemiesList;
    [SerializeField] private Camera mainCamera;


    private int amountEnemiesMixed;

    [Header("Wave")]
    [SerializeField] private int waveNumber = 1;

    [Header("Change the time each wave (calculator by minutes)")]
    private int enemiesLeft = 0;
    private int enemiesPerWave;

    private List<GameObject> listParentGameObject = new List<GameObject>();
    [SerializeField] private List<GameObject> selectedChildren;
    protected override void Awake()
    {
        base.Awake();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }
    protected override void Start()
    {
        base.Start();
        UIManager.Instance.UpdateWaveUI(waveNumber,enemiesLeft);
        CreateManageEnemiesParent();
        CreateEnemiesEachType();
        //SpawnRandomEnemies();
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
            UIManager.Instance.UpdateTimeToNextWave(CalculatorSeconds(_waves.listWaves[_waves.WaveElement(waveNumber)].timeForNextWave));
            yield return new WaitForSeconds(CalculatorSeconds(_waves.listWaves[_waves.WaveElement(waveNumber)].timeForNextWave));
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
        if(wave < 1) { yield return null; }
        for(int dem = 0; dem < amountEachWave; dem++)
        {
            int randomPositionSpawnWave = Random.Range(0, spawnPositionWaveEnemiesList.Count);
            Debug.Log("Spawn Position: "+randomPositionSpawnWave);
            Vector3 spawnPosition = spawnPositionWaveEnemiesList[randomPositionSpawnWave].position;
            GameObject enemy = transform.GetChild(_waves.listWaves[_waves.WaveElement(wave)].EnemyTypeIndex - 1).gameObject.transform.GetChild(dem).gameObject;
            enemy.SetActive(true);
            enemy.transform.position = spawnPosition;
            yield return new WaitForSeconds(2f);
        }
    }
    private void SpawnRandomEnemy(int wave, int amountEachWave)
    {
        enemiesPerWave = amountEachWave;
        enemiesLeft = CalculatorEnemiesLeft(enemiesPerWave);
        SpawnRandomEnemies(amountEachWave);
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
                enemy.SetActive(true);
                enemy.transform.position = RandomPositionSpawn();
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

    private float CalculatorSeconds(float _minutes)
    {
        return _minutes * 60;
    }
    private Vector3 RandomPositionSpawn()
    {
        Vector3 spawnPointPosition = Vector3.zero;
        bool IsVisible = true;
        while (IsVisible)
        {
            int randomPositionSpawnWave = Random.Range(0, spawnPositionWaveEnemiesList.Count);
            spawnPointPosition = spawnPositionWaveEnemiesList[randomPositionSpawnWave].position;
            Vector3 screenSpawnPoint = mainCamera.WorldToViewportPoint(spawnPointPosition);
            IsVisible = screenSpawnPoint.x > 0 && screenSpawnPoint.x < 1 && screenSpawnPoint.y > 0 && screenSpawnPoint.y < 1 && screenSpawnPoint.z > 0;
            if (!IsVisible)
            {
                return spawnPointPosition;
            }
        }
        return transform.position;
    }
}

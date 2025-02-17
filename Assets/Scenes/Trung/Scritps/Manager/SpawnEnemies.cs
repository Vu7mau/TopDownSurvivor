using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnEnemies : Singleton<SpawnEnemies>
{
    [SerializeField] private GameObject obj;
    [SerializeField] private EnemiesManageSO _spawn;
    [SerializeField] private Transform position;
    [SerializeField] private SpawnEnemiesSO _waves;

    [SerializeField] private Transform spawnMinWavePosition;
    [SerializeField] private Transform spawnMaxWavePosition;
    [SerializeField] private Camera mainCamera;


    private int amountEnemiesMixed;

    [Header("Wave")]
    [SerializeField] private int waveNumber = 1;

    [Header("Change the time each wave (calculator by minutes)")]
    private int enemiesLeft = 0;
    private int enemiesPerWave;

    private List<GameObject> listParentGameObject = new List<GameObject>();
    private List<GameObject> selectedChildren = new List<GameObject>();
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
        while (waveNumber < 11)
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
        enemiesLeft-= amount;
        if(enemiesLeft < 0) enemiesLeft = 0;
        UIManager.Instance.UpdateWaveUI(waveNumber, enemiesLeft);
    }
    private IEnumerator SpawnEnemy(int wave,int amountEachWave)
    {
        if(wave < 1) { yield return null; }
        for(int dem = 0; dem < amountEachWave; dem++)
        {
            Vector3 spawnPosition = RandomPositionSpawn();
            GameObject enemy = transform.GetChild(_waves.listWaves[_waves.WaveElement(wave)].EnemyTypeIndex - 1).gameObject.transform.GetChild(dem).gameObject;
            enemy.SetActive(true);
            enemy.transform.position = spawnPosition;
        }
    }
    private void SpawnRandomEnemy(int wave, int amountEachWave)
    {
        enemiesPerWave = _waves.listWaves[_waves.WaveElement(wave)].Amount;
        enemiesLeft = CalculatorEnemiesLeft(enemiesPerWave);
        UIManager.Instance.UpdateWaveUI(wave, enemiesLeft);
        SpawnRandomEnemies();
        GameObject spawnMixed = GameObject.Find("SpawnEnemiesMixed");
        int dem = 0;
        while (dem < amountEachWave)
        {
            GameObject enemy = spawnMixed.transform.GetChild(dem).gameObject;
            enemy.SetActive(true);
            enemy.transform.position = RandomPositionSpawn();
            dem++;
        }
        Debug.Log("Đây là đợt quái trộn!");
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
    private void SpawnRandomEnemies()
    {
        selectedChildren = SelectRandomChildren(listParentGameObject, amountEnemiesMixed);
        GameObject spawnMixed = GameObject.Find("SpawnEnemiesMixed");
        for(int i = 0;i < selectedChildren.Count;i++)
        {
            selectedChildren[i].transform.parent = spawnMixed.transform;
        }
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

        // Đảm bảo không chọn nhiều hơn tổng số con có sẵn
        totalToSelect = Mathf.Min(totalToSelect, allChildren.Count);

        // Trộn danh sách và lấy ngẫu nhiên 30 gameobject con
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
            spawnPointPosition = new Vector3(Random.Range(spawnMinWavePosition.position.x, spawnMaxWavePosition.position.x), 0, Random.Range(spawnMaxWavePosition.position.z, spawnMinWavePosition.position.z));
            Vector3 screenSpawnPoint = mainCamera.WorldToViewportPoint(spawnPointPosition);
            IsVisible = screenSpawnPoint.x > 0 && screenSpawnPoint.x < 1 && screenSpawnPoint.y > 0 && screenSpawnPoint.y < 1 && screenSpawnPoint.z > 0;
            if (!IsVisible) break;
        }
        return spawnPointPosition;
    }
}

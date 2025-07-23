using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;
using UnityEngine.AI;

public class SpawnEnemies :VuMonoBehaviour
{
    public static SpawnEnemies Instance;


    [SerializeField] private GameObject obj;
    [SerializeField] private GameObject panelBossFight;

    [Header("Manager")]
    [SerializeField] private EnemiesManageSO _spawn;
    [SerializeField] private EnemiesManageSO _spawnBosses;
    [SerializeField] private WaveManager _waves;
    [SerializeField] protected EnemiesSpawner enemiesSpawner;

    [Header("Position Spawn")]
    [SerializeField] protected Transform playerPosition;
    [SerializeField] protected Transform spawnPosition;
    [SerializeField] protected float offSetSpawn = 10f;
    [SerializeField] private List<Transform> listLimitPositionsSpawn;



    [Header("Wave")]
    [SerializeField] protected int waveNumber = 1;
    public int WaveNumber { get => waveNumber; }

    protected int amountWave;
    public int AmountWave { get => amountWave; }



    [Header("Change the time each wave (calculator by minutes)")]
    [SerializeField] private int maxEnemies = 20;
    private int enemiesLeft = 0;
    private int enemiesPerWave;
    private int totalAmountEnemiesEachWaves = 0;
    private int amountEnemiesPlayerKilled = 0;
    private int amountEnemiesMixed;
    private bool isStartFight = false;
    public bool IsStartFight { set => isStartFight = value; }

    private bool isFinale = false;
    public bool IsFinale { set => isFinale = value; }

    private List<GameObject> listParentGameObject = new List<GameObject>();
    private List<GameObject> listParentBossesGameObject = new List<GameObject>();
    [SerializeField] private List<GameObject> selectedChildren;

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
        amountWave = _waves.listWaves.Count;
    }
    protected override void Start()
    {
        UIManager.Instance.UpdateWaveUI(waveNumber,enemiesLeft);
        this.CreateAllEnemiesFirst();
        StartCoroutine(SpawnWave());
            
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadEnemiesSpawner();
        this.LoadPlayerPosition();
    }
    protected virtual void LoadEnemiesSpawner()
    {
        if (this.enemiesSpawner != null) return;
        this.enemiesSpawner = FindAnyObjectByType<EnemiesSpawner>();
    }
    protected virtual void LoadPlayerPosition()
    {
        if (this.playerPosition != null) return;
        this.playerPosition = FindAnyObjectByType<CharacterAnimHandle>().transform;
    }
    private void Update()
    {
        this.CheckFinish();
    }
    [SerializeField] private  bool timeIsUp = false;
    public void FinishTheBattle(bool _finish)
    {
        timeIsUp = _finish;
    }
    private void CheckFinish()
    {
        if (!isStartFight) return;
        if (this.waveNumber <= this._waves.listWaves.Count) return;
        GameObject panelFinish = GameObject.Find("PanelWhenFinishTheBattle");
        if (timeIsUp) 
        {
            if (!this.isFinale)
            {
                StartCoroutine(FinishPanelLoseRoutine(panelFinish, 1));
                return;
            }
        }
        else
        {
            if(enemiesLeft == 0 && isFinale)
            {
                StartCoroutine(FinishPanelWinRoutine(panelFinish, 0));
                return;
            }
        }

    }
    IEnumerator FinishPanelWinRoutine(GameObject obj, int index)
    {
        isStartFight = false;
        obj.transform.GetChild(index).gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        obj.transform.GetChild(index).gameObject.SetActive(false);
        yield return new WaitForSeconds(3f);
        Timer.Instance.StopCountDown(false, false);
        Menu.Instance.Win();
    }
    IEnumerator FinishPanelLoseRoutine(GameObject obj, int index)
    {
        isStartFight = false;
        obj.transform.GetChild(index).gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        obj.transform.GetChild(index).gameObject.SetActive(false);
        yield return new WaitForSeconds(3f);
        Menu.Instance.die();
    }
    IEnumerator SpawnWave()
    {
        this.isStartFight = true;
        if (waveNumber > _waves.listWaves.Count)
        {
            waveNumber = _waves.listWaves.Count;
            this.isFinale = true;
            yield break;
        }
        while (waveNumber <= _waves.listWaves.Count)
        {
            this.SpawnEnemiesFight(waveNumber);
            //if (_waves.listWaves[_waves.WaveElement(waveNumber)].CalculatorAmountBosses() != 0)
            //{
            //    amountEnemiesMixed = _waves.listWaves[_waves.WaveElement(waveNumber)].bossLists[0].Amount;
            //    StartCoroutine(SpawnBosses(waveNumber, amountEnemiesMixed));
            //}
            //if (_waves.listWaves[_waves.WaveElement(waveNumber)].waveMode == Wave.ModeWave.Mixed)
            //{
            //    amountEnemiesMixed = _waves.listWaves[_waves.WaveElement(waveNumber)].Amount;
            //    SpawnRandomEnemy(waveNumber, amountEnemiesMixed);
            //}
            UIManager.Instance.UpdateTimeToNextWave(_waves.listWaves[_waves.WaveElement(waveNumber)].timeForNextWave);
            timeIsUp = false;
            yield return new WaitForSeconds(_waves.listWaves[_waves.WaveElement(waveNumber)].timeForNextWave);
            ++waveNumber;
        }
    }
    public virtual void SpawnEnemiesFight(int wave)
    {
        this.enemiesPerWave = _waves.listWaves[_waves.WaveElement(wave)].CalculatorAmountEnemiesFight();
        //enemiesLeft = CalculatorEnemiesLeft(enemiesPerWave);
        //UIManager.Instance.UpdateWaveUI(waveNumber, enemiesLeft);
        StartCoroutine(SpawnEnemyAI(wave, this.enemiesPerWave));
    }

    public virtual void EnemyDefeated(int amount)
    {
        this.enemiesLeft -= amount;
        this.amountEnemiesPlayerKilled += amount;
        if (this.enemiesLeft < 0) this.enemiesLeft = 0;
        UIManager.Instance.UpdateWaveUI(waveNumber, this.enemiesLeft);
    }
    private IEnumerator SpawnEnemyAI(int wave,int amountEachWave)
    {
        if (wave < 1) { yield break; }
        int amountEnemyWillSpawn;
        this.totalAmountEnemiesEachWaves = amountEachWave;
        while (totalAmountEnemiesEachWaves > 0)
        {
            amountEnemyWillSpawn = this.totalAmountEnemiesEachWaves;
            if (amountEnemyWillSpawn > this.maxEnemies)
            {
                amountEnemyWillSpawn = this.maxEnemies;
            }
            this.totalAmountEnemiesEachWaves -= amountEnemyWillSpawn;
            Vector3 spawnPosition = SnapToNavMesh(GetPositionSpawn());

            EnemyCtrl enemyPrefab;
            for (int i = 0; i < this._waves.listWaves[this._waves.WaveElement(waveNumber)].enemiesAIList.Count; i++)
            {
                enemyPrefab = this._spawn.listEnemies[this._waves.listWaves[wave - 1].enemiesAIList[i].EnemyTypeIndex - 1].GetComponent<EnemyCtrl>();
                string nameParentEnemy = "List" + enemyPrefab.name;
                Transform parent = GameObject.Find(nameParentEnemy).transform;
                this.enemiesSpawner.SetHoldParent(parent);
                int amountEachType = this._waves.listWaves[this._waves.WaveElement(waveNumber)].enemiesAIList[i].Amount;
                for (int t = 0; t < amountEachType; t++)
                {
                    EnemyCtrl newEnemy = this.enemiesSpawner.Spawn(enemyPrefab, spawnPosition);
                    ++this.enemiesLeft;
                    UIManager.Instance.UpdateWaveUI(wave, this.enemiesLeft);
                    if(this.enemiesLeft == this.maxEnemies) yield return new WaitUntil(() => this.enemiesLeft <= 0);
                    yield return new WaitForSeconds(1f); 
                }
                yield return new WaitForSeconds(1f);
            }


            //if (this.amountEnemiesPlayerKilled < this.totalAmountEnemiesEachWaves)
            //{
            //    if (this.timeIsUp)
            //    {
            //        this.totalAmountEnemiesEachWaves = 0;
            //        this.enemiesLeft = 0;
            //        this.amountEnemiesPlayerKilled = 0;
            //        break;
            //    }
            //    yield return null;
            //}
        }
    }

    protected virtual Vector3 SnapToNavMesh(Vector3 position)
    {
        Vector3 pos = Vector3.zero ;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(position, out hit, 1000f, NavMesh.AllAreas))
        {
            pos = hit.position;
            Debug.Log("Snapped enemy to NavMesh at: " + hit.position);
        }
        else
        {
            Debug.LogWarning("No NavMesh found near enemy! Cannot place NavMeshAgent.");
        }
        return pos;
    }

    protected virtual Vector3 GetPositionSpawn()
    {
        Vector3 spawnPosition;
        //float positionSpawnX = Random.Range(this.playerPosition.position.x - offSetSpawn, this.playerPosition.position.x + offSetSpawn);
        //float positionSpawnZ = Random.Range(this.playerPosition.position.z - offSetSpawn, this.playerPosition.position.z + offSetSpawn);
        //spawnPosition = new Vector3(positionSpawnX, this.playerPosition.position.y, positionSpawnZ);
        //if(spawnPosition.x > listLimitPositionsSpawn[0].position.x) spawnPosition = listLimitPositionsSpawn[0].position;
        //if (spawnPosition.x < listLimitPositionsSpawn[1].position.x) spawnPosition = listLimitPositionsSpawn[1].position;
        //if (spawnPosition.z < listLimitPositionsSpawn[2].position.x) spawnPosition = listLimitPositionsSpawn[2].position;
        //if (spawnPosition.z > listLimitPositionsSpawn[3].position.x) spawnPosition = listLimitPositionsSpawn[3].position;
        spawnPosition = this.spawnPosition.position;
        return spawnPosition;
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
            int randomPositionSpawnWave = Random.Range(0, listLimitPositionsSpawn.Count);
            Vector3 spawnPosition = listLimitPositionsSpawn[randomPositionSpawnWave].position;
            GameObject enemy = obj.GetChild(_waves.listWaves[_waves.WaveElement(wave)].bossLists[0].BossType - 1).gameObject.transform.GetChild(dem).gameObject;
            if (enemy != null)
            {
                enemy.transform.position = spawnPosition;
                enemy.SetActive(true);
                enemy.gameObject.GetComponentInChildren<EnemyHealth>().CheckAmountIncreaseHealth(_waves.listWaves[_waves.WaveElement(wave)].amountHealthIncreasePercent);
            }
            yield return new WaitForSeconds(1f);
        }
        IsBossFight = false;
    }
    private void SpawnRandomEnemy(int wave, int amountEachWave)
    {
        SpawnRandomEnemies(amountEachWave);
        enemiesPerWave = amountEachWave;
        enemiesLeft = CalculatorEnemiesLeft(enemiesPerWave);
        UIManager.Instance.UpdateWaveUI(wave, enemiesLeft);
        StartCoroutine(SpawnRandomEnemiesRoutine(wave));
        //GameObject spawnMixed = GameObject.Find("SpawnEnemiesMixed");
        
        Debug.Log("Đây là đợt quái trộn!");
    }
    private IEnumerator SpawnRandomEnemiesRoutine(int wave)
    {
        yield return new WaitForSeconds(2f);
        listEnemiesRandom = listEnemiesRandom.Where(e => e != null).ToList();
        int dem = 0;
        while (dem < listEnemiesRandom.Count)
        {
            GameObject enemy = listEnemiesRandom[dem];
            if (enemy != null)
            {
                int randomPositionSpawnWave = Random.Range(0, listLimitPositionsSpawn.Count);
                Vector3 spawnPosition = listLimitPositionsSpawn[randomPositionSpawnWave].position + new Vector3(Random.Range(-10, 10),0, Random.Range(-10, 10));
                enemy.transform.position = spawnPosition;
                enemy.SetActive(true);
                enemy.gameObject.GetComponent<EnemyHealth>().CheckAmountIncreaseHealth(_waves.listWaves[_waves.WaveElement(wave)].amountHealthIncreasePercent);
            }
            dem++;
            listEnemiesRandom = listEnemiesRandom.Where(e => e != null).ToList();
            yield return new WaitForSeconds(0.5f);
        }
    }

    private int CalculatorEnemiesLeft(int _amount)
    {
        return this.enemiesLeft + _amount;
    }

    private void CreateEnemiesEachType()
    {
        for(int j= 0;j< _spawn.listEnemies.Count;j++)
        {
            for (int i = 0; i < maxEnemies; i++)
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
        if (obj == null) return;
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
            e.gameObject.name = "List" + _spawn.listEnemies[i].name;
            listParentGameObject.Add(e);
        }
    }
    //Tạo các GameObject cha lưu trữ các gameobject con (quái) theo từng loại
    private void CreateManageBossesParent()
    {
        Transform bossesManagementObj = GameObject.FindGameObjectWithTag("BossesManager").transform;
        if (bossesManagementObj == null) return;
        for (int i = 0; i < _spawnBosses.listEnemies.Count; i++)
        {
            GameObject e = Instantiate(obj);
            e.transform.parent = bossesManagementObj;
            e.gameObject.name = _spawnBosses.listEnemies[i].name;
            listParentBossesGameObject.Add(e);
        }
    }
    private void CreateAllEnemiesFirst()
    {
        this.CreateManageEnemiesParent();
        //CreateEnemiesEachType();
        CreateManageBossesParent();
        CreateBossesEachType();
    }
}

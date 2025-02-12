using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnEnemies : MonoBehaviour
{
    [SerializeField] private GameObject obj;
    [SerializeField] private EnemiesManageSO _spawn;
    [SerializeField] private Transform position;

    [Header("Amount enemies each fight")]
    [SerializeField] private int amountEnemiesEachTypeInEachFight;
    [SerializeField] private int amountEnemiesMixed;
    [SerializeField] private float timeEachEnemyAppear;
    public int waveNumber = 1;        // Số thứ tự của đợt
    private int enemiesLeft;          // Số lượng quái còn lại
    private int enemiesPerWave;  // Số quái mỗi đợt


    private List<GameObject> listParentGameObject = new List<GameObject>();
    private List<GameObject> selectedChildren = new List<GameObject>();
    private void Start()
    {
        enemiesPerWave = amountEnemiesEachTypeInEachFight;
        CreateManageEnemiesParent();
        CreateEnemiesEachType();
        //SpawnRandomEnemies();
        StartCoroutine(SpawnWave());
    }

    //Sinh ra đợt quái trộn
    //private IEnumerator SpawnMixed()
    //{
    //    yield return new WaitForSeconds(10f);
    //    SpawnRandomEnemies();
    //}
    ////Sinh ra quái theo từng đợt
    //private IEnumerator SpawnRoutine()
    //{
    //    yield return new WaitForSeconds(5f);
    //    StartCoroutine(SpawnEnemy());
    //}

    IEnumerator SpawnWave()
    {
        while (waveNumber < 11)
        {
            SpawnEnemiesFight(); // Sinh quái theo đợt

            // Chờ 3 giây trước khi đợt mới xuất hiện
            yield return new WaitForSeconds(3f);

            // Tăng số thứ tự đợt
            waveNumber++;

            // Nếu đợt từ 5 trở đi, tăng số quái lên 30 và trộn loại quái
            if (waveNumber > _spawn.listEnemies.Count && waveNumber <= 10)
            {
                enemiesPerWave = amountEnemiesMixed;
                StartCoroutine(SpawnRandomEnemy());
            }

            // Reset số quái và tiếp tục vòng lặp
        }
    }
    public void SpawnEnemiesFight()
    {

        //for (int i = 0; i < enemiesPerWave; i++)
        //{
        //    // Chọn điểm spawn ngẫu nhiên
        //    Transform spawnPoint = position;

        //    // Chọn loại quái xuất hiện:
        //    GameObject enemyPrefab;
        //    if (waveNumber < 5)
        //    {
        //        enemyPrefab = transform.GetChild(waveNumber-1).gameObject.transform.GetChild(i).gameObject; // Chỉ sinh một loại
        //    }
        //    else
        //    {
        //        enemyPrefab = selectedChildren[Random.Range(0, selectedChildren.Count)]; // Trộn ngẫu nhiên
        //    }

        //    // Tạo quái
        //    GameObject enemy = enemyPrefab;
        //    enemy.gameObject.SetActive(true);

        //    // Khi quái bị tiêu diệt, gọi sự kiện để giảm enemiesLeft
        //    enemy.GetComponent<EnemyHealth>().onDeath += EnemyDefeated;
        //}
        if(waveNumber <= _spawn.listEnemies.Count)
        {
            enemiesLeft = enemiesPerWave;
            StartCoroutine(SpawnEnemy());
        }
    }

    private void EnemyDefeated()
    {
        enemiesLeft--;
    }
    private IEnumerator SpawnEnemy()
    {
        for(int dem = 0; dem < enemiesPerWave; dem++)
        {
            yield return new WaitForSeconds(timeEachEnemyAppear);
            GameObject enemy = transform.GetChild(waveNumber - 1).gameObject.transform.GetChild(dem).gameObject;
            enemy.SetActive(true);
            enemy.transform.position = new Vector3(position.position.x, position.position.y, position.position.z);
            enemy.GetComponent<EnemyHealth>().onDeath += EnemyDefeated;
        }
    }
    private IEnumerator SpawnRandomEnemy()
    {
        SpawnRandomEnemies();
        GameObject spawnMixed = GameObject.Find("SpawnEnemiesMixed");
        int dem = 0;
        while (dem < 30)
        {
            yield return new WaitForSeconds(timeEachEnemyAppear);
            GameObject enemy = spawnMixed.transform.GetChild(dem).gameObject;
            enemy.SetActive(true);
            enemy.transform.position = new Vector3(position.position.x, position.position.y, position.position.z);
            enemy.GetComponent<EnemyHealth>().onDeath += EnemyDefeated;
            dem++;
        }
    }

    private void CreateEnemiesEachType()
    {
        for(int j= 0;j< _spawn.listEnemies.Count;j++)
        {
            for (int i = 0; i < amountEnemiesEachTypeInEachFight; i++)
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
        Debug.Log($"Đã chọn {selectedChildren.Count} gameobject con.");
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
}

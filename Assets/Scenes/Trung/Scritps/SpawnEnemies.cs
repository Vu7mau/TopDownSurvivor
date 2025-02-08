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


    private List<GameObject> listParentGameObject = new List<GameObject>();
    private List<GameObject> selectedChildren = new List<GameObject>();
    private void Start()
    {
        CreateManageEnemiesParent();
        CreateEnemiesEachType();
        StartCoroutine(SpawnRoutine());
    }

    //Sinh ra đợt quái trộn
    private IEnumerator SpawnMixed()
    {
        yield return new WaitForSeconds(10f);
        SpawnRandomEnemies();
    }
    //Sinh ra quái theo từng đợt
    private IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(5f);
        StartCoroutine(SpawnEnemy());
    }
    private IEnumerator SpawnEnemy()
    {
        int dem = 0;
        while (dem < 20)
        {
            yield return new WaitForSeconds(timeEachEnemyAppear);
            GameObject enemy = transform.GetChild(0).gameObject.transform.GetChild(dem).gameObject;
            enemy.SetActive(true);
            enemy.transform.position = new Vector3(position.position.x, position.position.y, position.position.z);
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
        selectedChildren = SelectRandomChildren(listParentGameObject, 30);
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

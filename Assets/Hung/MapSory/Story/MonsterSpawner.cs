using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MonsterSpawnerTrigger : MonoBehaviour
{
    public GameObject monsterPrefab;   // Prefab quái vật
    public int spawnAmount = 3;        // Số lượng quái spawn mỗi lần
    public float spawnDuration = 5f;
    public bool spawnOnlyOnce = false;  // Spawn 1 lần thôi

    private bool hasSpawned = false;

    private void OnTriggerEnter(Collider other)
    {
        // Kiểm tra xem collider nào vào trigger, ở đây là Player (cần gán tag Player cho nhân vật)
        if (hasSpawned && spawnOnlyOnce) return;

        if (other.CompareTag("Player"))
        {
            StartCoroutine(SpawnMonstersOverTime());
            Debug.Log("Hee");
        }
    }

    private IEnumerator SpawnMonstersOverTime()
    {
        float spawnInterval = spawnDuration / spawnAmount;

        for (int i = 0; i < spawnAmount; i++)
        {
            // Tạo offset ngẫu nhiên quanh vị trí trigger
            Vector3 offset = new Vector3(Random.Range(-1.5f, 1.5f), 0f, Random.Range(-1.5f, 1.5f));
            Vector3 spawnPos = transform.position + offset;
            Instantiate(monsterPrefab, spawnPos, Quaternion.identity);
            yield return new WaitForSeconds(spawnInterval);
        }
        spawnOnlyOnce = true;
    }
}

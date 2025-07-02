using UnityEngine;

public class MonsterSpawnerTrigger : MonoBehaviour
{
    public GameObject monsterPrefab;
    public int spawnAmount = 3;
    public bool spawnOnlyOnce = true;

    private bool hasSpawned = false;

    private void OnTriggerEnter(Collider other)
    {
        if (spawnOnlyOnce && hasSpawned) return;

        if (other.CompareTag("Player"))
        {
            SpawnMonsters();
        }
    }

    public void SpawnNow()
    {
        if (spawnOnlyOnce && hasSpawned) return;

        SpawnMonsters();
    }

    private void SpawnMonsters()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            Vector3 offset = new Vector3(
                Random.Range(-1.5f, 1.5f),
                0f,
                Random.Range(-1.5f, 1.5f)
            );
            Vector3 spawnPos = transform.position + offset;
            Instantiate(monsterPrefab, spawnPos, Quaternion.identity);
        }

        hasSpawned = true;
    }
}

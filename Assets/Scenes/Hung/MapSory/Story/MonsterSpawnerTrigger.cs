using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnerTrigger : MonoBehaviour
{
    [SerializeField] private List<GameObject> monsterPrefabs;
    [SerializeField] private float delayBetweenWaves = 2f;
    public int spawnAmount = 3;
    public bool spawnOnlyOnce = true;

    private bool hasSpawned = false;
   private BoxCollider colliders;

    private void Awake()
    {
        colliders = GetComponent<BoxCollider>();
    }

    public void SpawnWave(int waveCount)
    {
        if (spawnOnlyOnce && hasSpawned) return;
        StartCoroutine(SpawnByWave(waveCount));
    }

    private IEnumerator SpawnByWave(int waveCount)
    {
        if (colliders == null || monsterPrefabs.Count == 0) yield break;

        for (int wave = 0; wave < waveCount; wave++)
        {
            Debug.Log($"▶ Wave {wave + 1}/{waveCount}");
            SpawnMonsters();
            yield return new WaitForSeconds(delayBetweenWaves);
        }

        hasSpawned = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SpawnWave(1);
        }
    }
    private void SpawnMonsters()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            Vector3 randomOffset = new Vector3(
                Random.Range(-colliders.size.x / 2f, colliders.size.x / 2f),
                0f,
                Random.Range(-colliders.size.z / 2f, colliders.size.z / 2f)
            );

            Vector3 spawnPos = transform.position + colliders.center + randomOffset;

            GameObject prefab = monsterPrefabs[Random.Range(0, monsterPrefabs.Count)];
            Instantiate(prefab, spawnPos, Quaternion.identity);
        }
    }
}

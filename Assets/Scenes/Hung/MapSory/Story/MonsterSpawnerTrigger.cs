using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnerTrigger : MonoBehaviour
{
    [SerializeField] private List<GameObject> monsterPrefabs;
    [SerializeField] private float delayBetweenWaves = 2f;
    [SerializeField] private int waveCount = 3;
    [SerializeField] private int spawnAmount = 3;

    [Header("Reset Options")]
    [SerializeField] private bool canReset = false;
    [SerializeField] private float resetTime = 60f; // Thời gian để reset trạng thái đã spawn

    private bool hasSpawnedCompleted = false;
    private bool isSpawning = false;

    private BoxCollider colliders;
    private Coroutine resetCoroutine;

    private void Awake()
    {
        colliders = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (hasSpawnedCompleted || isSpawning)
                return;

            if (resetCoroutine != null)
                StopCoroutine(resetCoroutine);

            StartCoroutine(SpawnByWave(waveCount));
        }
    }

    public void Spawn(int wavesToSpawn)
    {
        if (hasSpawnedCompleted || isSpawning)
        {
            Debug.LogWarning("Không thể spawn. Đã hoàn thành hoặc đang trong quá trình spawn.");
            return;
        }

        if (resetCoroutine != null)
            StopCoroutine(resetCoroutine);

        StartCoroutine(SpawnByWave(wavesToSpawn));
    }

    private IEnumerator SpawnByWave(int wavesToSpawn)
    {
        if (colliders == null || monsterPrefabs == null || monsterPrefabs.Count == 0)
            yield break;

        isSpawning = true;

        for (int wave = 0; wave < wavesToSpawn; wave++)
        {
            Debug.Log($"▶ Wave {wave + 1}/{wavesToSpawn}");
            SpawnMonsters();
            yield return new WaitForSeconds(delayBetweenWaves);
        }

        isSpawning = false;
        hasSpawnedCompleted = true;

        if (canReset)
        {
            resetCoroutine = StartCoroutine(ResetSpawnStateAfterDelay(resetTime));
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

            // Tính vị trí spawn theo world space
            Vector3 spawnPos = colliders.transform.TransformPoint(colliders.center + randomOffset);

            // Random prefab
            GameObject prefab = monsterPrefabs[Random.Range(0, monsterPrefabs.Count)];
            Quaternion randomRot = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

            GameObject monster = null;
            try
            {
                if (prefab == null)
                    throw new System.Exception("Prefab null");

                monster = Instantiate(prefab, spawnPos, randomRot, transform);

                // Có thể thêm check tuỳ ý, ví dụ prefab có component MonsterAI
                // if (monster.GetComponent<MonsterAI>() == null) throw new System.Exception("Thiếu MonsterAI");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"❌ Spawn lỗi: {ex.Message}");

                // Nếu có object nhưng lỗi thì huỷ nó ngay
                if (monster != null)
                {
                    Destroy(monster);
                }

                // Giảm i để lặp lại lượt spawn này => đảm bảo tổng số spawnAmount không bị hụt
                i--;
            }
        }
    }


    private IEnumerator ResetSpawnStateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        hasSpawnedCompleted = false;
        Debug.Log("Trạng thái spawn đã được khôi phục.");
    }

    // Vẽ gizmo hiển thị vùng spawn trong Scene
    private void OnDrawGizmosSelected()
    {
        BoxCollider box = GetComponent<BoxCollider>();
        if (box != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position + box.center, box.size);
        }
    }
}

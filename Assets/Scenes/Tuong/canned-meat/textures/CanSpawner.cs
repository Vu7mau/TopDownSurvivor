using System.Collections.Generic;
using UnityEngine;

public class CanSpawner : MonoBehaviour
{
    [Header("Prefabs & Pool")]
    public List<GameObject> canPrefabs; 
    public int poolSize = 10;

    [Header("Player & Spawn Settings")]
    public Transform player;
    public Vector3 spawnBoxSize = new Vector3(6f, 1f, 6f); 
    public float minSpawnInterval = 8f;
    public float maxSpawnInterval = 15f;

    private List<GameObject> canPool = new List<GameObject>();
    private float spawnTimer;
    private float nextSpawnTime;

    private void Start()
    {
        // Tạo pool
        for (int i = 0; i < poolSize; i++)
        {
            GameObject prefab = canPrefabs[Random.Range(0, canPrefabs.Count)];
            GameObject can = Instantiate(prefab, Vector3.one * 1000f, Quaternion.identity);
            can.SetActive(false);
            canPool.Add(can);

            if (can.GetComponent<Rigidbody>() == null)
            {
                Rigidbody rb = can.AddComponent<Rigidbody>();
                rb.mass = 1f;
            }

            if (can.GetComponent<CanMovement>() == null)
            {
                can.AddComponent<CanMovement>();
            }
        }

        spawnTimer = 0f;
        nextSpawnTime = 2f;
    }

    private void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= nextSpawnTime)
        {
            SpawnCanNearPlayer();
            spawnTimer = 0f;
            nextSpawnTime = Random.Range(minSpawnInterval, maxSpawnInterval);
        }
    }

    void SpawnCanNearPlayer()
    {
        if (player == null) return;

        var inactiveCans = canPool.FindAll(c => !c.activeInHierarchy);
        if (inactiveCans.Count == 0) return;

        GameObject can = inactiveCans[Random.Range(0, inactiveCans.Count)];

        GameObject newPrefab = canPrefabs[Random.Range(0, canPrefabs.Count)];

        MeshFilter mf = can.GetComponent<MeshFilter>();
        MeshRenderer mr = can.GetComponent<MeshRenderer>();

        if (mf != null && newPrefab.GetComponent<MeshFilter>() != null)
            mf.mesh = newPrefab.GetComponent<MeshFilter>().sharedMesh;

        if (mr != null && newPrefab.GetComponent<MeshRenderer>() != null)
            mr.material = newPrefab.GetComponent<MeshRenderer>().sharedMaterial;

        can.transform.localScale = newPrefab.transform.localScale;

        float offsetX = Random.Range(-spawnBoxSize.x / 2f, spawnBoxSize.x / 2f);
        float offsetY = Random.Range(0f, spawnBoxSize.y); 
        float offsetZ = Random.Range(-spawnBoxSize.z / 2f, spawnBoxSize.z / 2f);

        Vector3 spawnPos = player.position + new Vector3(offsetX, offsetY + 1f, offsetZ); // +1f để tránh lọt xuống đất

        can.transform.position = spawnPos;
        can.SetActive(true);

        CanMovement movement = can.GetComponent<CanMovement>();
        if (movement != null) movement.SetRandomDirection();

        can.layer = LayerMask.NameToLayer("Can");
    }

    private void OnDrawGizmosSelected()
    {
        if (player == null) return;

        Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
        Gizmos.DrawWireCube(player.position + new Vector3(0f, spawnBoxSize.y / 2f, 0f), spawnBoxSize);
    }
}

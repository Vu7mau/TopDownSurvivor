using System.Collections;
using UnityEngine;
using System.Collections.Generic;
public class ChestSpawner : MonoBehaviour
{
    public GameObject chestPrefab;
    public float spawnInterval = 300f;
    public string groundTag = "Ground";
    public GameClock gameClock;

    private GameObject currentChest;
    private float lastSpawnTime = 0f;
    private Vector3 lastSpawnPosition = Vector3.zero;
    private void Update()
    {
        if(gameClock == null)
        {
            Debug.Log("GameClock chưa đc gán");
            return;
        }
        if(gameClock.gameTime - lastSpawnTime >= spawnInterval)
        {
            SpawnChest();
            lastSpawnTime = gameClock.gameTime;
        }
    }
    private void SpawnChest()
    {
        Vector3 spawnPosition;
        do
        {
            spawnPosition = GetRandomGroundPosition();
        }
        while (Vector3.Distance(spawnPosition, lastSpawnPosition) < 0.01f && spawnPosition != Vector3.zero);
        if (spawnPosition != Vector3.zero)
        {
            currentChest = Instantiate(chestPrefab, spawnPosition, Quaternion.identity);
            lastSpawnPosition = spawnPosition;
        }
    }
    Vector3 GetRandomGroundPosition()
    {
        GameObject[] groundObjects = GameObject.FindObjectsOfType<GameObject>();
        var validGrounds = new List<GameObject>();
        int groundLayer = LayerMask.NameToLayer(groundTag);
        foreach ( var obj in groundObjects )
        {
            if(obj.layer == groundLayer)
            {
                validGrounds.Add(obj);  
            }
        }
        if(validGrounds.Count == 0) 
            return Vector3.zero;
        GameObject randomGround = validGrounds[Random.Range(0,validGrounds.Count)];
        return randomGround.transform.position + Vector3.up * 0.01f;
    }
}

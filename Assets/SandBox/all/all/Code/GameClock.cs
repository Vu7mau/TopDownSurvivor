using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameClock : MonoBehaviour
{
    public TMP_Text timeText; 
    public float gameTime = 0f;
    public GameObject[] npcPrefab;
    public Transform[] spawnPoints;
    private bool npcSpawned = false;

    void Update()
    {
        gameTime += Time.deltaTime; 

        int hours = Mathf.FloorToInt(gameTime / 3600);
        int minutes = Mathf.FloorToInt((gameTime % 3600) / 60);
        int seconds = Mathf.FloorToInt(gameTime % 60);

        timeText.text = $"{hours:00}:{minutes:00}:{seconds:00}";
        if(minutes >=5 && !npcSpawned)
        {
            SpawnNPCs();
            npcSpawned = true;
        }
    }
    private void SpawnNPCs()
    {
        if (spawnPoints.Length < 4 && npcPrefab.Length < 4)
        {
            Debug.Log("Không đủ điểm spawn NPC");
            return;
        }
        for(int i = 0; i < 4; i++)
        {
            Instantiate(npcPrefab[i], spawnPoints[i].position, spawnPoints[i].rotation);
        }
    }
}

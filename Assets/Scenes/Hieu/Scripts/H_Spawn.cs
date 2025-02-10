using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class H_Spawn : MonoBehaviour
{
    [SerializeField] private GameObject SpawnPosition;
    [SerializeField] private GameObject enemy;
    [SerializeField] private float TimeSpwan = 30;
    private float dem = 0;
    private void Update()
    {
        Spawn();
    }
    private void Spawn()
    {
        dem += Time.deltaTime;
        Debug.Log("bodemthoigian : " + dem);
        if (dem >= TimeSpwan)
        {
            Instantiate(enemy, SpawnPosition.transform.position, Quaternion.identity);
            dem = 0;            
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class WaveConfig : MonoBehaviour
{
    public List<WaveData> waves;
}
[System.Serializable]
public class EnemySpawnInfo
{
    [Header("Loại quái")]
    public GameObject enemyPrefab;

    [Space]
    [Space]
    [Header("Số lượng")]
    public int totalAmount;
}

[System.Serializable]
public class WaveData
{
    [Header("Thời gian tồn tại của wave")]
    public float waveDuration = 60f;      // Tổng thời gian 1 wave (mặc định 60 giây)

    [Space]    
    [Space]    
    [Space]
    [Header("Số đợt trong mỗi wave")]
    public int timeSpawnEachWave = 3;     //Số đợt trong mỗi wave quái

    [Space]
    [Space]
    [Space]
    [Header("Danh sách AI sinh ra trong mỗi wave")]
    public List<EnemySpawnInfo> enemies;  // Danh sách loại quái trong wave
}






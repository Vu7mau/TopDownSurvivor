using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using static SpawnEnemiesSO;

[CreateAssetMenu(fileName ="ListEnemiesSpawnManager")]
public class SpawnEnemiesSO : ScriptableObject
{
    public List<Wave> listWaves = new List<Wave>();
}
[System.Serializable]
public struct Wave
{
    public int WaveIndex; //thứ tự đợt
    public int EnemyTypeIndex; //thứ tự loại quái
    public int Amount;
}

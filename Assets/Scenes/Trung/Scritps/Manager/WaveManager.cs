using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Only use for controll amount and type enemy (1 type enemy/1 wave)")]
    public List<Wave> listWaves;
    public int WaveElement(int waveIndex)
    {
        int waveElement;
        for (int i = 0; i < listWaves.Count; i++)
        {
            if (listWaves[i].WaveIndex == waveIndex)
            {
                return waveElement = i;
            }
        }
        return 0;
    }
    public int MaxAmountEachEnemyType(int enemyTypeIndex)
    {
        int maxAmount = 0;
        var abc = listWaves
    .Where(enemy => enemy.EnemyTypeIndex == enemyTypeIndex).Count();
        if (abc > 0)
        {
            maxAmount = listWaves
            .Where(enemy => enemy.EnemyTypeIndex == enemyTypeIndex) // Lọc đúng loại enemy
            .OrderByDescending(enemy => enemy.Amount).Select(enemy => enemy.Amount).Max();
        }
        return maxAmount;
    }
}
[System.Serializable]
public class Wave
{
    public int WaveIndex;
    public int EnemyTypeIndex;
    public int Amount;
    public float timeForNextWave;
    public List<Boss> bossLists;
    public enum ModeWave { EachType, Mixed, BossFight }
    public ModeWave waveMode;

    [Header("Health Enemy will be follow percent!")]
    public int amountHealthIncreasePercent;
    public int amountDamageIncreasePercent;
}
[System.Serializable]
public struct Boss
{
    public int BossType;
    public int Amount;
}


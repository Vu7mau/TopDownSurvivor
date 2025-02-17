using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateListWavesSO : MonoBehaviour
{
    [SerializeField] private SpawnEnemiesSO listWaveSO;
    private void Update()
    {
        UpdateEnemyWaveIndex();
    }
    private void UpdateEnemyWaveIndex()
    {
        for(int i = 0;i< listWaveSO.listWaves.Count;i++)
        {
            listWaveSO.listWaves[i].WaveIndex = i + 1;
        }
    }
}

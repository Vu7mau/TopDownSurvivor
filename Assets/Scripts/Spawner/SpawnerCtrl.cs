using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerCtrl : VuMonoBehaviour
{
    [Header("Spawner")]
    [SerializeField] protected Spawner _spawner;
    public Spawner Spawner => _spawner;

    [Header("Spawn Point")]
    [SerializeField] protected SpawnPoints _spawnPoints;
    public SpawnPoints SpawnPoints => _spawnPoints;

    protected override void LoadComponent()
    {
        this.LoadSpawner();
        this.LoadSpawnPoint();
        
    }

    protected virtual void LoadSpawner()
    {
        if (_spawner != null) return;

        _spawner = GetComponent<Spawner>();
        Debug.Log("Load Spawner Success at " + this.transform.name);
    }
    protected virtual void LoadSpawnPoint()
    {
        if (_spawnPoints != null) return;
        
        _spawnPoints=GetComponentInChildren<SpawnPoints>();
        Debug.Log("Load SpawnPoint Success at " + this.transform.name);
    }
}

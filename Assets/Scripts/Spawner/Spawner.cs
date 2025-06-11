using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spawner : VuMonoBehaviour
{
    [Header("Spawner")]
    [SerializeField] protected Transform _holder;

    [SerializeField] protected int _spawnCount = 0;
    public int spawnCount => _spawnCount;

    [SerializeField] protected List<Transform> _objPrefabs;
    [SerializeField] protected List<Transform> _poolObj;

    protected override void LoadComponents()
    {
        this.LoadHoder();
        this.LoadPrefab();
    }
    protected virtual void LoadHoder()
    {
        if (_holder != null) return;

        _holder = this.transform.Find("Holder");
        Debug.Log("Load Hoder Success at " + this.transform.name);
    }
    protected virtual void LoadPrefab()
    {
        if (_objPrefabs.Count > 0) return;

        Transform objTransform = this.transform.Find("Prefabs");

        foreach (Transform obj in objTransform)
        {
            this._objPrefabs.Add(obj);
            obj.gameObject.SetActive(false);
        }

        Debug.Log("Load Prefab Success at " + this.transform.name);
    }


    public virtual Transform Spawn(string prefabName, Vector3 spawnPosition, Quaternion rota)
    {

        Transform prefab = this.GetPrefabByName(prefabName);
        if (prefab == null) { Debug.Log("Prefab not found " + prefabName); return null; }
        return this.Spawn(prefab, spawnPosition, rota);
    }

    public virtual Transform Spawn(Transform prefab, Vector3 spawnPosition, Quaternion rota)
    {
        Transform newPrefab = this.GetObjectFormPool(prefab);
        newPrefab.SetPositionAndRotation(spawnPosition, rota);

        newPrefab.transform.parent = this._holder;
        newPrefab.gameObject.SetActive(true);
        this._spawnCount++;
        return newPrefab;
    }
    protected virtual Transform GetObjectFormPool(Transform obj)
    {

        foreach (Transform poolObj in _poolObj)
        {
            if (poolObj == null) continue;

            if (poolObj.name == obj.name)
            {
                _poolObj.Remove(poolObj);
                return poolObj;
            }
        }
        Transform newPre = Instantiate(obj);
        newPre.name = obj.name;
        return newPre;
    }

    protected virtual Transform GetPrefabByName(string prefabName)
    {

        foreach (Transform obj in this._objPrefabs)
        {
            if (obj.name != prefabName) continue;
            return obj;
        }
        return null;
    }

    public virtual void Despawn(Transform obj)
    {
        this._poolObj.Add(obj);
        obj.gameObject.SetActive(false);
        this._spawnCount--;
    }
}

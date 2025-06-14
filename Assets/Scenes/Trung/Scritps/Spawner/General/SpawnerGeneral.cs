using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpawnerGeneral<T> : VuMonoBehaviour where T : PoolObj
{
    [SerializeField] protected int spawnCount = 0;
    [SerializeField] protected List<T> inPoolObjs;
    [SerializeField] protected Transform holderParent;

    [SerializeField] protected int limitObjsInPool = 50;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadHoldParent();
    }
    public virtual Transform Spawn(Transform prefab)
    {
        Transform newObject = Instantiate(prefab);
        return newObject;
    }

    public virtual T Spawn(T prefab)
    {
        T newObject = this.GetObjFromPool(prefab);
        if(newObject == null)
        {
            if(ReachLimitObjsInPool()) return newObject;
            newObject = Instantiate(prefab);
            this.UpdateName(prefab.transform, newObject.transform);
            this.spawnCount++;
        }
        newObject.transform.SetParent(this.holderParent);
        newObject.gameObject.SetActive(true);
        return newObject;
    }

    protected virtual void LoadHoldParent()
    {
        if (this.holderParent != null) return;
        this.holderParent = this.transform;
    }
    protected virtual bool ReachLimitObjsInPool() => this.holderParent.gameObject.transform.childCount >= limitObjsInPool;
    public virtual T Spawn(T prefab, Vector3 position)
    {
        T newObject = this.Spawn(prefab);
        if(newObject == null) return newObject;
        newObject.transform.position = position;
        return newObject;
    }

    public virtual void Despawn(Transform obj)
    {
        Destroy(obj.transform);
    }
    public virtual void Despawn(T obj)
    {
        if(obj is MonoBehaviour monoBehaviour)
        {
            monoBehaviour.gameObject.SetActive(false);
            this.AddObjectToPool(obj);
        }
    }
    protected virtual void AddObjectToPool(T obj)
    {
        this.inPoolObjs.Add(obj);
    }
    protected virtual void RemoveObjectFromPool(T obj)
    {
        this.inPoolObjs.Remove(obj);
    }
    protected virtual void UpdateName(Transform prefab,Transform newPrefab)
    {
        newPrefab.name = prefab.name/* + "_" + spawnCount*/;
    }
    protected virtual T GetObjFromPool(T prefab)
    {
        if(this.inPoolObjs.Count == 0) return null;
        foreach(T inPoolObj in this.inPoolObjs)
        {
            if(prefab.GetName() == inPoolObj.GetName())
            {
                this.RemoveObjectFromPool(inPoolObj);
                return inPoolObj;
            }
        }
        return null;
    }
}

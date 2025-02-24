using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingObjectList : Singleton<PoolingObjectList>
{
    [SerializeField] private GameObject obj;
    [SerializeField] private GameObject poolingList;
    [SerializeField] private float amount;
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        CreateNewPooling();
    }

    public GameObject GetPoolingObject()
    {
        GameObject _newObj;
        if (poolingList.transform.childCount > 0)
        {
            for (int i = 0; i < poolingList.transform.childCount; i++)
            {
                if (!poolingList.transform.GetChild(i).gameObject.activeInHierarchy)
                {
                    _newObj = poolingList.transform.GetChild(i).gameObject;
                    return _newObj;
                }
            }
        }
        return null;
    }
    private void CreateNewPooling()
    {
        for(int i = 0; i< amount; i++)
        {
            GameObject newObj = Instantiate(obj);
            newObj.transform.parent = poolingList.transform;
            newObj.SetActive(false);
        }
    }
}

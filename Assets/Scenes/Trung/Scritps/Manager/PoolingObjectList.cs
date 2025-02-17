using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingObjectList : Singleton<PoolingObjectList>
{
    [SerializeField] private GameObject obj;
    [SerializeField] private GameObject poolingList;
    [SerializeField] private float amount;

    [SerializeField] private List<GameObject> list;
    protected override void Start()
    {
        CreateNewPooling();
    }

    public GameObject GetPoolingObject()
    {
        GameObject _newObj;
        if (list.Count > 0)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (!list[i].activeInHierarchy)
                {
                    _newObj = list[i];
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
            list.Add(newObj);
        }
    }
}

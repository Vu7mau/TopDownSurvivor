using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingObjectList : MonoBehaviour
{
    [SerializeField] private GameObject obj;
    [SerializeField] private GameObject poolingList;
    [SerializeField] private float amount;
    public static PoolingObjectList instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    private void Start()
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
        else
        {
            GameObject e = Instantiate(obj);
            e.SetActive(false);
            e.transform.parent = poolingList.transform;
            return e;
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

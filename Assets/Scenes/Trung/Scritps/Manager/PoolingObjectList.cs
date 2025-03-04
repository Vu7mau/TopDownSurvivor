using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingObjectList : MonoBehaviour
{
    public static PoolingObjectList instance;
    [SerializeField] private Transform obj;
    [SerializeField] private float amount;
    [SerializeField] private float maxAmountPoolingObj;

    [SerializeField] private List<Transform> listPooling;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        
    }

    public GameObject GetPoolingObject()
    {
        if(listPooling.Count <= maxAmountPoolingObj)
        {
            GameObject _newObj;
            if (listPooling.Count > 0)
            {
                for (int i = 0; i < listPooling.Count; i++)
                {
                    if (!listPooling[i].gameObject.activeInHierarchy)
                    {
                        _newObj = listPooling[i].gameObject;
                        listPooling.Remove(listPooling[i]);
                        return _newObj;
                    }
                }
            }
            else
            {
                _newObj = Instantiate(obj.gameObject);
                _newObj.transform.parent = transform;
                return _newObj;
            }
        }
        return null;
    }
    //private void CreateNewPooling()
    //{
    //    for(int i = 0; i< amount; i++)
    //    {
    //        GameObject newObj = Instantiate(obj.gameObject);
    //        newObj.transform.parent = transform;
    //        listPooling.Add(newObj.transform);
    //        newObj.SetActive(false);
    //    }
    //}
    public void ReturnToPool(Transform obj)
    {
        obj.gameObject.SetActive(false);
        listPooling.Add(obj);
    }
}

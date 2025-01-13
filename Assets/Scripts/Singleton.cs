using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : VuMonoBehaviour where T : VuMonoBehaviour
{
    private static T _instance = null;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                if (FindObjectOfType<T>() != null)
                    _instance = FindObjectOfType<T>();
                else
                    new GameObject().AddComponent<T>().name = "Singleton_" + typeof(T).ToString();
            }
            return _instance;
        }
    }
    protected override void Awake()
    {
        if(_instance != null&&_instance.gameObject.GetInstanceID()!=this.gameObject.GetInstanceID())
        {
            Debug.LogError("Singleton already exist " + _instance.gameObject.name);
            Destroy(this.gameObject);
        }
        else
            _instance = this.GetComponent<T>();
    }
}

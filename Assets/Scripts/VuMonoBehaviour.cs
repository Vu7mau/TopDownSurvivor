using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VuMonoBehaviour : MonoBehaviour
{
   protected virtual void Awake()
    {
        this.LoadComponents();
    }

    protected virtual void OnEnable()
    {

    }
    protected virtual void Start()
    {

    }
    protected virtual void LoadComponents()
    {

    }
    protected virtual void Reset()
    {
        this.LoadComponents();
        this.ResetValue();
    }
    protected virtual void ResetValue()
    {
        //For override
    }
    protected virtual void OnDisable()
    {
        
    }

}

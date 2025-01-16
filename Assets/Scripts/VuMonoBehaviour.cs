using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VuMonoBehaviour : MonoBehaviour
{
   protected virtual void Awake()
    {
        this.LoadComponent();
    }

    protected virtual void OnEnable()
    {

    }
    protected virtual void Start()
    {

    }
    protected virtual void LoadComponent()
    {

    }
    protected virtual void Reset()
    {
        this.LoadComponent();
    }

    protected virtual void OnDisable()
    {
        
    }

}

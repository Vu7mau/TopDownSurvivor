using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectitleCtrl : VuMonoBehaviour
{
    [SerializeField] protected ToxicAbyss toxicAbyss;

    public ToxicAbyss ToxicAbyss => toxicAbyss;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadToxicAbyss();
    }
    protected virtual void LoadToxicAbyss()
    {
        if (this.toxicAbyss != null) return;
        this.toxicAbyss = GetComponent<ToxicAbyss>();
        Debug.Log(transform.name + "Load ToxicAbyss", gameObject);
    }
}

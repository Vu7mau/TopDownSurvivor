using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class ZombieCtrl : VuMonoBehaviour
{
    [SerializeField] protected AbyssToxicSpawner spawner;
    [SerializeField] protected ToxicAbyss toxicAbyss;
    [SerializeField] protected Transform position;
    [SerializeField] protected Transform endPoint;

    [SerializeField] protected Vector3 offSet;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadSpawner();
        this.LoadToxicAbyss();
        this.LoadEndPoint();
    }
    protected virtual void LoadSpawner()
    {
        if (this.spawner != null) return;
        this.spawner = FindAnyObjectByType<AbyssToxicSpawner>();
        Debug.Log(transform.name + ": LoadAbyssToxicSpawner");
    }
    protected virtual void LoadToxicAbyss()
    {
        if (this.toxicAbyss != null) return;
        this.toxicAbyss = GetComponentInChildren<ToxicAbyss>();
        Debug.Log(transform.name + ": LoadToxicAbyss");
    }
    protected virtual void LoadEndPoint()
    {
        if (endPoint != null) return;
        this.endPoint = FindAnyObjectByType<CharacterAnimHandle>().transform;
        Debug.Log(transform.name + ": Load EndPoint!", gameObject);
    }

    protected virtual void Shooting()
    {
        ToxicAbyss newToxicAbyss = this.spawner.Spawn(toxicAbyss,position.position);
        newToxicAbyss.GetComponent<ToxicAbyss>().ShootAt(endPoint.position + offSet);
    }
}

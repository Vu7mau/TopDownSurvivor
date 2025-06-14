using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Drawing;
using UnityEngine;

public class ZombieSuitManCtrl : ZombieCtrl
{
    [SerializeField] protected AbyssToxicSpawner spawner;
    [SerializeField] protected ToxicAbyss toxicAbyss;

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
        List<ToxicAbyss> allMyComponents = ComponentFinder.FindAllComponentsInScene<ToxicAbyss>();
        this.toxicAbyss = allMyComponents[0];
        Debug.Log(transform.name + ": LoadToxicAbyss");
    }
    protected virtual void LoadEndPoint()
    {
        if (this.endPoint != null) return;
        this.endPoint = FindAnyObjectByType<CharacterAnimHandle>().transform;
        Debug.Log(transform.name + ": Load EndPoint!", gameObject);
    }

    protected override void Shooting()
    {
        ToxicAbyss newToxicAbyss = this.spawner.Spawn(this.toxicAbyss,this.position.position);
        if (newToxicAbyss == null) return;
        newToxicAbyss.GetComponent<ToxicAbyss>().ShootAt(this.endPoint.position + offSet);
    }
}

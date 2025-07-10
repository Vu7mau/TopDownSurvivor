using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Drawing;
using UnityEngine;

public class ZombieSuitManCtrl : EShooting
{
    [SerializeField] protected ToxicAbyss toxicAbyss;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadToxicAbyss();
    }
    protected virtual void LoadToxicAbyss()
    {
        if (this.toxicAbyss != null) return;
        List<ToxicAbyss> allMyComponents = ComponentFinder.FindAllComponentsInScene<ToxicAbyss>();
        this.toxicAbyss = allMyComponents[0];
        Debug.Log(transform.name + ": LoadToxicAbyss");
    }

    protected virtual void Shoot()
    {
        this.Shooting(this.toxicAbyss, this.positionSpawn);
        
        //Transform holdParent = GameObject.Find("ProjectitleHolder").transform;
        //if (holdParent != null) this.spawner.SetHoldParent(holdParent);
        if (this.newProjectitle == null) return;
        this.newProjectitle.GetComponent<ToxicAbyss>().ShootAt(this.targetPosition.position);
    }
}

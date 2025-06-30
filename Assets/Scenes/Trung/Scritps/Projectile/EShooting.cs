using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EShooting : VuMonoBehaviour
{
    [SerializeField] protected ProjectitleSpawner projectitleSpawner;

    [SerializeField] protected Transform projectitleHolder;
    
    protected Projectitle projectitle;
    protected Transform positionSpawn;
    protected Transform targetPosition;

    protected Projectitle newProjectitle;
    public Projectitle NewProjectitle { get => newProjectitle; set => newProjectitle = value; }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadProjectitle();
        this.LoadProjectitleSpawner();
        this.LoadProjectitleHolder();
        this.LoadProjectitlePosition();
        this.LoadTargetPosition();
    }





    protected virtual void LoadProjectitle()
    {
        if (this.projectitle != null) return;
        this.projectitle = GetComponentInChildren<Projectitle>();
    }
    protected virtual void LoadProjectitleSpawner()
    {
        if (this.projectitleSpawner != null) return;
        this.projectitleSpawner = GetComponentInChildren<ProjectitleSpawner>();
    }
    protected virtual void LoadProjectitleHolder()
    {
        if (this.projectitleHolder != null) return;
        this.projectitleHolder = GameObject.Find("ProjectitleHolderSpawner").transform;
    }

    protected virtual void LoadProjectitlePosition()
    {
        if (this.positionSpawn != null) return;
        this.positionSpawn = this.transform;
    }
    protected virtual void LoadTargetPosition()
    {
        if (this.targetPosition != null) return;
        this.targetPosition = FindAnyObjectByType<CharacterAnimHandle>().transform;
    }



    public virtual void Shooting(Projectitle projectitlePrefab,Transform positionSpawn)
    {
        this.projectitle = projectitlePrefab;
        this.positionSpawn = positionSpawn;

        if (this.projectitleHolder == null) return;
        if (this.projectitle == null) return;
        if(this.positionSpawn == null) return;


        this.projectitleSpawner.SetHoldParent(this.projectitleHolder);
        this.newProjectitle = this.projectitleSpawner.Spawn(this.projectitle, this.positionSpawn.position);

        if (this.newProjectitle == null) return;
        if(this.targetPosition == null) return;
    }
    
}

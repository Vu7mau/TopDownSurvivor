using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePoliceCtrl : ZombieCtrl
{
    [SerializeField] protected BulletInvisbleSpawner bulletInvisibleSpawner;
    [SerializeField] protected BulletInvisible bulletInvisible;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadBulletInvisbleSpawner();
        this.LoadBulletInvisible();
        this.LoadEndPoint();
    }
    protected virtual void LoadBulletInvisbleSpawner()
    {
        if (this.bulletInvisibleSpawner != null) return;
        this.bulletInvisibleSpawner = FindAnyObjectByType<BulletInvisbleSpawner>();
        Debug.Log(transform.name + ": LoadAbyssToxicSpawner");
    }
    protected virtual void LoadBulletInvisible()
    {
        if (this.bulletInvisible != null) return;
        List<BulletInvisible> allMyComponents = ComponentFinder.FindAllComponentsInScene<BulletInvisible>();
        this.bulletInvisible = allMyComponents[0];
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
        BulletInvisible newBulletInvisible = this.bulletInvisibleSpawner.Spawn(this.bulletInvisible, this.position.position);
        if (newBulletInvisible == null) return;
        newBulletInvisible.GetComponent<BulletInvisible>().ShootAt(this.endPoint.position + offSet);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbyssProjectitle : Projectitle
{
    [SerializeField] protected Explosion explosion;
    [SerializeField] protected ExplosionSpawner explosionSpawner;

    [SerializeField] protected CircleWarning circleWarning;
    [SerializeField] protected CircleWarningSpawner circleWarningSpawner;

    [SerializeField] protected CharacterAnimHandle playerPosition;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadExplosionSpawner();
        this.LoadExplosion();
        this.LoadAbyssFollowTargetSpawner();
        this.LoadAbyssFollowTarget();
        this.LoadPlayerPosition();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        this.SpawnCircleWarning();
    }

    protected virtual void LoadExplosion()
    {
        if (this.explosion != null) return;
        List<Explosion> allMyComponents = ComponentFinder.FindAllComponentsInScene<Explosion>();
        foreach (var myComponent in allMyComponents)
        {
            if (myComponent.GetComponent<Explosion>().Name == "Explode3")
            {
                this.explosion = myComponent;
                break;
            }
        }
    }
    protected virtual void LoadExplosionSpawner()
    {
        if (this.explosionSpawner != null) return;
        this.explosionSpawner = FindAnyObjectByType<ExplosionSpawner>();
    }
    protected virtual void LoadPlayerPosition()
    {
        if (this.playerPosition != null) return;
        this.playerPosition = FindAnyObjectByType<CharacterAnimHandle>();
    }

    protected virtual void LoadAbyssFollowTargetSpawner()
    {
        if (this.circleWarningSpawner != null) return;
        this.circleWarningSpawner = FindAnyObjectByType<CircleWarningSpawner>();
        Debug.Log(transform.name + ": LoadAbyssToxicSpawner");
    }
    protected virtual void LoadAbyssFollowTarget()
    {
        if (this.circleWarning != null) return;
        List<CircleWarning> allMyComponents = ComponentFinder.FindAllComponentsInScene<CircleWarning>();
        this.circleWarning = allMyComponents[0];
        Debug.Log(transform.name + ": LoadToxicAbyss");
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!other.transform.CompareTag("BulletEnemy") && !other.transform.CompareTag("Enemy") && !other.transform.CompareTag("bullet"))
        {
            Explosion newExplosion = this.explosionSpawner.Spawn(this.explosion,transform.position);
            newExplosion.transform.localScale = new Vector3(2f, 2f, 2f);
            this.Despawn.DoDespawn();
        }

    }
    protected virtual void SpawnCircleWarning()
    {
        CircleWarning circleWarning = this.circleWarningSpawner.Spawn(this.circleWarning,this.playerPosition.transform.position);
        circleWarning.transform.position = this.playerPosition.transform.position + new Vector3(0,-2f,0);
        circleWarning.transform.localScale = new Vector3(2f,2f,2f);
    }
}

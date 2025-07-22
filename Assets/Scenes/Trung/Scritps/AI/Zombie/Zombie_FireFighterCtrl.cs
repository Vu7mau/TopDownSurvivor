using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_FireFighterCtrl : VuMonoBehaviour
{


    [SerializeField] protected Explosion explosionPrefab;
    [SerializeField] protected EffectFXSpawner explosionSpawner;

    [SerializeField] protected Transform positionExplosion;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadExplosion();
        this.LoadExplosionSpawner();
    }
    protected virtual void LoadExplosion()
    {
        if (this.explosionPrefab != null) return;
        //List<Explosion> allMyComponents = ComponentFinder.FindAllComponentsInScene<Explosion>();
        //foreach (var myComponent in allMyComponents)
        //{
        //    if(myComponent.GetComponent<Explosion>().GetName() == "Explode1")
        //    {
        //        this.explosionPrefab = myComponent;
        //        break;
        //    }
        //}
    }
    protected virtual void LoadExplosionSpawner()
    {
        if (this.explosionSpawner != null) return;
        this.explosionSpawner = GetComponentInChildren<EffectFXSpawner>();
        if(this.explosionSpawner == null) return;
    }
    protected virtual void Explode()
    {
        if(this.explosionSpawner == null) return;
        if (this.explosionPrefab == null) return;
        if (this.positionExplosion == null)
        {
            Debug.LogWarning("Chưa có vị trí sinh ra vụ nổ kìa ba!");
            return;
        }
        EffectFX newExplosion = this.explosionSpawner.Spawn(this.explosionPrefab, this.positionExplosion.position);
        if (newExplosion == null) return;
    }
}

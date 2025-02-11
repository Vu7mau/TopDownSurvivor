using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSender : VuMonoBehaviour
{
    [SerializeField] protected int _damage = 1;

    // Update is called once per frame
   
    public virtual void Send(Transform obj)
    {
        DamageReceiver damageReceiver = obj.GetComponentInChildren<DamageReceiver>();
        if (damageReceiver == null ) return;

        this.Send(damageReceiver);
       
    }
    protected virtual void CreateImpactVFX()
    {
      //  string vfxName = this.GetVFXName();
        Vector3 hitPos = transform.position;

        Quaternion rota = transform.rotation;
      //  Transform vfx = VFXSpawner.Instance.Spawn(vfxName, hitPos, rota);
        //  Vector2 dir = Vector3.Normalize(transform.parent.position- vfx.position);
        //  float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        //  Quaternion rotate = Quaternion.Euler(0, 0, angle);
        //vfx.rotation = rotate;
        //vfx.gameObject.SetActive(true);

    }
    //protected virtual string GetVFXName()
    //{
    //    return VFXSpawner.vfx_Impact;
    //}
    public virtual void Send(DamageReceiver damageReceiver)
    {
        damageReceiver.Deduct(_damage);
      //  this.CreateImpactVFX();
        //this.DestroyObject();
    }
    protected virtual void DestroyObject()
    {
        Destroy(transform.parent.gameObject);
    }
}

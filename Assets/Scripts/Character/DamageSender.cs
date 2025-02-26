using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSender : VuMonoBehaviour
{
    [Space]
    [Header("DamageSender")]
    [SerializeField] protected int _basedDamage = 1;

 
    public virtual void Send(Transform obj)
    {

        DamageReceiver damageReceiver = obj.GetComponentInChildren<DamageReceiver>();

        if (damageReceiver == null ) return;
        Debug.Log("Truyen damage!");
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
        damageReceiver.Deduct(this.SetDamage());
      //  this.CreateImpactVFX();
        //this.DestroyObject();
    }
    protected virtual void DestroyObject()
    {
        Destroy(transform.parent.gameObject);
    }
    protected virtual int SetDamage()
    {
      return _basedDamage;
    }
   
}

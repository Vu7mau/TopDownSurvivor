using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFly : BulletAbstract
{
    [Space]
    [Header("Bullet Fly")]
    [SerializeField] protected Rigidbody _rb;
    [SerializeField] protected float _speed= 10f;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadRigidbody();
    }
    protected virtual void LoadRigidbody()
    {
        if (this._rb != null) return;
        this._rb=this.transform.parent.GetComponent<Rigidbody>();
        Debug.Log("Load Rigidbody Success " + this._rb.transform.name);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        
        this.transform.parent.rotation=Quaternion.Euler(0,this.transform.parent.forward.y,0);
        this._rb.velocity=BulletDirection()*_speed;
    }
    protected virtual Vector3 BulletDirection()
    {
      Vector3 direction= (this.AimPos().position-this.GunPos().position).normalized;

        if (_bulletCtrl.CharacterCtrl.CharacterAim.CanAimPrecisly() == false &&
          _bulletCtrl.CharacterCtrl.CharacterAim.GetTarget() == null)
        {
            direction.y = 0;
        }

        return direction;

    }
   
    protected Transform AimPos() => this._bulletCtrl.CharacterCtrl.CharacterAim.GetAim();
    protected Transform GunPos() => this._bulletCtrl.CharacterCtrl.CharacterShooting.GetGunPoint();
}

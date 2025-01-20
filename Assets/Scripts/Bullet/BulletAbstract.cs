using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAbstract : VuMonoBehaviour
{
    [Header("Bullet Abtract")]
    [SerializeField] protected BulletCtrl _bulletCtrl;
    public BulletCtrl BulletCtrl => _bulletCtrl;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadBulletCtrl();
    }

    protected virtual void LoadBulletCtrl()
    {
        if (this._bulletCtrl != null) return;
        this._bulletCtrl = transform.parent.GetComponent<BulletCtrl>();
        Debug.Log(transform.name + ": LoadBulletCtrl", gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : VuMonoBehaviour
{
    [Header("Bullet Ctrl")]
    [SerializeField] protected BulletDespawn _bulletDespawn;
    public BulletDespawn BulletDespawn=> _bulletDespawn;

    [SerializeField] protected CharacterCtrl _characterCtrl;
    public CharacterCtrl CharacterCtrl => _characterCtrl;
    [SerializeField] protected BulletImpact _bulletImpact;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadBulletDespawn();
        this.LoadCharacterCtrl();
        this.LoadBulletImpact();
    }
    protected virtual void LoadBulletDespawn()
    {
        if (this._bulletDespawn != null) return;

        this._bulletDespawn=this.transform.GetComponentInChildren<BulletDespawn>();
        Debug.Log("Load BulletDespawn Success "+this._bulletDespawn.transform.name);
    }
    protected virtual void LoadCharacterCtrl()
    {
        if(this._characterCtrl != null) return;

        this._characterCtrl=GameObject.FindObjectOfType<CharacterCtrl>();
        Debug.Log("Load CharacterCtrl Success " + this._characterCtrl.transform.name);


    }
    protected virtual void LoadBulletImpact()
    {
        if(this._bulletImpact != null) return;

        this._bulletImpact = GetComponentInChildren<BulletImpact>();
        Debug.Log("Load CharacterCtrl Success " + this._bulletImpact.transform.name);


    }
    private Rigidbody rb => GetComponent<Rigidbody>();
    private void OnCollisionEnter(Collision collision)
    {
        _bulletImpact.CreateParticleFX(collision);
        rb.velocity=Vector3.zero;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : VuMonoBehaviour
{
    [Header("Bullet Ctrl")]
    [SerializeField] protected BulletDespawn _bulletDespawn;
    public BulletDespawn BulletDespawn=> _bulletDespawn;

    public CharacterCtrl CharacterCtrl => _characterCtrl;
    [SerializeField] protected CharacterCtrl _characterCtrl;

    [SerializeField] protected BulletImpact _bulletImpact;

    [SerializeField] protected DamageSender _damageSender;
    public DamageSender DamageSender => _damageSender;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadBulletDespawn();
        this.LoadCharacterCtrl();
        this.LoadBulletImpact();
        this.LoadDamageSender();
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
    protected virtual void LoadDamageSender()
    {
        if(this._damageSender != null) return;

        this._damageSender = GetComponentInChildren<DamageSender>();
        Debug.Log("Load CharacterCtrl Success " + this._damageSender.transform.name);


    }
    private Rigidbody rb => GetComponent<Rigidbody>();
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("bullet")|| collision.transform.CompareTag("Player"))
            return;
        _bulletImpact.CreateParticleFX(collision);
        rb.velocity=Vector3.zero;
        _damageSender.Send(collision.transform);

    }
}

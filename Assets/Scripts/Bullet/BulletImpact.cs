using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(CapsuleCollider))]

public class BulletImpact : BulletAbstract
{
    [Header("Bullet Impart")]
    [SerializeField] protected SphereCollider _capsuleCollider;
    //[SerializeField] protected Rigidbody _rigidbody;

    [SerializeField] protected ParticleSystem _hitEffect;
    [SerializeField] private bool _isActive = false;
    protected override void OnDisable()
    {
        base.OnDisable();
        _isActive = false;
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        //this.LoadCollider();

    }

    protected virtual void LoadCollider()
    {
        if (this._capsuleCollider != null) return;
        this._capsuleCollider = GetComponent<SphereCollider>();
        //this._capsuleCollider.isTrigger = true;
        //this._capsuleCollider.radius = 0.05f;
        Debug.Log(transform.name + ": LoadCollider", gameObject);
    }

    public void CreateParticleFX(Collision collision)
    {
        if (this._isActive) return;
        _isActive = true;
        this.SetEffectPos(collision);
        this.ResetPlaybackTime();
        this.HandleVisualEffect();
        this.HandleSoundEffect();
        Invoke(nameof(Despawn), .2f);
        //this._bulletCtrl.BulletDespawn.DespawnObject();
        // Debug.Log("Do her");
    }
    protected virtual void Despawn()
    {
        _bulletCtrl.BulletDespawn.DespawnObject();
    }
    protected virtual void SetEffectPos(Collision collision)
    {
        _hitEffect.transform.position = collision.contacts[0].point;
        _hitEffect.transform.forward = collision.contacts[0].normal;
    }
    protected virtual void HandleVisualEffect()
    {
        _hitEffect.Play();
    }
    protected virtual void ResetPlaybackTime()
    {
        if (_hitEffect != null)
        {
            _hitEffect.Simulate(0, true, true);
        }
    }
    protected virtual void HandleSoundEffect()
    {

    }
}

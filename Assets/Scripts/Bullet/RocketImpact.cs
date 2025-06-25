using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketImpact : BulletImpact
{
    [Space]
    [Header("RocketImpact")]
    [SerializeField] private ParticleSystem _explosionEffect;
    [SerializeField] private float _explosionColliderSize;


    protected override void SetEffectPos(Collision collision)
    {
        base.SetEffectPos(collision);
        _explosionEffect.transform.position = collision.contacts[0].point;
        _explosionEffect.transform.forward = collision.contacts[0].normal;
    }
    protected override void HandleVisualEffect()
    {
        base.HandleVisualEffect();
       _explosionEffect.Play();
        _SphereCollider.radius = _explosionColliderSize;
    }
    protected override void ResetPlaybackTime()
    {
        base.ResetPlaybackTime();
        if (_explosionEffect != null)
        {
            _explosionEffect.Simulate(0, true, true);
        }

    }
    protected override void OnDisable()
    {
        base.OnDisable();
        _SphereCollider.radius = .04f;

    }
    protected override void HandleSoundEffect()
    {
        SoundFXManager.Instance.PlaySoundFXClip(SoundFXManager.Instance.rocketExplosion,this.transform);
        Debug.Log("Im here");
    }
}

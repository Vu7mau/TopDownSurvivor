using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbyssFollowTarget : PoolObj
{
    [SerializeField] protected Transform targetPosition;
    [SerializeField] protected float speed = 5f;

    [SerializeField] protected float energy = 100f;

    [SerializeField] protected CreateHitEnemy hit;

    public override string GetName() => "AbyssFollowTarget";

    protected override void OnEnable()
    {
        base.OnEnable();
        this.LoadHit();
    }
    protected virtual void LoadHit()
    {
        hit.CanTakeDamage = false;
        this.energy = 100f;
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadTargetPosition();
    }
    protected virtual void LoadTargetPosition()
    {
        if (this.targetPosition != null) return;
        this.targetPosition = FindAnyObjectByType<CharacterAnimHandle>().transform;
    }
    protected void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position,targetPosition.position,speed * Time.deltaTime);
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (!other.transform.CompareTag("BulletEnemy") && !other.transform.CompareTag("Enemy"))
        {
            this.Despawn.DoDespawn();
        }
        if (other.transform.CompareTag("bullet"))
        {
            this.energy -= 10f;
            CharacterEvents.characterDamaged?.Invoke(this.gameObject, 10f);
            Debug.Log("Energy còn: "+ this.energy);
            if(this.energy <= 0) { this.Despawn.DoDespawn(); }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbyssFollowTarget : PoolObj
{
    [SerializeField] protected Transform targetPosition;
    [SerializeField] protected float speed = 5f;

    [SerializeField] protected float energy = 500f;

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
        //if (!other.transform.gameObject.CompareTag("BulletEnemy") && !other.transform.gameObject.CompareTag("Enemy") && !other.gameObject.CompareTag("bullet"))
        //{
            
        //}
        if (other.transform.CompareTag("Player"))
        {
            this.hit.TriggerEnter(other);
            this.Despawn.DoDespawn();
        }
        //if (other.transform.gameObject.CompareTag("bullet"))
        //{
        //    this.energy -= 10f;
        //    CharacterEvents.characterDamaged?.Invoke(this.gameObject, 10f);
        //    Debug.Log("Energy còn: "+ this.energy);
        //    if(this.energy <= 0) { this.Despawn.DoDespawn(); }
        //}
    }
}

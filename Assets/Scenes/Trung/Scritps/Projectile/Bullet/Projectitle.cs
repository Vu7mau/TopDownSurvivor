using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Projectitle : PoolObj
{

    [SerializeField] protected float speed = 10f;
    public float Speed { get =>  speed; set => speed = value; }

    [SerializeField] protected Vector3 offSet = new Vector3(0, 0.947f,0);


    [SerializeField] protected CreateHitEnemy createHitEnemy;


    protected Vector3 _direction;
    public Vector3 Direction
    {
        get { return _direction; }
        set { _direction = value; }
    }

    [SerializeField] protected bool canDespawnByPlayer = true;
    
    protected override void OnEnable()
    {
        base.OnEnable();
        this.ResetProjectile();
    }


    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadCreateHitEnemy();
    }



    protected virtual void LoadCreateHitEnemy()
    {
        if (this.createHitEnemy != null) return;
        this.createHitEnemy = GetComponentInChildren<CreateHitEnemy>();
    }


    protected virtual void ResetProjectile()
    {
        if (this.createHitEnemy == null) return;
        this.createHitEnemy.CanTakeDamage = false;
    }






    public virtual void ShootAt(Vector3 target)
    {
        _direction = (target + this.offSet - transform.position).normalized;
    }
    public virtual void ShootAt(Vector3 target, float newSpeed)
    {
        this.speed = newSpeed;
        _direction = (target - transform.position).normalized;
    }
    public virtual void SetDirection(Vector3 direction)
    {
        this._direction = direction.normalized;
    }
    public virtual void SetVelocity(float newSpeed)
    {
        this.speed = newSpeed;
    }
    public virtual void SetDirection(Vector3 direction, float speed)
    {
        this._direction = direction.normalized;
        this.speed = speed;
    }




    protected void Update()
    {
        transform.position += _direction * speed * Time.deltaTime;
    }






    protected void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("Player") && this.createHitEnemy.CanTakeDamage)
        {
            this.createHitEnemy.CanTakeDamage = false;
            if (this.canDespawnByPlayer) this.Despawn.DoDespawn();
            this.DestroyProjectitleByPlayer();

        }
        if (!other.transform.CompareTag("Enemy") && !other.transform.CompareTag("BulletEnemy") && !other.transform.CompareTag("bullet") && !other.transform.CompareTag("Player"))
        {
            this.Despawn.DoDespawn();
            this.DestroyProjectitle();
        }
    }
    protected virtual void DestroyProjectitle()
    {
    }
    protected virtual void DestroyProjectitleByPlayer()
    {
    }
}

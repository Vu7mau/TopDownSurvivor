using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Projectitle : PoolObj
{
    [SerializeField] protected float speed = 10f;

    [SerializeField] protected CreateHitEnemy createHitEnemy;
    protected Vector3 _direction;
    public Vector3 Direction
    {
        get { return _direction; }
        set { _direction = value; }
    }
    public override string GetName()
    {
        return "Projectitle";
    }
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
        this.createHitEnemy.CanTakeDamage = false;
    }

    public virtual void ShootAt(Vector3 target)
    {
        _direction = (target - transform.position).normalized;
    }
    public virtual void ShootAt(Vector3 target, float newSpeed)
    {
        this.speed = newSpeed;
        _direction = (target - transform.position).normalized;
    }
    public virtual void SetDirection(Vector3 direction)
    {
        this._direction = direction;
    }
    public virtual void SetDirection(Vector3 direction, float speed)
    {
        this._direction = direction;
        this.speed = speed;
    }

    protected void Update()
    {
        transform.position += _direction * speed * Time.deltaTime;
    }
}

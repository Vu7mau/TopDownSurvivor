using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellFly : VuMonoBehaviour
{
    [Space]
    [Header("Shell Fly")]
    [SerializeField] protected Rigidbody _rb;
    [SerializeField] protected float _speed = 10f;
    [SerializeField] private float spreadAngle = 10;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadRigidbody();
    }
    protected virtual void LoadRigidbody()
    {
        if (this._rb != null) return;
        this._rb = this.transform.parent.GetComponent<Rigidbody>();
        Debug.Log("Load Rigidbody Success " + this._rb.transform.name);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        //  this.transform.parent.rotation = Quaternion.Euler(0, this.transform.parent.forward.y, 0);
        this._rb.velocity = BulletDirection() * _speed;
    }

    protected virtual Vector3 BulletDirection()
    {
        Vector3 direction = this.transform.position.normalized;


        Quaternion randomRotation = Quaternion.AngleAxis(
   Random.Range(-spreadAngle, spreadAngle),
   Random.onUnitSphere);


        return (randomRotation * direction).normalized;
    }
   
}

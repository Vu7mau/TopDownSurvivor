using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLookAtTarget : VuMonoBehaviour
{
    [Header("Object Look At Target")]
    [SerializeField] protected Vector3 _targetPosition;
    [SerializeField] protected float _rotSpeed=3;

    protected virtual void FixedUpdate()
    {
        this.LookAt();
    }

    public virtual void SetRotSpeed(float rotSpeed)
    {
        this._rotSpeed = rotSpeed;
    }
    protected virtual void LookAt()
    {
        Vector3 diff = this._targetPosition-transform.parent.position;
        diff.Normalize();
        float rot_Y= Mathf.Atan2(diff.y, diff.x)*Mathf.Rad2Deg;

        float timeSpeed= this._rotSpeed*Time.fixedDeltaTime;
        Quaternion targetEuler=Quaternion.Euler(0,rot_Y,0);
        Quaternion currentEuler =Quaternion.Lerp(transform.parent.rotation, targetEuler, timeSpeed);

        transform.parent.rotation = currentEuler;
    }
}

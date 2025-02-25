using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : VuMonoBehaviour
{
    [SerializeField] protected float _speed = 2f;
    [SerializeField] protected Transform _target;


    protected virtual void FixedUpdate()
    {
    
        this.Following();
    }
    protected virtual void Following()
    {
        if (this._target == null) return;

        transform.position = Vector3.Lerp(transform.position,this. _target.transform. position, _speed*Time.fixedDeltaTime);
    }
}

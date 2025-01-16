using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : VuMonoBehaviour
{
    [Header("Object Movement")]
    [SerializeField] protected Rigidbody _rb;
    [SerializeField] protected Vector3 _targetPosition;
    [SerializeField] protected bool _isMoving;
    [SerializeField] protected float _speed = 3f;
    [SerializeField] protected float _distance = 1f;
    [SerializeField] protected float _minDistance = 1f;

    protected virtual void FixedUpdate()
    {
        this.Moving();
    }
    protected virtual void Moving()
    {
        //this._distance = Vector3.Distance(this.transform.position, this._targetPosition);
        //if (_distance > _minDistance) return;

        //float lerpValue = Mathf.Clamp(Time.fixedDeltaTime * _speed, 0f, 1f);
        //Vector3 newPos = Vector3.Lerp(this.transform.position, this._targetPosition, lerpValue);
        //this.transform.parent.position = newPos;
        // this.transform.parent.Translate(_targetPosition * _speed * Time.fixedDeltaTime);

        _rb.MovePosition(_rb.position+_targetPosition * _speed * Time.fixedDeltaTime);
    }
    public virtual void SetMoveSpeed(float speed)
    {
        this._speed = speed;
    }

  
}

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
        if (_targetPosition != Vector3.zero)
            _rb.MovePosition(_rb.position + _targetPosition * _speed * Time.fixedDeltaTime);
        else
            _rb.velocity = Vector3.zero;
        //  _rb.transform.position = new Vector3(_rb.position.x + _targetPosition.x * _speed * Time.fixedDeltaTime, 0, _rb.position.z + _targetPosition.z * _speed * Time.fixedDeltaTime);
    }
    public virtual void SetMoveSpeed(float speed)
    {
        this._speed = speed;
    }
    public virtual void DeductMoveSpped(float deduct)
    {
        this._speed -= deduct;
    }

}

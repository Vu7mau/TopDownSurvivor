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

    [SerializeField] protected float _baseSpeed = 0;
    protected bool _isShooting = false;

    // flag để chỉ set _baseSpeed một lần
    private bool _baseSpeedInitialized = false;

    protected override void Start()
    {
        // bạn có thể để trống hoặc init khác
    }

    protected override void OnEnable()
    {
        Debug.Log("Start");

        if (!_baseSpeedInitialized)
        {
            _baseSpeed = _speed;          // chỉ gán 1 lần duy nhất
            _baseSpeedInitialized = true;
        }
    }

    protected virtual void FixedUpdate()
    {
        this.Moving();
    }

    protected virtual void Moving()
    {
        Vector3 movement = new Vector3(
            _targetPosition.x * _speed * Time.fixedDeltaTime,
            0,
            _targetPosition.z * _speed * Time.fixedDeltaTime
        );

        if (_targetPosition != Vector3.zero)
        {
            _rb.MovePosition(_rb.position + movement);
        }
        else
        {
            _rb.velocity = new Vector3(0, _rb.velocity.y, 0);
        }
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

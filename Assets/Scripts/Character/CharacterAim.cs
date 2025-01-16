using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CharacterAim : CharacterCtrlAbstract
{
    [Header("Aim visual")]
    [SerializeField] protected LineRenderer _aimLaser;

    [Header("Aim Info")]
    [SerializeField] protected Transform _aim;

    [SerializeField] private bool isAimingPrecisly;
    [SerializeField] private bool isLockingToTarget;

    [Header("Camera Control")]
    [SerializeField] protected Transform _cameraTarget;

    [Range(.5f, 1f)]
    [SerializeField] protected float _minCameraDistance = 1.5f;

    [Range(1f, 3f)]
    [SerializeField] protected float _maxCameraDistance = 4f;

    [Range(3f, 5f)]
    [SerializeField] protected float _cameraSensitivity = 5f;


    protected virtual void Update()
    {
       this.UpdateAimVisual();
        this.UpdateCameraPosition();
        this.UpdateAimPosition();
    }

    protected virtual void UpdateAimVisual()
    {
        Transform gun =this.transform;
        Vector3 laserDirection= this.transform.forward;

        float laserTipLength = .5f;
        float gunDistance = 4f;

        Vector3 endPoint = gun.position + laserDirection * gunDistance;

        if(Physics.Raycast(gun.position,laserDirection,out RaycastHit hit,gunDistance))
        {
            endPoint = hit.point;
            laserTipLength = 0;
        }

        _aimLaser.SetPosition(0,gun.position);
        _aimLaser.SetPosition(1,endPoint);
       _aimLaser.SetPosition(2,endPoint+laserDirection*laserTipLength);
    }
    protected void UpdateAimPosition()
    {
        Transform target = GetTarget();

        if (target != null && isLockingToTarget)
        {
            if (target.GetComponent<Renderer>() != null)
            {
                _aim.position = target.GetComponent<Renderer>().bounds.center;
            }
            else
            {
                _aim.position = target.position;
            }
        }

        _aim.position = _characterCtrl.InputManager.GetMouseHitInfo().point;

        if (!isAimingPrecisly)
        {
            _aim.position = new Vector3(_aim.position.x, transform.position.y + 1, _aim.position.z);
        }
    }


    public Transform GetAim() => _aim;
    public Transform GetTarget()
    {
        Transform target = null;

        if (_characterCtrl.InputManager.GetMouseHitInfo().transform.GetComponent<Target>() != null)
        {
            target = _characterCtrl.InputManager.GetMouseHitInfo().transform;
        }

        return target;
    }

    private void UpdateCameraPosition()
    {
        _cameraTarget.position = Vector3.Lerp(
                    _cameraTarget.position,
                    DesiredCameraPosition(),
                    _cameraSensitivity * Time.deltaTime);
    }
    private Vector3 DesiredCameraPosition()
    {
        float actualMaxCameraDistance = _characterCtrl.InputManager.MoveInput.y < -.5f ? _minCameraDistance : _maxCameraDistance;

        Vector3 desiredCameraPosition = _characterCtrl.InputManager.GetMouseHitInfo().point;
        Vector3 aimDirection = (desiredCameraPosition - transform.position).normalized;

        float distanceToDesiredPosition = Vector3.Distance(transform.position, desiredCameraPosition);
        float clampedDistance = Mathf.Clamp(distanceToDesiredPosition, _minCameraDistance, actualMaxCameraDistance);

        desiredCameraPosition = transform.position + aimDirection * clampedDistance;
        desiredCameraPosition.y = transform.position.y + 1;

        return desiredCameraPosition;
    }
}

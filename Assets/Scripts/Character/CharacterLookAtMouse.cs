using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLookAtMouse : CharacterCtrlAbstract
{
    [Space]
    [Header("Character Look At Mouse")]
    [SerializeField] protected float turnSpeed = 1.0f;


   
    protected virtual void FixedUpdate()
    {
          this.LookAtTarget();
    }
    protected virtual void LookAtTarget()
    {
        Vector3 lookAtPosition = _characterCtrl.InputManager.GetMouseHitInfo().point;
        //this.transform.parent.LookAt(lookAtPosition);

        Vector3 lookingDirection = lookAtPosition - transform.parent.position;
        lookingDirection.y = 0f;
        lookingDirection.Normalize();

        Quaternion desiredRotation = Quaternion.LookRotation(lookingDirection);
       this.transform.parent.rotation = Quaternion.Slerp(this.transform.parent.rotation, desiredRotation, turnSpeed * Time.deltaTime);
      

    }

}

    //Vector3 ObjPos = Camera.main.WorldToScreenPoint(transform.position); //Map the obj onto the Screen
    //Vector3 dir = Input.mousePosition - ObjPos; //Find the Direction
    //float rotZ = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg; //Turn the direction into an angle

    //this.transform.parent.rotation = Quaternion.Euler(0, rotZ, 0); //Rotate
    //protected virtual void LookAtTarget()
    //{
    //    Vector3 diff = _characterCtrl.InputManager.MousePosition - transform.parent.position;
    //    diff.Normalize();
    //    float rot_Y = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
    //    transform.parent.rotation = Quaternion.Euler(0f, rot_Y, 0f );
    //}

    //protected virtual void GetTarget(Vector3 target)
    //{
    //    this._targetPosition = target;
    //}
    //protected virtual Vector2 GetJoyInput()
    //{
    //    float hor = _characterJoystickCtrl.RotationJoystick.Horizontal;
    //    float ver = _characterJoystickCtrl.RotationJoystick.Vertical;
    //    return new Vector2(hor, ver);
    //}
    //protected virtual Vector3 ConvertInput(Vector2 input)
    //{
    //    Vector2 converted = this._characterCtrl.ConvertWithCamera(Camera.main.transform.position, input.x, input.y);
    //    Vector3 direction = new Vector3(converted.x, 0, converted.y).normalized;
    //    return direction;
    //}
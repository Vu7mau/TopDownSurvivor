using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterJoystickCtrl : VuMonoBehaviour
{
    [Space]
    [Header("Joystick Move")]
    [SerializeField] protected FixedJoystick _moveJoystick;
    public FixedJoystick MoveJoyStick=>_moveJoystick;
    [Header("Joystick Move")]
    [SerializeField] protected FixedJoystick _rotationJoystick;
    public FixedJoystick RotationJoystick=>_rotationJoystick;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadMoveJoystick();
        this.LoadRotaJoystick();
      //  this.LoadRigidbody();
    }
    protected virtual void LoadMoveJoystick()
    {
        if (this._moveJoystick != null) return;

        _moveJoystick = GameObject.Find("MoveJoystick").GetComponent<FixedJoystick>();
        Debug.Log("Load MoveJoystick Success at " + this.transform.name);
    } 
  protected virtual void LoadRotaJoystick()
    {
        if (this._rotationJoystick != null) return;

        _rotationJoystick = GameObject.Find("RotationJoystick").GetComponent<FixedJoystick>();
        Debug.Log("Load RotationJoystick Success at " + this.transform.name);
    } 
 
    //protected override void FixedUpdate()
    //{
    //    base.FixedUpdate();
    //    float hor = _moveJoystick.Horizontal;
    //    float ver = _moveJoystick.Vertical;
    //    Vector2 converted=this.ConvertWithCamera(Camera.main.transform.position, hor, ver);
    //    Vector3 direction = new Vector3(converted.x, 0, converted.y).normalized;
    //    this.GetTarget(direction);
        
    //}
    //protected virtual void GetTarget(Vector3 target)
    //{
    //    this._targetPosition = target;
    //}
    public virtual Vector2 ConvertWithCamera(Vector3 cameraPos,float hor,float ver)
    {
        Vector2 joyDirection = new Vector2(hor, ver).normalized;
        Vector2 camera2DPos= new Vector2(cameraPos.x, cameraPos.z);
        Vector2 characterPos= new Vector2(this.transform.position.x, this.transform.position.z);
        Vector2 camToCharacterDirection= (Vector2.zero-camera2DPos).normalized;
        float angle = Vector2.SignedAngle(camToCharacterDirection, new Vector2(0, 1));
        Vector2 finalDirection = this.RotateVector(joyDirection,-angle);
        return finalDirection;
    }
    protected virtual Vector2 RotateVector(Vector2 v, float angle)
    {
        float radian=angle*Mathf.Rad2Deg;
        float _x= v.x*Mathf.Cos(radian)-v.y*Mathf.Sin(radian);
        float _y= v.x*Mathf.Sin(radian)+ v.y*Mathf.Cos(radian);
        return new Vector2(_x, _y);

    }
}

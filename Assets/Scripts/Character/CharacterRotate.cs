using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRotate : VuMonoBehaviour
{
    [Header("CharacterJoystickCtrl")]
    [SerializeField] protected CharacterJoystickCtrl _characterJoystickCtrl;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadCharacterJoystickCtrl();
      
    }
    protected virtual void LoadCharacterJoystickCtrl()
    {
        if (this._characterJoystickCtrl != null) return;
        _characterJoystickCtrl = this.transform.parent.GetComponent<CharacterJoystickCtrl>();
        Debug.Log("Load CharacterJoystickCtrl Success at " + this.transform.name);
    }
    protected virtual void FixedUpdate()
    {
        this.LookJoystick();
    }
    protected virtual void LookJoystick()
    {
        Vector3 lookAtPosition = this.ConvertInput(this.GetJoyInput())+this.transform.parent.position;
        this.transform.parent.LookAt(lookAtPosition);
    }
    //protected override void FixedUpdate()
    //{
    //    base.FixedUpdate();
    //    //this.GetJoyInput();
    //    this.GetTarget(this.GetConvertInput(this.GetJoyInput())+transform.position);
    //}

    //protected virtual void GetTarget(Vector3 target)
    //{
    //    this._targetPosition = target;
    //}
    protected virtual Vector2 GetJoyInput()
    {
        float hor = _characterJoystickCtrl.RotationJoystick.Horizontal;
        float ver = _characterJoystickCtrl.RotationJoystick.Vertical;
        return new Vector2(hor, ver);
    }
    protected virtual Vector3 ConvertInput(Vector2 input)
    {
        Vector2 converted = this._characterJoystickCtrl.ConvertWithCamera(Camera.main.transform.position, input.x, input.y);
        Vector3 direction = new Vector3(converted.x, 0, converted.y).normalized;
        return direction;
    }
}

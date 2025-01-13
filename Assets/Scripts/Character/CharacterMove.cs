using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : ObjectMovement
{
    [Header("CharacterJoystickCtrl")]
    [SerializeField] protected CharacterJoystickCtrl _characterJoystickCtrl;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadCharacterJoystickCtrl();
        this.LoadRigidbody();
    }
    protected virtual void LoadCharacterJoystickCtrl()
    {
        if (this._characterJoystickCtrl != null) return;
        _characterJoystickCtrl = this.transform.parent.GetComponent<CharacterJoystickCtrl>();
        Debug.Log("Load CharacterJoystickCtrl Success at " + this.transform.name);
    }
    protected virtual void LoadRigidbody()
    {
        if (this._rb != null) return;

        this._rb = transform.parent.GetComponent<Rigidbody>();
        Debug.Log("Load LoadRigidbody Success at " + this.transform.name);
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
       //this.GetJoyInput();
       this.GetTarget(this.GetConvertInput(this.GetJoyInput()));
    }
    protected virtual void GetTarget(Vector3 target)
    {
        this._targetPosition = target;
    }
    protected virtual Vector2 GetJoyInput()
    {
       float hor=_characterJoystickCtrl.MoveJoyStick.Horizontal;
       float  ver=_characterJoystickCtrl.MoveJoyStick.Vertical;
        return new Vector2 ( hor, ver );
    }
    protected virtual Vector3 GetConvertInput(Vector2 input)
    {
        Vector2 converted =this._characterJoystickCtrl.ConvertWithCamera(Camera.main.transform.position, input.x, input.y);
        Vector3 direction = new Vector3(converted.x, 0, converted.y).normalized;
        return direction;
    }
}

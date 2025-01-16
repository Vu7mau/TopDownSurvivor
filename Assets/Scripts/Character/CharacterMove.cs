using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : ObjectMovement
{
    [Header("CharacterJoystickCtrl")]
    [SerializeField] protected CharacterCtrl _characterCtrl;
    protected Vector3 _moveDir;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadCharacterJoystickCtrl();
        this.LoadRigidbody();
    }
    protected virtual void LoadCharacterJoystickCtrl()
    {
        if (this._characterCtrl != null) return;
        _characterCtrl = this.transform.parent.GetComponent<CharacterCtrl>();
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
        this.GetMoveDirect();
        this.GetTarget(_moveDir);
    }
    protected virtual void GetTarget(Vector3 target)
    {
        this._targetPosition = target;
        if (_targetPosition != Vector3.zero) { _isMoving = true; return; }
        _isMoving = false;
    }
    protected virtual void GetMoveDirect()
    {
        this._moveDir = new Vector3(_characterCtrl.InputManager.MoveInput.x, 0, _characterCtrl.InputManager.MoveInput.y);
    }
    public virtual bool GetIsMoving()
    {
        return _isMoving;
    }
}
//protected virtual Vector3 GetConvertInput(Vector2 input)
//{
//    Vector2 converted =this._characterCtrl.ConvertWithCamera(Camera.main.transform.position, input.x, input.y);
//    Vector3 direction = new Vector3(converted.x, 0, converted.y).normalized;
//    return direction;
//}


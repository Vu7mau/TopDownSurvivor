using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCtrl : VuMonoBehaviour
{
  
 
    [Header("Character Ctrl")]
    [SerializeField] protected InputManager _inputManager;
    public PlayerControls playerControls { get; private set; }
    public InputManager InputManager => _inputManager;

    [SerializeField] protected CharacterMove _characterMove;
    public CharacterMove CharacterMove => _characterMove;
  
    protected override void LoadComponent()
    {
        base.LoadComponent();
       this.LoadInputManager();
        this.LoadCharacterMove();
    }
 
    protected virtual void LoadInputManager()
    {
        if (this._inputManager != null) return;

        this._inputManager=GameObject.FindObjectOfType<InputManager>();
        Debug.Log(" Load InputManager Success " + this.transform.name);
    }
    protected virtual void LoadCharacterMove()
    {
        if (this._characterMove != null) return;

        this._characterMove = this.transform.GetComponentInChildren<CharacterMove>();
        Debug.Log(" Load InputManager Success " + this._characterMove.transform.name);
    }
}




//public virtual Vector2 ConvertWithCamera(Vector3 cameraPos,float hor,float ver)
//{
//    Vector2 joyDirection = new Vector2(hor, ver).normalized;
//    Vector2 camera2DPos= new Vector2(cameraPos.x, cameraPos.z);
//    Vector2 characterPos= new Vector2(this.transform.position.x, this.transform.position.z);
//    Vector2 camToCharacterDirection= (Vector2.zero-camera2DPos).normalized;
//    float angle = Vector2.SignedAngle(camToCharacterDirection, new Vector2(0, 1));
//    Vector2 finalDirection = this.RotateVector(joyDirection,-angle);
//    return finalDirection;
//}
//protected virtual Vector2 RotateVector(Vector2 v, float angle)
//{
//    float radian=angle*Mathf.Rad2Deg;
//    float _x= v.x*Mathf.Cos(radian)-v.y*Mathf.Sin(radian);
//    float _y= v.x*Mathf.Sin(radian)+ v.y*Mathf.Cos(radian);
//    return new Vector2(_x, _y);

//}
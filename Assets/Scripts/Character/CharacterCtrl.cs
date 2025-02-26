using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCtrl : Singleton<CharacterCtrl>
{
  

    [Header("Character Ctrl")]
    [SerializeField] protected InputManager _inputManager;
    public PlayerControls playerControls { get; private set; }
    public InputManager InputManager => _inputManager;

    [SerializeField] protected CharacterMove _characterMove;
    public CharacterMove CharacterMove => _characterMove;

    [SerializeField] protected CharacterAim _characterAim;
    public CharacterAim CharacterAim => _characterAim;

      [SerializeField] protected CharacterShooting _characterShooting;
    public CharacterShooting CharacterShooting => _characterShooting;
  
      [SerializeField] protected ActiveWeapon _activeWeapon;
    public ActiveWeapon ActiveWeapon => _activeWeapon;

    [SerializeField] protected CharacterStats _characterStats;
    public CharacterStats CharacterStats => _characterStats; 
    [SerializeField] protected CharacterAnimHandle _characterAnimHandle;
    public CharacterAnimHandle CharacterAnimHandle => _characterAnimHandle; 
    [SerializeField] protected CharacterDamageReceiver _characterDamageReceiver;
    public CharacterDamageReceiver CharacterDamageReceiver => _characterDamageReceiver;  
    [SerializeField] protected CharacterLeveUp _characterLeveUp;
    public CharacterLeveUp CharacterLeveUp => _characterLeveUp;




    protected override void LoadComponents()
    {

        this.LoadInputManager();
        this.LoadCharacterMove();
        this.LoadCharacterAim();
        this.LoadCharacterShooting();
        this.LoadActiveWeapon();
        this.LoadCharacterStats();
        this.LoadCharacterAnimHandle();
        this.LoadCharacterDamageReceiver();
        this.LoadCharacterLeveUp();
       
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
    protected virtual void LoadCharacterAim()
    {
        if (this._characterAim != null) return;

        this._characterAim = this.transform.GetComponentInChildren<CharacterAim>();
        Debug.Log(" Load InputManager Success " + this._characterAim.transform.name);
    }

    protected virtual void LoadCharacterShooting()
    {
        if (this._characterShooting != null) return;

        this._characterShooting = this.transform.GetComponentInChildren<CharacterShooting>();
        Debug.Log(" Load CharacterShooting Success " + this._characterShooting.transform.name);
    }  
    protected virtual void LoadActiveWeapon()
    {
        if (this._activeWeapon != null) return;

        this._activeWeapon = GetComponent<ActiveWeapon>();
        Debug.Log(" Load CharacterShooting Success " + this._activeWeapon.transform.name);
    }

    protected virtual void LoadCharacterStats()
    {
        if (this._characterStats != null) return;

        this._characterStats = GameObject.FindObjectOfType<CharacterStats>();
        Debug.Log("LoadCharacterStats success " + this._characterStats.transform.name);
    } 
    protected virtual void LoadCharacterAnimHandle()
    {
        if (this._characterAnimHandle != null) return;

        this._characterAnimHandle = transform.GetComponentInChildren<CharacterAnimHandle>();
        Debug.Log("LoadCharacterAnimHandle success " + this._characterStats.transform.name);
    } 
    protected virtual void LoadCharacterDamageReceiver()
    {
        if (this._characterDamageReceiver != null) return;

        this._characterDamageReceiver = GetComponent<CharacterDamageReceiver>();
        Debug.Log("LoadCharacterAnimHandle success " + this._characterDamageReceiver.transform.name);
    }
    protected virtual void LoadCharacterLeveUp()
    {
        if (this._characterLeveUp != null) return;

        this._characterLeveUp = GetComponentInChildren<CharacterLeveUp>();
        Debug.Log("LoadCharacterAnimHandle success " + this._characterLeveUp.transform.name);
    }


    public int GetHealthFromStats()
    {
        return this._characterStats.currentHP;
    }

    public int GetDamageFromStats()
    {
        return this._characterStats.AttackEnemy();
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
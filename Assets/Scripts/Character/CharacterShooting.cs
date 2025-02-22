using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Rendering;

public class CharacterShooting :CharacterCtrlAbstract
{
    [SerializeField] protected RayCastWeapon _weapon => this._characterCtrl.ActiveWeapon.activeGun;
    [Space]
    [Header("Character Shooting")]
    [SerializeField] protected Rig _aim;
    [SerializeField] protected float _aimDuration;
   // [SerializeField] protected ActiveWeapon _activeWeapon;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadCharacterCtrlAbstract();
      //  this.LoadWeaponActivate();
      //  this.LoadWeapon();
    }
    //protected virtual void LoadWeapon()
    //{
    //    if (this._weapon != null) return;

    //    this._weapon = _activeWeapon.activeGun;
    //   // Debug.Log(" Load_weapon Success " + this._weapon.transform.name);
    //}
    //protected virtual void LoadCharacterCtrlAbstract()
    //{
    //    if (this._characterCtrl != null) return;
    //    _characterCtrl = this.transform.parent.GetComponent<CharacterCtrl>();
    //    Debug.Log("Load CharacterCtrl Abstract Success at " + this.transform.name);
    //}
  

    protected  void LateUpdate()
    {
      this.Aiming(/*this.IsShooting()*/);
    }
    public virtual bool IsShooting()
    {
        if (this._weapon != null)
            if (this._weapon.GetIsReloadingAmmo()) return false;
        return _characterCtrl.InputManager.IsFiring();
    }

    //protected override void Shoot()
    //{

    //    //Transform newBullet = BulletSpawner.Instance.Spawn(BulletSpawner.bulletOne, this._gunPoint.position, Quaternion.LookRotation(this._gunPoint.forward));  
    //    //if (newBullet == null) return;
    //    //newBullet.gameObject.SetActive(true);
    //    //this._weapon.FireMuzzelFlash();
    //    //Debug.Log("do here");          
    //}
    //protected virtual void LoadWeaponActivate()
    //{
    //    if (this._activeWeapon != null ) return;

    //    this._activeWeapon = this.transform.parent.GetComponent<ActiveWeapon>();
    //    Debug.Log("LoadWeaponActivate");

    //}
    public Transform? GetGunPoint()
    {
        if (!this._weapon) return null;
        return  this._weapon.GunPoint;
    }
    public RaycastHit GetTargetEnemy()
    {
        if (!this._weapon) return default;
        return this._weapon.TargetEnemy;
    }

    protected virtual void Aiming()
    {
        if (this._characterCtrl.ActiveWeapon != null&&this._weapon!=null)
        {
            // if(this._weapon.WeaponName != this._weapon.WeaponName)

            this._characterCtrl.ActiveWeapon._rigController.SetBool("isShooting",IsShooting());
        }
        //if (isAim)
        //    _aim.weight += Time.deltaTime / _aimDuration;
        //else
        //    _aim.weight -= Time.deltaTime / _aimDuration;

    }


}

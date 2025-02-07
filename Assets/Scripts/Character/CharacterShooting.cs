using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterShooting : VuMonoBehaviour
{
    [Space]
    [Header("Character Shooting")]
 //   [SerializeField] protected Transform _gunPoint;

    [SerializeField] protected CharacterCtrl _characterCtrl;

    [SerializeField] protected RayCastWeapon _weapon;
    [SerializeField] protected Rig _aim;
    [SerializeField] protected float _aimDuration;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadCharacterCtrlAbstract();
        this.LoadWeapon();
    }
    protected virtual void LoadWeapon()
    {
        if (this._weapon != null) return;

        this._weapon = GetComponentInChildren<RayCastWeapon>();
        Debug.Log(" Load_weapon Success " + this._weapon.transform.name);
    }
    protected virtual void LoadCharacterCtrlAbstract()
    {
        if (this._characterCtrl != null) return;
        _characterCtrl = this.transform.parent.GetComponent<CharacterCtrl>();
        Debug.Log("Load CharacterCtrl Abstract Success at " + this.transform.name);
    }
  

    protected  void Update()
    {
        this.Aiming(this.IsShooting());
    }
    public virtual bool IsShooting()
    {
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

    public Transform GetGunPoint()=>this._weapon.gunPoint;

    protected virtual void Aiming(bool isAim)
    {
        if (isAim)
            _aim.weight += Time.deltaTime / _aimDuration;
        else
            _aim.weight -= Time.deltaTime / _aimDuration;

    }


}

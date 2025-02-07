using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWeapon : VuMonoBehaviour
{
    [SerializeField] RayCastWeapon _weapon;
    [SerializeField] protected UnityEngine.Animations.Rigging.Rig handLK;

    protected override void LoadComponents()
    {
       
        this.LoadRayCastWeapon();
    }
   protected virtual void Update()
    {
        if(_weapon)
        {
            handLK.weight = 1.0f;
        }
        else
        {
            handLK.weight = 0.0f;
        }
    }
  
    protected virtual void LoadRayCastWeapon()
    {
        RayCastWeapon existingWeapon = GameObject.FindObjectOfType<RayCastWeapon>();
        if (existingWeapon != null) { this.Equip(existingWeapon); return; };


        this._weapon = GameObject.FindObjectOfType<RayCastWeapon>();
        this.Equip(this._weapon);
        Debug.Log("Load CharacterCtrl Abstract Success at " + this._weapon.name);
    }
    public virtual void Equip(RayCastWeapon newWeapon)
    {
        _weapon = newWeapon;
      
    }
}

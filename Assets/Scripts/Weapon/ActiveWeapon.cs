using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;

public class ActiveWeapon : VuMonoBehaviour
{
    public enum WeaponSlot { Primary, Secondary };

    //  [SerializeField] RayCastWeapon _weapon;
    [SerializeField] protected Transform[] weaponSlot;
    [SerializeField] public Animator _rigController;


    Transform animTransform;
    [SerializeField] RayCastWeapon[] equipped_Weapons = new RayCastWeapon[2];
    [SerializeField] int activateWeaponIndex;
    [SerializeField] bool isHolstered = false;


    public RayCastWeapon activeGun;
   // public Animator RigController=>_rigController;
    protected override void Start()
    {
        base.Start();
        // _rigController = GetComponentInChildren<Animator>();
        if (_rigController == null) Debug.LogWarning("Null anim");
        this.LoadRayCastWeapon();
    }

    protected override void LoadComponents()
    {
        this.animTransform = GameObject.Find("CharacterAnim").GetComponent<Transform>();
        if (animTransform == null) Debug.LogError("Null");
        // this.LoadRayCastWeapon();
    }


    protected virtual void LoadRayCastWeapon()
    {
        RayCastWeapon existingWeapon = GameObject.FindObjectOfType<RayCastWeapon>();
        if (existingWeapon != null) { this.Equip(existingWeapon); return; };

    }

    protected virtual void Update()
    {
        if (Input.GetKeyUp(KeyCode.X))
        {
            this.ToggelActivateWeapon();
        }

        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            this.SetActivateWeapon(WeaponSlot.Primary);
        }

        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            this.SetActivateWeapon(WeaponSlot.Secondary);
        }
    }
    protected virtual void ToggelActivateWeapon()
    {
        bool isHolster = this._rigController.GetBool("holster_weapon");
        if (isHolster) StartCoroutine(this.HolsterWeapon(activateWeaponIndex));
        else StartCoroutine(this.ActivateWeapon(activateWeaponIndex));
    }
    protected virtual RayCastWeapon GetWeapon(int index)
    {
        if (index < 0 || index >= equipped_Weapons.Length) return null;
        return this.equipped_Weapons[index];
    }
    public virtual void Equip(RayCastWeapon newWeapon)
    {
        int weaponSlotIndex = (int)newWeapon.weaponSlot;
        var _weapon = this.GetWeapon(weaponSlotIndex);
        if (_weapon)
        {
          //  Destroy(_weapon.gameObject);
          return;
        }
        if (equipped_Weapons[activateWeaponIndex]!=null)
        {
            this.equipped_Weapons[activateWeaponIndex].SetIsWeaponActivate(false);
        }
        _weapon = newWeapon;
        _weapon.transform.SetParent(weaponSlot[weaponSlotIndex], false);
        // _weapon.transform.localScale = _weapon.transform.localScale * 100;
        //_weapon.transform.localPosition = newWeapon.transform.position;
        //_weapon.transform.localRotation = newWeapon.transform.rotation;

        this.equipped_Weapons[weaponSlotIndex] = _weapon;
        this.activateWeaponIndex = weaponSlotIndex;
        this.activeGun = this.equipped_Weapons[weaponSlotIndex];
        this.SetActivateWeapon(newWeapon.weaponSlot);
    }
    protected virtual void SetActivateWeapon(WeaponSlot weaponSlot)
    {
        int holsterIndex = activateWeaponIndex;
        int activateIndex = (int)weaponSlot;
        if (holsterIndex == activateIndex)
        {
            holsterIndex = -1;
        }
        this.StartCoroutine(SwitchWeapon(holsterIndex, activateIndex));
    }
    IEnumerator SwitchWeapon(int holsterIndex, int activateIndex)
    {
        yield return StartCoroutine(this.HolsterWeapon(holsterIndex));
        yield return StartCoroutine(this.ActivateWeapon(activateIndex));
        activateWeaponIndex = activateIndex;
    }
    IEnumerator HolsterWeapon(int index)
    {
        this.isHolstered = true;
        var weapon = this.GetWeapon(index);
        if (weapon)
        {
            this._rigController.SetBool("holster_weapon", true);
            weapon.SetIsWeaponActivate(!this.isHolstered);
            do
            {
                yield return new WaitForEndOfFrame();
            } while (_rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);

        }


    }
    IEnumerator ActivateWeapon(int index)
    {
        var weapon = this.GetWeapon(index);
        if (weapon)
        {
            this._rigController.SetBool("holster_weapon", false);
            _rigController.Play("equip_" + weapon.WeaponName);
            do
            {
                yield return new WaitForEndOfFrame();
            } while (_rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
            this.isHolstered = false;
            weapon.SetIsWeaponActivate(!this.isHolstered);
        }
    }
    //[ContextMenu("Save WeaponPose")]
    //void SaveWeaponPose()
    //{
    //    GameObjectRecorder recorder = new GameObjectRecorder(animTransform.gameObject);
    //    recorder.BindComponentsOfType<Transform>(_weaponParent.gameObject, false);
    //    recorder.BindComponentsOfType<Transform>(_weaponRightGrip.gameObject,   false);
    //    recorder.BindComponentsOfType<Transform>(_weaponLeftGrip.gameObject, false   );
    //    recorder.TakeSnapshot(0.0f);
    //   // recorder.SaveToClip(_weapon.weaponAnimation);


    //}
}

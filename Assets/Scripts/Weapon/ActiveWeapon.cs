using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;
public class ActiveWeapon : VuMonoBehaviour
{


    [SerializeField] protected Transform[] weaponSlot;
    [SerializeField] public Animator _rigController;


    Transform animTransform;
    [SerializeField] List<RayCastWeapon> equipped_Weapons = new List<RayCastWeapon>();
    [SerializeField] int activateWeaponIndex;
    [SerializeField] bool isHolstered = false;
    [SerializeField] private bool isSwitchingWeapon = false;

    public List<RayCastWeapon> Equipped_Weapons => equipped_Weapons;
    public bool IsHolstered => isHolstered;
    public RayCastWeapon activeGun;

    // public Animator RigController=>_rigController;
    protected override void Start()
    {
        base.Start();
        // _rigController = GetComponentInChildren<Animator>();
        if (_rigController == null) Debug.LogWarning("Null anim");
        // this.LoadRayCastWeapon();
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
            if (equipped_Weapons.Count <1) return;
            this.SetActivateWeapon(0);
        }

        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            if (equipped_Weapons.Count < 2) return;

            this.SetActivateWeapon(1);
        }
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            if (equipped_Weapons.Count < 3) return;

            this.SetActivateWeapon(2);
        }
        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            if (equipped_Weapons.Count < 4) return;

            this.SetActivateWeapon(3);
        }
        if (Input.GetKeyUp(KeyCode.Alpha5))
        {
            if (equipped_Weapons.Count < 5) return;

            this.SetActivateWeapon(4);
        }
        if (Input.GetKeyUp(KeyCode.Alpha6))
        {
            if (equipped_Weapons.Count < 6) return;

            this.SetActivateWeapon(5);
        }
        if (Input.GetKeyUp(KeyCode.Alpha7))
        {
            if (equipped_Weapons.Count < 7) return;

            this.SetActivateWeapon(6);
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
        if (index < 0 || index >= equipped_Weapons.Count) return null;
        return this.equipped_Weapons[index];
    }

    // Lấy vị trí của weapon để set positon sau đó thêm vào list wepaon
    public virtual void Equip(RayCastWeapon newWeapon)
    {

        int weaponSlotIndex = (int)newWeapon.weaponSlot;
        int hasWeapon = equipped_Weapons.IndexOf(newWeapon);
        if (hasWeapon != -1)
        {
            //  Destroy(_weapon.gameObject);
            return;
        }
        var _weapon = newWeapon;
        _weapon.transform.SetParent(weaponSlot[weaponSlotIndex], false);

        // this.equipped_Weapons[weaponSlotIndex] = _weapon;
        this.equipped_Weapons.Add(_weapon);
        int newSlotIndex = equipped_Weapons.IndexOf(_weapon);
        CharacterUIManager.OnWeaponSelected?.Invoke(newSlotIndex);
        //this.activateWeaponIndex = newSlotIndex;

        this.SetActivateWeapon(newSlotIndex);

    }
    protected virtual void SetActivateWeapon(int weaponSlot)
    {

        int holsterIndex = activateWeaponIndex;
        int activateIndex = weaponSlot;
        if (weaponSlot > equipped_Weapons.Count) return;
        if (holsterIndex == activateIndex)
        {
            holsterIndex = -1;
        }
        this.StartCoroutine(SwitchWeapon(holsterIndex, activateIndex));
    }
    IEnumerator SwitchWeapon(int holsterIndex, int activateIndex)
    {
        activateWeaponIndex = activateIndex;
     
        if (equipped_Weapons.Count > 0 && equipped_Weapons.Count >= activateWeaponIndex)
        {
            if (equipped_Weapons[activateWeaponIndex] != null)
            {
                var wepon = this.equipped_Weapons[activateWeaponIndex];
                if (wepon.GetIsReloadingAmmo())
                    yield break;
                wepon.SetIsWeaponActivate(false);
            }
        }
        yield return StartCoroutine(this.HolsterWeapon(holsterIndex));
        yield return StartCoroutine(this.ActivateWeapon(activateIndex));

        // Sự kiện update UI súng đang chọn 
        CharacterUIManager.OnWeaponChange?.Invoke(this.activeGun.GunSprite(), this.activeGun.GetCurrentAmmour(), this.activeGun.GetMaxAmmour());
    }

    IEnumerator HolsterWeapon(int index)
    {

        if (isHolstered) yield break;

        this.isHolstered = true;
        var weapon = this.GetWeapon(index);
        if (weapon)
        {
            this._rigController.SetBool("holster_weapon", true);
            weapon.SetIsWeaponActivate(!this.isHolstered);
            do
            {
                yield return new WaitForEndOfFrame();
            } while (_rigController.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);

            yield return new WaitForSeconds(.5f);
            weapon.model.gameObject.SetActive(false);
        }


    }
    IEnumerator ActivateWeapon(int index)
    {
        if (!IsHolstered) yield break;


        var weapon = this.GetWeapon(index);
        if (weapon == null)
        {
            Debug.LogWarning($"Weapon at index {index} is null!");
            yield break;
        }
        weapon.model.gameObject.SetActive(true);
        if (weapon)
        {
             SoundFXManager.Instance.PlaySoundFXClip(SoundFXManager.Instance.pickUp, this.transform);
            this._rigController.SetBool("holster_weapon", false);
            do
            {
                yield return new WaitForEndOfFrame();
            } while (_rigController.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);

            _rigController.Play("equip_" + weapon.WeaponName);
            this.isHolstered = false;
            weapon.SetIsWeaponActivate(!this.isHolstered);
            this.activeGun = weapon;
        }
        else
        {
            this.activeGun = null;
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
public enum WeaponSlot { Primary, Secondary, Tertiary, Quaternary, Quinary, Senary, Septenary };

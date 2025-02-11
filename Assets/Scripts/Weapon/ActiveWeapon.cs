using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class ActiveWeapon : VuMonoBehaviour
{
    [SerializeField] RayCastWeapon _weapon;
    [SerializeField] protected UnityEngine.Animations.Rigging.Rig handLK;
    [SerializeField] protected Transform _weaponParent;
    [SerializeField] protected Transform _weaponLeftGrip;
    [SerializeField] protected Transform _weaponRightGrip;


    Animator anim;
    AnimatorOverrideController animOverride;
    Transform animTransform;
    protected override void Start()
    {
        base.Start();
        anim = GetComponentInChildren<Animator>();
        if (anim == null) Debug.LogWarning("Null anim");
        animOverride = anim.runtimeAnimatorController as AnimatorOverrideController;
        this.LoadRayCastWeapon();
    }

    protected override void LoadComponents()
    {
        this.animTransform = GameObject.Find("CharacterAnim").GetComponent<Transform>();
        if (animTransform == null) Debug.LogError("Null");
        // this.LoadRayCastWeapon();
    }
    protected virtual void Update()
    {
        if (_weapon!=null)
        {
            handLK.weight = 1.0f;
          

        }
        else
        {
            handLK.weight = 0.0f;
            anim.SetLayerWeight(1, 0.0f);
        }
    }

    protected virtual void LoadRayCastWeapon()
    {
        RayCastWeapon existingWeapon = GameObject.FindObjectOfType<RayCastWeapon>();
        if (existingWeapon != null) { this.Equip(existingWeapon); return; };


      //   this._weapon = GameObject.FindObjectOfType<RayCastWeapon>();
       // this.Equip(this._weapon);
       // Debug.Log("Load CharacterCtrl Abstract Success at " + this._weapon.name);
    }
    public virtual void Equip(RayCastWeapon newWeapon)
    {
        if (_weapon)
        {
            Destroy(this._weapon.gameObject);
        }
        _weapon = newWeapon;
      //  if (_weapon == null) return;
        this._weapon.transform.parent = this._weaponParent;
        _weapon.transform.localScale = _weapon.transform.localScale * 100;
        _weapon.transform.localPosition = newWeapon.transform.position;
        _weapon.transform.localRotation = newWeapon.transform.rotation;
        anim.SetLayerWeight(1, 1.0f);
        Invoke(nameof(SetAnimationDelay), 0.01f);

    }
    protected virtual void SetAnimationDelay()
    {
        animOverride["weapon_anim_empty"] = _weapon.weaponAnimation;           
    }
    [ContextMenu("Save WeaponPose")]
    void SaveWeaponPose()
    {
        GameObjectRecorder recorder = new GameObjectRecorder(animTransform.gameObject);
        recorder.BindComponentsOfType<Transform>(_weaponParent.gameObject, false);
        recorder.BindComponentsOfType<Transform>(_weaponRightGrip.gameObject,   false);
        recorder.BindComponentsOfType<Transform>(_weaponLeftGrip.gameObject, false   );
        recorder.TakeSnapshot(0.0f);
        recorder.SaveToClip(_weapon.weaponAnimation);

        if (recorder.isRecording)
        {
            Debug.Log("Recording successful, saving clip...");
            recorder.SaveToClip(_weapon.weaponAnimation);
        }
        else
        {
            Debug.LogError("Recording failed, no data saved!");
        }
    }
}

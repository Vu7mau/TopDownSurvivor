using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDamageReceiver : DamageReceiver
{
    [SerializeField] protected CharacterCtrl characterCtrl;
    protected override void Start()
    {
        base.Start();
        this.SetMaxHealth();
      //  HpBar.Instance.SetHealthMaxBarVolume(this._hpMax);
    
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadCharacterCtrl();
    }

    protected virtual void LoadCharacterCtrl()
    {
        if (characterCtrl != null) return;

        this.characterCtrl = GetComponent<CharacterCtrl>();
        Debug.Log("LoadCharacterCtrl");
    }
    protected override void HurtEffect()
    {
        CinemachineCtrl.Instance.CinemachineShake.ShakeCamera(5f, .1f);
        SoundFXManager.Instance.PlaySoundFXClip(SoundFXManager.Instance.maleHit, this.transform);
        characterCtrl.CharacterAnimHandle.ChracterAnimator.SetTrigger("IsHit");
    }
    protected override void OnDead()
    {
        Debug.Log("PLayer Death");
    }
    protected virtual void SetMaxHealth()
    {
        this._hpMax = this.characterCtrl.GetHealthFromStats();
        this.Reborn();
    }
    public override void Deduct(int Deduct)
    {
        HpBar.Instance.SetHealth((float)Deduct,(float)_hp,this._hpMax);
        base.Deduct(Deduct);
   
    }
    public override void Add(int add)
    {
        this._hpMax += add;
        HpBar.Instance.SetHealth(-(float)add, (float)this._hp, this._hpMax);
        base.Add(add);

    }

}

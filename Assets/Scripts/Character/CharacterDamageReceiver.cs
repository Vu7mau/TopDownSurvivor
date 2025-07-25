 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDamageReceiver : DamageReceiver
{
    [SerializeField] protected CharacterCtrl characterCtrl;

    [SerializeField] protected PanelDie _pnl;
    protected IEnumerator enumeratorDamageScreen;
    protected override void Start()
    {
       // base.Start();
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

    protected override void OnDead()
    {
        Debug.Log("PLayer Death");
        characterCtrl.CharacterAnimHandle.ChracterAnimator.SetTrigger("IsDead");
        characterCtrl.InputManager.DetachInputEvents();
        characterCtrl.DisableAllComponet();
        CharacterUIManager.OnUpdateHealth?.Invoke(0, 0);

        //  _pnl.pnlDie.gameObject.SetActive(true);

    }
    protected virtual void SetMaxHealth()
    {
        //  this._hpMax = this.characterCtrl.GetHealthFromStats();
        this.Reborn();
    }
    public override void Deduct(int Deduct)
    {
        //HpBar.Instance.SetHealth((float)Deduct,(float)_hp,this._hpMax);
        base.Deduct(Deduct);
        CharacterUIManager.OnUpdateHealth?.Invoke(_hp, _hpMax);
    }
    public override void Add(int add)
    {
        //int tagertHp=_hp+add;
        base.Add(add);
        CharacterUIManager.OnUpdateHealth?.Invoke(_hp, _hpMax);
        CharacterEffect.HealingEffect?.Invoke();

    }
    protected override void HurtEffect()
    {
        CinemachineCtrl.Instance.CinemachineShake.ShakeCamera(8f, .1f);
        int random = UnityEngine.Random.Range(0, SoundFXManager.Instance.maleHit.Length);
        SoundFXManager.Instance.PlaySoundFXClip(SoundFXManager.Instance.maleHit[random], this.transform);
        characterCtrl.CharacterAnimHandle.ChracterAnimator.SetTrigger("IsHit");
        // DamagerScreen.Instance.ActivateDamageScreen();

        if (enumeratorDamageScreen != null)
        {
            StopCoroutine(enumeratorDamageScreen);
        }
        enumeratorDamageScreen = this.DamageScreenEffect();
        StartCoroutine(enumeratorDamageScreen);
    }
    protected virtual IEnumerator DamageScreenEffect()
    {
        float remainingHealthPercent = this._hp / this._hpMax * 100;
        float duration = .7f;
        WaitForSeconds durationTurn = new WaitForSeconds(duration);
        DamagerScreen.Instance.ActivateDamageScreen();
        enumeratorDamageScreen = null;
        yield return durationTurn;

    }

}

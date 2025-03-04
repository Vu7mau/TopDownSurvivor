using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DamagerScreen : Singleton<DamagerScreen>
{
  [SerializeField]  protected  Volume _damageScreenVolume;
  [SerializeField]  protected  Animator _screenAnimator;
    private Vignette vignette;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadAnimator();
        this.LoadVolume();
    }

    protected virtual void LoadAnimator()
    {
        if (this._screenAnimator != null) return;

        this._screenAnimator = GetComponent<Animator>();
        Debug.Log("LoadAnimator success " + this._screenAnimator.transform.name);
    }
    protected virtual void LoadVolume()
    {
        if (this._damageScreenVolume != null) return;

        this._damageScreenVolume = GetComponent<Volume>();
        Debug.Log("LoadAnimator success " + this._damageScreenVolume.transform.name);
    }

    public virtual void ActivateDamageScreen()
    {
        this._damageScreenVolume.priority = 2;
        this._screenAnimator.SetTrigger("IsHit");
        if (_damageScreenVolume != null && _damageScreenVolume.profile.TryGet(out vignette))
        {
            ChangeVignetteColor(Color.red);
        }

    }
    public virtual void ResetVolumePriority()
    {
        this._damageScreenVolume.priority = 0;
    }
    public virtual void SetLeveUpScreen()
    {
        this._damageScreenVolume.priority = 2;
        this._screenAnimator.SetTrigger("IsLevelUp");
        if (_damageScreenVolume != null && _damageScreenVolume.profile.TryGet(out vignette))
        {
            ChangeVignetteColor(Color.green);
        }

    }
    public void ChangeVignetteColor(Color newColor)
    {
        if (vignette != null)
        {
            vignette.color.value = newColor;
        }
    }
}

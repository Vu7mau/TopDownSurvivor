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

    private Coroutine _fadeCoroutine;
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
        this._damageScreenVolume.enabled = true;
        //if (_damageScreenVolume.profile.TryGet(out vignette))
        //{
        //    ChangeVignetteColor(Color.red);
        //}

        // Huỷ coroutine cũ nếu đang chạy
        if (_fadeCoroutine != null)
        {
            StopCoroutine(_fadeCoroutine);
        }

        _fadeCoroutine = StartCoroutine(FadeInThenOut());
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

    private IEnumerator FadeInThenOut()
    {
        yield return FadeWeight(0f, 1f, 0.2f); // Fade-in nhanh
        yield return new WaitForSeconds(0.3f); // Đợi một chút
        yield return FadeWeight(1f, 0f, 0.5f); // Fade-out chậm
        _damageScreenVolume.enabled = false;
    }
    public void FadeInDamageEffect(float duration = 0.5f)
    {
        StartCoroutine(FadeWeight(0f, 1f, duration));
    }

    public void FadeOutDamageEffect(float duration = 0.5f)
    {
        StartCoroutine(FadeWeight(1f, 0f, duration));
    }

    private IEnumerator FadeWeight(float from, float to, float duration)
    {
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);
            _damageScreenVolume.weight = Mathf.Lerp(from, to, t);
            yield return null;
        }
        _damageScreenVolume.weight = to;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadAmmor : Singleton<ReloadAmmor>
{
    [SerializeField] protected Slider _reloadSlider;
    [SerializeField] protected float _speed;
    bool _loaded = false;



    public virtual void AmmoReload(float reloadTime,Vector3 target)
    { 
        if (_loaded) return;

        this._loaded = true;
        StartCoroutine(this.Reload(reloadTime));
    }

    IEnumerator Reload(float reloadTime)
    {
     
        this._reloadSlider.gameObject.SetActive(true);
        this._reloadSlider.value = 0;
        //this._reloadSlider.maxValue = reloadTime;
        float elapsedTime = 0f;

        while (elapsedTime < reloadTime)
        {
            elapsedTime += Time.deltaTime;
            this.UpdateReloadSlider(elapsedTime / reloadTime);
            yield return null;
        }
        _reloadSlider.gameObject.SetActive(false);
        this._reloadSlider.value = 0;
        SoundFXManager.Instance.PlaySoundFXClip(SoundFXManager.Instance.reloadAmmor, this.transform);
        Invoke(nameof(this.ResetLoaded), .1f);
    }
   
    protected virtual void UpdateReloadSlider(float value)
    {
        this._reloadSlider.value = value;
    }
    protected virtual void ResetLoaded()
    {
        this._loaded = false;
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadAmmorReloadSlider();
    }
    protected virtual void LoadAmmorReloadSlider()
    {
        if (this._reloadSlider != null) return;

        this._reloadSlider = GetComponentInChildren<Slider>();
        Debug.Log("LoadAmmorReloadSlider " + this._reloadSlider.transform.name);
    }
}

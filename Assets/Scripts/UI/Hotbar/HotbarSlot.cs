using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotbarSlot : VuMonoBehaviour
{
    [SerializeField] protected Image _weaponImage;
    [SerializeField] protected Image _hightlight;

    protected override void LoadComponents()
    {
        this.LoadWeaponImage();
        this.LoadHightlight();

    }

    public virtual void SetItem(Image weaponImage)
    {
        this._weaponImage = weaponImage;
        this._weaponImage.enabled = true;
       this.SetSelectedItem(true);
    }    

    public virtual void SetSelectedItem(bool selected)
    {
        if(selected) 
            _hightlight.enabled = true;
        else
            _hightlight.enabled = false;
    }    

    private void LoadWeaponImage()
    {
        if(this._weaponImage != null) return;
        this._weaponImage = this.transform.Find("Weapon Image").GetComponent<Image>();
        if (!this._weaponImage)
            Debug.Log("weapon image bị null tại "+this.transform.name);
        else
            this._weaponImage.enabled = false;

    }
    private void LoadHightlight()
    {
        if (this._hightlight != null) return;
        this._hightlight = this.transform.Find("Hightlight").GetComponent<Image>();
        if (!this._hightlight)
            Debug.Log("weapon image bị null tại " + this.transform.name);
        else
            this._hightlight.enabled = false;


    }
}

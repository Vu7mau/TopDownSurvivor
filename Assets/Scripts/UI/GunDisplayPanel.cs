using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GunDisplayPanel : VuMonoBehaviour
{
    [SerializeField] private Image gunImage;
    [SerializeField] private TMP_Text ammoText;

   
    protected override void LoadComponents()
    {
        this.LoadGunImage();
        this.LoadBulletText();
    }
    private void LoadGunImage()
    {
        if (gunImage != null) return;

        gunImage=this.transform.Find("GunImage").GetComponent<Image>();
    }  
    private void LoadBulletText()
    {
        if (ammoText != null) return;
        Debug.Log("Im here");
        ammoText = this.transform.Find("AmmoText").GetComponent<TMP_Text>();
    }
   public void UpdateAmmoDisplay(int ammoCount,int maxAmmo)
    {
        if (ammoText == null) return;

        ammoText.text = $"{ammoCount-1}/{maxAmmo}";
        Debug.Log($"Ammo count updated: {ammoCount}");
        // Cập nhật thanh đạn, số đạn, v.v.
    }  
    public void UpdateWeaponDisplay(Sprite gunSprite,int ammoCount, int maxAmmo)
    {
        if (ammoText == null || gunImage == null) return;
        
        gunImage.sprite = gunSprite;
        this.UpdateAmmoDisplay(ammoCount,maxAmmo);

        // Cập nhật thanh đạn, số đạn, v.v.
    }


}

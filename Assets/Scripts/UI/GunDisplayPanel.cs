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
        ammoText = this.transform.Find("AmmoText").GetComponent<TMP_Text>();
    }
   public void UpdateAmmoDisplay(int ammoCount,int maxAmmo)
    {
        if (ammoText == null) return;

        ammoText.text = $"{ammoCount}/{maxAmmo}";
       // Debug.Log($"Ammo count updated");
        // Cập nhật thanh đạn, số đạn, v.v.
    }  
    public void UpdateWeaponDisplay(Sprite gunSprite,int ammoCount, int maxAmmo)
    {
        if (ammoText == null || gunImage == null) return;
        Color color = gunImage.color;
        color.a = 1f;
        gunImage.color = color;
        gunImage.sprite = gunSprite;
        this.UpdateAmmoDisplay(ammoCount,maxAmmo);
        //Debug.Log($"Ammo count updated"+ammoCount+"|"+maxAmmo);


        // Cập nhật thanh đạn, số đạn, v.v.
    }


}

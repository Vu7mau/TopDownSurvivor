using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUIManager : VuMonoBehaviour
{

    [SerializeField] private GunDisplayPanel gunDisplayPanel;
    [SerializeField] private HotbarUI hotbarUI;
    [SerializeField] private HealthDisplay healthDisplay;

    public static Action<Sprite, int, int> OnWeaponChange;
    public static Action<int, int> OnWeaponReload;

    public static Action<int> OnWeaponSelected;

    public static Action<float,float> OnUpdateHealth;




    protected override void OnEnable()
    {
        OnWeaponReload += gunDisplayPanel.UpdateAmmoDisplay;
        OnWeaponChange += gunDisplayPanel.UpdateWeaponDisplay;


        OnWeaponSelected += hotbarUI.TriggerSelect;


        OnUpdateHealth += healthDisplay.SetHealth;
    }

    protected override void OnDisable()
    {
        OnWeaponReload -= gunDisplayPanel.UpdateAmmoDisplay;
        OnWeaponChange -= gunDisplayPanel.UpdateWeaponDisplay;

        OnWeaponSelected -= hotbarUI.TriggerSelect;

        OnUpdateHealth -= healthDisplay.SetHealth;

    }

    protected override void LoadComponents()
    {
        this.Load_GunDisplayPanel();
        this.Load_HotbarUI();
        this.Load_HealthDisplay();
    }

    private void Load_GunDisplayPanel()
    {
        if (gunDisplayPanel != null) return;

        gunDisplayPanel = this.transform.GetComponentInChildren<GunDisplayPanel>();
    }
    private void Load_HotbarUI()
    {
        if (hotbarUI != null) return;

        hotbarUI = this.transform.Find("Weapon Hotbar").GetComponentInChildren<HotbarUI>();
    } 
    private void Load_HealthDisplay()
    {
        if (healthDisplay != null) return;

        healthDisplay = this.transform.GetComponentInChildren<HealthDisplay>();
    }
}

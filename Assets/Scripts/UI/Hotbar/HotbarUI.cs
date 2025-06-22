using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HotbarUI : VuMonoBehaviour
{
    [SerializeField] public List<HotbarSlot> slots;
    [SerializeField] private int selectedIndex = -1;



 
    protected override void LoadComponents()
    {
        this.LoadAllSlot();
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            selectedIndex = 0;
            TriggerSelect(selectedIndex);
        }

        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            selectedIndex = 1;
            TriggerSelect(selectedIndex);

        }
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            selectedIndex = 2;
            TriggerSelect(selectedIndex);

        }
        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            selectedIndex = 3;
            TriggerSelect(selectedIndex);

        }
        if (Input.GetKeyUp(KeyCode.Alpha5))
        {
            selectedIndex = 4;
            TriggerSelect(selectedIndex);

        }
        if (Input.GetKeyUp(KeyCode.Alpha6))
        {
            selectedIndex = 5;
            TriggerSelect(selectedIndex);

        }
        if (Input.GetKeyUp(KeyCode.Alpha7))
        {
            selectedIndex = 6;
            TriggerSelect(selectedIndex);

        }
    }
    private void Select()
    {
        //if (slots.Count < 1)
        //{
        //    Debug.LogWarning("Danh sách hotbar slot trống !!!");
        //    return;
        //}

       
        if (selectedIndex < CharacterCtrl.Instance.ActiveWeapon.Equipped_Weapons.Count)
        {
            foreach (var slot in slots)
            {
                slot.SetSelectedItem(false);
            }

            var image = CharacterCtrl.Instance.ActiveWeapon.Equipped_Weapons[selectedIndex].GunSprite();
            if (image == null) return;
            slots[this.selectedIndex].SetItem(image);
        }
        else
        {

            slots[selectedIndex].SetSelectedItem(true);
        }    

    }
    // Gọi hàm này từ nơi khác để trigger chọn
    public void TriggerSelect(int index)
    {
        selectedIndex = index;
        Select();
    }
    private void LoadAllSlot()
    {
        if (slots.Count > 0) return;
        foreach (Transform slot in this.transform)
        {
            if (slot.gameObject.TryGetComponent<HotbarSlot>(out HotbarSlot hotbarSlot))
            {
                slots.Add(hotbarSlot);
            }

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagazineData
{
    public int MaxAmmo ;
    public int CurrentAmmo;
    public MagazineData(int maxAmmo)
    {
        this.MaxAmmo = maxAmmo;
        this.CurrentAmmo = maxAmmo;
    }
    public void reload()
    {
        CurrentAmmo = MaxAmmo;
    }
    public bool UseAmmo(int amount)
    {
        if (CurrentAmmo >= amount)
        {
            CurrentAmmo -= amount;
            return true;
        }
        return false;
    }

}

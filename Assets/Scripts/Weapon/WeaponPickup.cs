using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : VuMonoBehaviour
{
    [SerializeField] protected RayCastWeapon _weaponFab;

    private void OnTriggerEnter(Collider other)
    {
        ActiveWeapon activeWeapon =other.GetComponent<ActiveWeapon>();
        if (activeWeapon != null )
        {
           RayCastWeapon newWeapon=Instantiate(_weaponFab);
            activeWeapon.Equip(newWeapon);
        }
    }
}

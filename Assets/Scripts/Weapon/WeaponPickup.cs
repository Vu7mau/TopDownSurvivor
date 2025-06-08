using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : VuMonoBehaviour
{
    [SerializeField] protected RayCastWeapon _weaponFab;

    protected override void Start()
    {
        base.Start();
        transform.DORotate(new Vector3(0, 360, 0), 2f, RotateMode.FastBeyond360)
    .SetLoops(-1, LoopType.Restart)
    .SetEase(Ease.Linear);

    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        ActiveWeapon activeWeapon =other.GetComponent<ActiveWeapon>();
        if (activeWeapon != null )
        {
           RayCastWeapon newWeapon=Instantiate(_weaponFab);
            activeWeapon.Equip(newWeapon);
            this.gameObject.SetActive(false);
        }
    }
}

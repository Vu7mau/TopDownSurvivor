using DG.Tweening;
using UnityEngine;

public class WeaponPickup : VuMonoBehaviour
{
    [SerializeField] protected RayCastWeapon _weaponFab;
    [SerializeField] private UniqueId uniqueId;

    protected override void Start()
    {
        base.Start();
        if (uniqueId == null) uniqueId = gameObject.GetComponent<UniqueId>() ?? gameObject.AddComponent<UniqueId>();

        transform.DORotate(new Vector3(0, 360, 0), 2f, RotateMode.FastBeyond360)
                 .SetLoops(-1, LoopType.Restart)
                 .SetEase(Ease.Linear);

        var activeWeapon = FindObjectOfType<ActiveWeapon>();
        if (activeWeapon != null && activeWeapon.HasPicked(uniqueId.Id))
        {
<<<<<<< HEAD
           RayCastWeapon newWeapon=Instantiate(_weaponFab);
            activeWeapon.Equip(newWeapon);
             
            if(newWeapon != null)
            {
                DamageSender damageSender = GameObjectCustom.FindFirstComponentInChildren<DamageSender>(newWeapon.transform);
                //DamageSourceCtrl damageSource = GameObjectCustom.FindFirstComponentInChildren<DamageSourceCtrl>(newWeapon.transform);
                if (damageSender != null)
                {
                    CharacterEvents.OnDamageSourceListChanged?.Invoke(damageSender);
                }
                //if(damageSource != null)
                //{
                //    BulletSpawner.Instance.AddDamageSourceToPool(damageSource);
                //}
            }

            this.gameObject.SetActive(false);
=======
            gameObject.SetActive(false);
>>>>>>> Vu
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ActiveWeapon activeWeapon = other.GetComponent<ActiveWeapon>();
        if (activeWeapon == null) return;

        RayCastWeapon newWeapon = Instantiate(_weaponFab);
        activeWeapon.Equip(newWeapon);

        if (uniqueId == null) uniqueId = gameObject.GetComponent<UniqueId>() ?? gameObject.AddComponent<UniqueId>();
        activeWeapon.MarkPicked(uniqueId.Id);

        // === TỰ LƯU sau khi nhặt ===
        if (activeWeapon.autoSaveOnPickup)
            activeWeapon.SaveWeapons(activeWeapon.saveId);

        gameObject.SetActive(false);
    }

    // Cho ActiveWeapon truy cập
    public string GetWeaponName() => _weaponFab != null ? _weaponFab.WeaponName : string.Empty;
    public string GetId() => (uniqueId != null) ? uniqueId.Id : string.Empty;
    public RayCastWeapon GetPrefab() => _weaponFab;
}

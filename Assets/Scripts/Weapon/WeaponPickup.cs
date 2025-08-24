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
    }

    private void OnTriggerEnter(Collider other)
    {
        ActiveWeapon activeWeapon = other.GetComponent<ActiveWeapon>();
        if (activeWeapon == null || _weaponFab == null) return;

        // NHẶT AN TOÀN: không trùng -> cấp mới & tắt pickup; trùng -> chỉ chọn, pickup vẫn còn
        if (activeWeapon.TryEquipPrefab(_weaponFab, out var instance, selectIfOwned: true))
        {
            if (instance != null)
            {
                // thật sự thêm mới
                activeWeapon.MarkPicked(GetId());
                gameObject.SetActive(false);
                if (activeWeapon.autoSaveOnPickup) activeWeapon.SaveWeapons(activeWeapon.saveId);
            }
        }
    }

    // Cho ActiveWeapon truy cập
    public string GetWeaponName() => _weaponFab != null ? _weaponFab.WeaponName : string.Empty;
    public string GetId() => (uniqueId != null) ? uniqueId.Id : string.Empty;
    public RayCastWeapon GetPrefab() => _weaponFab;
}

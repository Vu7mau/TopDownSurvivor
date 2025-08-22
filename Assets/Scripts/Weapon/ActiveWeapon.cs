using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActiveWeapon : VuMonoBehaviour
{
    [Header("Rig & Slot")]
    [SerializeField] protected Transform[] weaponSlot;
    [SerializeField] public Animator _rigController;

    [Header("Save/Checkpoint")]
    public string saveId = "slot1";
    public bool autoSaveOnPickup = true;

    [Header("Auto-pickup khi Alt+Số")]
    public float autoPickupRange = 2.0f; // nếu đứng trong bán kính này so với pickup -> tự lấy

    Transform animTransform;
    [SerializeField] List<RayCastWeapon> equipped_Weapons = new List<RayCastWeapon>();
    [SerializeField] int activateWeaponIndex;
    [SerializeField] bool isHolstered = false;

    public List<RayCastWeapon> Equipped_Weapons => equipped_Weapons;
    public bool IsHolstered => isHolstered;
    public RayCastWeapon activeGun;

    // Save state
    private HashSet<string> _pickedUpIds = new();
    private string _wishedWeaponName = null;

    protected override void Start()
    {
        base.Start();
        if (_rigController == null) Debug.LogWarning("Null anim");
    }

    protected override void LoadComponents()
    {
        this.animTransform = GameObject.Find("CharacterAnim")?.GetComponent<Transform>();
        if (animTransform == null) Debug.LogError("Null CharacterAnim");
    }

    protected virtual void Update()
    {
        if (Input.GetKeyUp(KeyCode.X)) this.ToggelActivateWeapon();

        // 1..7
        if (Input.GetKeyUp(KeyCode.Alpha1)) { if (equipped_Weapons.Count >= 1) SetActivateWeapon(0); }
        if (Input.GetKeyUp(KeyCode.Alpha2)) { if (equipped_Weapons.Count >= 2) SetActivateWeapon(1); }
        if (Input.GetKeyUp(KeyCode.Alpha3)) { if (equipped_Weapons.Count >= 3) SetActivateWeapon(2); }
        if (Input.GetKeyUp(KeyCode.Alpha4)) { if (equipped_Weapons.Count >= 4) SetActivateWeapon(3); }
        if (Input.GetKeyUp(KeyCode.Alpha5)) { if (equipped_Weapons.Count >= 5) SetActivateWeapon(4); }
        if (Input.GetKeyUp(KeyCode.Alpha6)) { if (equipped_Weapons.Count >= 6) SetActivateWeapon(5); }
        if (Input.GetKeyUp(KeyCode.Alpha7)) { if (equipped_Weapons.Count >= 7) SetActivateWeapon(6); }

        // Q/E
        if (Input.GetKeyDown(KeyCode.Q)) PrevWeapon();
        if (Input.GetKeyDown(KeyCode.E)) NextWeapon();

        // Scroll
        float scroll = Input.mouseScrollDelta.y;
        if (Mathf.Abs(scroll) > 0.01f)
        {
            if (scroll > 0) NextWeapon();
            else PrevWeapon();
        }

        // Alt + số: nếu có -> chọn; nếu chưa có -> thử nhặt ngay (nếu gần); xa thì chỉ hướng
        if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
        {
            if (Input.GetKeyUp(KeyCode.Alpha1)) WishOrGrabByIndex(0);
            if (Input.GetKeyUp(KeyCode.Alpha2)) WishOrGrabByIndex(1);
            if (Input.GetKeyUp(KeyCode.Alpha3)) WishOrGrabByIndex(2);
            if (Input.GetKeyUp(KeyCode.Alpha4)) WishOrGrabByIndex(3);
            if (Input.GetKeyUp(KeyCode.Alpha5)) WishOrGrabByIndex(4);
            if (Input.GetKeyUp(KeyCode.Alpha6)) WishOrGrabByIndex(5);
            if (Input.GetKeyUp(KeyCode.Alpha7)) WishOrGrabByIndex(6);
        }

        // V -> trỏ đến pickup vũ khí chưa nhặt gần nhất (ưu tiên mong muốn); nếu đang ở đủ gần thì nhặt luôn
        if (Input.GetKeyUp(KeyCode.V))
        {
            AimNearestNotOwnedPickup(_wishedWeaponName);
        }

        // Demo hotkeys
        if (Input.GetKeyDown(KeyCode.F5)) SaveWeapons(saveId);
        if (Input.GetKeyDown(KeyCode.F9)) LoadWeapons(saveId);
        if (Input.GetKeyDown(KeyCode.F6)) SaveAndWipe(saveId); // Lưu & Xoá sạch vũ khí đã nhặt
    }

    private void NextWeapon()
    {
        if (equipped_Weapons.Count == 0) return;
        int next = (activateWeaponIndex + 1) % equipped_Weapons.Count;
        SetActivateWeapon(next);
    }

    private void PrevWeapon()
    {
        if (equipped_Weapons.Count == 0) return;
        int prev = (activateWeaponIndex - 1 + equipped_Weapons.Count) % equipped_Weapons.Count;
        SetActivateWeapon(prev);
    }

    private void WishOrGrabByIndex(int index)
    {
        // 1) Xác định "loại" mong muốn từ WeaponRegistry (ưu tiên) hoặc từ index slot
        string targetName = null;
        var reg = WeaponRegistry.Instance;
        if (reg != null && reg.weaponPrefabs.Count > index && reg.weaponPrefabs[index] != null)
            targetName = reg.weaponPrefabs[index].WeaponName;

        // 2) Nếu đã SỞ HỮU vũ khí loại đó -> chọn đúng khẩu đó (không đi tìm)
        int ownedIdx = -1;
        if (!string.IsNullOrEmpty(targetName))
        {
            ownedIdx = equipped_Weapons.FindIndex(w => w != null && w.WeaponName == targetName);
        }
        else
        {
            ownedIdx = equipped_Weapons.FindIndex(w => w != null && (int)w.weaponSlot == index);
        }

        if (ownedIdx >= 0)
        {
            SetActivateWeapon(ownedIdx);
            _wishedWeaponName = equipped_Weapons[ownedIdx].WeaponName;
            return;
        }

        // 3) Chưa có -> thử LẤY LUÔN nếu đang ở gần pickup đúng loại
        if (!string.IsNullOrEmpty(targetName))
        {
            // Tìm pickup gần nhất loại đó
            WeaponPickup closest = FindClosestPickupByName(targetName, out float dist);
            if (closest != null && dist <= autoPickupRange)
            {
                ForceCollectPickup(closest);
                if (autoSaveOnPickup) SaveWeapons(saveId);
                return;
            }
        }

        // 4) Xa quá/không có -> đặt mong muốn + chỉ đường (và nếu tới gần, AimNearestNotOwnedPickup sẽ tự nhặt)
        _wishedWeaponName = targetName;
        AimNearestNotOwnedPickup(_wishedWeaponName);
    }

    // ==== chọn/holster/activate như cũ ====
    protected virtual void ToggelActivateWeapon()
    {
        bool isHolster = this._rigController.GetBool("holster_weapon");
        if (isHolster) StartCoroutine(this.HolsterWeapon(activateWeaponIndex));
        else StartCoroutine(this.ActivateWeapon(activateWeaponIndex));
    }

    protected virtual RayCastWeapon GetWeapon(int index)
    {
        if (index < 0 || index >= equipped_Weapons.Count) return null;
        return this.equipped_Weapons[index];
    }

    public virtual void Equip(RayCastWeapon newWeapon)
    {
        int weaponSlotIndex = (int)newWeapon.weaponSlot;
        if (equipped_Weapons.Contains(newWeapon)) return;

        newWeapon.transform.SetParent(weaponSlot[weaponSlotIndex], false);
        equipped_Weapons.Add(newWeapon);

        int newSlotIndex = equipped_Weapons.IndexOf(newWeapon);
        CharacterUIManager.OnWeaponSelected?.Invoke(newSlotIndex);
        SetActivateWeapon(newSlotIndex);

        if (!string.IsNullOrEmpty(_wishedWeaponName) && _wishedWeaponName == newWeapon.WeaponName)
            _wishedWeaponName = null;
    }

    protected virtual void SetActivateWeapon(int weaponSlot)
    {
        int holsterIndex = activateWeaponIndex;
        int activateIndex = weaponSlot;
        if (weaponSlot >= equipped_Weapons.Count) return;
        if (holsterIndex == activateIndex) holsterIndex = -1;
        StartCoroutine(SwitchWeapon(holsterIndex, activateIndex));
    }

    IEnumerator SwitchWeapon(int holsterIndex, int activateIndex)
    {
        activateWeaponIndex = activateIndex;

        if (equipped_Weapons.Count > 0 && activateWeaponIndex < equipped_Weapons.Count)
        {
            var wepon = equipped_Weapons[activateWeaponIndex];
            if (wepon != null)
            {
                if (wepon.GetIsReloadingAmmo()) yield break;
                wepon.SetIsWeaponActivate(false);
            }
        }
        yield return StartCoroutine(HolsterWeapon(holsterIndex));
        yield return StartCoroutine(ActivateWeapon(activateIndex));

        CharacterUIManager.OnWeaponChange?.Invoke(
            this.activeGun?.GunSprite(),
            this.activeGun?.GetCurrentAmmour() ?? 0,
            this.activeGun?.GetMaxAmmour() ?? 0
        );
    }

    IEnumerator HolsterWeapon(int index)
    {
        if (isHolstered) yield break;

        this.isHolstered = true;
        var weapon = this.GetWeapon(index);
        if (weapon)
        {
            this._rigController.SetBool("holster_weapon", true);
            weapon.SetIsWeaponActivate(!this.isHolstered);
            do { yield return new WaitForEndOfFrame(); }
            while (_rigController.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);

            yield return new WaitForSeconds(.5f);
            weapon.model.gameObject.SetActive(false);
        }
    }

    IEnumerator ActivateWeapon(int index)
    {
        if (!IsHolstered) yield break;

        var weapon = this.GetWeapon(index);
        if (weapon == null)
        {
            Debug.LogWarning($"Weapon at index {index} is null!");
            yield break;
        }
        weapon.model.gameObject.SetActive(true);

        if (weapon)
        {
            SoundFXManager.Instance.PlaySoundFXClip(SoundFXManager.Instance.pickUp, this.transform);
            this._rigController.SetBool("holster_weapon", false);
            do { yield return new WaitForEndOfFrame(); }
            while (_rigController.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);

            _rigController.Play("equip_" + weapon.WeaponName);
            this.isHolstered = false;
            weapon.SetIsWeaponActivate(!this.isHolstered);
            this.activeGun = weapon;
        }
        else
        {
            this.activeGun = null;
        }
    }

    // ===== SAVE / LOAD =====
    public void SaveWeapons(string saveId)
    {
        var state = new ActiveWeaponState
        {
            isHolstered = this.isHolstered,
            activeIndex = this.activateWeaponIndex,
            wishedWeaponName = _wishedWeaponName,
            pickedUpIds = _pickedUpIds.ToList()
        };

        for (int i = 0; i < equipped_Weapons.Count; i++)
        {
            var w = equipped_Weapons[i];
            if (w == null) continue;

            state.equipped.Add(new WeaponState
            {
                weaponName = w.WeaponName,
                slotIndex = i,
                currentAmmo = w.GetCurrentAmmour(),
                totalAmmo = w.GetMaxAmmour(), // hoặc weaponInfo.totalAmmo nếu bạn có biến riêng
                isActive = (i == activateWeaponIndex && !isHolstered)
            });
        }
        WeaponSaveSystem.Save(saveId, state);
    }

    public void LoadWeapons(string saveId)
    {
        if (!WeaponSaveSystem.TryLoad(saveId, out var state))
        {
            Debug.LogWarning($"[Load] Không tìm thấy save {saveId}");
            return;
        }

        ClearInventory(alsoClearPicked: false);

        _pickedUpIds = new HashSet<string>(state.pickedUpIds ?? new List<string>());
        _wishedWeaponName = state.wishedWeaponName;

        var reg = WeaponRegistry.Instance;
        if (reg == null) { Debug.LogError("WeaponRegistry missing!"); return; }

        foreach (var ws in state.equipped.OrderBy(x => x.slotIndex))
        {
            var prefab = reg.GetPrefab(ws.weaponName);
            if (prefab == null)
            {
                Debug.LogWarning($"Prefab not found for {ws.weaponName}");
                continue;
            }

            var newWeapon = Object.Instantiate(prefab);
            Equip(newWeapon);

            newWeapon.UpdateTotalBullet(ws.totalAmmo - newWeapon.GetMaxAmmour());
            newWeapon.SetCurrentAmmo(ws.currentAmmo);
        }

        isHolstered = state.isHolstered;
        if (state.activeIndex >= 0 && state.activeIndex < equipped_Weapons.Count)
            SetActivateWeapon(state.activeIndex);
        else if (equipped_Weapons.Count > 0)
            SetActivateWeapon(0);

        RefreshPickupsVisibility();
    }

    // ======= “LƯU & XOÁ SẠCH” =======
    public void SaveAndWipe(string saveId)
    {
        SaveWeapons(saveId);
        ClearInventory(alsoClearPicked: true);
        ResetAllPickupsInScene(); // bật lại để có thể nhặt lại
    }

    // ===== PICKUP STATE =====
    public void MarkPicked(string pickupId)
    {
        if (!string.IsNullOrEmpty(pickupId)) _pickedUpIds.Add(pickupId);
    }

    public bool HasPicked(string pickupId)
    {
        return !string.IsNullOrEmpty(pickupId) && _pickedUpIds.Contains(pickupId);
    }

    private void RefreshPickupsVisibility()
    {
        var pickups = GameObject.FindObjectsOfType<WeaponPickup>(includeInactive: true);
        foreach (var p in pickups)
        {
            if (p == null) continue;
            bool shouldHide = HasPicked(p.GetId());
            if (p.gameObject.activeSelf == !shouldHide) continue;
            p.gameObject.SetActive(!shouldHide);
        }
    }

    public void ClearInventory(bool alsoClearPicked)
    {
        foreach (var w in equipped_Weapons.Where(x => x != null))
            Destroy(w.gameObject);
        equipped_Weapons.Clear();
        activeGun = null;
        isHolstered = true;
        activateWeaponIndex = 0;

        if (alsoClearPicked)
        {
            _pickedUpIds.Clear();
        }
    }

    public void ResetAllPickupsInScene()
    {
        var pickups = GameObject.FindObjectsOfType<WeaponPickup>(includeInactive: true);
        foreach (var p in pickups)
        {
            if (p == null) continue;
            p.gameObject.SetActive(true);
        }
    }

    // ===== Utils: tìm pickup theo tên gần nhất =====
    private WeaponPickup FindClosestPickupByName(string weaponName, out float distance)
    {
        distance = float.MaxValue;
        if (string.IsNullOrEmpty(weaponName)) return null;

        WeaponPickup best = null;
        Vector3 myPos = transform.position;
        var pickups = GameObject.FindObjectsOfType<WeaponPickup>(includeInactive: false);

        foreach (var p in pickups)
        {
            if (p == null || !p.gameObject.activeInHierarchy) continue;
            if (HasPicked(p.GetId())) continue;
            if (p.GetWeaponName() != weaponName) continue;

            float d = Vector3.Distance(p.transform.position, myPos);
            if (d < distance) { distance = d; best = p; }
        }
        return best;
    }

    // ===== Force pick: nhặt ngay một pickup (bỏ qua trigger) =====
    private void ForceCollectPickup(WeaponPickup p)
    {
        if (p == null) return;
        var prefab = p.GetPrefab();
        if (prefab == null) return;

        RayCastWeapon newWeapon = Instantiate(prefab);
        Equip(newWeapon);

        MarkPicked(p.GetId());
        p.gameObject.SetActive(false);

        if (autoSaveOnPickup) SaveWeapons(saveId);
    }

    // ===== Tìm & trỏ pickup chưa nhặt (ưu tiên mong muốn); nếu đủ gần thì NHẶT LUÔN =====
    private void AimNearestNotOwnedPickup(string preferWeaponName)
    {
        var ownedNames = new HashSet<string>(equipped_Weapons.Where(w => w != null).Select(w => w.WeaponName));
        var pickups = GameObject.FindObjectsOfType<WeaponPickup>(includeInactive: false);

        WeaponPickup best = null;
        float bestDistSqr = float.MaxValue;
        Vector3 myPos = transform.position;

        // Ưu tiên “mong muốn”
        foreach (var p in pickups)
        {
            if (p == null || !p.gameObject.activeInHierarchy) continue;
            if (HasPicked(p.GetId())) continue;

            if (!string.IsNullOrEmpty(preferWeaponName) && p.GetWeaponName() == preferWeaponName)
            {
                float d2 = (p.transform.position - myPos).sqrMagnitude;
                if (d2 < bestDistSqr) { best = p; bestDistSqr = d2; }
            }
        }

        // Log an toàn
        Debug.Log($"Press Alt prefer={preferWeaponName}, found={(best != null ? best.GetWeaponName() : "none")}");

        // Nếu chưa có theo “mong muốn” -> bất kỳ loại chưa sở hữu
        if (best == null)
        {
            foreach (var p in pickups)
            {
                if (p == null || !p.gameObject.activeInHierarchy) continue;
                if (HasPicked(p.GetId())) continue;
                if (ownedNames.Contains(p.GetWeaponName())) continue;

                float d2 = (p.transform.position - myPos).sqrMagnitude;
                if (d2 < bestDistSqr) { best = p; bestDistSqr = d2; }
            }
        }

        if (best != null)
        {
            float dist = Mathf.Sqrt(bestDistSqr);

            // Nếu đã đứng đủ gần -> NHẶT LUÔN
            if (dist <= autoPickupRange)
            {
                ForceCollectPickup(best);
                return;
            }

            // Chưa đủ gần -> chỉ hướng
            Debug.DrawLine(myPos, best.transform.position, Color.cyan, 2f);
            Vector3 dir = (best.transform.position - myPos);
            dir.y = 0;
            if (dir.sqrMagnitude > 0.01f) transform.forward = dir.normalized;

            // CharacterUIManager.OnWishWeapon?.Invoke(best.GetWeaponName(), best.transform.position);
        }
        else
        {
            Debug.Log("[Pickup Target] Không tìm thấy vũ khí chưa nhặt phù hợp.");
        }
    }
}

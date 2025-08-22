using System.Linq;
using UnityEngine;

public class CheatWeaponHotkeys : MonoBehaviour
{
    [Header("Refs")]
    public ActiveWeapon activeWeapon;           // Kéo reference ActiveWeapon của nhân vật vào đây
    public WeaponRegistry registry;             // Kéo GameObject có WeaponRegistry vào đây

    [Header("Options")]
   [SerializeField] public bool cheatEnabled = false;           // Cho phép bật/tắt cheat khi play
    [SerializeField] public string saveId = "slot1";             // Dùng chung với ActiveWeapon nếu có

    [Tooltip("Khi grant nếu đã sở hữu thì chỉ equip khẩu đó, không tạo thêm.")]
    public bool doNotDuplicate = true;

    [Tooltip("Tự động equip ngay sau khi grant.")]
    public bool autoEquipGranted = true;

    private void Reset()
    {
        activeWeapon = GetComponent<ActiveWeapon>();
        if (registry == null) registry = FindObjectOfType<WeaponRegistry>();
    }

    private void Awake()
    {
        if (activeWeapon == null) activeWeapon = GetComponent<ActiveWeapon>();
        if (registry == null) registry = FindObjectOfType<WeaponRegistry>();
    }

    private void Update()
    {
        // Toggle cheat mode
        if (Input.GetKeyDown(KeyCode.BackQuote) || Input.GetKeyDown(KeyCode.F10))
        {
            cheatEnabled = !cheatEnabled;
            Debug.Log($"[Cheat] {(cheatEnabled ? "ENABLED" : "DISABLED")}");
        }

        if (!cheatEnabled) return;

        // Grant/Remove theo Alt + number
        HandleGrantRemoveByNumber();

        // Clear all
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
            {
                if (Input.GetKeyDown(KeyCode.C))
                {
                    ClearAllWeapons();
                }
                if (Input.GetKeyDown(KeyCode.S))
                {
                    SaveNow();
                }
                if (Input.GetKeyDown(KeyCode.L))
                {
                    LoadNow();
                }
            }
        }
    }

    private void HandleGrantRemoveByNumber()
    {
        // Alt + number => grant, Shift + Alt + number => remove
        bool alt = Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
        bool shift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        if (!alt) return;

        // Map 1..7 -> index 0..6
        TryHandleKey(KeyCode.Alpha1, 0, shift);
        TryHandleKey(KeyCode.Alpha2, 1, shift);
        TryHandleKey(KeyCode.Alpha3, 2, shift);
        TryHandleKey(KeyCode.Alpha4, 3, shift);
        TryHandleKey(KeyCode.Alpha5, 4, shift);
        TryHandleKey(KeyCode.Alpha6, 5, shift);
        TryHandleKey(KeyCode.Alpha7, 6, shift);
    }

    private void TryHandleKey(KeyCode kc, int idx, bool shift)
    {
        if (!Input.GetKeyDown(kc)) return;

        if (shift)
        {
            // Remove weapon theo index
            RemoveByRegistryIndex(idx);
        }
        else
        {
            // Grant weapon theo index
            GrantByRegistryIndex(idx, autoEquipGranted);
        }
    }

    // ===== API chính =====

    public void GrantByRegistryIndex(int index, bool makeActive)
    {
        if (activeWeapon == null || registry == null)
        {
            Debug.LogWarning("[Cheat] Missing ActiveWeapon or WeaponRegistry");
            return;
        }

        var prefab = registry.GetPrefabByIndex(index);
        if (prefab == null)
        {
            Debug.LogWarning($"[Cheat] No prefab at registry index {index}");
            return;
        }

        GrantByName(prefab.WeaponName, makeActive);
    }

    public void GrantByName(string weaponName, bool makeActive)
    {
        if (string.IsNullOrEmpty(weaponName)) { Debug.LogWarning("[Cheat] weaponName null/empty"); return; }
        if (activeWeapon == null || registry == null)
        {
            Debug.LogWarning("[Cheat] Missing ActiveWeapon or WeaponRegistry");
            return;
        }

        // Nếu đã có và không cho phép duplicate
        if (doNotDuplicate && activeWeapon.Equipped_Weapons.Any(w => w != null && w.WeaponName == weaponName))
        {
            if (makeActive)
            {
                int idx = activeWeapon.Equipped_Weapons.FindIndex(w => w != null && w.WeaponName == weaponName);
                if (idx >= 0) SelectIndex(idx);
            }
            Debug.Log($"[Cheat] Already owned: {weaponName} -> {(makeActive ? "equipped" : "skipped")}");
            return;
        }

        var prefab = registry.GetPrefab(weaponName);
        if (prefab == null)
        {
            Debug.LogWarning($"[Cheat] Prefab not found for weaponName={weaponName}");
            return;
        }

        // Instantiate + Equip
        var newWeapon = Instantiate(prefab);
        activeWeapon.Equip(newWeapon);

        if (makeActive)
        {
            int idx = activeWeapon.Equipped_Weapons.FindIndex(w => w == newWeapon);
            if (idx >= 0) SelectIndex(idx);
        }

        Debug.Log($"[Cheat] Granted: {weaponName}");
    }

    public void RemoveByRegistryIndex(int index)
    {
        if (activeWeapon == null || registry == null)
        {
            Debug.LogWarning("[Cheat] Missing ActiveWeapon or WeaponRegistry");
            return;
        }
        var prefab = registry.GetPrefabByIndex(index);
        if (prefab == null) { Debug.LogWarning($"[Cheat] No prefab at index {index}"); return; }

        RemoveByName(prefab.WeaponName);
    }

    public void RemoveByName(string weaponName)
    {
        if (activeWeapon == null) return;
        var list = activeWeapon.Equipped_Weapons;
        int i = list.FindIndex(w => w != null && w.WeaponName == weaponName);
        if (i < 0)
        {
            Debug.Log($"[Cheat] Remove skipped. Not owned: {weaponName}");
            return;
        }

        var wpn = list[i];
        if (wpn != null) Destroy(wpn.gameObject);
        list.RemoveAt(i);

        // Sửa chỉ số active nếu cần
        if (list.Count == 0)
        {
            // không còn vũ khí
        }
        else
        {
            int newIdx = Mathf.Clamp(i - 1, 0, list.Count - 1);
            SelectIndex(newIdx);
        }

        Debug.Log($"[Cheat] Removed: {weaponName}");
    }

    public void ClearAllWeapons()
    {
        if (activeWeapon == null) return;
        var list = activeWeapon.Equipped_Weapons.ToList();
        foreach (var w in list)
        {
            if (w != null) Destroy(w.gameObject);
        }
        activeWeapon.Equipped_Weapons.Clear();
        Debug.Log("[Cheat] Cleared all weapons");
    }

    public void SaveNow()
    {
        if (activeWeapon == null) return;
        activeWeapon.SaveWeapons(saveId);
        Debug.Log($"[Cheat] Saved -> {saveId}");
    }

    public void LoadNow()
    {
        if (activeWeapon == null) return;
        activeWeapon.LoadWeapons(saveId);
        Debug.Log($"[Cheat] Loaded <- {saveId}");
    }

    private void SelectIndex(int idx)
    {
        // Gọi SetActivateWeapon qua public API có sẵn
        // (hàm này của bạn là protected virtual; nếu bạn để protected thì gọi trực tiếp không được.
        // Ở đây mình chuyển sang cách "gián tiếp": set bằng hotkey số tương ứng.)
        // Nếu bạn muốn gọi thẳng, hãy đổi SetActivateWeapon thành public.

        // Giải pháp an toàn: mô phỏng nhấn số (0..6 -> Alpha1..Alpha7) là không tiện.
        // Vậy mình cung cấp 1 extension nhỏ dưới, gọi qua SendMessage:

        SendMessage("SetActivateWeapon", idx, SendMessageOptions.DontRequireReceiver);
    }
}

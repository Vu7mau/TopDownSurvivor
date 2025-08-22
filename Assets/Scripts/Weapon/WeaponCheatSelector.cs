using System.Linq;
using UnityEngine;

public class WeaponCheatSelector : MonoBehaviour
{
    [Header("Refs")]
    public ActiveWeapon activeWeapon;       // Kéo ActiveWeapon của nhân vật
    public WeaponRegistry registry;         // Kéo WeaponRegistry trong scene
    public ChatNotify chat;                 // Kéo ChatNotify (đã có trong dự án)

    [Header("Enter Combo (tổ hợp phím vào mode chọn)")]
    public KeyCode comboKey = KeyCode.W;    // Phím chính trong combo (mặc định: W)
    public bool requireCtrl = true;         // Ctrl + ...
    public bool requireAlt = true;         // Alt  + ...
    public bool requireShift = false;       // Shift + ... (nếu muốn)

    [Header("Options")]
    public bool doNotDuplicate = true;      // Đã có thì chỉ equip, không cấp trùng
    public bool autoEquipGranted = true;
    public string saveId = "slot1";

    [Header("Behaviour")]
    public bool pauseGameWhileSelecting = false; // true: Time.timeScale = 0 khi chọn

    private bool selecting = false;
    private int selIndex = 0;

    private void Reset()
    {
        activeWeapon = GetComponent<ActiveWeapon>();
        if (registry == null) registry = FindObjectOfType<WeaponRegistry>();
        if (chat == null) chat = FindObjectOfType<ChatNotify>();
    }

    private void Awake()
    {
        if (activeWeapon == null) activeWeapon = GetComponent<ActiveWeapon>();
        if (registry == null) registry = FindObjectOfType<WeaponRegistry>();
        if (chat == null) chat = FindObjectOfType<ChatNotify>();
    }

    private void Update()
    {
        // 1) Vào mode chọn bằng tổ hợp phím
        if (!selecting && IsEnterComboPressedDown())
        {
            StartSelecting();
            return;
        }

        if (!selecting) return;

        // 2) Khi đang ở mode chọn: điều hướng / xác nhận / thoát
        HandleNavigate();
        HandleConfirmOrRemove();
        HandleUtilityKeys();
    }

    // ==== Combo detection ====
    private bool IsEnterComboPressedDown()
    {
        if (Input.GetKeyDown(comboKey))
        {
            bool ctrlOk = !requireCtrl || Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
            bool altOk = !requireAlt || Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
            bool shiftOk = !requireShift || Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            return ctrlOk && altOk && shiftOk;
        }
        return false;
    }

    // ==== Lifecycle ====
    private void StartSelecting()
    {
        if (registry == null || registry.weaponPrefabs == null || registry.weaponPrefabs.Count == 0)
        {
            Info("WeaponRegistry rỗng. Hãy kéo prefab vũ khí vào WeaponRegistry.");
            return;
        }

        selecting = true;

        // Nếu hiện đang có activeGun thì đặt con trỏ về đúng loại đó
        selIndex = 0;
        var active = activeWeapon != null ? activeWeapon.activeGun : null;
        if (active != null)
        {
            int found = registry.weaponPrefabs.FindIndex(p => p != null && p.WeaponName == active.WeaponName);
            if (found >= 0) selIndex = found;
        }

        AnnounceSelected();

        if (pauseGameWhileSelecting) Time.timeScale = 0f;
    }

    private void ExitSelecting(bool showExitMessage = true)
    {
        selecting = false;
        if (pauseGameWhileSelecting) Time.timeScale = 1f;
        if (showExitMessage)
            Info("Thoát chế độ chọn vũ khí.");
    }

    // ==== Navigate / Confirm / Remove ====
    private void HandleNavigate()
    {
        // W/Up, S/Down, scroll
        bool next = Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || Input.mouseScrollDelta.y < -0.1f;
        bool prev = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.mouseScrollDelta.y > 0.1f;

        if (next) Move(1);
        if (prev) Move(-1);

        // Esc => THOÁT (log: thoát)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitSelecting(showExitMessage: true);
        }
    }

    private void Move(int step)
    {
        int total = registry.weaponPrefabs.Count;
        if (total == 0) return;
        selIndex = (selIndex + step + total) % total;
        AnnounceSelected();
    }

    private void AnnounceSelected()
    {
        var pf = registry.weaponPrefabs[selIndex];
        string name = pf != null ? pf.WeaponName : "(null)";
        int total = registry.weaponPrefabs.Count;

        if (chat != null) chat.Selected(selIndex, total, selIndex + 1, $"Vũ khí: {name}");
        else Debug.Log($"[Select] {selIndex + 1}/{total} : {name}");
    }

    private void HandleConfirmOrRemove()
    {
        // Xác nhận: Enter / Space / Mouse0
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if (GrantAndEquipSelected(out string weaponName))
            {
                // ✅ Chọn thành công -> log "thành công rồi thoát mode" và THOÁT (không log thêm dòng 'thoát')
                Info($"Chọn thành công: {weaponName}. Thoát chế độ chọn.");
                ExitSelecting(showExitMessage: false);
            }
        }

        // Xoá: Delete / Backspace (không thoát)
        if (Input.GetKeyDown(KeyCode.Delete) || Input.GetKeyDown(KeyCode.Backspace))
        {
            RemoveSelectedIfOwned();
        }
    }

    private void HandleUtilityKeys()
    {
        // Ctrl+S/L/Backspace các thao tác phụ không làm thoát menu
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                if (activeWeapon != null)
                {
                    activeWeapon.SaveWeapons(saveId);
                    Info($"Đã lưu vũ khí ({saveId}).");
                }
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                if (activeWeapon != null)
                {
                    activeWeapon.LoadWeapons(saveId);
                    Info($"Đã tải vũ khí ({saveId}).");
                }
            }

            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                ClearAllWeapons();
            }
        }
    }

    // ==== Core actions ====
    private bool GrantAndEquipSelected(out string weaponName)
    {
        weaponName = null;

        if (activeWeapon == null || registry == null)
        {
            Error("Thiếu ActiveWeapon/WeaponRegistry");
            return false;
        }

        var pf = registry.weaponPrefabs[selIndex];
        if (pf == null)
        {
            Error($"Prefab null ở index {selIndex}");
            return false;
        }

        // gán biến local
        string selectedName = pf.WeaponName;
        weaponName = selectedName;

        // Nếu đã có và không cho dup -> chỉ equip
        int ownedIdx = activeWeapon.Equipped_Weapons.FindIndex(
            w => w != null && w.WeaponName == selectedName
        );
        if (ownedIdx >= 0 && doNotDuplicate)
        {
            ActiveWeaponExtensions.SelectIndexSafe(activeWeapon, ownedIdx);
            return true;
        }

        // Cấp & equip
        var newWeapon = Instantiate(pf);
        activeWeapon.Equip(newWeapon);

        if (autoEquipGranted)
        {
            int idx = activeWeapon.Equipped_Weapons.FindIndex(w => w == newWeapon);
            if (idx >= 0) ActiveWeaponExtensions.SelectIndexSafe(activeWeapon, idx);
        }

        return true;
    }

    private void RemoveSelectedIfOwned()
    {
        if (activeWeapon == null || registry == null) return;
        var pf = registry.weaponPrefabs[selIndex];
        if (pf == null) return;

        string weaponName = pf.WeaponName;
        var list = activeWeapon.Equipped_Weapons;
        int i = list.FindIndex(w => w != null && w.WeaponName == weaponName);
        if (i < 0) { Info($"Chưa sở hữu: {weaponName}"); return; }

        var wpn = list[i];
        if (wpn != null) Destroy(wpn.gameObject);
        list.RemoveAt(i);

        // Chọn lại nếu còn vũ khí
        if (list.Count > 0)
        {
            int newIdx = Mathf.Clamp(i - 1, 0, list.Count - 1);
            ActiveWeaponExtensions.SelectIndexSafe(activeWeapon, newIdx);
        }

        Info($"Đã gỡ: {weaponName}");
    }

    private void ClearAllWeapons()
    {
        if (activeWeapon == null) return;
        var listCopy = activeWeapon.Equipped_Weapons.ToList();
        foreach (var w in listCopy)
        {
            if (w != null) Destroy(w.gameObject);
        }
        activeWeapon.Equipped_Weapons.Clear();
        Info("Đã xoá tất cả vũ khí đang sở hữu.");
    }

    // ===== ChatNotify wrappers =====
    private void Info(string msg)
    {
        if (chat != null) chat.Info(msg);
        else Debug.Log(msg);
    }

    private void Error(string msg)
    {
        if (chat != null) chat.Error(msg);
        else Debug.LogError(msg);
    }
}

public static class ActiveWeaponExtensions
{
    public static void SelectIndexSafe(ActiveWeapon aw, int idx)
    {
        if (aw == null) return;
        // Gọi SetActivateWeapon(int) bằng SendMessage
        aw.gameObject.SendMessage("SetActivateWeapon", idx, SendMessageOptions.DontRequireReceiver);
    }
}

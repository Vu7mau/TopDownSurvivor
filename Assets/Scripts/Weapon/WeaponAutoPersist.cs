using System.Linq;
using UnityEngine;

/// <summary>
/// - Start: tự LOAD lần lưu trước (nếu có).
/// - Update: auto SAVE khi loadout thay đổi.
/// - Quit/Pause: CLEAR nếu bật clearOnQuit.
/// - Hotkey: bấm clearSaveHotkey để Clear ngay.
/// - UI Button: gọi ClearNow() để Clear ngay.
/// </summary>
public class WeaponAutoPersist : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private ActiveWeapon activeWeapon;   // gán trong Inspector (hoặc để trống sẽ tự tìm)
    [SerializeField] private WeaponRegistry registry;     // gán trong Inspector (hoặc tự FindObjectOfType)

    [Header("Persist Options")]
    [Tooltip("Xoá dữ liệu lưu khi thoát game (Editor & Build).")]
    [SerializeField] private bool clearOnQuit = true;

    [Tooltip("Phím nóng xoá file lưu ngay lập tức (None = tắt).")]
    [SerializeField] private KeyCode clearSaveHotkey = KeyCode.None; // ví dụ: KeyCode.F10

    [Tooltip("Khi Clear, có đồng thời gỡ toàn bộ vũ khí đang sở hữu không?")]
    [SerializeField] private bool alsoUnequipOnClear = false;

    // cache chữ ký để biết khi nào cần lưu
    private string _signature = "";

    private void Reset()
    {
        activeWeapon = GetComponent<ActiveWeapon>();
    }

    private void Awake()
    {
        if (!activeWeapon) activeWeapon = GetComponent<ActiveWeapon>();
        if (!registry) registry = FindObjectOfType<WeaponRegistry>();
    }

    private void Start()
    {
        // TỰ LOAD khi Play
        if (WeaponRuntimeSave.TryLoad(out var data))
        {
            if (activeWeapon && registry)
            {
                WeaponRuntimeSave.ApplyTo(activeWeapon, registry, data);
                _signature = BuildSignature();
               // ChatNotifyOrLog("Đã khôi phục vũ khí phiên gần nhất.");
            }
        }
        else
        {
            _signature = BuildSignature();
        }
   
    }


    private void Update()
    {
        if (activeWeapon)
        {
            var sig = BuildSignature();
            if (sig != _signature)
            {
                _signature = sig;
                WeaponRuntimeSave.SaveSnapshot(activeWeapon); // TỰ LƯU khi thay đổi
            }
        }

        // Hotkey xoá lưu ngay
        if (clearSaveHotkey != KeyCode.None && Input.GetKeyDown(clearSaveHotkey))
        {
            ClearNow();
        }
    }

    private void OnApplicationQuit()
    {
        if (clearOnQuit) ClearNow(silent: true);
    }

#if UNITY_EDITOR
    // Khi dừng Play trong Editor, OnApplicationQuit không luôn chạy -> dọn ở đây nữa
    private void OnDisable()
    {
        //if (!Application.isPlaying) return;
        //if (clearOnQuit) ClearNow(silent: true);
    }
#endif

    // ====== PUBLIC API ======
    /// <summary>Gắn hàm này vào UI Button để xoá file lưu ngay.</summary>
    public void ClearNow()
    {
        ClearNow(silent: false);
    }

    [ContextMenu("Clear Save Now")]
    private void Context_ClearNow()
    {
        ClearNow(silent: false);
    }

    // core clear
    private void ClearNow(bool silent)
    {
        // Xoá file/lần lưu
        WeaponRuntimeSave.Clear();

        // Tuỳ chọn: gỡ vũ khí hiện có trong phiên chơi
        if (alsoUnequipOnClear && activeWeapon)
        {
            var list = activeWeapon.Equipped_Weapons.ToList();
            foreach (var w in list)
            {
                if (w != null) Destroy(w.gameObject);
            }
            activeWeapon.Equipped_Weapons.Clear();
        }

        _signature = BuildSignature(); // reset chữ ký để tránh auto-save lại ngay

        if (!silent) ChatNotifyOrLog("Đã xoá lưu loadout vũ khí.");
    }

    // ====== Helpers ======
    private string BuildSignature()
    {
        if (!activeWeapon) return "";
        var names = activeWeapon.Equipped_Weapons
                    .Where(w => w != null)
                    .Select(w => $"{w.WeaponName}:{w.GetCurrentAmmour()}") // theo API của bạn
                    .ToArray();

        int activeIdx = activeWeapon.Equipped_Weapons.IndexOf(activeWeapon.activeGun);
        return string.Join("|", names) + $"#A{activeIdx}";
    }

    private void ChatNotifyOrLog(string msg)
    {
        var chat = FindObjectOfType<ChatNotify>();
        if (chat) chat.Info(msg);
        else Debug.Log(msg);
    }
}

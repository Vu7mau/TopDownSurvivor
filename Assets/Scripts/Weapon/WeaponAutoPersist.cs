using System.Linq;
using UnityEngine;

/// <summary>
/// Gắn script này lên cùng GameObject có ActiveWeapon.
/// - Start: tự LOAD lần lưu trước (nếu có).
/// - Update: phát hiện thay đổi loadout => tự SAVE.
/// - OnApplicationQuit / OnDisable (Editor dừng Play): tự CLEAR file.
/// Không cần sửa ActiveWeapon.
/// </summary>
public class WeaponAutoPersist : MonoBehaviour
{
    public ActiveWeapon activeWeapon;   // gán trong Inspector (hoặc để trống sẽ tự tìm)
    public WeaponRegistry registry;     // gán trong Inspector (hoặc tự FindObjectOfType)
    public bool clearOnQuit = true;     // xoá dữ liệu khi thoát game

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
                _signature = BuildSignature(); // cập nhật chữ ký sau khi load
                ChatNotifyOrLog("Đã khôi phục vũ khí phiên gần nhất.");
            }
        }
        else
        {
            _signature = BuildSignature();
        }
    }

    private void Update()
    {
        if (!activeWeapon) return;

        var sig = BuildSignature();
        if (sig != _signature)
        {
            _signature = sig;
            WeaponRuntimeSave.SaveSnapshot(activeWeapon); // TỰ LƯU khi thay đổi
#if UNITY_EDITOR
            // Debug.Log("[WeaponAutoPersist] Auto-saved.");
#endif
        }
    }

    private void OnApplicationQuit()
    {
        if (clearOnQuit) WeaponRuntimeSave.Clear();
    }

#if UNITY_EDITOR
    // Khi dừng Play trong Editor, OnApplicationQuit không luôn chạy -> dọn ở đây nữa
    private void OnDisable()
    {
        if (!Application.isPlaying) return;
        if (clearOnQuit) WeaponRuntimeSave.Clear();
    }
#endif

    private string BuildSignature()
    {
        if (!activeWeapon) return "";

        var names = activeWeapon.Equipped_Weapons
                    .Where(w => w != null)
                    .Select(w => $"{w.WeaponName}:{w.GetCurrentAmmour()}")
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

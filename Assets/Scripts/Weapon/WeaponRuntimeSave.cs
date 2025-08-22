using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[Serializable]
public class SavedWeapon
{
    public string weaponName;
    public int currentAmmo;
    public int maxAmmo;
    public int index;        // thứ tự trong list
    public bool isActive;
}

[Serializable]
public class SavedLoadout
{
    public List<SavedWeapon> weapons = new();
    public int activeIndex = -1;
}

public static class WeaponRuntimeSave
{
    private const string FileName = "session_loadout.json";

    private static string PathFull =>
        System.IO.Path.Combine(Application.persistentDataPath, FileName);

    public static void Clear()
    {
        try
        {
            if (File.Exists(PathFull)) File.Delete(PathFull);
#if UNITY_EDITOR
            Debug.Log($"[WeaponRuntimeSave] Cleared: {PathFull}");
#endif
        }
        catch (Exception e) { Debug.LogWarning(e); }
    }

    public static void SaveSnapshot(ActiveWeapon aw)
    {
        if (aw == null) return;

        var data = new SavedLoadout();
        for (int i = 0; i < aw.Equipped_Weapons.Count; i++)
        {
            var w = aw.Equipped_Weapons[i];
            if (w == null) continue;

            data.weapons.Add(new SavedWeapon
            {
                weaponName = w.WeaponName,
                currentAmmo = w.GetCurrentAmmour(),
                maxAmmo = w.GetMaxAmmour(),
                index = i,
                isActive = (aw.activeGun == w)
            });
        }

        data.activeIndex = Mathf.Clamp(
            aw.Equipped_Weapons.IndexOf(aw.activeGun), -1, aw.Equipped_Weapons.Count - 1);

        try
        {
            var json = JsonUtility.ToJson(data, true);
            File.WriteAllText(PathFull, json);
#if UNITY_EDITOR
            // Debug.Log($"[WeaponRuntimeSave] Saved:\n{json}");
#endif
        }
        catch (Exception e) { Debug.LogWarning(e); }
    }

    public static bool TryLoad(out SavedLoadout data)
    {
        data = null;
        try
        {
            if (!File.Exists(PathFull)) return false;
            var json = File.ReadAllText(PathFull);
            data = JsonUtility.FromJson<SavedLoadout>(json);
            return data != null;
        }
        catch (Exception e) { Debug.LogWarning(e); return false; }
    }

    /// <summary>
    /// Dùng WeaponRegistry để dựng lại vũ khí y như lần trước.
    /// Không yêu cầu ActiveWeapon sửa code.
    /// </summary>
    public static void ApplyTo(ActiveWeapon aw, WeaponRegistry reg, SavedLoadout data)
    {
        if (aw == null || reg == null || data == null) return;

        // Dọn inventory hiện tại
        var copy = aw.Equipped_Weapons.ToList();
        foreach (var w in copy) if (w) UnityEngine.Object.Destroy(w.gameObject);
        aw.Equipped_Weapons.Clear();

        // Khôi phục theo thứ tự index tăng dần
        foreach (var sw in data.weapons.OrderBy(x => x.index))
        {
            var pf = reg.GetPrefab(sw.weaponName);
            if (pf == null) { Debug.LogWarning($"[WeaponRuntimeSave] Missing prefab: {sw.weaponName}"); continue; }

            var newW = UnityEngine.Object.Instantiate(pf);
            aw.Equip(newW);
            // Khôi phục ammo hiện tại (nếu class bạn có API này)
            newW.SetCurrentAmmo(Mathf.Clamp(sw.currentAmmo, 0, newW.GetMaxAmmour()));
        }

        // Chọn active
        if (data.activeIndex >= 0 && data.activeIndex < aw.Equipped_Weapons.Count)
        {
            // gọi SetActivateWeapon bằng SendMessage để không đổi access modifier
            aw.gameObject.SendMessage("SetActivateWeapon", data.activeIndex, SendMessageOptions.DontRequireReceiver);
        }
        else if (aw.Equipped_Weapons.Count > 0)
        {
            aw.gameObject.SendMessage("SetActivateWeapon", 0, SendMessageOptions.DontRequireReceiver);
        }
    }
}

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
    public int index;
}

[Serializable]
public class SavedLoadout
{
    public List<SavedWeapon> weapons = new();
    public int activeIndex = -1; // index khẩu đang cầm tại thời điểm lưu
}

public static class WeaponRuntimeSave
{
    private const string FileName = "session_loadout.json";
    private static string PathFull => Path.Combine(Application.persistentDataPath, FileName);

    public static void Clear()
    {
        if (File.Exists(PathFull)) File.Delete(PathFull);
    }

    public static void SaveSnapshot(ActiveWeapon aw)
    {
        if (!aw) return;
        var data = new SavedLoadout();

        for (int i = 0; i < aw.Equipped_Weapons.Count; i++)
        {
            var w = aw.Equipped_Weapons[i];
            if (!w) continue;
            data.weapons.Add(new SavedWeapon
            {
                weaponName = w.WeaponName,
                currentAmmo = w.GetCurrentAmmour(),
                index = i
            });
        }
        data.activeIndex = Mathf.Clamp(aw.Equipped_Weapons.IndexOf(aw.activeGun), -1, aw.Equipped_Weapons.Count - 1);

        File.WriteAllText(PathFull, JsonUtility.ToJson(data, true));
    }

    public static bool TryLoad(out SavedLoadout data)
    {
        data = null;
        if (!File.Exists(PathFull)) return false;
        data = JsonUtility.FromJson<SavedLoadout>(File.ReadAllText(PathFull));
        return data != null;
    }

    /// Re‑equip: dựng lại loadout và chọn đúng khẩu active theo save.
    public static void ApplyTo(ActiveWeapon aw, WeaponRegistry reg, SavedLoadout data)
    {
        if (!aw || !reg || data == null) return;

        // dọn inventory
        foreach (var w in aw.Equipped_Weapons) if (w) UnityEngine.Object.Destroy(w.gameObject);
        aw.Equipped_Weapons.Clear();

        // dựng lại theo thứ tự
        foreach (var sw in data.weapons.OrderBy(x => x.index))
        {
            var pf = reg.GetPrefab(sw.weaponName);
            if (!pf) { Debug.LogWarning($"[Loadout] Missing prefab: {sw.weaponName}"); continue; }

            var newW = UnityEngine.Object.Instantiate(pf);
            aw.Equip(newW);
            // khôi phục băng đạn hiện tại (hàm này bạn đã có trong RayCastWeapon)
            newW.SetCurrentAmmo(sw.currentAmmo);
        }

        // chọn đúng khẩu active
        if (data.activeIndex >= 0 && data.activeIndex < aw.Equipped_Weapons.Count)
            aw.gameObject.SendMessage("SetActivateWeapon", data.activeIndex, SendMessageOptions.DontRequireReceiver);
        else if (aw.Equipped_Weapons.Count > 0)
            aw.gameObject.SendMessage("SetActivateWeapon", 0, SendMessageOptions.DontRequireReceiver);
    }
}

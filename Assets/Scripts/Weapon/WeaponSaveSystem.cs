using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class WeaponState
{
    public string weaponName;
    public int slotIndex;
    public int currentAmmo;
    public int totalAmmo;
    public bool isActive;
}

[Serializable]
public class ActiveWeaponState
{
    public List<WeaponState> equipped = new();
    public int activeIndex = -1;
    public bool isHolstered = false;
    public List<string> pickedUpIds = new();
    public string wishedWeaponName;
}

public static class WeaponSaveSystem
{
    public static string SavePath(string saveId)
    {
        var dir = Path.Combine(Application.persistentDataPath, "Saves");
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        return Path.Combine(dir, $"weapons_{saveId}.json");
    }

    public static void Save(string saveId, ActiveWeaponState data)
    {
        var json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath(saveId), json);
#if UNITY_EDITOR
        Debug.Log($"[Save] {SavePath(saveId)}:\n{json}");
#endif
    }

    public static bool TryLoad(string saveId, out ActiveWeaponState data)
    {
        var path = SavePath(saveId);
        if (File.Exists(path))
        {
            var json = File.ReadAllText(path);
            data = JsonUtility.FromJson<ActiveWeaponState>(json);
            return true;
        }
        data = null;
        return false;
    }
}

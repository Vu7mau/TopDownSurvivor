// File: CheckpointStore.cs
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CPItem
{
    public string name;
    public int mapIndex;
    public float[] p;   // x,y,z
    public float[] r;   // euler x,y,z
    public bool isAuto; // true = autosave (spawn), false = manual

    public Vector3 Pos => new Vector3(p[0], p[1], p[2]);
    public Quaternion Rot => Quaternion.Euler(r[0], r[1], r[2]);
}

[Serializable]
class CPList
{
    public List<CPItem> items = new List<CPItem>();
    public int index = -1;
}

public static class CheckpointStore
{
    private const string KEY = "cp_list_v2_single_scene";
    private static CPList _cache;

    public static event Action OnChanged;

    private static CPList Data
    {
        get
        {
            if (_cache != null) return _cache;
            if (!PlayerPrefs.HasKey(KEY)) { _cache = new CPList(); return _cache; }
            _cache = JsonUtility.FromJson<CPList>(PlayerPrefs.GetString(KEY)) ?? new CPList();
            Reindex();
            return _cache;
        }
    }

    private static void Flush()
    {
        PlayerPrefs.SetString(KEY, JsonUtility.ToJson(Data));
        PlayerPrefs.Save();
        OnChanged?.Invoke();
    }

    private static void Reindex()
    {
        _cache.index = (_cache.items.Count > 0) ? Mathf.Clamp(_cache.index, 0, _cache.items.Count - 1) : -1;
    }

    public static int Count => Data.items.Count;
    public static int CurrentIndex => Data.index;
    public static bool HasAny() => Count > 0;

    public static void Add(Transform t, int mapIndex, string name = null, bool isAuto = false)
    {
        if (!t) return;
        var cp = new CPItem
        {
            name = string.IsNullOrEmpty(name) ? DateTime.Now.ToString("HH:mm:ss dd/MM") : name,
            mapIndex = mapIndex,
            p = new[] { t.position.x, t.position.y, t.position.z },
            r = new[] { t.eulerAngles.x, t.eulerAngles.y, t.eulerAngles.z },
            isAuto = isAuto
        };
        Data.items.Add(cp);
        Data.index = Data.items.Count - 1;
        Flush();
        Debug.Log($"[CheckpointStore] Added #{Data.index} @map {mapIndex}: {cp.name} (auto={isAuto})");
    }

    public static bool MoveIndex(int delta)
    {
        if (!HasAny()) return false;
        int n = Count;
        Data.index = (Data.index + delta) % n;
        if (Data.index < 0) Data.index += n;
        Flush();
        return true;
    }

    public static bool TryGetCurrent(out CPItem meta)
    {
        meta = null;
        if (!HasAny() || Data.index < 0 || Data.index >= Count) return false;
        meta = Data.items[Data.index];
        return true;
    }

    // ===== Clear helpers =====
    public static int ClearManualByMap(int mapIndex) => ClearByFilter(cp => cp.mapIndex == mapIndex && !cp.isAuto);
    public static int ClearSpawnByMap(int mapIndex) => ClearByFilter(cp => cp.mapIndex == mapIndex && cp.isAuto);
    public static int ClearAllByMap(int mapIndex) => ClearByFilter(cp => cp.mapIndex == mapIndex);
    public static int ClearAllSpawn() => ClearByFilter(cp => cp.isAuto);
    public static int ClearManual() => ClearByFilter(cp => !cp.isAuto);

    public static void ClearAll()
    {
        Data.items.Clear();
        Reindex();
        Flush();
        Debug.Log("[CheckpointStore] Cleared all.");
    }

    public static int ClearByFilter(Predicate<CPItem> pred)
    {
        int before = Count;
        Data.items.RemoveAll(pred);
        Reindex();
        if (before != Count) Flush();
        return before - Count;
    }

    // ===== Per-map queries for slider/checkpoint UI =====
    public static int GetCountByMap(int mapIndex, bool? autoFlag = null)
    {
        int c = 0;
        foreach (var cp in Data.items)
            if (cp.mapIndex == mapIndex && (!autoFlag.HasValue || cp.isAuto == autoFlag.Value)) c++;
        return c;
    }

    public static List<int> GetAbsoluteIndexesOfMap(int mapIndex, bool? autoFlag = null)
    {
        var list = new List<int>();
        for (int i = 0; i < Data.items.Count; i++)
        {
            var cp = Data.items[i];
            if (cp.mapIndex == mapIndex && (!autoFlag.HasValue || cp.isAuto == autoFlag.Value))
                list.Add(i);
        }
        return list;
    }

    public static bool SetCurrentIndexSafe(int absoluteIndex)
    {
        if (absoluteIndex < 0 || absoluteIndex >= Data.items.Count) return false;
        Data.index = absoluteIndex;
        Flush();
        return true;
    }
}

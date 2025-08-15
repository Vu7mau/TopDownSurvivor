// File: CheckpointStore.cs
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CPItem
{
    public string name;     // nhãn gợi nhớ
    public int mapIndex;    // map chứa checkpoint
    public float[] p;       // world position x,y,z
    public float[] r;       // world euler x,y,z

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

    public static event Action OnChanged; // HUD / hotkeys có thể nghe

    private static CPList Data
    {
        get
        {
            if (_cache != null) return _cache;
            if (!PlayerPrefs.HasKey(KEY)) { _cache = new CPList(); return _cache; }
            _cache = JsonUtility.FromJson<CPList>(PlayerPrefs.GetString(KEY)) ?? new CPList();
            _cache.index = Mathf.Clamp(_cache.index, _cache.items.Count > 0 ? 0 : -1, _cache.items.Count - 1);
            return _cache;
        }
    }

    private static void Flush()
    {
        PlayerPrefs.SetString(KEY, JsonUtility.ToJson(Data));
        PlayerPrefs.Save();
        OnChanged?.Invoke();
    }

    public static int Count => Data.items.Count;
    public static int CurrentIndex => Data.index;
    public static bool HasAny() => Count > 0;

    public static void Add(Transform t, int mapIndex, string name = null)
    {
        if (!t) return;
        var cp = new CPItem
        {
            name = string.IsNullOrEmpty(name) ? DateTime.Now.ToString("HH:mm:ss dd/MM") : name,
            mapIndex = mapIndex,
            p = new[] { t.position.x, t.position.y, t.position.z },
            r = new[] { t.eulerAngles.x, t.eulerAngles.y, t.eulerAngles.z },
        };
        Data.items.Add(cp);
        Data.index = Data.items.Count - 1;
        Flush();
        Debug.Log($"[CheckpointStore] Added #{Data.index} @map {mapIndex}: {cp.name}");
    }

    /// Di chuyển con trỏ (delta=+1 next, -1 prev), có vòng.
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

    public static void ClearAll()
    {
        _cache = new CPList();
        PlayerPrefs.DeleteKey(KEY);
        OnChanged?.Invoke();
        Debug.Log("[CheckpointStore] Cleared all.");
    }
}

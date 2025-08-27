// File: CheckpointStore.cs
using System;
using System.Collections.Generic;
using UnityEngine;

#region Data Models
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

    public static CPItem From(Transform t, int mapIndex, string name, bool isAuto)
    {
        var e = t.eulerAngles;
        var p3 = t.position;
        return new CPItem
        {
            name = string.IsNullOrEmpty(name) ? DateTime.Now.ToString("HH:mm:ss dd/MM") : name,
            mapIndex = mapIndex,
            p = new[] { p3.x, p3.y, p3.z },
            r = new[] { e.x, e.y, e.z },
            isAuto = isAuto
        };
    }
}

[Serializable]
class CPList
{
    public int version = 3; // để dành cho migration nếu cần
    public List<CPItem> items = new List<CPItem>();
    public int index = -1;
}
#endregion

public static class CheckpointStore
{
    // Đổi key để tách biệt với bản cũ, chứa tất cả map.
    private const string KEY = "checkpoint_store_v3_all_maps";

    private static CPList _cache;
    public static event Action OnChanged;

    #region Runtime auto-clear hook
    // Gắn hook tự clear khi thoát game / app bị đưa nền (Editor & Build).
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InstallQuitHook()
    {
        var go = new GameObject("CheckpointStoreQuitHook");
        go.hideFlags = HideFlags.HideAndDontSave;
        UnityEngine.Object.DontDestroyOnLoad(go);
        go.AddComponent<_QuitHook>();
    }

    private class _QuitHook : MonoBehaviour
    {
        private bool _cleared = false;

        private void OnApplicationQuit()
        {
            TryClearOnce("OnApplicationQuit");
        }

#if !UNITY_EDITOR
        private void OnApplicationPause(bool pause)
        {
            if (pause) TryClearOnce("OnApplicationPause(true)");
        }
#endif

        private void TryClearOnce(string src)
        {
            if (_cleared) return;
            _cleared = true;
            try { CheckpointStore.ClearAll(); }
            catch (Exception ex)
            {
#if UNITY_EDITOR
                Debug.LogWarning($"[CheckpointStore] Auto clear failed ({src}): {ex.Message}");
#endif
            }
        }
    }
    #endregion

    #region Core persistence
    private static CPList Data
    {
        get
        {
            if (_cache != null) return _cache;

            if (!PlayerPrefs.HasKey(KEY))
            {
                _cache = new CPList();
                return _cache;
            }

            try
            {
                var json = PlayerPrefs.GetString(KEY);
                _cache = JsonUtility.FromJson<CPList>(json) ?? new CPList();
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[CheckpointStore] JSON parse error, reset store. {e.Message}");
                _cache = new CPList();
            }

            Reindex();
            return _cache;
        }
    }

    private static void Flush()
    {
        try
        {
            var json = JsonUtility.ToJson(Data);
            PlayerPrefs.SetString(KEY, json);
            PlayerPrefs.Save();
        }
        catch (Exception e)
        {
            Debug.LogWarning($"[CheckpointStore] Flush failed: {e.Message}");
        }

        OnChanged?.Invoke();
    }

    private static void Reindex()
    {
        _cache.index = (_cache.items.Count > 0)
            ? Mathf.Clamp(_cache.index, 0, _cache.items.Count - 1)
            : -1;
    }
    #endregion

    #region Public API (queries/state)
    public static int Count => Data.items.Count;
    public static int CurrentIndex => Data.index;
    public static bool HasAny() => Count > 0;

    public static bool TryGetCurrent(out CPItem meta)
    {
        meta = null;
        if (!HasAny() || Data.index < 0 || Data.index >= Count) return false;
        meta = Data.items[Data.index];
        return true;
    }

    public static bool SetCurrentIndexSafe(int absoluteIndex)
    {
        if (absoluteIndex < 0 || absoluteIndex >= Data.items.Count) return false;
        Data.index = absoluteIndex;
        Flush();
        return true;
    }

    public static int GetCountByMap(int mapIndex, bool? autoFlag = null)
    {
        int c = 0;
        var list = Data.items;
        for (int i = 0; i < list.Count; i++)
        {
            var cp = list[i];
            if (cp.mapIndex == mapIndex && (!autoFlag.HasValue || cp.isAuto == autoFlag.Value))
                c++;
        }
        return c;
    }

    public static List<int> GetAbsoluteIndexesOfMap(int mapIndex, bool? autoFlag = null)
    {
        var res = new List<int>();
        var list = Data.items;
        for (int i = 0; i < list.Count; i++)
        {
            var cp = list[i];
            if (cp.mapIndex == mapIndex && (!autoFlag.HasValue || cp.isAuto == autoFlag.Value))
                res.Add(i);
        }
        return res;
    }
    #endregion

    #region Public API (mutations)
    public static void Add(Transform t, int mapIndex, string name = null, bool isAuto = false)
    {
        if (!t) return;
        var cp = CPItem.From(t, mapIndex, name, isAuto);
        Data.items.Add(cp);
        Data.index = Data.items.Count - 1;
        Flush();

#if UNITY_EDITOR
        Debug.Log($"[CheckpointStore] Added #{Data.index} @map {mapIndex}: {cp.name} (auto={isAuto})");
#endif
    }

    public static bool MoveIndex(int delta)
    {
        if (!HasAny()) return false;
        int n = Count;
        int next = (Data.index + delta) % n;
        if (next < 0) next += n;
        Data.index = next;
        Flush();
        return true;
    }

    // ===== Clear helpers =====
    public static int ClearManualByMap(int mapIndex) => ClearByFilter(cp => cp.mapIndex == mapIndex && !cp.isAuto);
    public static int ClearSpawnByMap(int mapIndex) => ClearByFilter(cp => cp.mapIndex == mapIndex && cp.isAuto);
    public static int ClearAllByMap(int mapIndex) => ClearByFilter(cp => cp.mapIndex == mapIndex);
    public static int ClearAllSpawn() => ClearByFilter(cp => cp.isAuto);
    public static int ClearManual() => ClearByFilter(cp => !cp.isAuto);

    /// <summary>
    /// Xoá toàn bộ checkpoint của TẤT CẢ các map + reset index. Trả về số lượng CP đã xoá.
    /// </summary>
    public static int ClearAll()
    {
        int removed = Count;

        // Reset bộ nhớ
        _cache = new CPList();

        // Xoá luôn key để chắc chắn không “hồi sinh” ở lần chạy sau
        try
        {
            PlayerPrefs.DeleteKey(KEY);
            PlayerPrefs.Save();
        }
        catch (Exception e)
        {
            Debug.LogWarning($"[CheckpointStore] DeleteKey failed: {e.Message}. Fallback to Flush().");
            Flush(); // fallback
        }

        OnChanged?.Invoke();

#if UNITY_EDITOR
        Debug.Log($"[CheckpointStore] Cleared ALL. removed={removed}");
#endif
        return removed;
    }

    public static int ClearByFilter(Predicate<CPItem> pred)
    {
        int before = Count;
        if (before == 0) return 0;

        Data.items.RemoveAll(pred);
        Reindex();

        if (before != Count) Flush();
        return before - Count;
    }
    #endregion

    #region Optional: export/import (hữu ích khi debug)
    public static string ExportJson()
    {
        return JsonUtility.ToJson(Data);
    }

    public static bool ImportJsonReplaceAll(string json)
    {
        if (string.IsNullOrEmpty(json)) return false;
        try
        {
            var obj = JsonUtility.FromJson<CPList>(json);
            if (obj == null) return false;
            _cache = obj;
            Reindex();
            Flush();
            return true;
        }
        catch (Exception e)
        {
            Debug.LogWarning($"[CheckpointStore] Import failed: {e.Message}");
            return false;
        }
    }
    #endregion
}

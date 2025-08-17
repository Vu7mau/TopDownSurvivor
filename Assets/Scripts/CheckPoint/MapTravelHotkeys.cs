// File: MapSelectHotkeys.cs
using System.Linq;
using UnityEngine;

public class MapSelectHotkeys : MonoBehaviour
{
    [Header("Keys")]
    [Tooltip("Chọn map kế (không chuyển ngay)")]
    public KeyCode selectNextKey = KeyCode.RightBracket;   // ]
    [Tooltip("Chọn map trước (không chuyển ngay)")]
    public KeyCode selectPrevKey = KeyCode.LeftBracket;    // [
    [Tooltip("Thực hiện chuyển sang map đang chọn (spawn)")]
    public KeyCode goKey = KeyCode.Backslash;              // \

    [Header("Options")]
    [Tooltip("Tự đồng bộ map đang chọn theo map hiện tại khi bật script")]
    public bool syncToCurrentOnEnable = true;

    private GameController GC => GameController.Instance;
    private int _selectedMapIndex = -1;

    private void OnEnable()
    {
        if (GC == null) return;

        if (syncToCurrentOnEnable)
            _selectedMapIndex = GC.CurrentMapIndex;
        else if (_selectedMapIndex < 0)
            _selectedMapIndex = Mathf.Clamp(GC.CurrentMapIndex, 0, Mathf.Max(0, GC.MapsCount - 1));

        GC.OnMapSwitched += HandleMapSwitched;
    }

    private void OnDisable()
    {
        if (GC != null)
            GC.OnMapSwitched -= HandleMapSwitched;
    }

    private void Update()
    {
        if (GC == null || GC.MapsCount <= 0) return;

        if (Input.GetKeyDown(selectNextKey)) SelectDelta(+1);
        if (Input.GetKeyDown(selectPrevKey)) SelectDelta(-1);
        if (Input.GetKeyDown(goKey)) GoToSelectedMap();
    }

    private void HandleMapSwitched()
    {
        if (GC == null) return;
        _selectedMapIndex = GC.CurrentMapIndex;
        NotifySelected(_selectedMapIndex);
    }

    private void SelectDelta(int delta)
    {
        int n = GC.MapsCount;
        if (n <= 0) return;

        if (_selectedMapIndex < 0) _selectedMapIndex = GC.CurrentMapIndex;

        _selectedMapIndex = (_selectedMapIndex + delta) % n;
        if (_selectedMapIndex < 0) _selectedMapIndex += n;

        NotifySelected(_selectedMapIndex);
    }

    private void GoToSelectedMap()
    {
        if (GC == null || _selectedMapIndex < 0 || _selectedMapIndex >= GC.MapsCount) return;

        var (name, idx) = GetMapNameAndIndex(_selectedMapIndex);
        ChatNotify.Instance?.MapJumping(idx, name);
        GC.GoToMapSpawn(idx);
        // ChatNotify.MapSwitched sẽ được gọi từ GameController sau khi chuyển xong
    }

    private void NotifySelected(int idx)
    {
        var (name, i) = GetMapNameAndIndex(idx);
        ChatNotify.Instance?.MapSelected(i, name);
    }

    private (string mapName, int index) GetMapNameAndIndex(int idx)
    {
        string mapName = $"Map {idx}";
        int index = idx;

        if (GC != null)
        {
            var maps = GC.transform.GetComponentsInChildren<Map_Controller>(true);
            var m = maps.FirstOrDefault(x => x.MapIndex == idx);
            if (m != null)
                mapName = m.map ? m.map.name : m.gameObject.name;
        }

        return (mapName, index);
    }
}

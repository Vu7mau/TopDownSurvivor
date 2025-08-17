// File: CheckpointHotkeys.cs
using System.Collections;
using UnityEngine;

public class CheckpointHotkeys : MonoBehaviour
{
    [Header("Keys")]
    public KeyCode addKey = KeyCode.F5;
    public KeyCode nextKey = KeyCode.F6;
    public KeyCode prevKey = KeyCode.F7;
    public KeyCode goKey = KeyCode.F9;
    public KeyCode clearKey = KeyCode.None; // ví dụ F12

    [Header("Options")]
    public bool fadeWhenTeleport = true;

    [Header("Clear Behavior")]
    [Tooltip("Khoảng thời gian nhấn lần 2 để xoá toàn bộ CP của map hiện tại + PositionSave")]
    public float clearDoublePressWindow = 0.9f;

    private float _lastClearTime = -999f;
    private bool _primedClear = false;

    private GameController GC => GameController.Instance;
    private ChatNotify N => ChatNotify.Instance;

    private void Update()
    {
        if (Input.GetKeyDown(addKey)) AddCheckpointHere();
        if (Input.GetKeyDown(nextKey)) SelectDelta(+1);
        if (Input.GetKeyDown(prevKey)) SelectDelta(-1);
        if (Input.GetKeyDown(goKey)) JumpToCurrent();
        if (clearKey != KeyCode.None && Input.GetKeyDown(clearKey)) ClearSmart();
    }

    private void AddCheckpointHere()
    {
        if (!GC || !GC.Character || !GC.CurrentMap)
        {
            N?.Error("Không thể lưu: thiếu Character/CurrentMap.");
            return;
        }
        CheckpointStore.Add(GC.Character, GC.CurrentMap.MapIndex, isAuto: false);

        if (CheckpointStore.TryGetCurrent(out var meta))
            N?.Added(CheckpointStore.CurrentIndex, CheckpointStore.Count, meta.mapIndex, meta.name);
    }

    private void SelectDelta(int delta)
    {
        if (!CheckpointStore.MoveIndex(delta))
        {
            N?.Error("Chưa có checkpoint nào.");
            return;
        }
        if (CheckpointStore.TryGetCurrent(out var meta))
            N?.Selected(CheckpointStore.CurrentIndex, CheckpointStore.Count, meta.mapIndex, meta.name);
    }

    private void JumpToCurrent()
    {
        if (!GC) { N?.Error("Không tìm thấy GameController."); return; }
        if (!CheckpointStore.TryGetCurrent(out var meta))
        {
            N?.Error("Chưa chọn checkpoint.");
            return;
        }

        if (GC.CurrentMap == null || GC.CurrentMap.MapIndex != meta.mapIndex)
            StartCoroutine(JumpCrossMap(meta));
        else
            StartCoroutine(JumpSameMap(meta));
    }

    private IEnumerator JumpSameMap(CPItem meta)
    {
        if (fadeWhenTeleport)
        {
            GC.ScreenFadeOut();
            if (GC.FadeDuration > 0f) yield return new WaitForSeconds(GC.FadeDuration);
        }
        GC.TeleportSafe(meta.Pos, meta.Rot);
        yield return null;
        if (fadeWhenTeleport) GC.ScreenFadeIn();

        N?.Jumped(meta.mapIndex, meta.name);
    }

    private IEnumerator JumpCrossMap(CPItem meta)
    {
        GC.SwitchMap(meta.mapIndex);
        if (GC.FadeDuration > 0f) yield return new WaitForSeconds(GC.FadeDuration + 0.05f);

        if (fadeWhenTeleport)
        {
            GC.ScreenFadeOut();
            if (GC.FadeDuration > 0f) yield return new WaitForSeconds(GC.FadeDuration);
        }
        GC.TeleportSafe(meta.Pos, meta.Rot);
        yield return null;
        if (fadeWhenTeleport) GC.ScreenFadeIn();

        N?.Jumped(meta.mapIndex, meta.name);
    }

    // ====== Clear: lần 1 manual, lần 2 tất cả (map hiện tại) + xoá PositionSave ======
    private void ClearSmart()
    {
        if (!CheckpointStore.HasAny())
        {
            N?.Error("Không có gì để xoá.");
            return;
        }

        int mapIdx = GC && GC.CurrentMap ? GC.CurrentMap.MapIndex : -1;
        if (mapIdx < 0)
        {
            N?.Error("Không xác định được map hiện tại.");
            return;
        }

        float now = Time.time;
        if (_primedClear && now - _lastClearTime <= clearDoublePressWindow)
        {
            _primedClear = false;

            int removed = CheckpointStore.ClearAllByMap(mapIdx); // xoá manual + spawn của map hiện tại
            PositionSave.Clear(); // xoá vị trí đơn lẻ

            if (removed > 0) N?.Info($"Đã xoá {removed} checkpoint của Map {mapIdx} và xoá vị trí đã lưu.");
            else N?.Info($"Map {mapIdx} không còn checkpoint. Đã xoá vị trí đã lưu.");
        }
        else
        {
            _primedClear = true; _lastClearTime = now;

            int removedManual = CheckpointStore.ClearManualByMap(mapIdx);
            if (removedManual > 0)
                N?.Info($"Đã xoá {removedManual} checkpoint (manual) của Map {mapIdx}. Nhấn xoá lần nữa để xoá TẤT CẢ của map + vị trí đã lưu.");
            else
                N?.Info($"Map {mapIdx} không có checkpoint (manual). Nhấn xoá lần nữa để xoá TẤT CẢ của map + vị trí đã lưu.");

            StartCoroutine(ClearPrimeTimeout());
        }
    }

    private IEnumerator ClearPrimeTimeout()
    {
        yield return new WaitForSeconds(clearDoublePressWindow);
        _primedClear = false;
    }
}

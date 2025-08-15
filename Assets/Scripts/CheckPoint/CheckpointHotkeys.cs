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

    private GameController GC => GameController.Instance;
    private ChatNotify N => ChatNotify.Instance;

    private void Update()
    {
        if (Input.GetKeyDown(addKey)) AddCheckpointHere();
        if (Input.GetKeyDown(nextKey)) SelectDelta(+1);
        if (Input.GetKeyDown(prevKey)) SelectDelta(-1);
        if (Input.GetKeyDown(goKey)) JumpToCurrent();
        if (clearKey != KeyCode.None && Input.GetKeyDown(clearKey)) ClearAll();
    }

    private void AddCheckpointHere()
    {
        if (!GC || !GC.Character || !GC.CurrentMap)
        {
            N?.Error("Không thể lưu: thiếu Character/CurrentMap.");
            return;
        }
        CheckpointStore.Add(GC.Character, GC.CurrentMap.MapIndex);

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

    private void ClearAll()
    {
        if (!CheckpointStore.HasAny())
        {
            N?.Error("Không có gì để xoá.");
            return;
        }
        CheckpointStore.ClearAll();
        N?.Cleared();
    }
}

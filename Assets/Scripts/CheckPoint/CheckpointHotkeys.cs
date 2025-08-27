// File: CheckpointHotkeys.cs
using System.Collections;
using UnityEngine;

public class CheckpointHotkeys : MonoBehaviour
{
    [Header("Keys")]
    [SerializeField] private KeyCode addKey = KeyCode.F5;
    [SerializeField] private KeyCode nextKey = KeyCode.F6;
    [SerializeField] private KeyCode prevKey = KeyCode.F7;
    [SerializeField] private KeyCode goKey = KeyCode.F9;

    [Header("Delete/Clear (Global)")]
    [Tooltip("Phím xoá toàn bộ checkpoint của TẤT CẢ các map + PositionSave.")]
    [SerializeField] private KeyCode clearKey = KeyCode.None; // ví dụ: KeyCode.F12

    [Header("Options")]
    [SerializeField] private bool fadeWhenTeleport = true;

    [Tooltip("Bảo đảm nhân vật luôn được bật/enable khi dịch chuyển, đặc biệt khi cross-map.")]
    [SerializeField] private bool ensureCharacterActive = true;

    [Tooltip("Timeout chờ map mới load xong (giây) khi cross-map).")]
    [SerializeField] private float waitMapLoadedTimeout = 5f;

    // state
    private bool _isShuttingDown = false;

    private GameController GC => GameController.Instance;
    private ChatNotify N => ChatNotify.Instance;

    private void OnEnable()
    {
        // Đảm bảo dọn dẹp khi thoát game (Editor & Build)
        Application.quitting += OnAppQuitting;
    }

    private void OnDisable()
    {
        Application.quitting -= OnAppQuitting;
    }

    private void OnApplicationQuit()
    {
        // Backup để chắc chắn (một số platform chỉ gọi OnApplicationQuit hoặc chỉ gọi Application.quitting)
        SafeClearAll("OnApplicationQuit");
    }

    private void OnApplicationPause(bool pause)
    {
#if !UNITY_EDITOR
        // Trên mobile, khi app bị đưa về nền (pause=true) coi như sẽ thoát -> dọn luôn
        if (pause) SafeClearAll("OnApplicationPause(true)");
#endif
    }

    private void OnAppQuitting()
    {
        SafeClearAll("Application.quitting");
    }

    private void Update()
    {
        if (Input.GetKeyDown(addKey)) AddCheckpointHere();
        if (Input.GetKeyDown(nextKey)) SelectDelta(+1);
        if (Input.GetKeyDown(prevKey)) SelectDelta(-1);
        if (Input.GetKeyDown(goKey)) JumpToCurrent();

        if (clearKey != KeyCode.None && Input.GetKeyDown(clearKey))
        {
            // YÊU CẦU: delete phải xoá toàn bộ ở tất cả map
            SafeClearAll("Hotkey");
        }
    }

    // ================= Core gameplay =================
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

        if (ensureCharacterActive) EnsureCharacterActive();

        GC.TeleportSafe(meta.Pos, meta.Rot);
        yield return null;

        if (fadeWhenTeleport) GC.ScreenFadeIn();

        N?.Jumped(meta.mapIndex, meta.name);
    }

    private IEnumerator JumpCrossMap(CPItem meta)
    {
        if (fadeWhenTeleport)
        {
            // Fade out TRƯỚC khi chuyển map để tránh lộ khung hình
            GC.ScreenFadeOut();
            if (GC.FadeDuration > 0f) yield return new WaitForSeconds(GC.FadeDuration);
        }

        // Chuyển map
        GC.SwitchMap(meta.mapIndex);

        // Đợi map mới load xong (hoặc đến khi CurrentMap đúng index)
        float t = 0f;
        while (t < waitMapLoadedTimeout)
        {
            if (GC.CurrentMap != null && GC.CurrentMap.MapIndex == meta.mapIndex)
                break;
            t += Time.unscaledDeltaTime;
            yield return null;
        }
        // Thêm 1 frame để các object trong scene ổn định
        yield return null;

        // Đảm bảo bật Player
        if (ensureCharacterActive) EnsureCharacterActive();

        // Teleport đến vị trí checkpoint trong map mới
        GC.TeleportSafe(meta.Pos, meta.Rot);
        yield return null;

        if (fadeWhenTeleport) GC.ScreenFadeIn();

        N?.Jumped(meta.mapIndex, meta.name);
    }

    // Bật Player và các component quan trọng nếu đang bị tắt/disable
    private void EnsureCharacterActive()
    {
        if (!GC || !GC.Character) return;

        var go = GC.Character.gameObject;

        if (!go.activeSelf) go.SetActive(true);

        // Enable các component thường bị disable khi load/chuyển map
        var cc = go.GetComponent<CharacterController>();
        if (cc && !cc.enabled) cc.enabled = true;

        var agent = go.GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent && !agent.enabled) agent.enabled = true;

        var anim = go.GetComponentInChildren<Animator>(true);
        if (anim && !anim.enabled) anim.enabled = true;

        // Wake up physics nếu có
        var rbs = go.GetComponentsInChildren<Rigidbody>(true);
        foreach (var rb in rbs) { if (rb != null) rb.WakeUp(); }
    }

    // ================= Global clear =================
    /// <summary>
    /// Xoá toàn bộ checkpoint của TẤT CẢ các map + xoá PositionSave.
    /// Dùng nơi khác nhau (hotkey / quitting / pause) và an toàn khi shutdown.
    /// </summary>
    private void SafeClearAll(string source)
    {
        if (_isShuttingDown) return;
        _isShuttingDown = true;

        try
        {
            // BẮT BUỘC: cần có CheckpointStore.ClearAll() xóa hết mọi map và reset index.
            int removed = CheckpointStore.ClearAll();

            // Xoá PositionSave (toàn cục)
            PositionSave.Clear();

            // Thông báo (nếu còn tồn tại)
            N?.Cleared();

#if UNITY_EDITOR
            Debug.Log($"[CheckpointHotkeys] ClearAll from {source}: removed={removed}");
#endif
        }
        catch (System.Exception ex)
        {
#if UNITY_EDITOR
            Debug.LogWarning($"[CheckpointHotkeys] ClearAll failed ({source}): {ex.Message}");
#endif
        }
    }
}

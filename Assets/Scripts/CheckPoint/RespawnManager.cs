// File: RespawnManager.cs
using System.Collections;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager Instance { get; private set; }

    private GameController GC => GameController.Instance;

    private void Awake()
    {
        if (Instance && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// Gọi từ Health của bạn khi chết.
    public void RespawnNow()
    {
        if (!GC) return;

        StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        // nếu có checkpoint hiện tại → chuyển tới map của checkpoint rồi teleport
        if (CheckpointStore.TryGetCurrent(out var meta))
        {
            if (GC.CurrentMap == null || GC.CurrentMap.MapIndex != meta.mapIndex)
            {
                GC.SwitchMap(meta.mapIndex);
                if (GC.FadeDuration > 0f) yield return new WaitForSeconds(GC.FadeDuration + 0.05f);
            }

            GC.ScreenFadeOut();
            if (GC.FadeDuration > 0f) yield return new WaitForSeconds(GC.FadeDuration);
            GC.TeleportSafe(meta.Pos, meta.Rot);
            yield return null;
            GC.ScreenFadeIn();
            yield break;
        }

        // fallback: về spawn của currentMap
        if (GC.CurrentMap && GC.CurrentMap.currentMapSpawnPoint)
        {
            GC.ScreenFadeOut();
            if (GC.FadeDuration > 0f) yield return new WaitForSeconds(GC.FadeDuration);
            GC.MoveCharacterPos(GC.CurrentMap.currentMapSpawnPoint);
            yield return null;
            GC.ScreenFadeIn();
        }
    }
}

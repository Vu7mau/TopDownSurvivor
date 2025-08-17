// File: CheckpointAutoClearOnQuit.cs
using UnityEngine;

public class CheckpointAutoClearOnQuit : MonoBehaviour
{
    [Header("Behavior")]
    [Tooltip("Clear khi ApplicationQuit (build) và khi dừng Play Mode trong Editor.")]
    [SerializeField] private bool clearOnQuit = true;

    [Tooltip("Clear khi ApplicationPause(true) trên mobile.")]
    [SerializeField] private bool clearOnPause = false;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void OnApplicationQuit()
    {
        if (!clearOnQuit) return;
        DoClear("OnApplicationQuit");
    }

    private void OnApplicationPause(bool pause)
    {
        if (!clearOnPause) return;
        if (pause) DoClear("OnApplicationPause");
    }

    private static void DoClear(string reason)
    {
        CheckpointStore.ClearAll();
        PositionSave.Clear();
        PlayerPrefs.Save();
        Debug.Log($"[CheckpointAutoClearOnQuit] Cleared ({reason}).");
    }
}

// File: ChatNotify.cs
using UnityEngine;

public class ChatNotify : MonoBehaviour
{
    public static ChatNotify Instance { get; private set; }

    [Header("Refs")]
    [SerializeField] private ChatDialogue dialogue;        // Kéo ChatDialogue trong scene vào đây

    [Header("Defaults")]
    [SerializeField] private string speaker = "System";
    [SerializeField] private Sprite speakerAvatar;         // Optional
    [SerializeField] private DialogueAnchor anchor = DialogueAnchor.Footer;
    [SerializeField, Min(0.1f)] private float duration = 1.5f;

    [Header("SFX (optional)")]
    [SerializeField] private AudioClip sfxAdd;
    [SerializeField] private AudioClip sfxSelect;
    [SerializeField] private AudioClip sfxJump;
    [SerializeField] private AudioClip sfxClear;
    [SerializeField] private AudioClip sfxError;

    private void Awake()
    {
        if (Instance && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Say(string msg, AudioClip sfx = null, float? time = null, DialogueAnchor? anch = null)
    {
        if (!dialogue)
        {
            Debug.Log($"[Notify] {msg}");
            return;
        }
        dialogue.ShowDialogue(
            content: msg,
            time: time ?? duration,
            audioClip: sfx,
            speakerName: speaker,
            avatar: speakerAvatar,
            anchor: anch ?? anchor
        );
    }

    // ===== APIs chuyên dùng =====
    public void Added(int idx, int total, int mapIndex, string name)
        => Say($"Đã lưu CP #{idx + 1}/{total}: {name} (Map {mapIndex})", sfxAdd);

    public void Selected(int idx, int total, int mapIndex, string name)
        => Say($"Đang chọn CP #{idx + 1}/{total}: {name} (Map {mapIndex})", sfxSelect);

    public void Jumped(int mapIndex, string name)
        => Say($"Đã dịch chuyển tới: {name} (Map {mapIndex})", sfxJump);

    public void Cleared()
        => Say("Đã xoá tất cả checkpoint.", sfxClear);

    public void Error(string msg)
        => Say(msg, sfxError, time: 2f, anch: DialogueAnchor.Header);
}

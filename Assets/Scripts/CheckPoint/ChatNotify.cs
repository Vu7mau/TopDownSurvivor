
using UnityEngine;

public class ChatNotify : Singleton<ChatNotify>
{
   // public static ChatNotify Instance { get; private set; }

    [Header("Refs")]
    [SerializeField] private ChatDialogue dialogue;

    [Header("Defaults")]
    [SerializeField] private string speaker = "System";
    [SerializeField] private Sprite speakerAvatar;
    [SerializeField] private DialogueAnchor anchor = DialogueAnchor.Footer;
    [SerializeField, Min(0.1f)] private float duration = 1.5f;

    [Header("SFX (Checkpoint)")]
    [SerializeField] private AudioClip sfxAdd;
    [SerializeField] private AudioClip sfxSelect;
    [SerializeField] private AudioClip sfxJump;
    [SerializeField] private AudioClip sfxClear;
    [SerializeField] private AudioClip sfxError;

    [Header("SFX (Map)")]
    [SerializeField] private AudioClip sfxMapSwitch;
    [SerializeField] private AudioClip sfxMapSelect;

    private void Awake()
    {
        //if (Instance && Instance != this) { Destroy(gameObject); return; }
        //Instance = this;
        //DontDestroyOnLoad(gameObject);
    }

    private void Say(string msg, AudioClip sfx = null, float? time = null, DialogueAnchor? anch = null)
    {
        if (!dialogue) { Debug.Log($"[Notify] {msg}"); return; }
        dialogue.ShowDialogue(
            content: msg,
            time: time ?? duration,
            audioClip: sfx,
            speakerName: speaker,
            avatar: speakerAvatar,
            anchor: anch ?? anchor
        );
    }

    // Checkpoint notifications
    public void Added(int idx, int total, int mapIndex, string name)
        => Say($"Đã lưu CP #{idx + 1}/{total}: {name} (Map {mapIndex})", sfxAdd);

    public void Selected(int idx, int total, int mapIndex, string name)
        => Say($"Đang chọn CP #{idx + 1}/{total}: {name} (Map {mapIndex})", sfxSelect);

    public void Jumped(int mapIndex, string name)
        => Say($"Đã dịch chuyển tới: {name} (Map {mapIndex})", sfxJump);

    public void Info(string msg) => Say(msg);
    public void Cleared() => Say("Đã xoá tất cả checkpoint.", sfxClear);
    public void Error(string msg) => Say(msg, sfxError, time: 2f, anch: DialogueAnchor.Header);

    // Map notifications
    public void MapSelected(int mapIndex, string mapName)
        => Say($"Đang chọn Map {mapIndex}: {mapName}", sfxMapSelect);

    public void MapSwitched(int mapIndex, string mapName)
        => Say($"Đã chuyển sang Map {mapIndex}: {mapName}", sfxMapSwitch);

    public void MapJumping(int mapIndex, string mapName)
        => Say($"Đang chuyển đến Map {mapIndex}: {mapName}...", sfxMapSwitch);
}

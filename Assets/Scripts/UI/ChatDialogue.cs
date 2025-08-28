using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class ChatDialogue : VuMonoBehaviour
{
    [Header("UI Refs")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text content;
    [SerializeField] private TMP_Text speakerName;
    [SerializeField] private Image speakerAvatar;

    [Header("Anchor Points (assign outside)")]
    [SerializeField] private RectTransform headerPoint;
    [SerializeField] private RectTransform bottomPoint;
    [SerializeField] private RectTransform footerPoint;

    [Header("Defaults")]
    [SerializeField] private DialogueAnchor defaultAnchor = DialogueAnchor.Footer;

    [Header("Dialogue effect")]
    [Tooltip("Thời gian scale popup")]
    [SerializeField] private float popupDuration = 0.3f;

    [Tooltip("Giây / ký tự. <= 0 để hiện ngay (mặc định).")]
    [SerializeField] private float defaultTypingSpeed = 0.03f;

    [Header("Sequence Options")]
    [Tooltip("Độ trễ phụ sau khi audio kết thúc trước khi chuyển mục tiếp theo.")]
    [SerializeField] private float tailDelayAfterAudio = 0.05f;

    // State
    private Coroutine typingCoroutine;
    private Coroutine autoHideCoroutine;
    private Coroutine playingSequenceCoroutine;
    private string lastFullText = null;

    #region Lifecycle / Auto Load
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadDialoguePanel();
        LoadContentTMP();
        LoadSpeakerNameTMP();
        LoadSpeakerAvatar();
    }

    private void LoadDialoguePanel()
    {
        if (dialoguePanel != null) return;
        var bg = transform.Find("BG");
        if (bg != null) dialoguePanel = bg.gameObject;
    }

    private void LoadContentTMP()
    {
        if (content != null) return;
        var tf = transform.Find("BG/Content");
        if (tf != null) content = tf.GetComponent<TMP_Text>();
    }

    private void LoadSpeakerNameTMP()
    {
        if (speakerName != null) return;
        var tf = transform.Find("BG/Nick");
        if (tf != null) speakerName = tf.GetComponentInChildren<TMP_Text>();
    }

    private void LoadSpeakerAvatar()
    {
        if (speakerAvatar != null) return;
        var tf = transform.Find("BG/Nick/Avatar/Img");
        if (tf != null) speakerAvatar = tf.GetComponent<Image>();
    }
    #endregion

    /// <summary>
    /// Gán 3 điểm neo qua code (tuỳ chọn).
    /// </summary>
    public void SetAnchorPoints(RectTransform header, RectTransform bottom, RectTransform footer)
    {
        headerPoint = header;
        bottomPoint = bottom;
        footerPoint = footer;
    }

    /// <summary>
    /// Hiện hội thoại tại vị trí mong muốn (mặc định Footer nếu không truyền).
    /// typingSpeedOverride: GIÂY / KÝ TỰ cho riêng câu này. null => dùng defaultTypingSpeed; <= 0 => hiện ngay.
    /// </summary>
    public void ShowDialogue(
        string content,
        float time,
        AudioClip audioClip = null,
        string speakerName = "Player",
        Sprite avatar = null,
        DialogueAnchor anchor = DialogueAnchor.Footer,
        float? typingSpeedOverride = null
    )
    {
        if (dialoguePanel == null)
        {
            Debug.LogWarning($"{name}: dialoguePanel is null!");
            return;
        }

        // Lưu để SkipTyping có thể hiện ngay câu hiện tại
        lastFullText = content;

        // Định vị panel
        ApplyAnchor(anchor);

        // Clear tween/coroutine cũ để tránh xung đột
        dialoguePanel.transform.DOKill(true);
        if (typingCoroutine != null) { StopCoroutine(typingCoroutine); typingCoroutine = null; }
        if (autoHideCoroutine != null) { StopCoroutine(autoHideCoroutine); autoHideCoroutine = null; }

        // Chuẩn bị UI
        dialoguePanel.SetActive(true);
        dialoguePanel.transform.localScale = Vector3.zero;

        if (this.speakerName != null) this.speakerName.text = speakerName;
        if (this.speakerAvatar != null && avatar != null) this.speakerAvatar.sprite = avatar;

        if (this.content != null)
        {
            this.content.maxVisibleCharacters = 0;
            this.content.text = string.Empty;
        }

        // SFX
        if (audioClip != null)
            SoundFXManager.Instance.PlaySoundFXClip(audioClip, transform);

        // Chọn tốc độ gõ cho lần này
        float speedForThisLine = typingSpeedOverride.HasValue ? typingSpeedOverride.Value : defaultTypingSpeed;

        // Popup vào & bắt đầu gõ + auto-hide
        dialoguePanel.transform
            .DOScale(Vector3.one, popupDuration)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                if (this.content != null)
                    typingCoroutine = StartCoroutine(TypeText(lastFullText, speedForThisLine));

                if (time > 0)
                    autoHideCoroutine = StartCoroutine(AutoHideAfter(time));
            });
    }

    /// <summary>
    /// Bỏ qua hiệu ứng gõ và hiện ngay toàn bộ câu hiện tại.
    /// </summary>
    public void SkipTyping()
    {
        if (content == null) return;

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        if (!string.IsNullOrEmpty(lastFullText))
        {
            content.text = lastFullText;
            content.ForceMeshUpdate();
            content.maxVisibleCharacters = int.MaxValue;
        }
    }

    /// <summary>
    /// Ẩn hội thoại với tween đóng.
    /// </summary>
    public void HideDialogue()
    {
        if (typingCoroutine != null) { StopCoroutine(typingCoroutine); typingCoroutine = null; }
        if (autoHideCoroutine != null) { StopCoroutine(autoHideCoroutine); autoHideCoroutine = null; }

        dialoguePanel.transform.DOKill(true);

        dialoguePanel.transform
            .DOScale(Vector3.zero, popupDuration)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                if (content != null)
                {
                    content.text = string.Empty;
                    content.maxVisibleCharacters = 0;
                }
                dialoguePanel.SetActive(false);
            });
    }

    // ======================== SEQUENCE PLAYBACK ========================

    /// <summary>
    /// Phát lần lượt theo danh sách ChatContentSO.
    /// - Nếu có audio: tốc độ gõ được tính để HOÀN TẤT nội dung đúng bằng thời lượng audio.
    /// - Nếu không audio: dùng defaultTypingSpeed và/hoặc chatDelatTimeBeforHide của SO.
    /// </summary>
    public void PlaySequence(IList<ChatContentSO> items, DialogueAnchor? anchorOverride = null)
    {
        StopSequenceIfAny();
        playingSequenceCoroutine = StartCoroutine(Co_PlaySequence(items, anchorOverride));
    }

    public void StopSequenceIfAny()
    {
        if (playingSequenceCoroutine != null)
        {
            StopCoroutine(playingSequenceCoroutine);
            playingSequenceCoroutine = null;
        }
        HideDialogue();
    }

    private IEnumerator Co_PlaySequence(IList<ChatContentSO> items, DialogueAnchor? anchorOverride)
    {
        if (items == null || items.Count == 0) yield break;

        for (int i = 0; i < items.Count; i++)
        {
            var it = items[i];
            if (it == null) continue;

            // Tính tốc độ gõ & thời lượng hiển thị mục này
            ComputeTiming(it, out float typingSpeed, out float showDuration);

            // Chọn anchor
            var anchor = anchorOverride ?? defaultAnchor;

            // Show
            ShowDialogue(
                it.chatLines,
                showDuration,
                it.notificationAudio,
                string.IsNullOrEmpty(it.speakerName) ? "Player" : it.speakerName,
                it.speakerAvatar,
                anchor,
                typingSpeed
            );

            // Đợi xong mục này: thời lượng hiển thị + chút đệm (để tween đóng hoàn tất)
            yield return new WaitForSeconds(showDuration + tailDelayAfterAudio + popupDuration * 0.5f);
        }

        playingSequenceCoroutine = null;
    }

    /// <summary>
    /// Tính (typingSpeed giây/ký tự) và thời lượng hiển thị cho một mục.
    /// - Có audio: gõ xong đúng lúc audio kết thúc (showDuration = audio.length).
    /// - Không audio: showDuration lấy từ SO (chatDelatTimeBeforHide > 0) hoặc tính theo defaultTypingSpeed.
    /// </summary>
    private void ComputeTiming(ChatContentSO item, out float typingSpeed, out float showDuration)
    {
        int visibleChars = CountVisibleChars(item.chatLines);

        if (item.notificationAudio != null && item.notificationAudio.length > 0.01f && visibleChars > 0)
        {
            showDuration = item.notificationAudio.length;

            // Tránh bị hụt do thời gian bật popup/đóng popup, trừ đi phần nhỏ nếu cần
            float budgetForTyping = Mathf.Max(0.01f, showDuration - (popupDuration * 0.2f));
            typingSpeed = Mathf.Max(0.0001f, budgetForTyping / visibleChars);
        }
        else
        {
            // Không có audio → fallback
            typingSpeed = (defaultTypingSpeed > 0f) ? defaultTypingSpeed : 0.03f;

            if (item.chatDelatTimeBeforHide > 0)
            {
                showDuration = item.chatDelatTimeBeforHide;
            }
            else
            {
                // Ước lượng thời gian hiển thị tối thiểu = thời gian gõ + một chút đệm
                showDuration = Mathf.Clamp(visibleChars * typingSpeed + 0.6f, 0.5f, 60f);
            }
        }
    }

    private static readonly Regex _richTagRegex = new Regex("<.*?>", RegexOptions.Singleline);

    /// <summary>
    /// Đếm ký tự hiển thị: bỏ rich text tags (<b>, <color>, ...).
    /// </summary>
    private int CountVisibleChars(string s)
    {
        if (string.IsNullOrEmpty(s)) return 0;
        string stripped = _richTagRegex.Replace(s, "");
        return stripped.Length;
    }

    #region Internals
    private void ApplyAnchor(DialogueAnchor anchor)
    {
        // Nếu caller không truyền điểm hợp lệ, dùng default; nếu vẫn thiếu thì giữ nguyên
        var chosen = GetPoint(anchor) ?? GetPoint(defaultAnchor);
        if (chosen == null)
        {
            Debug.LogWarning($"{name}: Chưa gán anchor point phù hợp (Header/Bottom/Footer). Giữ nguyên vị trí hiện tại.");
            return;
        }

        var panelRt = dialoguePanel.transform as RectTransform;
        var targetRt = chosen;

        if (panelRt != null && targetRt != null)
        {
            // Không đổi parent để tránh phá layout; đặt theo world position
            panelRt.position = targetRt.position;
        }
    }

    private RectTransform GetPoint(DialogueAnchor anchor)
    {
        switch (anchor)
        {
            case DialogueAnchor.Header: return headerPoint;
            case DialogueAnchor.Bottom: return bottomPoint;
            case DialogueAnchor.Footer: return footerPoint;
            default: return null;
        }
    }

    private IEnumerator AutoHideAfter(float time)
    {
        yield return new WaitForSeconds(time);
        HideDialogue();
    }

    /// <summary>
    /// Typewriter dùng TMP_Text.maxVisibleCharacters để gõ theo từng ký tự, an toàn cho rich text.
    /// </summary>
    private IEnumerator TypeText(string fullText, float speed)
    {
        if (content == null) yield break;

        // speed <= 0: hiện ngay
        if (speed <= 0f)
        {
            content.text = fullText;
            content.ForceMeshUpdate();
            content.maxVisibleCharacters = int.MaxValue;
            yield break;
        }

        // Gán full text, sau đó lộ dần ký tự
        content.text = fullText;
        content.ForceMeshUpdate();

        int total = content.textInfo.characterCount;
        if (total == 0)
        {
            // Cho TMP 1 frame cập nhật textInfo nếu cần
            yield return null;
            content.ForceMeshUpdate();
            total = content.textInfo.characterCount;
        }

        content.maxVisibleCharacters = 0;

        for (int i = 0; i < total; i++)
        {
            content.maxVisibleCharacters = i + 1;
            yield return new WaitForSeconds(speed); // GIÂY / ký tự
        }
    }
    #endregion
}

// Nếu enum chưa nằm ở file khác, để kèm tại đây:
public enum DialogueAnchor { Header, Bottom, Footer }

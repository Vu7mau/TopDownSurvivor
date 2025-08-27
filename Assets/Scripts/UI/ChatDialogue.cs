using System.Collections;
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

    // State
    private Coroutine typingCoroutine;
    private Coroutine autoHideCoroutine;
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

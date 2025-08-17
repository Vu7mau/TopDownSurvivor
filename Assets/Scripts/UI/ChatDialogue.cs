using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using PlayFab.EconomyModels;
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

    [Space]
    [Header("Dialogue effect")]
    [Tooltip("Thời gian scale popup")]
    [SerializeField] private float popupDuration = 0.3f;

    [Tooltip("Giây / mỗi ký tự. <= 0 để hiện ngay.")]
    [SerializeField] private float typingSpeed = 0.03f;

    private Coroutine typingCoroutine;
    private Coroutine autoHideCoroutine;

    // Giữ state cho SkipTyping()
    private string lastFullText = null;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadContentTMP();
        this.LoadspeakerNameTMP();
        this.LoadSpeakerAvatar();
        this.LoadDialoguePanel();
    }

    private void LoadDialoguePanel()
    {
        if (dialoguePanel != null) return;
        var bg = this.transform.Find("BG");
        if (bg != null) dialoguePanel = bg.gameObject;
    }

    private void LoadContentTMP()
    {
        if (content != null) return;
        var tf = this.transform.Find("BG/Content");
        if (tf != null) content = tf.GetComponent<TMP_Text>();
    }

    private void LoadspeakerNameTMP()
    {
        if (speakerName != null) return;
        var tf = this.transform.Find("BG/Nick");
        if (tf != null) speakerName = tf.GetComponentInChildren<TMP_Text>();
    }

    private void LoadSpeakerAvatar()
    {
        if (speakerAvatar != null) return;
        var tf = this.transform.Find("BG/Nick/Avatar/Img");
        if (tf != null) speakerAvatar = tf.GetComponent<Image>();
    }

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
    /// typingSpeed = GIÂY / KÝ TỰ (giữ nguyên semantics cũ). <= 0 => hiện ngay.
    /// </summary>
    public void ShowDialogue(
        string content,
        float time,
        AudioClip audioClip = null,
        string speakerName = "Player",
        Sprite avatar = null,
        DialogueAnchor anchor = DialogueAnchor.Footer
    )
    {
        if (dialoguePanel == null)
        {
            Debug.LogWarning("dialoguePanel is null!");
            return;
        }

        // Lưu nội dung để có thể SkipTyping
        lastFullText = content;

        // Định vị trước khi popup
        ApplyAnchor(anchor);

        // Hủy tween cũ (nếu có) để tránh xung đột khi gọi liên tục
        dialoguePanel.transform.DOKill(true);

        dialoguePanel.SetActive(true);
        dialoguePanel.transform.localScale = Vector3.zero;

        if (this.speakerName != null) this.speakerName.text = speakerName;
        if (this.speakerAvatar != null && avatar != null) this.speakerAvatar.sprite = avatar;

        if (audioClip != null)
            SoundFXManager.Instance.PlaySoundFXClip(audioClip, this.transform);

        // Ngắt coroutine cũ nếu có
        if (typingCoroutine != null) { StopCoroutine(typingCoroutine); typingCoroutine = null; }
        if (autoHideCoroutine != null) { StopCoroutine(autoHideCoroutine); autoHideCoroutine = null; }

        // Reset trạng thái text trước khi scale-in (tránh nháy)
        if (this.content != null)
        {
            this.content.maxVisibleCharacters = 0;
            this.content.text = string.Empty;
        }

        dialoguePanel.transform
            .DOScale(Vector3.one, popupDuration)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                if (this.content != null)
                    typingCoroutine = StartCoroutine(TypeText(lastFullText));

                if (time > 0)
                    autoHideCoroutine = StartCoroutine(AutoHideAfter(time));
            });
    }

    /// <summary>
    /// Neo panel đến vị trí chỉ định. Nếu thiếu, dùng defaultAnchor. Nếu vẫn thiếu -> giữ nguyên.
    /// </summary>
    private void ApplyAnchor(DialogueAnchor anchor)
    {
        // Nếu caller không truyền, dùng default
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
            // Không đổi parent để tránh phá layout của bạn; đặt theo toạ độ thế giới
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
        }
        return null;
    }

    private IEnumerator AutoHideAfter(float time)
    {
        yield return new WaitForSeconds(time);
        HideDialogue();
    }

    /// <summary>
    /// Typewriter dùng TMP_Text.maxVisibleCharacters để luôn gõ theo ký tự (không nhảy theo từ / không lỗi rich text).
    /// </summary>
    private IEnumerator TypeText(string fullText)
    {
        if (this.content == null) yield break;

        // Trường hợp tốc độ <= 0: hiện ngay
        if (typingSpeed <= 0f)
        {
            this.content.text = fullText;
            this.content.ForceMeshUpdate();
            this.content.maxVisibleCharacters = int.MaxValue;
            yield break;
        }

        // Gán toàn bộ text trước, rồi "lộ dần" ký tự
        this.content.text = fullText;
        this.content.ForceMeshUpdate();

        int total = this.content.textInfo.characterCount;
        if (total == 0)
        {
            // Cho TMP 1 frame để cập nhật textInfo khi cần
            yield return null;
            this.content.ForceMeshUpdate();
            total = this.content.textInfo.characterCount;
        }

        this.content.maxVisibleCharacters = 0;

        // typingSpeed là GIÂY / KÝ TỰ -> delay = typingSpeed
        float delay = typingSpeed;

        for (int i = 0; i < total; i++)
        {
            this.content.maxVisibleCharacters = i + 1;
            yield return new WaitForSeconds(delay);
        }
    }

    /// <summary>
    /// Cho phép bỏ qua hiệu ứng gõ và hiện ngay toàn bộ câu hiện tại.
    /// </summary>
    public void SkipTyping()
    {
        if (this.content == null) return;

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        if (!string.IsNullOrEmpty(lastFullText))
        {
            this.content.text = lastFullText;
            this.content.ForceMeshUpdate();
            this.content.maxVisibleCharacters = int.MaxValue;
        }
    }

    public void HideDialogue()
    {
        // Ngắt typing/auto-hide đang chạy
        if (typingCoroutine != null) { StopCoroutine(typingCoroutine); typingCoroutine = null; }
        if (autoHideCoroutine != null) { StopCoroutine(autoHideCoroutine); autoHideCoroutine = null; }

        // Hủy tween cũ để bảo đảm tween đóng hoạt động mượt
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
}

public enum DialogueAnchor { Header, Bottom, Footer }

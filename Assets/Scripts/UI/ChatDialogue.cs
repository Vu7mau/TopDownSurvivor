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
    [SerializeField] private float popupDuration = 0.3f;
    [SerializeField] private float typingSpeed = 0.3f;

    private Coroutine typingCoroutine;
    private Coroutine autoHideCoroutine;

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

        // Định vị trước khi popup
        ApplyAnchor(anchor);

        dialoguePanel.SetActive(true);
        dialoguePanel.transform.localScale = Vector3.zero;

        if (this.speakerName != null) this.speakerName.text = speakerName;
        if (this.speakerAvatar != null && avatar != null) this.speakerAvatar.sprite = avatar;

        if (audioClip != null)
            SoundFXManager.Instance.PlaySoundFXClip(audioClip, this.transform);

        // Ngắt coroutine cũ nếu có
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        if (autoHideCoroutine != null) StopCoroutine(autoHideCoroutine);

        dialoguePanel.transform.DOScale(Vector3.one, popupDuration)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                if (this.content != null)
                    typingCoroutine = StartCoroutine(TypeText(content));

                if (time > 0)
                    autoHideCoroutine = StartCoroutine(AutoHideAfter(time));
            });
    }

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

        // Đặt panel về cùng Canvas với target (nếu khác cha, vẫn OK)
        if (panelRt != null && targetRt != null)
        {
            // Không đổi parent để tránh phá layout của bạn; chỉ đặt theo toạ độ thế giới
            // (Nếu muốn bám layout, bạn có thể đổi parent panelRt.SetParent(targetRt, false);)
            panelRt.position = targetRt.position;

            // Tuỳ chọn: khớp kích thước theo target (bỏ nếu không cần)
            // panelRt.sizeDelta = targetRt.rect.size;
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

    private IEnumerator TypeText(string fullText)
    {
        if (this.content == null) yield break;

        this.content.text = "";
        foreach (char c in fullText)
        {
            this.content.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void HideDialogue()
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        if (autoHideCoroutine != null) StopCoroutine(autoHideCoroutine);

        dialoguePanel.transform.DOScale(Vector3.zero, popupDuration)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                if (content != null) content.text = string.Empty;
                dialoguePanel.SetActive(false);
            });
    }
}
public enum DialogueAnchor { Header, Bottom, Footer }

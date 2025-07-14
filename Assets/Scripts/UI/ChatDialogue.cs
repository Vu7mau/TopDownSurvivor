using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using PlayFab.EconomyModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatDialogue : VuMonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text content;
    [SerializeField] private TMP_Text speakerName;
    [SerializeField] private UnityEngine.UI.Image speakerAvatar;


    [Space]
    [Header("Dialogue effect")]
    [SerializeField] private float popupDuration = 0.3f;
    [SerializeField] private float typingSpeed = 0.3f;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadContentTMP();
        this.LoadspeakerNameTMP();
        this.LoadSpeakerAvatar();
        this.LoadDialoguePanel();
    }
    protected override void OnEnable()
    {
        //  ShowDialogue();
    }
    private void LoadDialoguePanel()
    {
        if (dialoguePanel != null) return;

        dialoguePanel = this.transform.Find("BG").gameObject;
    }
    private void LoadContentTMP()
    {
        if (content != null) return;
        content = this.transform.Find("BG").transform.Find("Content").GetComponent<TMP_Text>();
    }
    private void LoadspeakerNameTMP()
    {
        if (speakerName != null) return;
        speakerName = this.transform.Find("BG").transform.Find("Nick").GetComponentInChildren<TMP_Text>();
    }
    private void LoadSpeakerAvatar()
    {
        if (speakerAvatar != null) return;
        speakerAvatar = this.transform.Find("BG").transform.Find("Nick").transform.Find("Avatar").transform.Find("Img").GetComponent<UnityEngine.UI.Image>();
    }

    public void ShowDialogue(string speakerName, Sprite avatar, string content)
    {
        if (dialoguePanel == null)
        {
            Debug.LogWarning("dialoguePanel is null!");
            return;
        }

        dialoguePanel.SetActive(true);
        dialoguePanel.transform.localScale = Vector3.zero;

        if (this.speakerName != null)
            this.speakerName.text = speakerName;

        if (this.speakerAvatar != null)
            this.speakerAvatar.sprite = avatar;

        if (dialoguePanel.transform != null)
        {
            dialoguePanel.transform.DOScale(Vector3.one, popupDuration)
                .SetEase(Ease.OutBack)
                .OnComplete(() =>
                {
                    if (this.content != null)
                        StartCoroutine(TypeText(content));
                });
        }
    }

    private IEnumerator TypeText(string fullText)
    {
        if (this.content == null)
            yield break;

        this.content.text = "";

        foreach (char c in fullText)
        {
            this.content.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
    public void HideDialogue()
    {
        // Thu nhỏ và ẩn
        dialoguePanel.transform.DOScale(Vector3.zero, popupDuration)
        .SetEase(Ease.InBack)
            .OnComplete(() => { content.text = string.Empty; dialoguePanel.SetActive(false); }); 
    }
}

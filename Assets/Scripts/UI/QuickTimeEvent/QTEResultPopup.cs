using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QTEResultPopup : VuMonoBehaviour
{
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private TMP_Text popupMessage;
    [SerializeField] private TMP_Text title;
    [SerializeField] private Button popupCloseButton;

    private RectTransform panelRect;

    protected override void Awake()
    {
        base.Awake();

        if (popupPanel != null)
        {
            popupPanel.SetActive(false);
            panelRect = popupPanel.GetComponent<RectTransform>();
        }

        popupCloseButton.onClick.AddListener(HidePopup);
    }

    public void Show(string titleText, string message)
    {
        Debug.Log($"Title: {titleText} | Message: {message}");

        if (popupPanel == null || panelRect == null) return;

        if (title != null)
            title.text = titleText;

        popupMessage.characterSpacing = 10f;
        popupMessage.text = message;

        popupPanel.SetActive(true);
        panelRect.localScale = Vector3.zero;
        panelRect.DOScale(Vector3.one, 0.4f)
                 .SetEase(Ease.OutBack);
    }
    public void HidePopup()
    {
        if (popupPanel == null || panelRect == null) return;

        panelRect.DOScale(Vector3.zero, 0.25f)
                 .SetEase(Ease.InBack)
                 .OnComplete(() => popupPanel.SetActive(false));
    }
}

using UnityEngine;
using TMPro;
using DG.Tweening;
using System;

public class NotificationUI : MonoBehaviour
{
    public static NotificationUI Instance;
    [SerializeField] private GameObject panel;
    [SerializeField] private RectTransform background;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private RectTransform textRect;

    private CanvasGroup canvasGroup;
    private Sequence seq;       
    private bool isShowing = false;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        canvasGroup = background.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = background.gameObject.AddComponent<CanvasGroup>();

        panel.SetActive(false);
    }

    public void Show(string message, float duration = 2f, Action onComplete = null)
    {
        if(isShowing && messageText.text == message) return;
        if (seq != null && seq.active)
        {
            seq.Kill();
            seq = null; 
        }
        isShowing = true;
        panel.transform.SetAsLastSibling(); 
        messageText.text = message;
        panel.SetActive(true);
        Vector2 startPos = new Vector2(0, -200f);
        background.anchoredPosition = startPos;
        textRect.anchoredPosition = startPos;
        messageText.color = new Color(  
            messageText.color.r,
            messageText.color.g,
            messageText.color.b,
            1f
        );
        seq = DOTween.Sequence();
        seq.Append(background.DOAnchorPosY(0, 0.3f).SetEase(Ease.OutBack))
            .Join(textRect.DOAnchorPosY(0, 0.3f).SetEase(Ease.OutBack))// Nhảy lên vị trí
           .AppendInterval(duration) // Giữ vị trí trong th gian
           .Append(background.DOAnchorPosY(-200f, 0.3f).SetEase(Ease.InBack))
           .Join(textRect.DOAnchorPosY(-200f, 0.3f).SetEase(Ease.InBack))
           .Join(messageText.DOFade(0f, 0.3f))
           .OnComplete(() =>
           {
               panel.SetActive(false);
               isShowing = false;
               seq = null; 
               onComplete?.Invoke();
           });
    }
    public void HideImmediately()
    {
        if (seq != null && seq.IsActive())
        {
            seq.Kill();
            seq = null;
        }
        panel.SetActive(false);
        isShowing = false;
    }
}

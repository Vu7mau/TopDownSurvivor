using UnityEngine;
using DG.Tweening;
using System.Collections;
using System;

public class PanelIntructEffect : MonoBehaviour
{
    public static PanelIntructEffect Instance;
    public CanvasGroup panelCg;
    public RectTransform[] lines; 
    public float delayBetweenLines = 0.05f;
    public float moveDistance = 50f; 
    public float fadeDuration = 0.4f;
    public float moveDuration = 0.4f;
    private void wake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void ShowPanel(Action onComplete = null)
    {
        gameObject.SetActive(true);
        panelCg.alpha = 0f;
        panelCg.DOFade(1f, 0.4f);

        for (int i = 0; i < lines.Length; i++)
        {
            RectTransform line = lines[i];
            CanvasGroup cg = line.GetComponent<CanvasGroup>();
            if (cg == null) cg = line.gameObject.AddComponent<CanvasGroup>();

            cg.alpha = 0f;
            Vector2 startPos = line.anchoredPosition;
            line.anchoredPosition = startPos + new Vector2(0, moveDistance);

            // Hiệu ứng fade in + move
            Tween fadeTween = cg.DOFade(1f, fadeDuration).SetDelay(i * delayBetweenLines);
            Tween moveTween = line.DOAnchorPosY(startPos.y, moveDuration)
                                  .SetDelay(i * delayBetweenLines)
                                  .SetEase(Ease.OutQuad);

            // Nếu là dòng cuối cùng thì gọi onComplete khi xong
            if (i == lines.Length - 1)
            {
                // Chạy song song fade + move và gọi onComplete khi xong
                DOTween.Sequence()
                       .Join(fadeTween)
                       .Join(moveTween)
                       .OnComplete(() => onComplete?.Invoke());
            }
            else
            {
                // Các dòng còn lại vẫn chạy như thường
                fadeTween.Play();
                moveTween.Play();
            }
        }
    }


    public void HidePanel(Action onComplete = null)
    {
        // Ẩn từng dòng trước
        for (int i = 0; i < lines.Length; i++)
        {
            RectTransform line = lines[i];
            CanvasGroup cg = line.GetComponent<CanvasGroup>();
            if (cg == null) cg = line.gameObject.AddComponent<CanvasGroup>();

            Vector2 startPos = line.anchoredPosition;

            Tween fadeTween = cg.DOFade(0f, fadeDuration).SetDelay(i * delayBetweenLines);
            Tween moveTween = line.DOAnchorPosY(startPos.y + moveDistance, moveDuration)
                                  .SetDelay(i * delayBetweenLines)
                                  .SetEase(Ease.InQuad);

            // Dòng cuối thì gọi đóng panel và callback
            if (i == lines.Length - 1)
            {
                DOTween.Sequence()
                       .Join(fadeTween)
                       .Join(moveTween)
                       .OnComplete(() =>
                       {
                           // Sau khi dòng cuối biến mất, ẩn panel chính
                           panelCg.DOFade(0f, 0.3f)
                                  .OnComplete(() =>
                                  {
                                      gameObject.SetActive(false);
                                      onComplete?.Invoke();
                                  });
                       });
            }
        }
    }

}

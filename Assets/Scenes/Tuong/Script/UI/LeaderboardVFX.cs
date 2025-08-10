using UnityEngine;
using DG.Tweening;

public class LeaderboardVFX : MonoBehaviour
{
    public RectTransform[] panelTransforms;
    public Vector2[] offscreenOffsets = new Vector2[2] { new Vector2(-500, 0), new Vector2(500, 0) };

    public float moveDuration = 0.6f;
    public float scaleDuration = 0.4f;
    private Vector2[] originalPositions;
    public static LeaderboardVFX Instance;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        originalPositions = new Vector2[panelTransforms.Length];
        for (int i = 0; i < panelTransforms.Length; i++)
        {
            originalPositions[i] = panelTransforms[i].anchoredPosition;
        }
    }
    public void PrepareHide()
    {
        for (int i = 0; i < panelTransforms.Length; i++)
        {
            panelTransforms[i].localScale = Vector3.zero;
            panelTransforms[i].anchoredPosition = originalPositions[i] + offscreenOffsets[i];
        }
    }

    public void ShowPanels(System.Action onComplete = null)
    {
        int completed = 0;
        for (int i = 0; i < panelTransforms.Length; i++)
        {
            int index = i;
            Sequence seq = DOTween.Sequence();
            seq.Append(panelTransforms[i].DOAnchorPos(originalPositions[i], moveDuration).SetEase(Ease.OutExpo));
            seq.Join(panelTransforms[i].DOScale(Vector3.one, scaleDuration).SetEase(Ease.OutBack));
            seq.OnComplete(() =>
            {
                completed++;
                if (completed == panelTransforms.Length && onComplete != null)
                    onComplete();
            });
        }
    }
    public void HidePanels(System.Action onComplete = null)
    {
        int completed = 0;
        for (int i = 0; i < panelTransforms.Length; i++)
        {
            Vector2 currentPos = panelTransforms[i].anchoredPosition;
            Vector2 targetPos = originalPositions[i] + offscreenOffsets[i];

            Sequence seq = DOTween.Sequence();
            seq.Append(panelTransforms[i].DOAnchorPos(targetPos, moveDuration).SetEase(Ease.InBack));
            seq.OnComplete(() =>
            {
                completed++;
                if (completed == panelTransforms.Length && onComplete != null)
                    onComplete();
            });
        }
    }
}
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class FlagHoverSizeDelta : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform flag; 
    public float hoverLengthMultiplier = 1.2f;
    public float duration = 0.3f;

    private Vector2 originalSize;

    void Awake()
    {
        if (flag == null)
            flag = GetComponent<RectTransform>();

        originalSize = flag.sizeDelta;
        flag.pivot = new Vector2(flag.pivot.x, 1f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        flag.DOSizeDelta(
            new Vector2(originalSize.x, originalSize.y * hoverLengthMultiplier),
            duration
        ).SetEase(Ease.OutBack);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        flag.DOSizeDelta(originalSize, duration).SetEase(Ease.OutBack);
    }
}
f
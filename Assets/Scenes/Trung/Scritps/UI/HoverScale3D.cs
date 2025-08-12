using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverScale3D : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float hoverScale = 1.2f;   // Tỉ lệ phóng to khi hover
    [SerializeField] private float duration = 0.2f;     // Thời gian tween
    [SerializeField] private Ease easeType = Ease.OutBack;

    private RectTransform rectTransform;
    private Vector3 originalScale;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Hover!");
        rectTransform.DOScale(originalScale * hoverScale, duration).SetEase(easeType).SetUpdate(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Not Hover!");
        rectTransform.DOScale(originalScale, duration).SetEase(easeType).SetUpdate(true);
    }
}

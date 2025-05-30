using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
public class ButtonClickEffectDOTween : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Vector2 originalScale;
    [Header("Click Scale Button")]
    [SerializeField] private float scaleFactor = 0.9f;
    [SerializeField] private float duration = 0.1f;
    private void Start()
    {
        originalScale = transform.localScale;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        transform.DOScale(originalScale * scaleFactor, duration).SetEase(Ease.OutQuad);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        transform.DOScale(originalScale , duration).SetEase(Ease.OutQuad);
    }

}

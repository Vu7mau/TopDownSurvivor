using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class PulseOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Tween pulseTween;
    public float pulseDuration = 0.8f;
    public float pulseScale = 1.2f;
    public void OnPointerEnter(PointerEventData eventData)
    {
        pulseTween = transform.DOScale(pulseScale, pulseDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (pulseTween != null && pulseTween.IsActive())
        {
            pulseTween.Kill();
        }
        transform.localScale = Vector3.one;
    }
}

using DG.Tweening;
using UnityEngine;
public class IconBounce : MonoBehaviour
{
    [SerializeField] private float scaleAmount = 1.1f;  
    [SerializeField] private float duration = 1.5f;     
    private Vector3 originalScale;
    private Tween scaleTween;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    private void OnEnable()
    {
        transform.localScale = originalScale;

        scaleTween = transform.DOScale(originalScale * scaleAmount, duration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .SetTarget(gameObject)
            .SetUpdate(true);  
    }

    private void OnDisable()
    {
        if (scaleTween != null && scaleTween.IsActive())
        {
            scaleTween.Kill();
            scaleTween = null;
        }

        if (transform != null)
            transform.localScale = originalScale;
    }

    private void OnDestroy()
    {
        if (scaleTween != null && scaleTween.IsActive())
        {
            scaleTween.Kill();
            scaleTween = null;
        }
    }
}

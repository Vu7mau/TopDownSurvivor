using UnityEngine;
using DG.Tweening;

public class WindTurbine : MonoBehaviour
{
    [Header("Cấu hình xoay")]
    public float duration = 1f; 

    private Tween rotateTween;

    void Start()
    {
        rotateTween = transform
            .DOLocalRotate(new Vector3(0f, 0f, 360f), duration, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);
    }

    void OnDestroy()
    {
        if (rotateTween != null && rotateTween.IsActive())
        {
            rotateTween.Kill();
        }
    }
}

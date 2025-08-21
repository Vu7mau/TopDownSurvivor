using UnityEngine;
using DG.Tweening;
public class ModelRotator : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5f; 
    [SerializeField] private RotateMode rotateMode = RotateMode.FastBeyond360;

    private Tween rotateTween;
    private Quaternion originalRotation;

    private void Awake()
    {
        originalRotation = transform.localRotation;     
    }
    private void OnEnable()
    {
        transform.localRotation = originalRotation; 
        rotateTween = transform
            .DORotate(new Vector3(0, 360f, 0), rotationSpeed, rotateMode)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart); 
    }

    private void OnDisable()
    {
        if (rotateTween != null && rotateTween.IsActive())
            rotateTween.Kill();
        transform.localRotation = originalRotation;
    }
    private void OnDestroy()
    {
        if (rotateTween != null && rotateTween.IsActive())
            rotateTween.Kill();
    }
}

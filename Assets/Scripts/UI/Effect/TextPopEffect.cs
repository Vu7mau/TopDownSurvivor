using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class TextPopEffect : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private TMP_Text targetText; // Kéo TextMeshPro vào đây

    [Header("Effect Settings")]
    [SerializeField] private float popScale = 1.5f;    // Tỷ lệ phóng to
    [SerializeField] private float duration = 0.3f;    // Thời gian phóng to/thu nhỏ
    [SerializeField] private Ease easeType = Ease.OutBack;
    [SerializeField] private bool autoLoop = false;    // Loop khi bắt đầu game

    private Vector3 originalScale;
    private Tween loopTween;

    private void Awake()
    {
        if (targetText == null)
            targetText = GetComponent<TMP_Text>();

        originalScale = targetText.transform.localScale;
    }

    private void Start()
    {
        if (autoLoop)
            StartLoop();
    }

    [ContextMenu("Play Pop Once")]
    public void PlayPopOnce()
    {
        targetText.transform.localScale = originalScale; // Reset
        targetText.transform
            .DOScale(popScale, duration)
            .SetEase(easeType)
            .OnComplete(() =>
            {
                targetText.transform
                    .DOScale(originalScale, duration)
                    .SetEase(Ease.InBack);
            });
    }

    [ContextMenu("Start Loop")]
    public void StartLoop()
    {
        StopLoop(); // Dừng loop cũ nếu có
        targetText.transform.localScale = originalScale;

        loopTween = targetText.transform
            .DOScale(popScale, duration)
            .SetEase(easeType)
            .SetLoops(-1, LoopType.Yoyo); // Loop vô hạn nở – thu
    }

    [ContextMenu("Stop Loop")]
    public void StopLoop()
    {
        loopTween?.Kill();
        targetText.transform.localScale = originalScale;
    }
}

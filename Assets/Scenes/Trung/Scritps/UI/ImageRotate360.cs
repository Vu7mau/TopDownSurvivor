using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ImageRotate360 : MonoBehaviour
{
    [SerializeField] private RectTransform targetImage;
    [SerializeField] private float duration = 2f; // thời gian quay 1 vòng

    [SerializeField] private RectTransform panel;
    //[SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float scaleDuration = 0.5f;


    private void OnEnable()
    {
        RotateLoop();
    }

    public void DisplayPanel()
    {
            // Đảm bảo bắt đầu từ scale 0
            panel.localScale = Vector3.zero;

        // Phóng to từ 0 -> 1
        panel.DOScale(Vector3.one, scaleDuration)
              .SetEase(Ease.OutBack); // Ease đẹp hơn
    }
    public void HidePanel()
    {
        panel.DOScale(Vector3.zero, scaleDuration)
              .SetEase(Ease.InBack)
              .OnComplete(() => gameObject.SetActive(false));
    }

    private void RotateLoop()
    {
        float z = targetImage.localEulerAngles.z;

        DOTween.To(
            () => z,
            x => {
                z = x;
                targetImage.localEulerAngles = new Vector3(0, 0, z); // giữ nguyên X=0, Y=0
            },
            z - 360f,
            duration
        )
        .SetEase(Ease.Linear)
        .SetLoops(-1, LoopType.Restart)
        .SetUpdate(true); // chạy ngay cả khi object/cha bị disable
    }
}

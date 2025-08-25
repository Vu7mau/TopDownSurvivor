using UnityEngine;
using DG.Tweening;

public class CrowFlyAway : MonoBehaviour
{
    void Start()
    {
        FlyAway();
    }

    void FlyAway()
    {
        // Tính vị trí bay lên ngẫu nhiên
        Vector3 targetPos = transform.position + new Vector3(
            Random.Range(-5f, 5f),   // ngang trái/phải
            Random.Range(8f, 12f),   // bay cao lên trời
            Random.Range(-5f, 5f)    // ra trước/sau
        );

        // Bay tới vị trí mới trong 2 giây
        transform.DOMove(targetPos, 2f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                Destroy(gameObject); // bay xong thì biến mất
            });

        // Xoay vòng vòng cho giống đang bay loạn
        transform.DORotate(
            new Vector3(0, 360f, 0),
            1.5f,
            RotateMode.FastBeyond360
        ).SetLoops(-1).SetEase(Ease.Linear);
    }

    void OnDestroy()
    {
        // Kill tất cả tween gắn vào object này khi bị destroy
        DOTween.Kill(transform);
    }
}

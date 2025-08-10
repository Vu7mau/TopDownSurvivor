using UnityEngine;
using DG.Tweening;

public class BorderHighlight : MonoBehaviour
{
    public RectTransform glowPoint;
    public RectTransform targetImage;
    public float duration = 2f;

    private Tween borderTween;

    void OnEnable()
    {
        // Nếu bị destroy hoặc null thì không làm gì
        if (targetImage == null || glowPoint == null)
            return;

        StartCoroutine(StartAfterLayout());
    }

    System.Collections.IEnumerator StartAfterLayout()
    {
        yield return null; // chờ 1 frame

        if (this == null || targetImage == null || glowPoint == null)
            yield break; // object bị destroy giữa chừng

        RunBorderLoop();
    }

    void RunBorderLoop()
    {
        if (targetImage == null || glowPoint == null)
            return;

        Vector3[] path = new Vector3[]
        {
            new Vector3( targetImage.rect.width/2,  targetImage.rect.height/2, 0),
            new Vector3( targetImage.rect.width/2, -targetImage.rect.height/2, 0),
            new Vector3(-targetImage.rect.width/2, -targetImage.rect.height/2, 0),
            new Vector3(-targetImage.rect.width/2,  targetImage.rect.height/2, 0),
            new Vector3( targetImage.rect.width/2,  targetImage.rect.height/2, 0)
        };

        glowPoint.localPosition = path[0];

        // Kill tween cũ nếu còn
        KillTween();

        // Tạo tween mới
        borderTween = glowPoint
            .DOLocalPath(path, duration, PathType.Linear, PathMode.Full3D)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart)
            .SetAutoKill(false);
    }

    void OnDisable()
    {
        KillTween();
    }

    void OnDestroy()
    {
        KillTween();
    }

    private void KillTween()
    {
        if (borderTween != null)
        {
            borderTween.Kill();
            borderTween = null;
        }
    }
}

using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
public class CloseEffect : MonoBehaviour
{
    public Transform closeButtonTransform;
    public Image closeImage; 
    public float idleScale = 1.05f;
    public float idleDuration = 1f;
    private void OnEnable()
    {
        PlayIdleButtonEffect(closeButtonTransform, closeImage);
    }
    private void OnDisable()
    {
        closeButtonTransform.DOKill();
        closeImage.DOKill();
    }
    void PlayIdleButtonEffect(Transform target, Image img)
    {
        if (target == null || img == null) return;

        target.DOKill();
        img.DOKill();

        target.localScale = Vector3.one;
        target.DOScale(idleScale, idleDuration)
              .SetEase(Ease.InOutSine)
              .SetLoops(-1, LoopType.Yoyo);

        img.color = Color.white;
        img.DOColor(new Color(1.2f, 1.2f, 1.2f), 1f)
           .SetLoops(-1, LoopType.Yoyo);
    }
}

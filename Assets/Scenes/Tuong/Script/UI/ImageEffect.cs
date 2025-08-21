using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
public class ImageEffect : MonoBehaviour
{
    [SerializeField] private Image goldIcon;
    [SerializeField] private Image silverIcon;
    [SerializeField] private Image bronzeIcon;

    private void OnEnable()
    {
        if (goldIcon != null)
            PlayIconEffect(goldIcon.transform, goldIcon);
        if (silverIcon != null)
            PlayIconEffect(silverIcon.transform, silverIcon);
        if (bronzeIcon != null)
            PlayIconEffect(bronzeIcon.transform, bronzeIcon);
    }

    void PlayIconEffect(Transform target, Image img)
    {
        if (target == null || img == null) return;
        target.DOKill();
        img.DOKill();

        Vector3 originalPos = target.localPosition;
        Vector3 originalScale = Vector3.one;
        Vector3 originalRot = Vector3.zero;

        void DoRandomEffect()
        {
            target.localPosition = originalPos;
            target.localScale = originalScale;
            target.localRotation = Quaternion.Euler(originalRot);
            img.color = Color.white;

            int effectType = UnityEngine.Random.Range(0, 5);

            Tween tween = null;

            switch (effectType)
            {
                case 0: 
                    float angle = UnityEngine.Random.Range(-8f, 8f);
                    tween = target.DOLocalRotate(new Vector3(0, 0, angle), 0.35f) 
                        .SetEase(Ease.InOutSine)
                        .SetLoops(2, LoopType.Yoyo);
                    break;

                case 1: 
                    tween = target.DOLocalMoveY(originalPos.y + 5f, 0.3f) 
                        .SetEase(Ease.OutSine)
                        .SetLoops(2, LoopType.Yoyo);
                    break;

                case 2: 
                    float scaleVal = UnityEngine.Random.Range(0.95f, 1f);
                    tween = target.DOScale(scaleVal, 0.35f)
                        .SetLoops(2, LoopType.Yoyo)
                        .SetEase(Ease.InOutSine);
                    break;

                case 3:
                    {
                        var seq = DOTween.Sequence();
                        seq.Append(target.DORotate(new Vector3(0, 90, 0), 0.3f).SetEase(Ease.InSine));
                        seq.Append(target.DORotate(new Vector3(0, 0, 0), 0.3f).SetEase(Ease.OutSine)); 
                        tween = seq.SetLoops(1);
                    }
                    break;

                case 4:
                    tween = target.DOScale(1.03f, 0.6f) 
                        .SetEase(Ease.InOutSine)
                        .SetLoops(2, LoopType.Yoyo);
                    break;
            }

            if (tween != null)
            {
                tween.OnComplete(() =>
                {
                    DOVirtual.DelayedCall(0.15f, DoRandomEffect); 
                });
            }
        }
        DoRandomEffect();
    }
}

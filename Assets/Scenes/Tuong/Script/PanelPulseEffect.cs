using DG.Tweening;
using UnityEngine;

public class PanelPulseEffect : MonoBehaviour
{
    public static PanelPulseEffect Instance;
    private Tween pulseTween;
    private void Awake()
    {
        if(Instance == null )
            Instance = this;
    }
    public void PulseEffect(Transform target)
    {
        if(target == null || target.gameObject == null || !target.gameObject.activeInHierarchy)
        {
            return;
        }
        if(target != null)
        {
            pulseTween?.Kill();
            transform.DOScale(1.05f, 1f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine)
                .SetLink(target.gameObject, LinkBehaviour.KillOnDestroy); // LinkB là enum để Dw bt làm j khi bị destroy
        }
    }
    private void OnDestroy()
    {
        pulseTween?.Kill();
    }

}

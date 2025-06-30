using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;

public class ButtonSlideUp : MonoBehaviour
{
    [SerializeField] private VerticalLayoutGroup layoutGroup;
    [SerializeField] private List<RectTransform> buttons = new List<RectTransform>();
    [Header("Animation Settings")]
    [SerializeField] private float slideDuration = 0.5f;
    [SerializeField] private float delayBetween = 0.1f;
    [SerializeField] private Ease easeType = Ease.OutBack;
    [SerializeField] private float startOffsetY = -200f;  
    private void OnEnable()
    {
        StartCoroutine(PlayAfterBuild());
    }
    private IEnumerator PlayAfterBuild()
    {
        yield return null;  
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroup.GetComponent<RectTransform>());
        Play();
    }
    public void Play()
    {
        layoutGroup.enabled = false;
        for (int i = 0; i < buttons.Count; i++)
        {
            RectTransform btn = buttons[i];
            Vector2 targetPos = btn.anchoredPosition;
            btn.anchoredPosition = new Vector2(targetPos.x, targetPos.y + startOffsetY);
            btn
                .DOAnchorPosY(targetPos.y, slideDuration)
                .SetDelay(i * delayBetween)
                .SetEase(easeType);
        }
        float totalTime = slideDuration + (buttons.Count - 1) * delayBetween;
        DOVirtual.DelayedCall(totalTime + 0.1f, () =>
        {
            layoutGroup.enabled = true;
        });
    }
}

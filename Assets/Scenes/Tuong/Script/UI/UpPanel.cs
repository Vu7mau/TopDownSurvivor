using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class UpPanel : MonoBehaviour
{
    public HorizontalLayoutGroup layoutGroup;
    public RectTransform[] buttons;  
    public float dropDistance = 75f;
    public float dropDuration = 0.3f;
    public float delayBetweenButtons = 0.1f;
    public float initialDelay = 0.2f;
    public void Play()
    {
        StartCoroutine(PlayButtonIntro());
    }

    private IEnumerator PlayButtonIntro()
    {
        RectTransform panelRect = layoutGroup.GetComponent<RectTransform>();
        LayoutRebuilder.ForceRebuildLayoutImmediate(panelRect);
        Vector2[] targetPositions = new Vector2[buttons.Length];
        for (int i = 0; i < buttons.Length; i++)
        {
            RectTransform btn = buttons[i];
            targetPositions[i] = btn.anchoredPosition;
            btn.anchoredPosition = targetPositions[i] + Vector2.up * dropDistance;
            btn.gameObject.SetActive(false);
        }
        layoutGroup.enabled = false;
        yield return new WaitForSeconds(initialDelay);
        for (int i = 0; i < buttons.Length; i++)
        {
            RectTransform btn = buttons[i];
            btn.gameObject.SetActive(true);
            btn.DOAnchorPosY(targetPositions[i].y, dropDuration)
               .SetEase(Ease.OutBounce);
            yield return new WaitForSeconds(delayBetweenButtons);
        }
        yield return new WaitForSeconds(dropDuration);
        layoutGroup.enabled = true;
        LayoutRebuilder.ForceRebuildLayoutImmediate(panelRect);
    }
}

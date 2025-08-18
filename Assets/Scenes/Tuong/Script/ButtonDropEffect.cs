using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class ButtonDropEffect : MonoBehaviour
{
    public RectTransform[] buttons; 
    public VerticalLayoutGroup verticalLayoutGroup;
    public float dropDistance = 150f; 
    public float duration = 0.5f;    
    public float delayStep = 0.1f;
    public void OnEnable()
    {
        PlayDrop();
    }
    public void PlayDrop()
    {
        if(verticalLayoutGroup != null)
        {
            verticalLayoutGroup.enabled = false;
        }
        for (int i = buttons.Length - 1; i >= 0; i--)
        {
            RectTransform btn = buttons[i];
            Vector3 originalPos = btn.localPosition;
            Vector3 startPos = originalPos + Vector3.up * dropDistance;

            btn.localPosition = startPos;
            int index = i;

            btn.DOLocalMove(originalPos, duration)
               .SetEase(Ease.OutBack)
               .SetDelay(delayStep * (buttons.Length - 1 - i))
               .OnComplete(() =>
               {
                   if(index == 0 && verticalLayoutGroup != null)
                   {
                       verticalLayoutGroup.enabled = true;
                   }
               }).SetUpdate(true); 
        }
    }
}

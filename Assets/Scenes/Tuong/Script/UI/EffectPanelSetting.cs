using UnityEngine;
using DG.Tweening;
using System;
public class EffectPanelSetting : MonoBehaviour
{
    [SerializeField] private RectTransform panelSetting;
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private float hideOffetY = 600f;
    private Vector2 originalPos;
    private Vector2 hidePos;
    private void OnEnable()
    {
        if (originalPos == Vector2.zero)
        {
            originalPos = panelSetting.anchoredPosition;
            hidePos = originalPos + Vector2.up * hideOffetY;
        }
    }
    public void ShowPanel()
    {
        panelSetting.DOKill(true);
        panelSetting.anchoredPosition = hidePos;
        gameObject.SetActive(true);
        MainMenuTwo.Instance.PlayMenu.SetActive(false);
        MainMenuTwo.Instance.SettingPanel.SetActive(true);
        panelSetting.DOAnchorPos(originalPos, duration).SetEase(Ease.OutCubic);
    }
    public void HidePanel(Action onComplete = null)
    {
        panelSetting.DOKill(true);
        panelSetting.DOAnchorPos(hidePos, duration).SetEase(Ease.InCubic)
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
                onComplete?.Invoke();
            });
    }
}

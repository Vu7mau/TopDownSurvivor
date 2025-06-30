using DG.Tweening;
using UnityEngine;
public class FighPanel : MonoBehaviour
{
    [Header("Fight Panel Settings")]
    public static FighPanel Instance;
    public RectTransform fightPanel;
    public RectTransform modePanel;
    public float startOffsetY = -500f;
    public float startOffsetX = -500f;
    public float duration = 0.5f;
    private float initialDelay = 0f;
    private Vector2 fightTargetPosition;
    private Vector2 modeTargetPosition;
    public GameObject fightButton;
    [Header("UI Settings")]
    public GameObject campaignMode; 
    public GameObject surviveMode;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        fightButton.SetActive(false);
        fightTargetPosition = fightPanel.anchoredPosition;
        modeTargetPosition = modePanel.anchoredPosition;

        fightPanel.gameObject.SetActive(false);
        modePanel.gameObject.SetActive(false);
    }
    public void PlayEntrance()
    {
        fightPanel.anchoredPosition = fightTargetPosition + Vector2.up * startOffsetY;

        fightPanel.DOKill();
        fightPanel.gameObject.SetActive(true);
        DOVirtual.DelayedCall(initialDelay, () =>
        {
            fightPanel.DOAnchorPosY(fightTargetPosition.y, duration)
                 .SetEase(Ease.OutCubic);
        });
    }
    public void PlayModePanel()
    {
        modePanel.anchoredPosition = new Vector2(modeTargetPosition.x + startOffsetX, modeTargetPosition.y);
        modePanel.DOKill();
        modePanel.gameObject.SetActive(true);
        modePanel.DOAnchorPosX(modeTargetPosition.x, duration)
            .SetEase(Ease.OutCubic);
    }
    public void HideFightPanel()
    {
        fightPanel.DOKill();
        fightPanel.DOAnchorPosY(fightTargetPosition.y + startOffsetY, duration)
            .SetEase(Ease.InCubic)
            .OnComplete(() => fightPanel.gameObject.SetActive(false));
    }
    public void HideModePanel()
    {
        modePanel.DOKill();
        modePanel.DOAnchorPosX(modeTargetPosition.x + startOffsetX, duration)
            .SetEase(Ease.InCubic)
            .OnComplete(() => modePanel.gameObject.SetActive(false));
    }
}

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTrePanel : MonoBehaviour
{
    public static SkillTrePanel Instance;
    public RectTransform skillTreePanel;
    public float startOffsetY = 500f;
    public float duration = 0.5f;
    private float initialDelay = 0f;
    private Vector2 fightTargetPosition;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        fightTargetPosition = skillTreePanel.anchoredPosition;
        skillTreePanel.gameObject.SetActive(false);
    }
    public void PlayEntrance()
    {
        skillTreePanel.anchoredPosition = fightTargetPosition + Vector2.down * startOffsetY; 
        skillTreePanel.DOKill();
        skillTreePanel.gameObject.SetActive(true);
        DOVirtual.DelayedCall(initialDelay, () =>
        {
            skillTreePanel.DOAnchorPosY(fightTargetPosition.y, duration)
                 .SetEase(Ease.OutCubic);
        });
    }
    public void HideFightPanel()
    {
        skillTreePanel.DOKill();
        skillTreePanel.DOAnchorPosY(fightTargetPosition.y + startOffsetY, duration)
            .SetEase(Ease.InCubic)
            .OnComplete(() => skillTreePanel.gameObject.SetActive(false));
    }
}

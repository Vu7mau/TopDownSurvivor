using DG.Tweening;
using UnityEngine;

public class PanelDieEffect : MonoBehaviour
{
    public static PanelDieEffect Instance;

    public RectTransform crownIcon; 
    public RectTransform parentPanel;

    public float slideDuration = 0.5f;  
    public float crownJumpDuration = 0.5f; 
    public float crownJumpHeight = 100f;  

    private Vector2 panelOriginalPos;
    private Vector2 crownOriginalPos;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (parentPanel != null)
            panelOriginalPos = parentPanel.anchoredPosition;

        if (crownIcon != null)
            crownOriginalPos = crownIcon.anchoredPosition;
    }

    public void Show()
    {
        gameObject.SetActive(true);

        if (parentPanel != null)
        {
            parentPanel.anchoredPosition = panelOriginalPos + Vector2.up * 500f;
            parentPanel.DOAnchorPos(panelOriginalPos, slideDuration).SetEase(Ease.OutCubic);
        }

        if (crownIcon != null)
        {
            crownIcon.anchoredPosition = crownOriginalPos + Vector2.up * crownJumpHeight;
            crownIcon.DOAnchorPos(crownOriginalPos, crownJumpDuration)
                     .SetEase(Ease.OutBounce);
        }
    }

    public void Close()
    {
        if (parentPanel != null)
        {
            parentPanel.DOAnchorPos(panelOriginalPos + Vector2.up * 500f, slideDuration)
                       .SetEase(Ease.InCubic)
                       .OnComplete(() => gameObject.SetActive(false));
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}

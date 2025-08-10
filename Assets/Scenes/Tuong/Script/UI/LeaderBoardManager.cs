using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class LeaderBoardManager : MonoBehaviour
{
    public static LeaderBoardManager Instance;
    [SerializeField] protected TextMeshProUGUI playerRankCampignText;
    [SerializeField] protected TextMeshProUGUI playerRankSurviveText;
    [SerializeField] protected RectTransform hightLightCampign;
    [SerializeField] protected RectTransform hightLightSurvive;
    [SerializeField] protected Canvas canvas;
    [SerializeField] protected Transform contentCampign;
    [SerializeField] protected Transform contentSurvive;
    public ScrollRect scrollRectCampign;
    public ScrollRect scrollRectSurvive;
    protected const string leaderboardStat = "CampaignManual";
    protected const string timeStat = "CampaignTime";
    protected const string leaderboardSurvive = "SurviveManual";
    protected const string timeSurvive = "SurviveTime";
    protected int lastMyIndex = -1;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            RebindText();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    protected void RebindText()
    {
        var allTexts = Resources.FindObjectsOfTypeAll<TextMeshProUGUI>();
        playerRankCampignText = allTexts.FirstOrDefault(t => t.name == "PlayerRankCampaignText");
        playerRankSurviveText = allTexts.FirstOrDefault(t => t.name == "PlayerRankSurviveText");
        var allRects = Resources.FindObjectsOfTypeAll<RectTransform>();
        hightLightCampign = allRects.FirstOrDefault(t => t.name == "HightLightCampign");
        hightLightSurvive = allRects.FirstOrDefault(t => t.name == "HightLightSurvive");
        var allCanvases = Resources.FindObjectsOfTypeAll<Canvas>();
        canvas = allCanvases.FirstOrDefault(t => t.name == "Canvas");
        var allTransforms = Resources.FindObjectsOfTypeAll<Transform>();
        contentCampign = allTransforms.FirstOrDefault(t => t.name == "ContentCampign");
        contentSurvive = allTransforms.FirstOrDefault(t => t.name == "ContentSurvive");
        var allScrollRects = Resources.FindObjectsOfTypeAll<ScrollRect>();
        scrollRectCampign = allScrollRects.FirstOrDefault(t => t.name == "ScrollRectCampign");
        scrollRectSurvive = allScrollRects.FirstOrDefault(t => t.name == "ScrollRectSurvive");
    }
    public void SetMyRankIndex(int index)
    {
        lastMyIndex = index;
    }
    private void OnEnable()
    {
        if (lastMyIndex >= 0 && scrollRectCampign != null && contentCampign != null)
        {
            StartCoroutine(ScrollAfterLayout(scrollRectCampign, (RectTransform)contentCampign, lastMyIndex, 100f));
        }
    }
    public void ScrollToMyRank(ScrollRect scrollRect, RectTransform content, int myIndex, float scrollPadding = 100f)
    {
        if (scrollRect == null || content == null) return;
        if (myIndex < 0 || myIndex >= content.childCount) return;

        RectTransform target = content.GetChild(myIndex).GetComponent<RectTransform>();
        if (target == null) return;

        float contentHeight = content.rect.height;
        float itemPos = Mathf.Abs(target.anchoredPosition.y);
        float viewHeight = scrollRect.viewport.rect.height;

        float scrollValue = Mathf.Clamp01((itemPos - scrollPadding) / (contentHeight - viewHeight));
        scrollRect.verticalNormalizedPosition = 1 - scrollValue;
    }
    public void ScrollToMyRankSmooth(ScrollRect scrollRect, RectTransform content, int myIndex, float scrollPadding = 100f)
    {
        if (scrollRect == null || content == null) return;
        if (myIndex < 0 || myIndex >= content.childCount) return;
        RectTransform target = content.GetChild(myIndex).GetComponent<RectTransform>();
        if (target == null) return;

        float contentHeight = content.rect.height;
        float itemPos = Mathf.Abs(target.anchoredPosition.y);
        float viewHeight = scrollRect.viewport.rect.height;

        float scrollValue = Mathf.Clamp01((itemPos - scrollPadding) / (contentHeight - viewHeight));
        scrollRect.verticalNormalizedPosition = 1 - scrollValue;
    }
    private IEnumerator ScrollAfterLayout(ScrollRect scrollRect, RectTransform content, int myIndex, float scrollPadding)
    {
        yield return new WaitForEndOfFrame(); 
        LayoutRebuilder.ForceRebuildLayoutImmediate(content); 
        RectTransform target = content.GetChild(myIndex).GetComponent<RectTransform>();
        if (target == null) yield break;

        float contentHeight = content.rect.height;
        float itemPos = Mathf.Abs(target.anchoredPosition.y);
        float viewHeight = scrollRect.viewport.rect.height;

        float scrollValue = Mathf.Clamp01((itemPos - scrollPadding) / (contentHeight - viewHeight));
        float targetPos = 1f - scrollValue;

        yield return SmoothScroll(scrollRect, targetPos, 0.5f); 
    }

    private IEnumerator SmoothScroll(ScrollRect scrollRect, float target, float duration)
    {
        float start = scrollRect.verticalNormalizedPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            scrollRect.verticalNormalizedPosition = Mathf.Lerp(start, target, Mathf.SmoothStep(0f, 1f, elapsed / duration));
            yield return null;
        }

        scrollRect.verticalNormalizedPosition = target;
    }

}

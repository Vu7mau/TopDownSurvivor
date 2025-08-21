using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
public class LeaderBoardManager : MonoBehaviour
{
    public static LeaderBoardManager Instance;
    [SerializeField] protected Canvas canvas;
    [SerializeField] protected Transform contentCampign;
    [SerializeField] protected Transform contentSurvive;
    public ScrollRect scrollRectCampign;
    public ScrollRect scrollRectSurvive;
    [SerializeField] protected TextMeshProUGUI top1CampaignNameText;
    [SerializeField] protected TextMeshProUGUI top1CampaignScoreText;
    [SerializeField] protected TextMeshProUGUI top2CampaignNameText;
    [SerializeField] protected TextMeshProUGUI top2CampaignScoreText;
    [SerializeField] protected TextMeshProUGUI top3CampaignNameText;
    [SerializeField] protected TextMeshProUGUI top3CampaignScoreText;
    [SerializeField] protected TextMeshProUGUI rankCampaign;
    [SerializeField] protected TextMeshProUGUI timeCampignTwo;
    [SerializeField] protected TextMeshProUGUI nameCampaign;
    [SerializeField] protected TextMeshProUGUI scoreCampaign;
    [SerializeField] protected TextMeshProUGUI top1SurviveNameText;
    [SerializeField] protected TextMeshProUGUI top1SurviveScoreText;
    [SerializeField] protected TextMeshProUGUI top2SurviveNameText;
    [SerializeField] protected TextMeshProUGUI top2SurviveScoreText;
    [SerializeField] protected TextMeshProUGUI top3SurviveNameText;
    [SerializeField] protected TextMeshProUGUI top3SurviveScoreText;
    [SerializeField] protected TextMeshProUGUI rankSurvive;
    [SerializeField] protected TextMeshProUGUI timeSurviveTwo;
    [SerializeField] protected TextMeshProUGUI nameSurvive;
    [SerializeField] protected TextMeshProUGUI scoreSurvive;

    protected const string leaderboardCampign = "CampaignManual";
    protected const string timeCampign = "CampaignTime";
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
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0)
        {
            RebindText();
        }
    }
    protected void RebindText()
    {
        var allTexts = Resources.FindObjectsOfTypeAll<TextMeshProUGUI>();
        top1CampaignNameText = allTexts.FirstOrDefault(t => t.name == "Top1CampaignNameText");
        top1CampaignScoreText = allTexts.FirstOrDefault(t => t.name == "Top1CampaignScoreText");
        top2CampaignNameText = allTexts.FirstOrDefault(t => t.name == "Top2CampaignNameText");
        top2CampaignScoreText = allTexts.FirstOrDefault(t => t.name == "Top2CampaignScoreText");
        top3CampaignNameText = allTexts.FirstOrDefault(t => t.name == "Top3CampaignNameText");
        top3CampaignScoreText = allTexts.FirstOrDefault(t => t.name == "Top3CampaignScoreText");
        rankCampaign = allTexts.FirstOrDefault(t => t.name == "RankCampaign");
        nameCampaign = allTexts.FirstOrDefault(t => t.name == "NameCampaign");
        scoreCampaign = allTexts.FirstOrDefault(t => t.name == "ScoreCampaign");
        top1SurviveNameText = allTexts.FirstOrDefault(t => t.name == "Top1SurviveNameText");
        top1SurviveScoreText = allTexts.FirstOrDefault(t => t.name == "Top1SurviveScoreText");
        top2SurviveNameText = allTexts.FirstOrDefault(t => t.name == "Top2SurviveNameText");
        top2SurviveScoreText = allTexts.FirstOrDefault(t => t.name == "Top2SurviveScoreText");
        top3SurviveNameText = allTexts.FirstOrDefault(t => t.name == "Top3SurviveNameText");
        top3SurviveScoreText = allTexts.FirstOrDefault(t => t.name == "Top3SurviveScoreText");
        rankSurvive = allTexts.FirstOrDefault(t => t.name == "RankSurvive");
        nameSurvive = allTexts.FirstOrDefault(t => t.name == "NameSurvive");
        scoreSurvive = allTexts.FirstOrDefault(t => t.name == "ScoreSurvive");
        timeSurviveTwo = allTexts.FirstOrDefault(t => t.name == "TimeSurvive");
        timeCampignTwo = allTexts.FirstOrDefault(t => t.name == "TimeCampign");
        var allRects = Resources.FindObjectsOfTypeAll<RectTransform>();
        var allCanvases = Resources.FindObjectsOfTypeAll<Canvas>();
        canvas = allCanvases.FirstOrDefault(t => t.name == "Canvas");
        var allTransforms = Resources.FindObjectsOfTypeAll<Transform>();
        contentCampign = allTransforms.FirstOrDefault(t => t.name == "ContentCampign" && t.gameObject.scene.IsValid());
        contentSurvive = allTransforms.FirstOrDefault(t => t.name == "ContentSurvive" && t.gameObject.scene.IsValid());
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
